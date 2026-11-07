using System;
using System.Text;
using F1DriverSystem.Core;
using F1DriverSystem.Models;

namespace F1DriverSystem.RacesConsoleApp
{
    internal static class Program
    {
        private static int Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                var logic = new RaceLogic();
                while (true)
                {
                    Console.WriteLine("\nКАТАЛОГ ГРАН-ПРИ");
                    Console.WriteLine("1. Все этапы\n2. Найти по ID\n3. Добавить\n4. Изменить\n5. Удалить\n0. Выход");
                    Console.Write("Выбор: ");
                    string choice = Console.ReadLine();
                    if (choice == null || choice == "0") return 0;
                    switch (choice)
                    {
                        case "1":
                            var races = logic.GetAllRaces();
                            if (races.Count == 0) Console.WriteLine("Каталог пуст.");
                            foreach (var race in races) Print(race);
                            break;
                        case "2":
                            Print(logic.FindById(ReadPositive("ID: ")));
                            break;
                        case "3":
                            Console.WriteLine(logic.AddRace(ReadRequired("Название: "),
                                ReadRequired("Страна: "), ReadPositive("Количество кругов: "))
                                ? "Этап добавлен." : "Некорректные данные.");
                            break;
                        case "4":
                            int id = ReadPositive("ID: ");
                            if (logic.FindById(id) == null)
                            {
                                Console.WriteLine("Этап не найден.");
                                break;
                            }
                            Console.WriteLine(logic.UpdateRace(id, ReadRequired("Название: "),
                                ReadRequired("Страна: "), ReadPositive("Количество кругов: "))
                                ? "Этап изменён." : "Некорректные данные.");
                            break;
                        case "5":
                            Console.WriteLine(logic.DeleteRace(ReadPositive("ID: "))
                                ? "Этап удалён." : "Этап не найден.");
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда.");
                            break;
                    }
                }
            }
            catch (OperationCanceledException) { return 0; }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Ошибка: " + ex.Message);
                Console.Error.WriteLine("Проверьте SQL Server LocalDB и строку F1Database в App.config.");
                return 1;
            }
        }

        private static void Print(Race race)
        {
            Console.WriteLine(race == null ? "Этап не найден." :
                $"ID: {race.Id} | {race.Name} | Страна: {race.Country} | Круги: {race.Laps}");
        }

        private static string ReadRequired(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = Console.ReadLine();
                if (value == null) throw new OperationCanceledException();
                if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
                Console.WriteLine("Поле не должно быть пустым.");
            }
        }

        private static int ReadPositive(string prompt)
        {
            while (true)
            {
                int value;
                if (int.TryParse(ReadRequired(prompt), out value) && value > 0) return value;
                Console.WriteLine("Введите целое число больше нуля.");
            }
        }
    }
}
