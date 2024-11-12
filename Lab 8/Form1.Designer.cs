namespace Lab_8
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButtonToCelsius = new System.Windows.Forms.RadioButton();
            this.radioButtonToFahrenheit = new System.Windows.Forms.RadioButton();
            this.buttonTempConvert = new System.Windows.Forms.Button();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonToCelsius
            // 
            this.radioButtonToCelsius.AutoSize = true;
            this.radioButtonToCelsius.Location = new System.Drawing.Point(10, 30);
            this.radioButtonToCelsius.Name = "radioButtonToCelsius";
            this.radioButtonToCelsius.Size = new System.Drawing.Size(94, 20);
            this.radioButtonToCelsius.TabIndex = 0;
            this.radioButtonToCelsius.TabStop = true;
            this.radioButtonToCelsius.Text = "В градусы";
            this.radioButtonToCelsius.UseVisualStyleBackColor = true;
            this.radioButtonToCelsius.CheckedChanged += new System.EventHandler(this.radioButtonToCelsius_CheckedChanged);
            // 
            // radioButtonToFahrenheit
            // 
            this.radioButtonToFahrenheit.AutoSize = true;
            this.radioButtonToFahrenheit.Location = new System.Drawing.Point(10, 60);
            this.radioButtonToFahrenheit.Name = "radioButtonToFahrenheit";
            this.radioButtonToFahrenheit.Size = new System.Drawing.Size(121, 20);
            this.radioButtonToFahrenheit.TabIndex = 1;
            this.radioButtonToFahrenheit.TabStop = true;
            this.radioButtonToFahrenheit.Text = "В фаренгейты";
            this.radioButtonToFahrenheit.UseVisualStyleBackColor = true;
            this.radioButtonToFahrenheit.CheckedChanged += new System.EventHandler(this.radioButtonToFahrenheit_CheckedChanged);
            // 
            // buttonTempConvert
            // 
            this.buttonTempConvert.AutoSize = true;
            this.buttonTempConvert.Location = new System.Drawing.Point(203, 149);
            this.buttonTempConvert.Name = "buttonTempConvert";
            this.buttonTempConvert.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTempConvert.Size = new System.Drawing.Size(135, 30);
            this.buttonTempConvert.TabIndex = 2;
            this.buttonTempConvert.Text = "Конвертировать!";
            this.buttonTempConvert.UseVisualStyleBackColor = true;
            this.buttonTempConvert.Click += new System.EventHandler(this.buttonTempConvert_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(40, 50);
            this.textBoxInput.MaxLength = 10;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(137, 22);
            this.textBoxInput.TabIndex = 3;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(203, 50);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.Size = new System.Drawing.Size(200, 57);
            this.textBoxOutput.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Ввод";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Вывод";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.radioButtonToCelsius);
            this.groupBox1.Controls.Add(this.radioButtonToFahrenheit);
            this.groupBox1.Location = new System.Drawing.Point(40, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(137, 101);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Конвертация ";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(203, 113);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Size = new System.Drawing.Size(200, 30);
            this.button1.TabIndex = 8;
            this.button1.Text = "Конвертировать локально!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(427, 208);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonTempConvert);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(50, 30, 50, 30);
            this.Text = "Конвертер температуры";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonToCelsius;
        private System.Windows.Forms.RadioButton radioButtonToFahrenheit;
        private System.Windows.Forms.Button buttonTempConvert;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
    }
}

