using System;
using System.Collections.Generic;
using F1DriverSystem.Core;
using Ninject;

namespace F1DriverSystem.ConsoleApp
{
    /// <summary>
    /// Консольное приложение системы учёта гонщиков Ф1.
    /// Объект бизнес-логики создаётся через IoC-контейнер Ninject,
    /// который автоматически внедряет репозиторий в объект BL.
    /// </summary>
    internal class Program
    {
        private static readonly IKernel ninjectKernel =
            new StandardKernel(new SimpleConfigModule());

        private static readonly Logic logic = ninjectKernel.Get<Logic>();

        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("СИСТЕМА УЧЁТА ГОНЩИКОВ Ф1");
                Console.WriteLine("Сезон 2026: данные после Гран-при Азербайджана (26.09.2026)");
                Console.WriteLine("1. Показать всех гонщиков");
                Console.WriteLine("2. Добавить гонщика");
                Console.WriteLine("3. Удалить гонщика по ID");
                Console.WriteLine("4. Изменить данные гонщика");
                Console.WriteLine("5. Найти гонщиков по команде");
                Console.WriteLine("6. Рассчитать среднее количество очков");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();
                if (choice == "0") return;
                Console.Clear();
                switch (choice)
                {
                    case "1": PrintDrivers(logic.GetAllDrivers()); break;
                    case "2": AddDriver(); break;
                    case "3": DeleteDriver(); break;
                    case "4": EditDriver(); break;
                    case "5": FilterDrivers(); break;
                    case "6": Console.WriteLine("Среднее количество очков: " + logic.CalculateAveragePoints().ToString("F2")); break;
                    default: Console.WriteLine("Неизвестная команда."); break;
                }
                Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Выводит список гонщиков на консоль.
        /// </summary>
        /// <param name="drivers">Список гонщиков.</param>
        private static void PrintDrivers(List<Driver> drivers)
        {
            if (drivers.Count == 0)
            {
                Console.WriteLine("Гонщики не найдены.");
                return;
            }
            foreach (Driver driver in drivers)
                Console.WriteLine("ID: " + driver.Id + " | " + driver.Name + " | Команда: " + driver.Team + " | Очки: " + driver.Points);
        }

        /// <summary>
        /// Считывает обязательное строковое значение.
        /// </summary>
        /// <param name="prompt">Текст приглашения.</param>
        /// <returns>Непустая строка.</returns>
        private static string ReadRequired(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
                Console.WriteLine("Поле не должно быть пустым.");
            }
        }

        /// <summary>
        /// Считывает неотрицательное количество очков.
        /// </summary>
        /// <returns>Целое число, не меньше нуля.</returns>
        private static int ReadPoints()
        {
            while (true)
            {
                Console.Write("Очки (целое неотрицательное число): ");
                int points;
                if (int.TryParse(Console.ReadLine(), out points) && points >= 0) return points;
                Console.WriteLine("Введите 0 или больше.");
            }
        }

        /// <summary>
        /// Добавляет гонщика на основе введённых данных.
        /// </summary>
        private static void AddDriver()
        {
            string name = ReadRequired("Имя гонщика: ");
            string team = ReadRequired("Команда: ");
            int points = ReadPoints();
            Console.WriteLine(logic.AddDriver(name, team, points) ? "Гонщик добавлен." : "Ошибка добавления.");
        }

        /// <summary>
        /// Удаляет гонщика по введённому идентификатору.
        /// </summary>
        private static void DeleteDriver()
        {
            PrintDrivers(logic.GetAllDrivers());
            Console.Write("ID для удаления: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id)) { Console.WriteLine("Неверный ID."); return; }
            Console.WriteLine(logic.DeleteDriver(id) ? "Гонщик удалён." : "Гонщик не найден.");
        }

        /// <summary>
        /// Изменяет данные существующего гонщика.
        /// </summary>
        private static void EditDriver()
        {
            PrintDrivers(logic.GetAllDrivers());
            Console.Write("ID для изменения: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id) || logic.FindById(id) == null)
            { Console.WriteLine("Гонщик не найден."); return; }
            string name = ReadRequired("Новое имя: ");
            string team = ReadRequired("Новая команда: ");
            int points = ReadPoints();
            Console.WriteLine(logic.UpdateDriver(id, name, team, points) ? "Данные обновлены." : "Ошибка обновления.");
        }

        /// <summary>
        /// Запрашивает название команды и выводит её гонщиков.
        /// </summary>
        private static void FilterDrivers()
        {
            string team = ReadRequired("Название команды: ");
            PrintDrivers(logic.GetDriversByTeam(team));
        }
    }
}
