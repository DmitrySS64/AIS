using AIS_Client.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AIS_Client.View
{
    /// <summary>
    /// Логика взаимодействия для EditEntityWindow.xaml
    /// </summary>
    public partial class EditEntityWindow : Window
    {
        // Поле для хранения данных записи
        public Dictionary<string, object> EditedData => DynamicForm.FormData;

        public EditEntityWindow(DataRowView entityData, FormDescription formDescription)
        {
            InitializeComponent();
            DynamicForm.BuildForm(formDescription);
            InitializeFields(entityData);
        }

        private void InitializeFields(DataRowView entityData)
        {
            // Пройдём по всем полям в DataRowView и инициализируем UI элементы
            foreach (DataColumn column in entityData.Row.Table.Columns)
            {
                string fieldName = column.ColumnName; // Имя поля
                var fieldValue = entityData[fieldName]; // Значение поля

                // Применяем значение поля к соответствующему элементу UI
                foreach (UIElement element in DynamicForm.FormPanel.Children)
                {
                    if (element is TextBox textBox && (string)textBox.Tag == fieldName)
                    {
                        textBox.Text = fieldValue?.ToString() ?? "";
                    }
                    else if (element is CheckBox checkBox && (string)checkBox.Tag == fieldName)
                    {
                        checkBox.IsChecked = fieldValue as bool?;
                    }
                    // Добавьте другие типы элементов по необходимости
                }
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Получаем данные из CustomControl
            DialogResult = true; // Закрываем окно и возвращаем результат
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
