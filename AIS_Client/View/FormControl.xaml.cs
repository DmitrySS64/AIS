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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIS_Client.View
{
    /// <summary>
    /// Логика взаимодействия для FormControl.xaml
    /// </summary>
    public partial class FormControl : UserControl
    {

        private Dictionary<string, object> _formData = new Dictionary<string, object>();

        public FormControl()
        {
            InitializeComponent();
        }

        public Dictionary<string, object> FormData => _formData;

        public void BuildForm(FormDescription form)
        {
            FormPanel.Children.Clear();
            _formData.Clear();
            foreach (var field in form.Fields)
            {
                TextBlock label = new TextBlock { Text = field.Label, Margin = new Thickness(0, 10, 0, 5) };
                FormPanel.Children.Add(label);

                UIElement control = CreateFormControl(field);
                FormPanel.Children.Add(control);
            }
        }

        private UIElement CreateFormControl(FormField field)
        {
            UIElement control = null;

            switch (field.Type.ToLower())
            {
                case "text":
                    TextBox textBox = new TextBox { Tag = field.Name }; // Привязка имени поля
                    textBox.TextChanged += (s, e) => _formData[field.Name] = textBox.Text;
                    control = textBox;
                    break;

                case "number":
                    TextBox numberBox = new TextBox { Tag = field.Name };
                    numberBox.TextChanged += (s, e) =>
                    {
                        if (int.TryParse(numberBox.Text, out int value))
                            _formData[field.Name] = value;
                    };
                    control = numberBox;
                    break;

                case "checkbox":
                    CheckBox checkBox = new CheckBox { Tag = field.Name };
                    checkBox.Checked += (s, e) => _formData[field.Name] = checkBox.IsChecked;
                    checkBox.Unchecked += (s, e) => _formData[field.Name] = checkBox.IsChecked;
                    control = checkBox;
                    break;

                default:
                    TextBlock unsupported = new TextBlock { Text = "Unsupported field type", Foreground = Brushes.Red };
                    control = unsupported;
                    break;
            }

            return control;
        }

        public void InitializeFields(DataRowView entityData)
        {
            foreach (var field in entityData.Row.Table.Columns)
            {
                string fieldName = field.ToString(); // Имя поля в DataRowView

                if (_formData.ContainsKey(fieldName))
                {
                    _formData[fieldName] = entityData[fieldName];

                    // Применяем значение к соответствующему элементу UI
                    foreach (UIElement element in FormPanel.Children)
                    {
                        if (element is TextBox textBox && (string)textBox.Tag == fieldName)
                        {
                            textBox.Text = entityData[fieldName]?.ToString() ?? "";
                        }
                        else if (element is CheckBox checkBox && (string)checkBox.Tag == fieldName)
                        {
                            checkBox.IsChecked = entityData[fieldName] as bool?;
                        }
                    }
                }
            }
        }
        public void InitializeFields(Dictionary<string, object> initialValues)
        {
            foreach (var field in initialValues)
            {
                if (_formData.ContainsKey(field.Key))
                {
                    _formData[field.Key] = field.Value;

                    // Применяем значение к соответствующему элементу UI
                    foreach (UIElement element in FormPanel.Children)
                    {
                        if (element is TextBox textBox && (string)textBox.Tag == field.Key)
                        {
                            textBox.Text = field.Value?.ToString() ?? "";
                        }
                        else if (element is CheckBox checkBox && (string)checkBox.Tag == field.Key)
                        {
                            checkBox.IsChecked = field.Value as bool?;
                        }
                    }
                }
            }
        }
    }
}
