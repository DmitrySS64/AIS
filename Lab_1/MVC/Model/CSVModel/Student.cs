using ArchitectureOfInformationSystems.MVC.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.Model.Entity
{
    public class Student
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        private int _age { get; set; }
        public int Age {
            get
            {
                return _age;
            }
            set
            {
                if (value < 0 || value > 120) {
                    _age = 0;
                }
                else
                {
                    _age = value;
                }
            }
        }
        public bool IsStudent { get; set; }


        public Student()
        {
            Name = string.Empty;
            LastName = string.Empty;
            Age = 0;
            IsStudent = false;
        }

        public Student(string name, string lastName, int age, bool isStudent)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            IsStudent = isStudent;
        }

        public static List<Student> AddRandomEntries(int count)
        {
            Random random = new Random();
            List<Student> randomStudents = new List<Student>();

            // Генерируем 10 случайных студентов
            for (int i = 0; i < count; i++)
            {
                string[] names = { "Дмитрий", "Петр", "Иван", "Сергей", "Мария", "Алексей", "Екатерина", "Анна", "Роман" };
                string[] lastNames = { "Сергеев", "Петров", "Иванов", "Матлахов", "Кузнецов", "Михайлов", "Сидоров", "Попов", "Ковалев", "Дмитриев" };

                string name = names[random.Next(names.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                int age = random.Next(14, 90); // Возраст от 14 до 90
                bool isStudent = random.Next(0, 2) == 0; // Случайное значение для IsStudent (True/False)

                randomStudents.Add(new Student(name, lastName, age, isStudent));
            }
            return randomStudents;
        }
    }
}
