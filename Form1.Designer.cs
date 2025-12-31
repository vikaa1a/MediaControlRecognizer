namespace MediaControlRecognizer
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxTest = new System.Windows.Forms.PictureBox();
            this.lblAccordResult = new System.Windows.Forms.Label();
            this.lblMyResult = new System.Windows.Forms.Label();
            this.lblTestSymbol = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numEpochs = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnTrain = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbCameras = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStartCamera = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pictureBoxWebcam = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblCaptureAccordResult = new System.Windows.Forms.Label();
            this.lblCaptureMyResult = new System.Windows.Forms.Label();
            this.pictureBoxProcessed = new System.Windows.Forms.PictureBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTest)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEpochs)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWebcam)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessed)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxTest);
            this.groupBox1.Controls.Add(this.lblAccordResult);
            this.groupBox1.Controls.Add(this.lblMyResult);
            this.groupBox1.Controls.Add(this.lblTestSymbol);
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 350);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тестирование сети";
            // 
            // pictureBoxTest
            // 
            this.pictureBoxTest.BackColor = System.Drawing.Color.White;
            this.pictureBoxTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxTest.Location = new System.Drawing.Point(20, 25);
            this.pictureBoxTest.Name = "pictureBoxTest";
            this.pictureBoxTest.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTest.TabIndex = 1;
            this.pictureBoxTest.TabStop = false;
            // 
            // lblAccordResult
            // 
            this.lblAccordResult.AutoSize = true;
            this.lblAccordResult.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAccordResult.Location = new System.Drawing.Point(20, 295);
            this.lblAccordResult.Name = "lblAccordResult";
            this.lblAccordResult.Size = new System.Drawing.Size(101, 19);
            this.lblAccordResult.TabIndex = 4;
            this.lblAccordResult.Text = "Accord.NET: -";
            // 
            // lblMyResult
            // 
            this.lblMyResult.AutoSize = true;
            this.lblMyResult.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMyResult.Location = new System.Drawing.Point(20, 265);
            this.lblMyResult.Name = "lblMyResult";
            this.lblMyResult.Size = new System.Drawing.Size(113, 19);
            this.lblMyResult.TabIndex = 3;
            this.lblMyResult.Text = "Собственная: -";
            // 
            // lblTestSymbol
            // 
            this.lblTestSymbol.AutoSize = true;
            this.lblTestSymbol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTestSymbol.Location = new System.Drawing.Point(20, 235);
            this.lblTestSymbol.Name = "lblTestSymbol";
            this.lblTestSymbol.Size = new System.Drawing.Size(113, 15);
            this.lblTestSymbol.TabIndex = 2;
            this.lblTestSymbol.Text = "Тестовый символ:";
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.SteelBlue;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Location = new System.Drawing.Point(240, 25);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(200, 40);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "Тестировать сети";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numEpochs);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.progressBar);
            this.groupBox2.Controls.Add(this.btnTrain);
            this.groupBox2.Location = new System.Drawing.Point(12, 368);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 200);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Обучение нейронных сетей";
            // 
            // numEpochs
            // 
            this.numEpochs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numEpochs.Location = new System.Drawing.Point(240, 90);
            this.numEpochs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numEpochs.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numEpochs.Name = "numEpochs";
            this.numEpochs.Size = new System.Drawing.Size(120, 23);
            this.numEpochs.TabIndex = 3;
            this.numEpochs.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(20, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Количество эпох обучения:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 140);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(410, 23);
            this.progressBar.TabIndex = 1;
            // 
            // btnTrain
            // 
            this.btnTrain.BackColor = System.Drawing.Color.SeaGreen;
            this.btnTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrain.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTrain.ForeColor = System.Drawing.Color.White;
            this.btnTrain.Location = new System.Drawing.Point(20, 25);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(410, 50);
            this.btnTrain.TabIndex = 0;
            this.btnTrain.Text = "Обучить нейронные сети";
            this.btnTrain.UseVisualStyleBackColor = false;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbCameras);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.btnStartCamera);
            this.groupBox4.Location = new System.Drawing.Point(489, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(450, 160);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Управление камерой";
            // 
            // cmbCameras
            // 
            this.cmbCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameras.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCameras.FormattingEnabled = true;
            this.cmbCameras.Location = new System.Drawing.Point(20, 55);
            this.cmbCameras.Name = "cmbCameras";
            this.cmbCameras.Size = new System.Drawing.Size(410, 23);
            this.cmbCameras.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(20, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Выберите камеру:";
            // 
            // btnStartCamera
            // 
            this.btnStartCamera.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnStartCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartCamera.ForeColor = System.Drawing.Color.White;
            this.btnStartCamera.Location = new System.Drawing.Point(20, 100);
            this.btnStartCamera.Name = "btnStartCamera";
            this.btnStartCamera.Size = new System.Drawing.Size(410, 40);
            this.btnStartCamera.TabIndex = 2;
            this.btnStartCamera.Text = "Запустить камеру";
            this.btnStartCamera.UseVisualStyleBackColor = false;
            this.btnStartCamera.Click += new System.EventHandler(this.btnStartCamera_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.pictureBoxWebcam);
            this.groupBox5.Location = new System.Drawing.Point(489, 181);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(450, 350);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Изображение с веб-камеры";
            // 
            // pictureBoxWebcam
            // 
            this.pictureBoxWebcam.BackColor = System.Drawing.Color.Black;
            this.pictureBoxWebcam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxWebcam.Location = new System.Drawing.Point(20, 30);
            this.pictureBoxWebcam.Name = "pictureBoxWebcam";
            this.pictureBoxWebcam.Size = new System.Drawing.Size(410, 300);
            this.pictureBoxWebcam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxWebcam.TabIndex = 0;
            this.pictureBoxWebcam.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lblCaptureAccordResult);
            this.groupBox6.Controls.Add(this.lblCaptureMyResult);
            this.groupBox6.Controls.Add(this.pictureBoxProcessed);
            this.groupBox6.Controls.Add(this.btnCapture);
            this.groupBox6.Location = new System.Drawing.Point(951, 15);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(500, 516);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Обработанное изображение и результат";
            // 
            // lblCaptureAccordResult
            // 
            this.lblCaptureAccordResult.AutoSize = true;
            this.lblCaptureAccordResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCaptureAccordResult.Location = new System.Drawing.Point(25, 340);
            this.lblCaptureAccordResult.Name = "lblCaptureAccordResult";
            this.lblCaptureAccordResult.Size = new System.Drawing.Size(112, 21);
            this.lblCaptureAccordResult.TabIndex = 3;
            this.lblCaptureAccordResult.Text = "Accord.NET: -";
            // 
            // lblCaptureMyResult
            // 
            this.lblCaptureMyResult.AutoSize = true;
            this.lblCaptureMyResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCaptureMyResult.Location = new System.Drawing.Point(25, 300);
            this.lblCaptureMyResult.Name = "lblCaptureMyResult";
            this.lblCaptureMyResult.Size = new System.Drawing.Size(125, 21);
            this.lblCaptureMyResult.TabIndex = 2;
            this.lblCaptureMyResult.Text = "Собственная: -";
            // 
            // pictureBoxProcessed
            // 
            this.pictureBoxProcessed.BackColor = System.Drawing.Color.White;
            this.pictureBoxProcessed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxProcessed.Location = new System.Drawing.Point(25, 120);
            this.pictureBoxProcessed.Name = "pictureBoxProcessed";
            this.pictureBoxProcessed.Size = new System.Drawing.Size(450, 160);
            this.pictureBoxProcessed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxProcessed.TabIndex = 1;
            this.pictureBoxProcessed.TabStop = false;
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.Crimson;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCapture.ForeColor = System.Drawing.Color.White;
            this.btnCapture.Location = new System.Drawing.Point(25, 40);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(450, 50);
            this.btnCapture.TabIndex = 0;
            this.btnCapture.Text = "Распознать с камеры";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Green;
            this.lblStatus.Location = new System.Drawing.Point(12, 571);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 19);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Готово";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1509, 735);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Распознавание символов медиа-плеера";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTest)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEpochs)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWebcam)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBoxTest;
        private System.Windows.Forms.Label lblAccordResult;
        private System.Windows.Forms.Label lblMyResult;
        private System.Windows.Forms.Label lblTestSymbol;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numEpochs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cmbCameras;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStartCamera;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox pictureBoxWebcam;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lblCaptureAccordResult;
        private System.Windows.Forms.Label lblCaptureMyResult;
        private System.Windows.Forms.PictureBox pictureBoxProcessed;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Label lblStatus;
    }
}