using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

var client = new TelegramBotClient("6624351365:AAHu-M4QNLFBJYA4y71uJVXd5iU1xsd2ZVY");
client.StartReceiving(Update,Error);

async static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
{
    Console.WriteLine(exception.Message);
}

async static Task Update(ITelegramBotClient client, Update update, CancellationToken token)
{
    var message = update.Message;
    //лалалала
    switch (update.Type)
    {
        case UpdateType.Message:
            switch (update.Message.Type) 
            {
                case MessageType.Text:
                    Console.WriteLine($"{message.Chat.Username} {update.Message.Text}");
                    if (update.Message.Text == "/start")
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "новичок, да? я могу добавлять, удалять и изменять уведомления. " +
                            "Чтобы выбрать одну из функций нужно вызвать меню командой /menu");
                        Console.WriteLine($"id чата: {update.Message.Chat.Id}");
                        string solutionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        string fileName = $"{update.Message.Chat.Id}.txt";
                        string filePath = Path.Combine(solutionPath, fileName);
                        System.IO.File.Create(filePath);
                    }
                    if (update.Message.Text == "/leavechat" || update.Message.Text == "/kickme")
                    {
                        await client.LeaveChatAsync(update.Message.Chat.Id);
                        Console.WriteLine($"Бот покинул чат: {update.Message.Chat.Id}");
                        string solutionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        string fileName = $"{update.Message.Chat.Id}.txt";
                        string filePath = Path.Combine(solutionPath, fileName);
                        System.IO.File.Delete(filePath);
                    }
                    if (update.Message.Text == "/menu")
                    {
                        // Тут создаем нашу клавиатуру
                        var inlineKeyboard = new InlineKeyboardMarkup(
                            new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                            {
                                        // Каждый новый массив - это дополнительные строки,
                                        // а каждая дополнительная строка (кнопка) в массиве - это добавление ряда

                                        new InlineKeyboardButton[] // тут создаем массив кнопок
                                        {
                                            InlineKeyboardButton.WithCallbackData("Добавить уведомление", "add_not"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Изменить уведомление", "mod_not"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Удалить уведомление", "del_not"),
                                        },
                            });

                        await client.SendTextMessageAsync(
                            update.Message.Chat.Id,
                            "Чтобы добавить уведомление надо:\nнаписать пару токенов,\nнаписать цену,\nнаписать биржу." +
                            "\nЧтобы изменить уведомление надо:\nвыбрать уведомление из списка добавленных уведомлений,\nнаписать цену,\nнаписать биржу."+
                            "\nЧтобы изменить удалить уведомление надо:\nвыбрать уведомление из списка добавленных уведомлений"
                            ,
                            replyMarkup: inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup
                    }
                    if (update.Message.Text == "Здарова")
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "здарова");
                    }
                    break;
                case MessageType.Photo:
                    var fileId = update.Message.Photo.Last().FileId;
                    var fileInfo = await client.GetFileAsync(fileId);
                    var filepath = fileInfo.FilePath;

                    const string destinationFilePath = "D:\\C#\\telegrambot\\telegrambot\\bin\\Debug\\net7.0\\buzo.jpg";

                    Stream fileStream = System.IO.File.Create(destinationFilePath);
                    await client.DownloadFileAsync(
                        filePath: filepath,
                        destination: fileStream,
                        cancellationToken: token);

                    break;
            }
            break;
        case UpdateType.CallbackQuery:
            // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
            var callbackQuery = update.CallbackQuery;

            // Аналогично и с Message мы можем получить информацию о чате, о пользователе и т.д.
            var user = callbackQuery.From;

            // Выводим на экран нажатие кнопки
            //Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

            // Вот тут нужно уже быть немножко внимательным и не путаться!
            // Мы пишем не callbackQuery.Chat , а callbackQuery.Message.Chat , так как
            // кнопка привязана к сообщению, то мы берем информацию от сообщения.
            var chat = callbackQuery.Message.Chat;

            // Добавляем блок switch для проверки кнопок
            switch (callbackQuery.Data)
            {
                // Data - это придуманный нами id кнопки, мы его указывали в параметре
                // callbackData при создании кнопок. У меня это button1, button2 и button3

                case "add_not":
                    {
                        // В этом типе клавиатуры обязательно нужно использовать следующий метод
                        await client.AnswerCallbackQueryAsync(callbackQuery.Id);
                        // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку

                        await client.SendTextMessageAsync(
                            chat.Id,
                            $"Вы нажали на {callbackQuery.Data}");
                        break;
                    }

                case "mod_not":
                    {
                        // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                        await client.AnswerCallbackQueryAsync(callbackQuery.Id/*, "Тут может быть ваш текст!"*/);

                        await client.SendTextMessageAsync(
                            chat.Id,
                            $"Вы нажали на {callbackQuery.Data}");
                        break;
                    }

                case "del_not":
                    {
                        // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                        await client.AnswerCallbackQueryAsync(callbackQuery.Id, "А это полноэкранный текст!", showAlert: true);

                        await client.SendTextMessageAsync(
                            chat.Id,
                            $"Вы нажали на {callbackQuery.Data}");
                        break;
                    }
            }
            break;
    }
}

//классы и списки


//методы
static void AddNot(string crypto, string price)
{

}
Console.ReadLine();