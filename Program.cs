using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;


namespace ConsoleApp_Level1
{
    class Program
    {
        class Person
        {
            public string fio { get; set; }
            public string dateBirth { get; set; }
            public byte day { get; set; }
            public byte month { get; set; }
            public ushort year { get; set; }
            public Person() { }
            public Person(string _fio, byte _day, byte _month, ushort _year)
            {
                fio = _fio;
                day = _day;
                month = _month;
                year = _year;

            }
        }

        static void printMenu(string[] items, int row, int col, int index, int indexFirst)
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
            if(index==0 && indexFirst == 1)
            {
                Console.CursorLeft = 0;
                items[0] = "\t\t<- Показать список ближайших ДР ->";
            }
            if (index == 0 && indexFirst == 2)
            {
                items[0] = "\t\t<- Показать список прошедших ДР ->";
                
            }
            for(int i=0; i < items.Length; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor= ConsoleColor.Black;
                }
                Console.WriteLine(items[i]);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        static void createBinaryFile(string path, List<Person> persons) // Создание | Чтение файла
        {
            Console.WriteLine("\nName of file: " + path);
            if (File.Exists(path))
            {
                Console.WriteLine("\n File exists, " +
                    "Open file " + path + "\n");
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
                Console.WriteLine("File doesn't exist. Create new file.\n");
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    Console.WriteLine("New file was created successfully\n");
                }
            }
            Console.ReadKey();
            Console.Clear();
        }

        static void SaveBinaryFile(string path, List<Person> persons) // Сохранение файла
        {
            Console.WriteLine("\nCheck existing of file" + path);
            if (File.Exists(path))
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    foreach (Person person in persons)
                    {
                        writer.Write(person.fio);
                        writer.Write(person.dateBirth);
                    }

                }
                Console.WriteLine("\nFile exist. Save file success.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void addPerson(List<Person> persons) // Добавление новой записи
        {
            try
            {
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
                persons.Add(temp);
                Console.WriteLine("Запись добавлена успешно. Возврат влавное меню.");
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

        static void showAll(List<Person> persons) // Печать всех записей
        {
            if (persons.Count == 0)
            {
                Console.WriteLine("В списке Поздравлятора нет записей :(");
                Console.ReadKey();
                Console.Clear();   
                return;
            }
            Console.Clear();
            Console.WriteLine("\t\t Список всех записей");
            DateTime futurDate;
            ushort futurAge;
            ushort futurYear;
            DateTime TodayDate = DateTime.Now;
            int dayToBirth;
            Console.WriteLine();
            foreach (Person person in persons)
            {
                futurAge = Convert.ToUInt16(TodayDate.Year - person.year);
                if (person.month < TodayDate.Month && person.day < TodayDate.Day)
                {
                    futurYear = Convert.ToUInt16(TodayDate.Year);
                    futurYear += 1;
                    futurDate = new DateTime(futurYear, person.month, person.day);
                    dayToBirth = (futurDate - TodayDate).Days;
                }
                else
                {
                    futurYear = Convert.ToUInt16(TodayDate.Year);
                    futurDate = new DateTime(futurYear, person.month, person.day);
                    dayToBirth = (futurDate - TodayDate).Days;
                }
                Console.WriteLine("\n" + person.fio + " " + person.day + "." + person.month + "." + person.year + " - Исполнится " + futurAge + " через " + dayToBirth + " дней");
            }
        Console.ReadKey();
        Console.Clear();
        }

        static void Main(string[] args) // Точка входа
        {
            Console.Title = "Поздравлятор";
            Console.SetWindowSize(65, 50);
            int row = 0;
            int col = 0;
            string[] menuItems = new string[] { 
                "\t\t        Показать весь список  ->   ",
                "\t\t         Добавление записи         ",
                "\t\t          Удаление записи          ",
                "\t\t       Редактирование записи       ",
                "\t\t     Создать или загрузить файл    ",
                "\t\t      Сохранить данные в файл      ",
                "\t\t         Выход из программы        "};
            string path; // путь файла
            List<Person> persons = new List<Person>();
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            int index = 0;
            int indexFirst = 0;
            while (true)
            {
                //Console.Clear();
                printMenu(menuItems, row, col, index,indexFirst);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (index < menuItems.Length - 1)
                        {
                            index++;
                            Console.Beep(100,100);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (index > 0) 
                        {
                            index--;
                            Console.Beep(100,100);
                        }
                break;
                    case ConsoleKey.RightArrow:
                        if (indexFirst < 2) 
                        {
                            indexFirst++;
                            Console.Beep(100,100);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (indexFirst > 0 )
                        {
                            indexFirst--;
                            Console.Beep(100, 100);
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Beep(100,100);
                        switch (index) // меню действий
                        {
                            case 0: // Показать весь список | Показать ближайшие (неделя) | Показать прошедшие (неделя)
                                showAll(persons);
                                break;
                            case 1:  // Add
                                addPerson(persons);
                                break;
                            case 2: // Delete

                                break;
                            case 3: // Edit

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