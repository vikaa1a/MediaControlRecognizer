using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Video.DirectShow;

namespace MediaControlRecognizer
{
    public partial class Form1 : Form
    {
        // Собственная нейросеть
        private NeuralNetwork myNetwork;

        // Библиотечная нейросеть (Accord.NET)
        private ActivationNetwork accordNetwork;
        private BackPropagationLearning teacher;

        private Random rnd = new Random();
        private VideoCaptureDevice videoSource;
        private bool isCapturing = false;
        private FilterInfoCollection videoDevices;

        private TelegramBotHandler botHandler;

        // Имена символов для отображения
        private readonly string[] symbolNames =
        {
            "▶ Play", "⏸ Pause", "⏹ Stop", "⏪ Rewind", "⏩ FastForward",
            "⏭ Next", "⏮ Previous", "● Record", "⏏ Eject", "🔊 Volume"
        };

        public Form1()
        {
            InitializeComponent();
            InitializeNetworks();
            InitializeCamera();

      
            InitializeTelegramBot();
        }


        private void InitializeTelegramBot()
        {
            try
            {
                // ВСТАВЬТЕ ВАШ ТОКЕН ЗДЕСЬ!
                string telegramToken = "8532913559:AAHB-tNvVBXeHjVaVs4AxNASTFrVzwzgWaU"; // пример

                botHandler = new TelegramBotHandler(telegramToken);
                botHandler.Start();

                lblStatus.Text = "Telegram бот инициализирован";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Ошибка Telegram бота: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void InitializeNetworks()
        {
            try
            {
                // Инициализация собственной нейросети
                myNetwork = new NeuralNetwork(400, 200, 100, 10); // Увеличены скрытые слои

                // Инициализация нейросети Accord.NET с улучшенной архитектурой
                accordNetwork = new ActivationNetwork(
                    new SigmoidFunction(),
                    400,    // входы
                    200,    // первый скрытый слой
                    100,    // второй скрытый слой
                    10      // выходы
                );

                // Инициализация весов Accord.NET (используем NguyenWidrow)
                NguyenWidrow initializer = new NguyenWidrow(accordNetwork);
                initializer.Randomize();

                // Создаем учителя для Accord.NET с настройками
                teacher = new BackPropagationLearning(accordNetwork);
                teacher.LearningRate = 0.1; // Увеличен learning rate
                teacher.Momentum = 0.5; // Добавлен momentum
            }
            catch (Exception ex)
            {

                // Резервный вариант - только собственная сеть
                myNetwork = new NeuralNetwork(400, 200, 100, 10);
                accordNetwork = null;
                teacher = null;
            }
        }

        private void InitializeCamera()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                btnStartCamera.Enabled = false;
                btnCapture.Enabled = false;
                return;
            }

            cmbCameras.Items.Clear();
            foreach (FilterInfo device in videoDevices)
            {
                cmbCameras.Items.Add(device.Name);
            }
            cmbCameras.SelectedIndex = 0;

        }

        // ============ ГЕНЕРАЦИЯ ОБУЧАЮЩИХ ИЗОБРАЖЕНИЙ ============

        private Bitmap GenerateTrainingImage(int symbolIndex, bool addNoise = true)
        {
            Bitmap bmp = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);

                // Установки для рисования
                Pen pen = new Pen(Color.Black, 4);
                Brush brush = Brushes.Black;

                int centerX = 50;
                int centerY = 50;
                int size = 60;

                // Рисуем символ в зависимости от типа
                switch (symbolIndex)
                {
                    case 0: // Play - Треугольник вправо
                        Point[] playPoints = {
                            new Point(centerX - size/3, centerY - size/2),
                            new Point(centerX + size/2, centerY),
                            new Point(centerX - size/3, centerY + size/2)
                        };
                        g.FillPolygon(brush, playPoints);
                        break;

                    case 1: // Pause - Два прямоугольника
                        int barWidth = 12;
                        g.FillRectangle(brush, centerX - size / 3 - barWidth, centerY - size / 3, barWidth, size * 2 / 3);
                        g.FillRectangle(brush, centerX + size / 3, centerY - size / 3, barWidth, size * 2 / 3);
                        break;

                    case 2: // Stop - Квадрат
                        g.FillRectangle(brush, centerX - size / 2, centerY - size / 2, size, size);
                        break;

                    case 3: // Rewind - Два треугольника влево
                        Point[] rewLeft = {
                            new Point(centerX, centerY - size/3),
                            new Point(centerX - size/3, centerY),
                            new Point(centerX, centerY + size/3)
                        };
                        Point[] rewRight = {
                            new Point(centerX + size/4, centerY - size/3),
                            new Point(centerX - size/12, centerY),
                            new Point(centerX + size/4, centerY + size/3)
                        };
                        g.FillPolygon(brush, rewLeft);
                        g.FillPolygon(brush, rewRight);
                        break;

                    case 4: // FastForward - Два треугольника вправо
                        Point[] ffLeft = {
                            new Point(centerX - size/4, centerY - size/3),
                            new Point(centerX + size/12, centerY),
                            new Point(centerX - size/4, centerY + size/3)
                        };
                        Point[] ffRight = {
                            new Point(centerX, centerY - size/3),
                            new Point(centerX + size/3, centerY),
                            new Point(centerX, centerY + size/3)
                        };
                        g.FillPolygon(brush, ffLeft);
                        g.FillPolygon(brush, ffRight);
                        break;

                    case 5: // Next - Треугольник и вертикальная линия
                        Point[] nextTri = {
                            new Point(centerX - size/4, centerY - size/3),
                            new Point(centerX + size/4, centerY),
                            new Point(centerX - size/4, centerY + size/3)
                        };
                        g.FillPolygon(brush, nextTri);
                        g.FillRectangle(brush, centerX + size / 6, centerY - size / 3, 8, size * 2 / 3);
                        break;

                    case 6: // Previous - Вертикальная линия и треугольник
                        g.FillRectangle(brush, centerX - size / 6 - 8, centerY - size / 3, 8, size * 2 / 3);
                        Point[] prevTri = {
                            new Point(centerX + size/4, centerY - size/3),
                            new Point(centerX - size/4, centerY),
                            new Point(centerX + size/4, centerY + size/3)
                        };
                        g.FillPolygon(brush, prevTri);
                        break;

                    case 7: // Record - Круг
                        g.FillEllipse(brush, centerX - size / 2, centerY - size / 2, size, size);
                        break;

                    case 8: // Eject - Треугольник вверх над прямоугольником
                        Point[] ejectTri = {
                            new Point(centerX, centerY - size/3),
                            new Point(centerX - size/3, centerY),
                            new Point(centerX + size/3, centerY)
                        };
                        g.FillPolygon(brush, ejectTri);
                        g.FillRectangle(brush, centerX - size / 3, centerY + size / 6, size * 2 / 3, 10);
                        break;

                    case 9: // Volume - Символ динамика
                        // Основной треугольник
                        Point[] volTri = {
                            new Point(centerX - size/4, centerY - size/3),
                            new Point(centerX + size/4, centerY - size/6),
                            new Point(centerX - size/4, centerY + size/3)
                        };
                        g.FillPolygon(brush, volTri);

                        // Волны
                        for (int i = 0; i < 3; i++)
                        {
                            Rectangle wave = new Rectangle(
                                centerX + size / 4 + i * 10,
                                centerY - size / 4 + i * 5,
                                8,
                                size / 2 - i * 10
                            );
                            g.DrawEllipse(pen, wave);
                        }
                        break;
                }

                // Добавляем случайные искажения для улучшения обучения
                if (addNoise)
                {
                    AddDistortions(bmp);
                }
            }

            return bmp;
        }

        private void AddDistortions(Bitmap bmp)
        {
            // Случайный шум
            int noiseCount = rnd.Next(50, 150);
            for (int i = 0; i < noiseCount; i++)
            {
                int x = rnd.Next(0, bmp.Width);
                int y = rnd.Next(0, bmp.Height);
                if (rnd.NextDouble() > 0.7)
                    bmp.SetPixel(x, y, Color.Black);
                else
                    bmp.SetPixel(x, y, Color.White);
            }

            // Случайные аффинные преобразования
            if (rnd.NextDouble() > 0.6)
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.TranslateTransform(rnd.Next(-5, 5), rnd.Next(-5, 5));
                }
            }
        }

        // ============ ОБРАБОТКА ИЗОБРАЖЕНИЙ ============

        private double[] ImageToVector(Bitmap bmp)
        {
            // Приводим к размеру 20x20
            Bitmap resized = ResizeImage(bmp, 20, 20);

            // Бинаризация
            Bitmap binary = Binarize(resized, 150);

            // Преобразование в вектор (400 значений)
            double[] vector = new double[400];
            int index = 0;

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    Color pixel = binary.GetPixel(x, y);
                    vector[index++] = (pixel.R < 128) ? 1.0 : 0.0;
                }
            }

            return vector;
        }

        private Bitmap ProcessCameraImage(Bitmap input)
        {
            // 1. Конвертация в градации серого
            Bitmap gray = Grayscale(input);

            // 2. Бинаризация с адаптивным порогом
            Bitmap binary = Binarize(gray, GetAdaptiveThreshold(gray));

            // 3. Нахождение границ объекта
            Rectangle bounds = FindBoundingBox(binary);

            // 4. Если объект найден - обрезаем и масштабируем
            if (bounds.Width > 10 && bounds.Height > 10)
            {
                Bitmap cropped = CropImage(binary, bounds);
                Bitmap resized = ResizeImage(cropped, 100, 100);
                return resized;
            }

            // Если объект не найден, возвращаем черное изображение
            return new Bitmap(100, 100);
        }

        private Rectangle FindBoundingBox(Bitmap binary)
        {
            int minX = binary.Width, minY = binary.Height;
            int maxX = 0, maxY = 0;
            bool found = false;

            for (int y = 0; y < binary.Height; y++)
            {
                for (int x = 0; x < binary.Width; x++)
                {
                    if (binary.GetPixel(x, y).R < 128) // Черный пиксель
                    {
                        found = true;
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            if (!found)
                return new Rectangle(0, 0, binary.Width, binary.Height);

            // Добавляем отступы
            int padding = 10;
            minX = Math.Max(0, minX - padding);
            minY = Math.Max(0, minY - padding);
            maxX = Math.Min(binary.Width - 1, maxX + padding);
            maxY = Math.Min(binary.Height - 1, maxY + padding);

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            return new Rectangle(minX, minY, width, height);
        }

        private int GetAdaptiveThreshold(Bitmap gray)
        {
            // Простой адаптивный порог на основе среднего значения
            long sum = 0;
            int count = 0;

            for (int y = 0; y < gray.Height; y += 2)
            {
                for (int x = 0; x < gray.Width; x += 2)
                {
                    sum += gray.GetPixel(x, y).R;
                    count++;
                }
            }

            int average = (int)(sum / Math.Max(1, count));
            return Math.Max(100, Math.Min(200, average - 30));
        }

        // ============ ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ДЛЯ ИЗОБРАЖЕНИЙ ============

        private Bitmap Grayscale(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Color color = input.GetPixel(x, y);
                    int gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                    output.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            return output;
        }

        private Bitmap Binarize(Bitmap input, int threshold)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Color color = input.GetPixel(x, y);
                    int value = (color.R < threshold) ? 0 : 255;
                    output.SetPixel(x, y, Color.FromArgb(value, value, value));
                }
            }

            return output;
        }

        private Bitmap CropImage(Bitmap input, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                return new Bitmap(1, 1);

            Bitmap output = new Bitmap(rect.Width, rect.Height);

            using (Graphics g = Graphics.FromImage(output))
            {
                g.DrawImage(input, 0, 0, rect, GraphicsUnit.Pixel);
            }

            return output;
        }

        private Bitmap ResizeImage(Bitmap input, int width, int height)
        {
            Bitmap output = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(output))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(input, 0, 0, width, height);
            }

            return output;
        }

        // ============ ОБУЧЕНИЕ НЕЙРОСЕТИ ============

        private async void btnTrain_Click(object sender, EventArgs e)
        {
            if (myNetwork == null || accordNetwork == null)
            {
                MessageBox.Show("Нейросети не инициализированы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Блокируем кнопки во время обучения
            btnTrain.Enabled = false;
            btnTest.Enabled = false;
            btnCapture.Enabled = false;
            btnStartCamera.Enabled = false;


            int epochs = (int)numEpochs.Value;
            int samplesPerEpoch = 300; // Увеличено количество примеров

            // Создаем прогресс-бар
            Progress<int> progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
                lblStatus.Text = $"Обучение: {value}%";
            });

            try
            {
                await Task.Run(() => TrainNetworks(epochs, samplesPerEpoch, progress));


                MessageBox.Show("Обучение успешно завершено!", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обучения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Разблокируем кнопки
                btnTrain.Enabled = true;
                btnTest.Enabled = true;
                btnCapture.Enabled = isCapturing;
                btnStartCamera.Enabled = videoDevices.Count > 0;
                progressBar.Value = 0;
                lblStatus.Text = "Готово";
            }
        }

        private void TrainNetworks(int epochs, int samplesPerEpoch, IProgress<int> progress)
        {
            double bestError = double.MaxValue;

            for (int epoch = 1; epoch <= epochs; epoch++)
            {
                double myTotalError = 0;
                double accordTotalError = 0;

                for (int i = 0; i < samplesPerEpoch; i++)
                {
                    // Случайный символ для обучения
                    int symbolIndex = rnd.Next(0, 10);
                    double[] target = new double[10];
                    target[symbolIndex] = 1.0;

                    // Генерация обучающего изображения
                    using (Bitmap bmp = GenerateTrainingImage(symbolIndex, true))
                    {
                        double[] inputs = ImageToVector(bmp);

                        // Обучение собственной нейросети
                        myNetwork.Train(inputs, target);
                        double[] myOutput = myNetwork.FeedForward(inputs);
                        myTotalError += CalculateError(target, myOutput);

                        // Обучение нейросети Accord.NET
                        if (accordNetwork != null && teacher != null)
                        {
                            try
                            {
                                // Прямое распространение
                                double[] accordOutput = accordNetwork.Compute(inputs);

                                // Обратное распространение
                                double error = teacher.Run(inputs, target);
                                accordTotalError += error;
                            }
                            catch (Exception ex)
                            {
                                // В случае ошибки добавляем штраф
                                accordTotalError += 1.0;
                            }
                        }
                    }
                }

                double myAvgError = myTotalError / samplesPerEpoch;
                double accordAvgError = accordTotalError / samplesPerEpoch;

                // Обновляем прогресс (каждые 10 эпох)
                if (epoch % 10 == 0 || epoch == epochs)
                {
                    int percent = (int)((double)epoch / epochs * 100);
                    progress?.Report(percent);

                    // Выводим информацию в лог через Invoke
                    this.Invoke(new Action(() =>
                    {
                        string logMessage = $"Эпоха {epoch,4}/{epochs} | " +
                                          $"Собств. ошибка: {myAvgError:F6}";

                        if (accordNetwork != null)
                            logMessage += $" | Accord ошибка: {accordAvgError:F6}";

                    }));
                }
            }
        }

        private double CalculateError(double[] target, double[] output)
        {
            double error = 0;
            for (int i = 0; i < target.Length; i++)
            {
                error += Math.Pow(target[i] - output[i], 2);
            }
            return error / target.Length;
        }

        // ============ ТЕСТИРОВАНИЕ ============

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (myNetwork == null || accordNetwork == null)
            {
                MessageBox.Show("Сначала обучите нейросеть!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Генерируем случайный символ
            int actualSymbol = rnd.Next(0, 10);
            Bitmap testImage = GenerateTrainingImage(actualSymbol, false);

            // Отображаем тестовое изображение
            pictureBoxTest.Image = testImage;
            lblTestSymbol.Text = $"Тестовый символ: {symbolNames[actualSymbol]}";
            lblTestSymbol.ForeColor = Color.Blue;

            // Преобразуем в вектор
            double[] inputs = ImageToVector(testImage);

            // Распознавание собственной нейросетью
            double[] myOutput = myNetwork.FeedForward(inputs);
            int myPrediction = GetMaxIndex(myOutput);
            double myConfidence = myOutput[myPrediction] * 100;

            // Распознавание нейросетью Accord.NET
            double[] accordOutput = accordNetwork.Compute(inputs);
            int accordPrediction = GetMaxIndex(accordOutput);
            double accordConfidence = accordOutput[accordPrediction] * 100;

            // Отображаем результаты
            DisplayTestResults(actualSymbol, myPrediction, myConfidence,
                              accordPrediction, accordConfidence, myOutput, accordOutput);
        }

        private void DisplayTestResults(int actual, int myPred, double myConf,
                                       int accordPred, double accordConf,
                                       double[] myOutput, double[] accordOutput)
        {
            bool myCorrect = (myPred == actual);
            bool accordCorrect = (accordPred == actual);

            // Отображаем предсказания
            lblMyResult.Text = $"Собственная сеть: {symbolNames[myPred]} ({myConf:F1}%)";
            lblMyResult.ForeColor = myCorrect ? Color.Green : Color.Red;

            lblAccordResult.Text = $"Accord.NET сеть: {symbolNames[accordPred]} ({accordConf:F1}%)";
            lblAccordResult.ForeColor = accordCorrect ? Color.Green : Color.Red;

        }

        // ============ РАБОТА С КАМЕРОЙ ============

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            if (isCapturing)
            {
                StopCamera();
                btnStartCamera.Text = "Запустить камеру";
                btnCapture.Enabled = false;
            }
            else
            {
                StartCamera();
                if (isCapturing)
                {
                    btnStartCamera.Text = "Остановить камеру";
                    btnCapture.Enabled = true;
                }
            }
        }

        private void StartCamera()
        {
            try
            {
                if (videoDevices.Count == 0)
                {
                    MessageBox.Show("Камеры не найдены!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                videoSource = new VideoCaptureDevice(videoDevices[cmbCameras.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                isCapturing = true;
            }
            catch (Exception ex)
            {
                isCapturing = false;
            }
        }

        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
            isCapturing = false;
            pictureBoxWebcam.Image = null;
        }

        private void VideoSource_NewFrame(object sender, Accord.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                if (pictureBoxWebcam.InvokeRequired)
                {
                    pictureBoxWebcam.Invoke(new Action(() =>
                    {
                        pictureBoxWebcam.Image = (Bitmap)eventArgs.Frame.Clone();
                    }));
                }
                else
                {
                    pictureBoxWebcam.Image = (Bitmap)eventArgs.Frame.Clone();
                }
            }
            catch
            {
                // Игнорируем ошибки при остановке камеры
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (pictureBoxWebcam.Image == null)
            {
                MessageBox.Show("Нет изображения с камеры!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (myNetwork == null || accordNetwork == null)
            {
                MessageBox.Show("Сначала обучите нейросеть!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap capturedImage = new Bitmap(pictureBoxWebcam.Image);

            // Обрабатываем изображение
            Bitmap processedImage = ProcessCameraImage(capturedImage);
            pictureBoxProcessed.Image = processedImage;

            // Распознаем
            double[] inputs = ImageToVector(processedImage);

            double[] myOutput = myNetwork.FeedForward(inputs);
            int myPrediction = GetMaxIndex(myOutput);
            double myConfidence = myOutput[myPrediction] * 100;

            double[] accordOutput = accordNetwork.Compute(inputs);
            int accordPrediction = GetMaxIndex(accordOutput);
            double accordConfidence = accordOutput[accordPrediction] * 100;

            if (myConfidence > accordConfidence)
            {
                botHandler.UpdateLastSymbol(symbolNames[myPrediction]);
            }
            else
            {
                botHandler.UpdateLastSymbol(symbolNames[accordPrediction]);
            }

            string recognizedSymbol = symbolNames[myPrediction]; // или accordPrediction

            // Обновляем бота
            if (botHandler != null)
            {
                botHandler.UpdateLastSymbol(recognizedSymbol);
            }

            lblStatus.Text = $"Распознано: {recognizedSymbol} (отправлено в Telegram)";

            // Отображаем результаты
            lblCaptureMyResult.Text = $"Собст.: {symbolNames[myPrediction]} ({myConfidence:F0}%)";
            lblCaptureAccordResult.Text = $"Accord: {symbolNames[accordPrediction]} ({accordConfidence:F0}%)";

            string recognizedName = symbolNames[myPrediction]; // Например, "Play"
            botHandler.UpdateLastSymbol(recognizedName); // Отправляем боту в память

            // Подсвечиваем наиболее уверенный результат
            if (myConfidence > accordConfidence)
            {
                lblCaptureMyResult.Font = new Font(lblCaptureMyResult.Font, FontStyle.Bold);
                lblCaptureAccordResult.Font = new Font(lblCaptureAccordResult.Font, FontStyle.Regular);
            }
            else
            {
                lblCaptureMyResult.Font = new Font(lblCaptureMyResult.Font, FontStyle.Regular);
                lblCaptureAccordResult.Font = new Font(lblCaptureAccordResult.Font, FontStyle.Bold);
            }
        }

        // ============ ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ============

        private int GetMaxIndex(double[] array)
        {
            int maxIndex = 0;
            double maxValue = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > maxValue)
                {
                    maxValue = array[i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();

            // Останавливаем Telegram бота
            botHandler?.Stop();
        }


    }
}