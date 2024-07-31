using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Reflection;
using System.Diagnostics;


namespace ConsoleApp_Level1
{
    class Program
    {
        class Person
        {
            public string fio { get; set; }
            //public string dateBirth { get; set; }
            public byte day { get; set; }
            public byte month { get; set; }
            public ushort year { get; set; }
            public int dayToBirth { get; set; }
            public Person() { }
            public Person(string _fio, byte _day, byte _month, ushort _year)
            {
                fio = _fio;
                day = _day;
                month = _month;
                year = _year;

            }
        }

        static void printMenu(string[] items, int row, int col, int index, int indexFirst) // Печать меню
        {
            Console.SetCursorPosition(col, row);
            Console.WriteLine("\n\n                               )\\\r\n                              (__)");
            Console.WriteLine("\r                               /\\\r\n                              [[]]\r\n                           @@@[[]]@@@\r\n                     @@@@@@@@@[[]]@@@@@@@@@\r\n                 @@@@@@@      [[]]      @@@@@@@\r\n             @@@@@@@@@        [[]]        @@@@@@@@@\r\n            @@@@@@@           [[]]           @@@@@@@\r\n            !@@@@@@@@@                    @@@@@@@@@!\r\n            !    @@@@@@@                @@@@@@@    !\r\n            !        @@@@@@@@@@@@@@@@@@@@@@        !\r\n            !              @@@@@@@@@@@             !\r\n            !             ______________           !\r");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n            !         ПОЗДРАВЛЯТОР version1.0      !\n");
            Console.ResetColor();
            Console.WriteLine("\r            !             --------------           !\r\n            !!!!!!!                          !!!!!!!\r\n                 !!!!!!!                !!!!!!!\r\n                     !!!!!!!!!!!!!!!!!!!!!!!\r\n");
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            if (index == 0 && indexFirst == 0)
            {
                items[0] = "\t\t        Показать весь список    ->";
            }
            if (index == 0 && indexFirst == 1)
            {
                Console.CursorLeft = 0;
                items[0] = "\t\t<- Показать список ближайших ДР ->";
            }
            if (index == 0 && indexFirst == 2)
            {
                items[0] = "\t\t<- Показать список прошедших ДР ->";

            }
            for (int i = 0; i < items.Length; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(items[i]);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        static void createBinaryFile(string path, List<Person> persons) // Создание | Чтение файла
        {
            Console.WriteLine("\n Введите имя файла: " + path);
            if (File.Exists(path))
            {
                Console.WriteLine("\n Файл с таким именем найден, " +
                    "попытка чтения данных " + path + "\n");
                persons.Clear();
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    //пока не конец
                    while (reader.PeekChar() > -1)
                    {
                        string fio = reader.ReadString();
                        byte day = reader.ReadByte();
                        byte month = reader.ReadByte();
                        ushort year = reader.ReadUInt16();
                        //по считываемым данным добавляем персону в список
                        persons.Add(new Person(fio, day, month, year));
                    }
                }
            }
            else
            {
                Console.WriteLine("Файл с таким именем не найден. Создание нового файла.\n");
                persons.Clear();
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    Console.WriteLine("Новый файл " + path + " был создан успешно.");
                }
            }
            Console.WriteLine("Выход в главное меню.");
            Console.ReadKey();
            Console.Clear();
        }

        static void SaveBinaryFile(string path, List<Person> persons) // Сохранение файла
        {
            if (persons.Count == 0)
            {
                Console.WriteLine("В списке Поздравлятора нет записей :(");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            Console.WriteLine("\nПопытка сохранения данных в файл " + path);
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            {
                foreach (Person person in persons)
                {
                    writer.Write(person.fio);
                    writer.Write(person.day);
                    writer.Write(person.month);
                    writer.Write(person.year);
                }

            }
            Console.WriteLine("\nДанные были успешно сохранены.");
            Console.WriteLine("Выход в главное меню.");
            Console.ReadKey();
            Console.Clear();
        }

        static void addPerson(List<Person> persons) // Добавление новой записи
                                                    // TODO: чтобы добавлялся в едином формате - чтобы при удалении и редактировании при поиске не возникали сложности
        {
            try
            {
                int dayToBirth;
                Person temp = new Person();
                Console.WriteLine("\nДобавление новой записи");
                Console.WriteLine("\nВведите ФИО человека -> ");
                temp.fio = Console.ReadLine();
                if (temp.fio == "") throw new Exception("ФИО не может быть пустым");
                Console.WriteLine("\nВведите день рождения человека (число) -> ");
                temp.day = byte.Parse(Console.ReadLine());
                if (temp.day == null || temp.day > 31) throw new Exception();
                Console.WriteLine("\nВведите месяц рождения человека (число) -> ");
                temp.month = byte.Parse(Console.ReadLine());
                if (temp.month == null || temp.month > 12) throw new Exception();
                Console.WriteLine("\nВведите год рождения человека (число) -> ");
                temp.year = ushort.Parse(Console.ReadLine());
                if (temp.year == null || temp.year > DateTime.Now.Year) throw new Exception();
                DateTime TodayDate = DateTime.Now;
                foreach (Person person in persons)
                {
                    if (temp.fio == person.fio && temp.day == person.day && temp.month == person.month && temp.year == person.year)
                    {
                        Console.WriteLine("Возникла ошибка. Запись с такими данными уже есть в базе. Возврат в главное меню. Нажмите любую клавишу.");
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }
                }
                persons.Add(temp);
                Console.WriteLine("Запись добавлена успешно. Возврат в главное меню. Нажмите любую клавишу.");
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine("Возникла ошибка ввода информации, возврат в главное меню");
                Console.ReadKey();
                Console.Clear();
                return;
            }
        }

        static void deletePerson(List<Person> persons) // Удаление записи
        {
            if (persons.Count == 0)
            {
                Console.WriteLine("В списке Поздравлятора нет записей :(");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            Console.WriteLine("Введите ФИО записи, которую вы хотите удалить.");
            string inputFio = Console.ReadLine();
            Console.Write("Идёт поиск записи ");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".\n");
            foreach (Person person in persons)
            {
                if (inputFio == person.fio)
                {
                    Console.WriteLine($"Запись с ФИО: {inputFio} найдена, ее дата рождения: {person.day}.{person.month}.{person.year}.");
                    Console.WriteLine("!Подтвердите операцию!");
                    Console.WriteLine("'Y' - УДАЛИТЬ \n 'N' - СОХРАНИТЬ");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Y:
                            persons.Remove(person);
                            Console.WriteLine("Данные записи были успешно удалены. Возврат в главное меню. Нажмите любую клавишу.");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                            break;
                        case ConsoleKey.N:
                            Console.WriteLine("Данные о записи были сохранены. Возврат в главное меню. Нажмите любую клавишу.");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                            break;
                        default:
                            Console.WriteLine("Ошибка ввода. Возврат в главное меню.");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                            break;
                    }
                }
            }
            Console.WriteLine($"Запись с ФИО = {inputFio} не была найдена. Возврат в главное меню. Нажмите любую клавишу.");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        static void editPerson(List<Person> persons)
        {
            if (persons.Count == 0)
            {
                Console.WriteLine("В списке Поздравлятора нет записей :(");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            Console.WriteLine("Введите ФИО записи, которую вы хотите изменить.");
            string inputFio = Console.ReadLine();
            Console.Write("Идёт поиск записи ");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".");
            Thread.Sleep(333); Console.Write(".\n");
            string tempFio;
            byte tempDay, tempMonth;
            ushort tempYear;

            foreach (Person person in persons)
            {
                if (inputFio == person.fio)
                {
                    Console.WriteLine($"Запись с ФИО: {inputFio} найдена, ее дата рождения: {person.day}.{person.month}.{person.year}.");
                    Console.WriteLine("Введите новые данные");
                    try
                    {
                        int dayToBirth;
                        Console.WriteLine("\nДобавление новой записи");
                        Console.WriteLine("\nВведите ФИО человека -> ");
                        tempFio = Console.ReadLine();
                        if (tempFio == null || tempFio == "") throw new Exception("ФИО не может быть пустым");
                        Console.WriteLine("\nВведите день рождения человека (число) -> ");
                        tempDay = byte.Parse(Console.ReadLine());
                        if (tempDay == null || tempDay > 31) throw new Exception();
                        Console.WriteLine("\nВведите месяц рождения человека (число) -> ");
                        tempMonth = byte.Parse(Console.ReadLine());
                        if (tempMonth == null || tempMonth > 12) throw new Exception();
                        Console.WriteLine("\nВведите год рождения человека (число) -> ");
                        tempYear = ushort.Parse(Console.ReadLine());
                        if (tempYear == null || tempYear > DateTime.Now.Year) throw new Exception();
                        DateTime TodayDate = DateTime.Now;
                        foreach (Person person1 in persons)
                        {
                            if (tempFio == person1.fio && tempDay == person1.day && tempMonth == person1.month && tempYear == person1.year)
                            {
                                Console.WriteLine("Возникла ошибка. Запись с такими данными уже есть в базе. Возврат в главное меню. Нажмите любую клавишу.");
                                Console.ReadKey();
                                Console.Clear();
                                return;
                            }
                        }
                        Console.WriteLine("Запись изменена успешно. Возврат в главное меню. Нажмите любую клавишу.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Возникла ошибка ввода информации, возврат в главное меню");
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }
                    Console.WriteLine("!Подтвердите операцию!");
                    Console.WriteLine("'Y' - ИЗМЕНИТЬ \n 'N' - ОТМЕНИТЬ");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Y:
                            person.fio = tempFio;
                            person.day = tempDay;
                            person.month = tempMonth;
                            person.year = tempYear;
                            Console.WriteLine("Данные записи были успешно изменены. Возврат в главное меню. Нажмите любую клавишу.");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                            break;
                        case ConsoleKey.N:
                            Console.WriteLine("Данные о записи были сохранены. Возврат в главное меню. Нажмите любую клавишу.");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                            break;
                        default:
                            Console.WriteLine("Ошибка ввода. Возврат в главное меню. Нажмите любую клавишу.");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                            break;
                    }
                }
            }
            Console.WriteLine($"Запись с ФИО = {inputFio} не была найдена. Возврат в главное меню. Нажмите любую клавишу.");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        static void showAll(List<Person> persons, int countDayFrom, int countDayTo, int indexMenu) // печать списка
        {
            if (persons.Count == 0)
            {
                Console.WriteLine("В списке Поздравлятора нет записей :(");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            int currentPage = 0; // Текущая страница
            const int pageSize = 10; // Количество записей на странице
            bool exitMode = false; // Флаг выхода из режима просмотра

            while (!exitMode)
            {
                Console.Clear(); // Очищаем консоль перед выводом новой страницы
                if (indexMenu != 2)
                {
                    Console.WriteLine("\n-------------------------------------------------------------------" +
                              "\r\n|     Фамилия Имя Отчество     | Дата рождения | Лет | Через Дней |");
                    Console.WriteLine("-------------------------------------------------------------------");
                } // Выводим заголовок таблицы
                else
                {
                    Console.WriteLine("\n-------------------------------------------------------------------" +
                              "\r\n|     Фамилия Имя Отчество     | Дата рождения | Лет | Дней назад |");
                    Console.WriteLine("-------------------------------------------------------------------");
                }
                int startIndex = currentPage * pageSize; // Начальный индекс для текущей страницы
                int endIndex = Math.Min(startIndex + pageSize, persons.Count); // Конечный индекс для текущей страницы
                for (int i = startIndex; i < endIndex; i++)
                {
                    DisplayPerson(persons[i], countDayFrom, countDayTo); // Вывод информации о человеке
                }

                PrintFooter(currentPage, persons.Count / pageSize); // Выводим информацию о навигации

                var key = Console.ReadKey(true).Key; // Читаем нажатую клавишу
                switch (key)
                {
                    case ConsoleKey.LeftArrow: // Листаем назад
                        if (currentPage > 0)
                            currentPage--;
                        break;
                    case ConsoleKey.RightArrow: // Листаем вперед
                        if ((currentPage + 1) * pageSize < persons.Count)
                            currentPage++;
                        break;
                    case ConsoleKey.S: // сортировка по кол-ву дней до ДР
                        persons.Sort((p1, p2) => p1.dayToBirth.CompareTo(p2.dayToBirth));
                        //Console.Clear();
                        currentPage--;
                        currentPage++;
                        break;
                    case ConsoleKey.N:
                        persons.Sort((p1, p2) => p1.fio.CompareTo(p2.fio));
                        Console.Clear();
                        currentPage--;
                        currentPage++;
                        break;
                    case ConsoleKey.Escape: // Выходим из режима просмотра
                        exitMode = true;
                        Console.Clear();
                        break;
                }
            }
        }

        static void DisplayPerson(Person person, int countDayFrom, int countDayTo)
        {
            DateTime todayDate = DateTime.Now;
            DateTime birthDateThisYear = new DateTime(todayDate.Year, person.month, person.day);
            DateTime futureBirthday;

            // Определяем, когда будет следующий день рождения
            if (todayDate > birthDateThisYear)
                futureBirthday = new DateTime(todayDate.Year + 1, person.month, person.day);
            else
                futureBirthday = birthDateThisYear;

            int daysDifference = (futureBirthday - todayDate).Days + 1;
            person.dayToBirth = daysDifference;
            if (daysDifference >= 358 && daysDifference < 365) { person.dayToBirth -= 365; }
            if (daysDifference == 365) { daysDifference = 0; person.dayToBirth = 0; }
            int age = todayDate.Year - person.year;

            // Проверяем, попадает ли день рождения в заданный диапазон
            if ((daysDifference >= countDayFrom && daysDifference <= countDayTo) ||
                (daysDifference < 0 && daysDifference >= -7))
            {
                string dateOfBirth = $"{person.day}.{person.month}.{person.year}";
                Console.Write($"|{person.fio,-30}|{dateOfBirth,15}|{age,5}|");

                // Изменяем цвет фона и текста в зависимости от количества дней до дня рождения
                if (daysDifference < 3)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (daysDifference >= 3 && daysDifference < 7)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (daysDifference >= 7 && daysDifference <= 14)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                // Изменение для дней рождения от вчера до недели назад
                else if (daysDifference >= 358 && daysDifference < 365)
                {
                    daysDifference = daysDifference * (-1) + 365; // Делаем количество дней положительным для вывода
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write($"{daysDifference,12}|\n");
                Console.ResetColor(); // Сбрасываем цвета к стандартным
                Console.WriteLine("-------------------------------------------------------------------");
            }
        }



        static void PrintFooter(int currentPage, int totalPages)
        {
            Console.WriteLine($"\nСтраница {currentPage + 1} из {totalPages}");
            Console.WriteLine("[<-] - Назад    [ESC] - Выход     [->] - Вперед");
            Console.WriteLine("[S] - Сортировка по дням до ДР   [N]- Сортировка по алфавиту.");
        }

        static void Main(string[] args) // Точка входа
        {
            Console.Title = "Поздравлятор";
            Console.SetWindowSize(70, 50);
            Console.SetWindowPosition(0, 0);
            int row = 0;
            int col = 0;
            string[] menuItems = new string[] {
                "\t\t        Показать весь список  ->   ",
                "\t\t         Добавление записи         ",
                "\t\t          Удаление записи          ",
                "\t\t       Редактирование записи       ",
                "\t\t     Создать или загрузить файл    ",
                "\t\t      Сохранить данные в файл      ",
                "\t\t      [ESC] Выход из программы     "};
            string path; // путь файла
            List<Person> persons = new List<Person>();
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            int index = 0;
            int indexFirst = 0;
            while (true)
            {
                printMenu(menuItems, row, col, index, indexFirst);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (index < menuItems.Length - 1)
                        {
                            index++;
                            Console.Beep(100, 100);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                        {
                            index--;
                            Console.Beep(100, 100);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (indexFirst < 2)
                        {
                            indexFirst++;
                            Console.Beep(100, 100);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (indexFirst > 0)
                        {
                            indexFirst--;
                            Console.Beep(100, 100);
                        }
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.WriteLine("Спасибо за запуск моего конмольного приложения <<Поздравлятор>>");
                        Console.WriteLine("Если Вы найдете ошибку или баг - ruslan.pivnik@yandex.ru");
                        Console.WriteLine("Нажмите [Enter], чтобы перейти на github разработчика");
                        Console.WriteLine("Нажмите [Escape], чтобы закрыть консольное приложение");
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                Process.Start("explorer", "https://github.com/double-3-bitter-lemon/Pozdravlyator-Test-Case-Level-01");
                                Console.Clear();
                                break;
                            case ConsoleKey.Escape:
                                Console.WriteLine("Хорошего времяпровождения!");
                                Thread.Sleep(333);
                                Console.Write(".");
                                Thread.Sleep(333);
                                Console.Write(".");
                                Thread.Sleep(333);
                                Console.Write(".");
                                Thread.Sleep(333);
                                Console.Write(".");
                                Thread.Sleep(333);
                                Console.Write(".");
                                return;
                            default:
                                Console.WriteLine("Ошибка ввода. Возврат в главное меню. Нажмите любую клавишу.");
                                Thread.Sleep(333);
                                Console.Write(".");
                                Thread.Sleep(333);
                                Console.Write(".");
                                Thread.Sleep(333);
                                Console.Write(".");
                                Console.ReadKey();
                                Console.Clear();
                                break;
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Beep(100, 100);
                        switch (index) // меню действий
                        {
                            case 0: // Показать весь список | Показать ближайшие (неделя) | Показать прошедшие (неделя)
                                if (indexFirst == 0)
                                {
                                    showAll(persons, 0, 365, 0);
                                }
                                if (indexFirst == 1)
                                {
                                    showAll(persons, 0, 14, 1);
                                }
                                if (indexFirst == 2)
                                {
                                    showAll(persons, 358, 365, 2);
                                }
                                break;
                            case 1:  // Add
                                addPerson(persons);
                                break;
                            case 2: // Delete
                                deletePerson(persons);
                                break;
                            case 3: // Edit
                                editPerson(persons);
                                break;
                            case 4: // Create or Read
                                Console.WriteLine("\nВведите имя файла (новое для создания или существующее для загрузки данных)\n");
                                path = Console.ReadLine();
                                path = path + ".dat";
                                createBinaryFile(path, persons);
                                break;
                            case 5: // Save
                                Console.WriteLine("\nВведите имя файла для сохранения\n");
                                path = Console.ReadLine();
                                path = path + ".dat";
                                SaveBinaryFile(path, persons);
                                break;
                            case 6:
                                Console.WriteLine("\nКонец работы программы");
                                return;
                        }
                        break;
                }
            }
        }
    }
}