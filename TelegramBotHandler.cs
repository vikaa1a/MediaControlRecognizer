using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace MediaControlRecognizer
{
    public class TelegramBotHandler
    {
        private string _botToken;
        private string _lastSymbol = "ничего не распознано";
        private Timer _updateTimer;
        private long _chatId = 0;
        private int _lastUpdateId = 0;
        private bool _isRunning = false;

        public TelegramBotHandler(string botToken)
        {
            if (string.IsNullOrEmpty(botToken) || botToken.Contains("ВАШ_ТЕЛЕГРАМ"))
            {
                Console.WriteLine("Токен не установлен!");
                return;
            }

            _botToken = botToken;
            _isRunning = true;

            Console.WriteLine($"Telegram бот создан. Токен: {_botToken.Substring(0, Math.Min(5, _botToken.Length))}...");

            // Запускаем проверку сообщений
            _updateTimer = new Timer(CheckMessages, null, 2000, 3000);
        }

        public void Start()
        {
            Console.WriteLine("Telegram бот запущен");

            // Отправляем приветственное сообщение, если уже есть chat_id
            if (_chatId != 0)
            {
                SendMessage("🔄 Бот перезапущен и готов к работе!");
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _updateTimer?.Dispose();
            Console.WriteLine("Telegram бот остановлен");
        }

        public void UpdateLastSymbol(string symbol)
        {
            _lastSymbol = symbol;
            Console.WriteLine($"Telegram бот: обновлен символ - {symbol}");

            if (_chatId != 0)
            {
                SendMessage($"🔔 Распознан новый символ: {symbol}");
            }
        }

        private void CheckMessages(object state)
        {
            if (!_isRunning) return;

            try
            {
                string url = $"https://api.telegram.org/bot{_botToken}/getUpdates?offset={_lastUpdateId + 1}&timeout=1";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    string response = client.DownloadString(url);

                    // Простой парсинг JSON без библиотек
                    ProcessResponse(response);
                }
            }
            catch (Exception ex)
            {
                // Тихая обработка ошибок - не выводим в консоль постоянно
                if (!ex.Message.Contains("таймаута") && !ex.Message.Contains("timeout"))
                {
                    Console.WriteLine($"Ошибка Telegram: {ex.Message}");
                }
            }
        }

        private void ProcessResponse(string json)
        {
            int pos = 0;
            while ((pos = json.IndexOf("\"update_id\"", pos)) != -1)
            {
                // Получаем update_id
                int idStart = json.IndexOf(":", pos) + 1;
                int idEnd = json.IndexOf(",", idStart);
                if (idEnd == -1) idEnd = json.IndexOf("}", idStart);

                string updateIdStr = json.Substring(idStart, idEnd - idStart).Trim();
                int updateId = int.Parse(updateIdStr);
                _lastUpdateId = updateId;

                // Ищем сообщение
                int msgStart = json.IndexOf("\"message\"", idEnd);
                if (msgStart == -1) break;

                // Ищем текст сообщения
                int textStart = json.IndexOf("\"text\"", msgStart);
                if (textStart == -1)
                {
                    pos = msgStart;
                    continue;
                }

                textStart = json.IndexOf(":", textStart) + 1;
                int textEnd = json.IndexOf("\"", textStart + 1);
                string text = json.Substring(textStart + 1, textEnd - textStart - 1);

                // Ищем chat_id
                int chatStart = json.IndexOf("\"chat\"", msgStart);
                chatStart = json.IndexOf("\"id\"", chatStart);
                chatStart = json.IndexOf(":", chatStart) + 1;
                int chatEnd = json.IndexOf(",", chatStart);
                if (chatEnd == -1) chatEnd = json.IndexOf("}", chatStart);

                string chatIdStr = json.Substring(chatStart, chatEnd - chatStart).Trim();
                long chatId = long.Parse(chatIdStr);

                // Обрабатываем команду
                HandleCommand(chatId, text);

                pos = chatEnd;
            }
        }

        private void HandleCommand(long chatId, string command)
        {
            // Сохраняем chat_id при первой команде
            if (_chatId == 0)
            {
                _chatId = chatId;
                Console.WriteLine($"Получен chat_id: {_chatId}");
            }

            // Проверяем, что сообщение от нашего пользователя
            if (_chatId != chatId) return;

            Console.WriteLine($"Получена команда: {command}");

            switch (command.ToLower())
            {
                case "/start":
                    SendMessage($"🤖 Бот для распознавания символов медиа-плеера активирован!\n\n" +
                               $"📋 Команды:\n" +
                               $"/status - текущий статус\n" +
                               $"/symbol - последний распознанный символ\n" +
                               $"/help - справка\n\n" +
                               $"🔤 Распознаваемые символы:\n" +
                               $"▶ ⏸ ⏹ ⏪ ⏩ ⏭ ⏮ ● ⏏ 🔊");
                    break;

                case "/status":
                    SendMessage($"📊 Статус бота:\n" +
                               $"• Последний символ: {_lastSymbol}\n" +
                               $"• Время сервера: {DateTime.Now:HH:mm:ss}\n" +
                               $"• Дата: {DateTime.Now:dd.MM.yyyy}\n" +
                               $"• Статус: ✅ Активен");
                    break;

                case "/symbol":
                    SendMessage($"🎵 Последний распознанный символ:\n{_lastSymbol}");
                    break;

                case "/help":
                    SendMessage($"🆘 Справка по командам:\n\n" +
                               $"• /start - активация бота\n" +
                               $"• /status - текущий статус\n" +
                               $"• /symbol - последний распознанный символ\n" +
                               $"• /help - эта справка\n\n" +
                               $"📷 Как использовать:\n" +
                               $"1. Запустите программу\n" +
                               $"2. Обучите нейросеть\n" +
                               $"3. Распознайте символ с камеры\n" +
                               $"4. Бот пришлет уведомление");
                    break;

                case "привет":
                case "hello":
                case "hi":
                    SendMessage($"👋 Привет! Я бот для распознавания символов.\n" +
                               $"Последний символ: {_lastSymbol}");
                    break;

                default:
                    if (command.StartsWith("/"))
                    {
                        SendMessage($"❌ Неизвестная команда: {command}\n" +
                                   $"Используйте /help для списка команд");
                    }
                    break;
            }
        }

        private void SendMessage(string text)
        {
            if (_chatId == 0)
            {
                Console.WriteLine("Не могу отправить сообщение: chat_id не установлен");
                return;
            }

            try
            {
                string url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    string postData = $"chat_id={_chatId}&text={Uri.EscapeDataString(text)}";
                    byte[] data = Encoding.UTF8.GetBytes(postData);

                    byte[] response = client.UploadData(url, "POST", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    Console.WriteLine($"Сообщение отправлено в Telegram");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки в Telegram: {ex.Message}");
            }
        }
    }
}