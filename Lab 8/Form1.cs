using Lab_8.MyTempConvertServiceReference;
using Lab_8.TempConvertServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBoxInput.KeyPress += textBoxInput_KeyPress;
        }

        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры, запятую/точку и клавишу Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.' && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void radioButtonToFahrenheit_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonToFahrenheit.Checked)
            {
                label1.Text = "Введите °C"; // Ввод в Цельсиях
                label2.Text = "Результат °F"; // Вывод в Фаренгейтах
            }
        }

        private void radioButtonToCelsius_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonToCelsius.Checked)
            {
                label1.Text = "Введите °F"; // Ввод в Фаренгейтах
                label2.Text = "Результат °C"; // Вывод в Цельсиях
            }
        }

        private void buttonTempConvert_Click(object sender, EventArgs e)
        {
            string value = Convert.ToString(textBoxInput.Text);
            string result = string.Empty;
            try
            {
                if (radioButtonToFahrenheit.Checked)
                {
                    result = soapClient.CelsiusToFahrenheit(value);
                }
                else if (radioButtonToCelsius.Checked)
                {
                    result = soapClient.FahrenheitToCelsius(value);
                }
            }
            catch (Exception ex) { 
                result = "Ошибка сервера: " + ex.Message;
            }

            textBoxOutput.Text = result;
        }

        public TempConvertSoapClient soapClient = new TempConvertSoapClient("TempConvertSoap");
        public TempConvertServiceSoapClient mySoapClient = new TempConvertServiceSoapClient("TempConvertServiceSoap");
        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxInput.Text = "0";
            radioButtonToCelsius.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string value = Convert.ToString(textBoxInput.Text);
            string result = string.Empty;
            try
            {
                if (radioButtonToFahrenheit.Checked)
                {
                    result = mySoapClient.CelsiusToFahrenheit(value);
                }
                else if (radioButtonToCelsius.Checked)
                {
                    result = mySoapClient.FahrenheitToCelsius(value);
                }
            }
            catch (Exception ex)
            {
                result = "Ошибка сервера: " + ex.Message;
            }
            textBoxOutput.Text = result;
        }
    }
}
