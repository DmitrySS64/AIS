using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vkontakte.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private VkApi api;
        private string accessToken;
        private string userId;

        public HomePage()
        {
            InitializeComponent();
            accessToken = Properties.Settings.Default.AccessToken;
            userId = Properties.Settings.Default.UserId;
            api = new VkApi(accessToken);
        }
        private async void GetProfileInfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для получения информации о текущем пользователе
            console.Text = "Запрос информации о себе...\n";
            // TODO: Добавьте вызов метода API для получения информации о профиле
            if (string.IsNullOrEmpty(accessToken)) { MessageBox.Show($"Вы не авторизованы!"); return; }
            string response = await api.GetProfileInfoAsync();
            var profileInfo = JsonConvert.DeserializeObject<Methods.Account.GetProfileInfoResponse>(response)._profileInfo;

            DisplayProperties(profileInfo);
        }
        private async void GetFriendsButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для получения списка друзей
            console.Text += "Запрос списка друзей...\n";
            // TODO: Добавьте вызов метода API для получения списка друзей
            if (string.IsNullOrEmpty(accessToken)) { MessageBox.Show($"Вы не авторизованы!"); return; }
            var response = await api.GetFriendsAsync();
            DisplayProperties(response);
        }

        private async void GetUserInfoButton_Click(object sender, RoutedEventArgs e)
        {
            string usersId = userInput.Text;
            console.Text = $"Запрос информации о пользователе с ID: {userId}\n";
            if (string.IsNullOrEmpty(accessToken)) { MessageBox.Show($"Вы не авторизованы!"); return; }
            int[] users;
            try
            {
                users = usersId.Split(',').Select(id => int.Parse(id.Trim())).ToArray();
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректный формат ID. Убедитесь, что вы ввели числа, разделённые запятыми.");
                return;
            }
            var response = await api.GetUsersAsync(users);
            DisplayProperties(response);
        }

        private void RemoveAccessToken_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void GetFriendsInfoButton_Click(object sender, RoutedEventArgs e)
        {
            console.Text = "Запрос списка друзей...\n";
            // TODO: Добавьте вызов метода API для получения списка друзей
            if (string.IsNullOrEmpty(accessToken)) { MessageBox.Show($"Вы не авторизованы!"); return; }
            var response = await api.GetFriendsAsync();
            console.Text += $"Найдено: {response.Count} друзей\n";
            int[] users = response.Items.ToArray();

            var response2 = await api.GetUsersAsync(users);
            DisplayProperties(response2);
        }


        private void DisplayProperties(object obj, int level = 0)
        {
            if (obj == null) return;

            string indent = new string('\t', level); // Табуляция для вложенности

            // Проверяем, является ли сам объект коллекцией
            if (obj is System.Collections.IEnumerable collection && !(obj is string))
            {
                var elementType = obj.GetType().IsGenericType
                    ? obj.GetType().GetGenericArguments()[0]
                    : obj.GetType().GetElementType();

                console.Text += $"{indent}Коллекция элементов ({elementType?.Name ?? "Неизвестный тип"}):\n";

                foreach (var item in collection)
                {
                    if (elementType != null && elementType.IsClass && elementType != typeof(string))
                    {
                        // Рекурсивный вызов для вложенных объектов в коллекции
                        DisplayProperties(item, level + 1);
                    }
                    else
                    {
                        // Вывод простых значений элементов коллекции
                        console.Text += $"{indent}\t- {item}\n";
                    }
                }
                return; // Выход, так как это была коллекция, и дальше обрабатывать свойства не нужно
            }

            // Если это не коллекция, то обрабатываем как обычный объект
            var properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    var value = property.GetValue(obj);

                    // Проверка на коллекцию в значении свойства
                    if (value is System.Collections.IEnumerable innerCollection && !(value is string))
                    {
                        console.Text += $"{indent}{property.Name} (коллекция):\n";

                        var innerElementType = value.GetType().IsGenericType
                            ? value.GetType().GetGenericArguments()[0]
                            : value.GetType().GetElementType();

                        if (innerElementType != null && innerElementType.IsClass && innerElementType != typeof(string))
                        {
                            foreach (var item in innerCollection)
                            {
                                DisplayProperties(item, level + 1);
                            }
                        }
                        else
                        {
                            foreach (var item in innerCollection)
                            {
                                console.Text += $"{indent}\t- {item}\n";
                            }
                        }
                    }
                    // Обработка строковых свойств, без попытки их интерпретировать как коллекции
                    else if (value is string stringValue)
                    {
                        console.Text += $"{indent}{property.Name}: {stringValue}\n";
                    }
                    else if (value != null && value.GetType().IsClass && !value.GetType().IsPrimitive)
                    {
                        // Если свойство - вложенный объект, рекурсивно углубляемся
                        console.Text += $"{indent}{property.Name}:\n";
                        DisplayProperties(value, level + 1);
                    }
                    else
                    {
                        // Выводим простое значение свойства
                        console.Text += $"{indent}{property.Name}: {value}\n";
                    }
                }
                catch (TargetParameterCountException)
                {
                    console.Text += $"{indent}{property.Name}: (не удалось получить значение из-за несоответствия параметров)\n";
                }
                catch (Exception ex)
                {
                    console.Text += $"{indent}{property.Name}: (ошибка - {ex.Message})\n";
                }
            }
        }

    }
}
