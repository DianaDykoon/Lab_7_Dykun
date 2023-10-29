using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Lab_7
{
    public class Program
    {
        static void Main()
        {
            // Ініціалізація списку об'єктів класу Account
            List<Account> accounts = new();

            Random random = new();
            int withdrawLimit = random.Next(500, 2000);
            Account.WithdrawLimit = withdrawLimit;

            bool restart = true;
            while (restart)
            {
                Console.WriteLine(" 1 - Додати об'єкт\n 2 - Вивести об'єкти на екран\n" +
                    " 3 - Знайти об'єкт\n 4 - Видалити об'єкт\n 5 - Демонстрацiя поведiнки об'єктiв\n" +
                    " 6 – Демонстрацiя роботи static методiв\n " +
                    " 0 - Вийти з програми");
                Console.Write("Виберiть пункт меню (0 - 6) --->");
                int menu = int.Parse(Console.ReadLine());

                switch (menu)
                {
                    case 0:
                        restart = false;
                        break;

                    case 1:
                        AddObject(ref accounts);
                        break;

                    case 2:
                        if (accounts.Count == 0)
                        {
                            Console.WriteLine("\nСписок порожнiй..\n");
                            break;
                        }
                        else
                        {
                            foreach (Account account in accounts)
                            {
                                Console.WriteLine(account.ToString() + "\n");
                            }
                        }

                        Console.WriteLine($"--- Лiмiт для зняття грошей для усiх рахункiв = {Account.WithdrawLimit}");
                        Console.WriteLine($"--- Всього об'єктiв класу Account - {accounts.Count}\n");
                        break;

                    case 3:
                        Console.Write("За якою характеристикою ви хочете знайти об'єкт? (1 - Iм'я користувача, 2 - Номер рахунку) --->");
                        int type = int.Parse(Console.ReadLine());

                        switch (type)
                        {
                            case 1:
                                Console.Write("Введiть iм'я користувача ---> ");
                                string? name = Console.ReadLine();
                                var findObjByName = FindObjectByName(accounts, name!);

                                if (findObjByName.Count == 0)
                                    Console.WriteLine("\nОб'єкт не знайдено");
                                else
                                {
                                    foreach (var account in findObjByName)
                                    {
                                        Console.WriteLine("\nЗнайдено наступний об'єкт:\n" + account.ToString());
                                    }
                                }
                                break;

                            case 2:
                                Console.Write("Введiть номер рахунку ---> ");
                                int accountNumber = int.Parse(Console.ReadLine());
                                var findObjById = FindObjectByAccountNumber(accounts, accountNumber);

                                if (findObjById.Count == 0)
                                    Console.WriteLine("\nОб'єкт не знайдено");
                                else
                                {
                                    foreach (var account in findObjById)
                                    {
                                        Console.WriteLine("\nЗнайдено наступний об'єкт:\n" + account.ToString());
                                    }
                                }
                                break;

                            default:
                                Console.WriteLine("Помилка вводу!");
                                break;
                        }
                        break;

                    case 4:
                        RemoveObject(ref accounts);
                        break;

                    case 5:
                        foreach (Account account in accounts)
                        {
                            int deposit = random.Next(0, 2000);
                            account.Deposit(deposit);
                            Console.WriteLine($"Баланс поповнено успiшно на {deposit} гривень");
                            account.TransactionCount++;

                            int withdraw = random.Next(0, 1000);
                            if (account.WithdrawMoney(withdraw))
                            {
                                Console.WriteLine($"Успiшно знято {withdraw} гривень");
                                account.TransactionCount++;
                            }
                            else
                                Console.WriteLine("Недостатньо грошей на балансi для зняття, або перевищено лiмiт!");

                            Console.WriteLine("\n\t---Демонстрацiя роботи перевантажених методiв---");
                            account.Deposit(deposit, 20);
                            Console.WriteLine($"Баланс поповнено успiшно на {deposit - 20} гривень, з урахуванням комiсiї");
                            account.TransactionCount++;

                            double depositOverload = random.NextDouble() * 1000.0;
                            account.Deposit(depositOverload);
                            Console.WriteLine($"Баланс поповнено успiшно на {depositOverload:F1} гривень\n");
                            account.TransactionCount++;
                        }
                        break;

                    case 6:
                        int age = random.Next(15, 25);
                        int income = random.Next(100, 1200);
                        int amount = random.Next(income) * 12;
                        if (Account.CreditAllowed(amount, income, age))
                            Console.WriteLine($"\nМожна оформити кредит на суму: {amount}, з доходом {income} та вiком {age}");
                        else
                            Console.WriteLine($"\nНеможливо оформити кредит на суму: {amount}, з доходом {income} та вiком {age}!");
                        break;

                    default:
                        Console.WriteLine("Такого пункту меню не iснує\n");
                        break;
                }
            }
        }

        // Метод додавання об'єкту у список
        static void AddObject(ref List<Account> accounts)
        {
            Console.Write("Як ви хочете ввести характеристики об'єкта?\n" +
                "1 - Вводити по черзi, 2 - Ввести рядок з характеристиками --->");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.Write("\nЗа допомогою якого конструктора ви хочете створити об'єкт?\n" +
                    "(1 - Конструктор без параметрiв, 2 - Конструктор з параметрами," +
                    "\n3 - Конструктор реалiзований через виклик iншого власного конструктора) --->");
                    int typeOfConstructor = int.Parse(Console.ReadLine());

                    AccountState state = new();
                    try
                    {
                        Console.WriteLine("Input state --->");
                        state = (AccountState)Enum.Parse(typeof(AccountState), Console.ReadLine(), true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}. \n Встановлено значення за замовчуванням - New");
                    }

                    Console.WriteLine("Input name --->");
                    string name = Console.ReadLine();
                    Console.WriteLine("Input address --->");
                    string address = Console.ReadLine();
                    Console.WriteLine("Input account number --->");
                    int accountNumber = int.Parse(Console.ReadLine());
                    Console.WriteLine("Input balance --->");
                    double balance = double.Parse(Console.ReadLine());
                    Console.WriteLine("Input overdraft --->");
                    short overdraft = short.Parse(Console.ReadLine());

                    switch (typeOfConstructor)
                    {
                        case 1:
                            Account account1 = new()
                            {
                                Name = name,
                                Address = address,
                                AccountNumber = accountNumber,
                                Balance = balance,
                                State = state,
                                Overdraft = overdraft
                            };
                            accounts.Add(account1);

                            Console.WriteLine("Об'єкт успiшно створено за допомогою конструктора без параметрiв.");
                            break;

                        case 2:
                            Account account2 = new(state, name, address, accountNumber, balance, overdraft);
                            accounts.Add(account2);
                            Console.WriteLine("Об'єкт успiшно створено за допомогою конструктора з параметрами.");
                            break;

                        case 3:
                            Account account3 = new(state, name, address, balance);
                            accounts.Add(account3);
                            Console.WriteLine("Об'єкт успiшно створено за допомогою конструктора, який реалiзован через виклик iншого.");
                            break;
                    }

                    break;

                case 2:
                    Console.WriteLine("\nВведiть характеристики об'єкта за прикладом:\n" +
                        "Стан рахунку, Iм'я користувача, Адреса, Номер рахунку, Баланс, Овердрафт");
                    string str = Console.ReadLine();

                    bool result = Account.TryParse(str, out Account account);
                    if (result)
                    {
                        accounts.Add(account);
                        Console.WriteLine("Об'єкт успiшно створено.");
                    }
                    break;

                default:
                    Console.WriteLine("Помилка вводу!");
                    break;
            }
        }

        // Метод пошуку об'єкта за ім'ям користувача
        public static List<Account> FindObjectByName(List<Account> accounts, string name)
        {
            List<Account> findObjByName = accounts.FindAll(findObj => findObj.Name == name);

            return findObjByName;
        }

        // Метод пошуку об'єкта за номером рахунку
        public static List<Account> FindObjectByAccountNumber(List<Account> accounts, int accountNumber)
        {
            List<Account> findObjById = accounts.FindAll(findObj => findObj.AccountNumber == accountNumber);

            return findObjById;
        }

        // Метод видалення об'єкту зі списку
        static void RemoveObject(ref List<Account> accounts)
        {
            Console.Write("Як ви хочете видалити об'єкт? (1 - За номером об'єкта, 2 - За iм'ям користувача) --->");
            int type = int.Parse(Console.ReadLine());
            switch (type)
            {
                case 1:
                    Console.Write("Введiть номер об'єкта для видалення: ");
                    int numOfObj = int.Parse(Console.ReadLine());

                    try
                    {
                        accounts.RemoveAt(numOfObj);
                        Console.WriteLine($"Об'єкт пiд номером {numOfObj} видалено успiшно");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Об'єкта пiд таким номером не iснує");
                    }

                    break;

                case 2:
                    Console.Write("Введiть iм'я користувача ---> ");
                    string name = Console.ReadLine();

                    if (!accounts.Exists(p => p.Name == name))
                    {
                        Console.WriteLine("Об'єкта з таким iм'ям не iснує");
                        break;
                    }

                    accounts.RemoveAll(deleteObjByName => deleteObjByName.Name == name);
                    Console.WriteLine($"Об'єкт з iм'ям {name} видалено успiшно");
                    break;

                default:
                    Console.WriteLine("Помилка вводу!");
                    break;
            }
        }

        // Метод збереження списку об'єктів у файл *csv
        static void SaveToFileCSV(List<Account> accounts, string path)
        {
            List<string> lines = new();
            foreach (var item in accounts)
            {
                lines.Add(item.ToString());
            }
            try
            {
                File.WriteAllLines(path, lines);
                Console.WriteLine($"Об'єкти збережно у: {Path.GetFullPath(path)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Метод зчитування списку з файлу *csv
        static List<Account> ReadFromFileCSV(string path)
        {
            List<Account> accounts = new();
            try
            {
                List<string> lines = new();
                lines = File.ReadAllLines(path).ToList();

                Console.WriteLine("\nВмiст файлу CSV:\n");
                foreach (var item in lines)
                {
                    Console.WriteLine(item);
                    bool result = Account.TryParse(item, out Account? account);
                    if (result)
                        accounts.Add(account);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Помилка при читаннi з CSV файлу: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return accounts;
        }

        // Метод збереження списку об'єктів у файл *json
        static void SaveToFileJson(List<Account> accounts, string path)
        {
            try
            {
                string jsonstring = "";
                jsonstring = JsonSerializer.Serialize(accounts);
                File.WriteAllText(path, jsonstring);
                Console.WriteLine($"Об'єкти збережено у: {Path.GetFullPath(path)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Метод зчитування списку з файлу *json
        static List<Account>? ReadFromFileJson(string path)
        {
            List<Account>? accounts = null;
            try
            {
                accounts = JsonSerializer.Deserialize<List<Account>>(File.ReadAllText(path));
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Помилка при читаннi з JSON файлу: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return accounts;
        }
    }
}
