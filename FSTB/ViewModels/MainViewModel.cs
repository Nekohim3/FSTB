using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using FSTB.Model;
using FSTB.Utils;
using Newtonsoft.Json;
using ReactiveUI;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FSTB.ViewModels;

public class MainViewModel : ViewModelBase
{
    private DateTime _startTime;
    private int _lastMsgId;
    private Thread _botThread;
    private Thread _botAlertThread;
    private bool _statusOnVisible;
    private bool _statusOffVisible;
    private bool _botWork;

    public ReactiveCommand<Unit, Unit> StartBotCmd { get; }

    public ReactiveCommand<Unit, Unit> StopBotCmd { get; }

    public ReactiveCommand<Unit, Unit> Test1Cmd { get; }

    public bool StatusOnVisible
    {
        get => _statusOnVisible;
        set => this.RaiseAndSetIfChanged(ref _statusOnVisible, value, nameof(StatusOnVisible));
    }

    public bool StatusOffVisible
    {
        get => _statusOffVisible;
        set => this.RaiseAndSetIfChanged(ref _statusOffVisible, value, nameof(StatusOffVisible));
    }

    public bool BotWork
    {
        get => _botWork;
        set
        {
            this.RaiseAndSetIfChanged(ref _botWork, value, nameof(BotWork));
            StatusOnVisible = value;
            StatusOffVisible = !value;
        }
    }

    public List<ChatWithUser> ChatList { get; set; } = new();

    public MainViewModel()
    {
        StatusOffVisible = true;
        StatusOnVisible = false;
        StartBotCmd = ReactiveCommand.Create(OnStartBot);
        StopBotCmd = ReactiveCommand.Create(OnStopBot);
        Test1Cmd = ReactiveCommand.Create(OnTest1);
        OnStartBot();
    }

    private void Load()
    {
        if (!File.Exists("data"))
            return;
        try
        {
            ChatList = JsonConvert.DeserializeObject<List<ChatWithUser>>(File.ReadAllText("data")) ?? new List<ChatWithUser>();
        }
        catch (Exception ex)
        {
        }
    }

    private void Save() => File.WriteAllText("data", JsonConvert.SerializeObject(ChatList));

    private async void BotUpdatesThreadFunc()
    {
        _startTime = DateTime.Now;
        while (BotWork)
        {
            try
            {
                var updates = await g.TLC.GetUpdatesAsync(_lastMsgId + 1);
                if (updates.Any())
                {
                    var updateArray = updates;
                    foreach (var x in updateArray)
                    {
                        _lastMsgId = x.Id;
                        if (x.Message?.From != null)
                        {
                            if (x.Message.Date.AddHours(3.5) > _startTime)
                            {
                                var          x1   = x;
                                var chat = ChatList.FirstOrDefault(_ => _.UserId == x1.Message.From.Id);
                                if (chat == null)
                                {
                                    chat = new ChatWithUser(x.Message.From);
                                    ChatList.Add(chat);
                                }
                                var lower = x.Message.Text.ToLower();
                                if ((lower == "/bot" ? 0 : (!(lower == "/start") ? 1 : 0)) == 0)
                                {
                                    try
                                    {
                                        var chatWithUser = chat;
                                        var tlc = g.TLC;
                                        var chat1 = chat.Chat;
                                        IReplyMarkup mainMenu = InlineService.GetMainMenu();
                                        var messageThreadId = new int?();
                                        var parseMode = new ParseMode?();
                                        var disableWebPagePreview = new bool?();
                                        var disableNotification = new bool?();
                                        var protectContent = new bool?();
                                        var replyToMessageId = new int?();
                                        var allowSendingWithoutReply = new bool?();
                                        var replyMarkup = mainMenu;
                                        var cancellationToken = new CancellationToken();
                                        var message = await tlc.SendTextMessageAsync(chat1, "Меню", messageThreadId, parseMode, disableWebPagePreview: disableWebPagePreview, disableNotification: disableNotification, protectContent: protectContent, replyToMessageId: replyToMessageId, allowSendingWithoutReply: allowSendingWithoutReply, replyMarkup: replyMarkup, cancellationToken: cancellationToken);
                                        chatWithUser.LastMessageId = message.MessageId;
                                        chatWithUser               = null;
                                        message                    = null;
                                    }
                                    catch (Exception ex)
                                    {
                                        //DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
                                        //interpolatedStringHandler.AppendLiteral("==========\n");
                                        //interpolatedStringHandler.AppendFormatted(ex.Message);
                                        //interpolatedStringHandler.AppendLiteral("\n\n\n");
                                        //interpolatedStringHandler.AppendFormatted(ex.StackTrace);
                                        //interpolatedStringHandler.AppendLiteral("\n=======\n");
                                        //await System.IO.File.AppendAllTextAsync("log", interpolatedStringHandler.ToStringAndClear());
                                    }
                                }
                                chat = null;
                            }
                            else
                                continue;
                        }
                        if (x.CallbackQuery?.From != null)
                        {
                            if (!(x.CallbackQuery.Message.Date.AddHours(3.5) < _startTime))
                            {
                                var chat = ChatList.FirstOrDefault(_ => _.UserId == x.CallbackQuery.From.Id);
                                if (chat == null)
                                {
                                    chat = new ChatWithUser(x.CallbackQuery.From);
                                    ChatList.Add(chat);
                                }
                                if (chat.LastMessageId == 0)
                                {
                                    var chatWithUser = chat;
                                    var tlc = g.TLC;
                                    var chat2 = chat.Chat;
                                    IReplyMarkup mainMenu = InlineService.GetMainMenu();
                                    var messageThreadId = new int?();
                                    var parseMode = new ParseMode?();
                                    var disableWebPagePreview = new bool?();
                                    var disableNotification = new bool?();
                                    var protectContent = new bool?();
                                    var replyToMessageId = new int?();
                                    var allowSendingWithoutReply = new bool?();
                                    var replyMarkup = mainMenu;
                                    var cancellationToken = new CancellationToken();
                                    var message = await tlc.SendTextMessageAsync(chat2, "Меню", messageThreadId, parseMode, disableWebPagePreview: disableWebPagePreview, disableNotification: disableNotification, protectContent: protectContent, replyToMessageId: replyToMessageId, allowSendingWithoutReply: allowSendingWithoutReply, replyMarkup: replyMarkup, cancellationToken: cancellationToken);
                                    chatWithUser.LastMessageId = message.MessageId;
                                    chatWithUser               = null;
                                    message                    = null;
                                }
                                else if (chat.LastMessageId != x.CallbackQuery.Message.MessageId)
                                {
                                    var chatWithUser = chat;
                                    var tlc = g.TLC;
                                    var chat3 = chat.Chat;
                                    IReplyMarkup mainMenu = InlineService.GetMainMenu();
                                    var messageThreadId = new int?();
                                    var parseMode = new ParseMode?();
                                    var disableWebPagePreview = new bool?();
                                    var disableNotification = new bool?();
                                    var protectContent = new bool?();
                                    var replyToMessageId = new int?();
                                    var allowSendingWithoutReply = new bool?();
                                    var replyMarkup = mainMenu;
                                    var cancellationToken = new CancellationToken();
                                    var message = await tlc.SendTextMessageAsync(chat3, "Меню", messageThreadId, parseMode, disableWebPagePreview: disableWebPagePreview, disableNotification: disableNotification, protectContent: protectContent, replyToMessageId: replyToMessageId, allowSendingWithoutReply: allowSendingWithoutReply, replyMarkup: replyMarkup, cancellationToken: cancellationToken);
                                    chatWithUser.LastMessageId = message.MessageId;
                                    chatWithUser               = null;
                                    message                    = null;
                                }
                                else if (x.CallbackQuery.Data == "1")
                                {
                                    var rm = InlineService.GetTickets();
                                    if (rm.InlineKeyboard.Count() == 1)
                                    {
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Не удалось загрузить расписание с сайта. Попробуйте еще разВот расписание");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, rm);
                                    }
                                    else
                                    {
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Вот расписание");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, rm);
                                    }
                                    rm = null;
                                }
                                else if (x.CallbackQuery.Data == "2")
                                {
                                    await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Активные оповещения");
                                    await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetAlerts(chat));
                                }
                                else if (!(x.CallbackQuery.Data == "3"))
                                {
                                    if (x.CallbackQuery.Data == "1-1")
                                    {
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Меню");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetMainMenu());
                                    }
                                    else if (x.CallbackQuery.Data == "2-2")
                                    {
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Выберите день");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetDaysForNewAlert());
                                    }
                                    else if (x.CallbackQuery.Data == "2-3")
                                    {
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Выберите день для удаления оповещения");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetAlertsForDelete(chat));
                                    }
                                    else if (x.CallbackQuery.Data == "2-4")
                                    {
                                        chat.AlarmDays.Clear();
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Активные оповещения");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetAlerts(chat));
                                    }
                                    else if (x.CallbackQuery.Data == "2-5")
                                    {
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Меню");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetMainMenu());
                                    }
                                    else if (x.CallbackQuery.Data == "2-2-1")
                                    {
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Активные оповещения");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetAlerts(chat));
                                    }
                                    else if (x.CallbackQuery.Data.StartsWith("2-2-Date-"))
                                    {
                                        var   dateStr = x.CallbackQuery.Data.Replace("2-2-Date-", "");
                                        var date    = DateTime.Parse(dateStr);
                                        if (chat.AlarmDays.All(_ => _ != date))
                                            chat.AlarmDays.Add(date);
                                        chat.AlarmDays = chat.AlarmDays.OrderBy(_ => _.Date).ToList();
                                        Save();
                                        await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Активные оповещения");
                                        await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetAlerts(chat));
                                        dateStr = null;
                                    }
                                    else if (x.CallbackQuery.Data.StartsWith("2-3-"))
                                    {
                                        var str = x.CallbackQuery.Data.Replace("2-3-", "");
                                        if (str == "999")
                                        {
                                            await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Активные оповещения");
                                            await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetAlerts(chat));
                                        }
                                        else
                                        {
                                            chat.AlarmDays.RemoveAt(int.Parse(str));
                                            Save();
                                            await g.TLC.EditMessageTextAsync(chat.Chat, chat.LastMessageId, "Активные оповещения");
                                            await g.TLC.EditMessageReplyMarkupAsync(chat.Chat, chat.LastMessageId, InlineService.GetAlerts(chat));
                                        }
                                        str = null;
                                    }
                                }
                                chat = null;
                            }
                            else
                                continue;
                        }
                    }
                    updateArray = null;
                }
                updates = null;
            }
            catch (Exception ex)
            {
                //DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
                //interpolatedStringHandler.AppendLiteral("==========\n");
                //interpolatedStringHandler.AppendFormatted(ex.Message);
                //interpolatedStringHandler.AppendLiteral("\n\n\n");
                //interpolatedStringHandler.AppendFormatted(ex.StackTrace);
                //interpolatedStringHandler.AppendLiteral("\n=======\n");
                //await System.IO.File.AppendAllTextAsync("log", interpolatedStringHandler.ToStringAndClear());
            }
            Thread.Sleep(100);
        }
    }

    private async void BotAlertThreadFunc()
    {
        while (BotWork)
        {
            var lst = FsService.GetTickets();
            if (lst != null)
            {
                var                  lastData   = lst.Select(_ => _.Date.Date).Distinct().Last();
                var alertUsers = ChatList.Where(_ => _.AlarmDays.Contains(lastData));
                foreach (var x in alertUsers)
                {
                    x.AlarmDays.Remove(lastData);
                    Save();
                    await g.TLC.SendTextMessageAsync(x.Chat, "=======================================\nВ продаже появились билеты на " + lastData.ToShortDateString() + "\n=======================================");
                    x.LastMessageId = 0;
                }
                alertUsers = null;
            }
            Thread.Sleep(60000);
            lst = null;
        }
    }

    private void OnStartBot()
    {
        if (BotWork)
            return;
        Load();
        BotWork = true;
        _botThread = new Thread(BotUpdatesThreadFunc);
        _botThread.Start();
        _botAlertThread = new Thread(BotAlertThreadFunc);
        _botAlertThread.Start();
    }

    private void OnStopBot() => BotWork = false;

    private void OnTest1()
    {
    }
}
