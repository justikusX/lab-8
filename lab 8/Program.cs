using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public struct Message
{
    public string SenderEmail;
    public string RecipientEmail;
    public DateTime SendDate;
    public TimeSpan SendTime;
    public double MessageSizeKB;
}

public class Program
{
    private static List<Message> messages = new List<Message>();

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Ввести данные о сообщении");
            Console.WriteLine("2. Вывести статистику всех сообщений");
            Console.WriteLine("3. Вывести отправителей за дату и интервал времени");
            Console.WriteLine("4. Выход");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    EnterMessageData();
                    break;
                case 2:
                    DisplayAllMessages();
                    break;
                case 3:
                    DisplaySendersByDateAndTime();                            
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    static void EnterMessageData()
    {
        Message message = new Message();

        Console.Write("Введите e-mail отправителя: ");
        message.SenderEmail = Console.ReadLine();

        Console.Write("Введите e-mail получателя: ");
        message.RecipientEmail = Console.ReadLine();

        Console.Write("Введите дату отправки (ДДММГГГГ): ");
        string dateString = Console.ReadLine();
        if (!DateTime.TryParseExact(dateString, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            Console.WriteLine("Некорректный формат даты.");
            return;
        }
        message.SendDate = date;

        Console.Write("Введите время отправки (ЧЧ:ММ:СС): ");
        string timeString = Console.ReadLine();
        if (!TimeSpan.TryParseExact(timeString, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out TimeSpan time))
        {
            Console.WriteLine("Некорректный формат времени.");
            return;
        }

        message.SendTime = time;


        Console.Write("Введите размер сообщения (Кб): ");
        if (!double.TryParse(Console.ReadLine(), out double size))
        {
            Console.WriteLine("Некорректный формат размера сообщения.");
            return;
        }
        message.MessageSizeKB = size;

        messages.Add(message);
        Console.WriteLine("Сообщение добавлено.");

    }

    static void DisplayAllMessages()
    {
        if (messages.Count == 0)
        {
            Console.WriteLine("Нет данных о сообщениях.");
            return;
        }

        Console.WriteLine("\nСтатистика сообщений:");
        foreach (var message in messages)
        {
            int daysAgo = (DateTime.Now - (message.SendDate + message.SendTime)).Days;
            Console.WriteLine($"Отправитель: {message.SenderEmail}, Получатель: {message.RecipientEmail}, " +
                              $"Дата: {message.SendDate.ToString("dd.MM.yyyy")}, Время: {message.SendTime:hh\\:mm\\:ss}, " +
                              $"Размер: {message.MessageSizeKB} Кб, Давность: {daysAgo} дней");
        }
    }

    static void DisplaySendersByDateAndTime()
    {
        Console.Write("Введите дату (ДДММГГГГ): ");
        string dateString = Console.ReadLine();
        if (!DateTime.TryParseExact(dateString, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            Console.WriteLine("Некорректный формат даты.");
            return;
        }

        Console.Write("Введите начальное время (ЧЧ:ММ:СС): ");
        string startTimeString = Console.ReadLine();
        if (!TimeSpan.TryParseExact(startTimeString, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out TimeSpan startTime))
        {
            Console.WriteLine("Некорректный формат времени.");
            return;
        }

        Console.Write("Введите конечное время (ЧЧ:ММ:СС): ");
        string endTimeString = Console.ReadLine();
        if (!TimeSpan.TryParseExact(endTimeString, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out TimeSpan endTime))
        {
            Console.WriteLine("Некорректный формат времени.");
            return;
        }
        if (startTime >= endTime)
        {
            Console.WriteLine("Неверный интервал времени.");
            return;
        }

        var filteredMessages = messages
            .Where(m => m.SendDate == date && m.SendTime >= startTime && m.SendTime <= endTime)
            .ToList();

        if (filteredMessages.Count == 0)
        {
            Console.WriteLine("Нет сообщений за указанную дату и время.");
            return;
        }

        Console.WriteLine("\nОтправители за указанную дату и интервал времени:");
        foreach (var message in filteredMessages)
        {
            Console.WriteLine($"Отправитель: {message.SenderEmail}, Время: {message.SendTime:hh\\:mm\\:ss}");
        }
    }
}
//DisplaySendersByDateAndTime Метод:
//Запрашивает дату и интервал времени.
//Фильтрует список сообщений по введенной дате и интервалу времени с помощью LINQ запроса и метода Where.
//Выводит информацию об отправителях сообщений, соответствующих критериям.
//Выводит сообщение, если сообщений за указанную дату и время не найдeно

//DisplayAllMessages Метод:
//Проходит по списку messages и выводит информацию о каждом сообщении.
//Вычисляет давность отправки сообщения (количество дней, прошедших с момента отправки до текущей даты)

//EnterMessageData Метод:
//Запрашивает у пользователя ввод данных о сообщении.
//Использует DateTime.TryParseExact и TimeSpan.TryParseExact с нужными форматами (ddMMyyyy и hh\\:mm\\:ss) для проверки корректности введенных данных.
//Создает объект Message и добавляет его в список messages.

//Объект ((CultureInfo)) определяет функционал, который зависит от культурного контекста, например, форматирование дат, времени, чисел, валюты, работа с календарем.
//При запуске приложения каждый поток в .NET определяет два объекта: CultureInfo.CurrentCulture - текущую языковую культуру и CultureInfo.CurrentUICulture - языковая культура для пользовательского интерфейса.
//ASP.NET Core использует эти свойства для рендеринга значений, которые зависят от настройки культуры. Например, в зависимости от культуры может меняться отображение даты и времени.

//CultureInfo.InvariantCulture Свойство используется,
//если вы форматируете или анализируете строку,
//которая должна анализироваться программным обеспечением независимо от локальных настроек пользователя.

//Объект TimeSpan представляет интервал времени (длительность времени или истекшее время),
//измеряемый как положительное или отрицательное количество дней, часов, минут, секунд и долей секунды.