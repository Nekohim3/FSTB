using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FSTB.Model;
using Telegram.Bot.Types.ReplyMarkups;

namespace FSTB.Utils
{
    public static class InlineService
    {
        public static string GetDayOfWeek(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "ВС";
                case DayOfWeek.Monday:
                    return "ПН";
                case DayOfWeek.Tuesday:
                    return "ВТ";
                case DayOfWeek.Wednesday:
                    return "СР";
                case DayOfWeek.Thursday:
                    return "ЧТ";
                case DayOfWeek.Friday:
                    return "ПТ";
                case DayOfWeek.Saturday:
                    return "СБ";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetDayWithDOW(DateTime date)
        {
            var interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
            interpolatedStringHandler.AppendFormatted(date.Day);
            interpolatedStringHandler.AppendLiteral(" [");
            interpolatedStringHandler.AppendFormatted(GetDayOfWeek(date));
            interpolatedStringHandler.AppendLiteral("]");
            return interpolatedStringHandler.ToStringAndClear();
        }

        public static InlineKeyboardMarkup GetMainMenu() => new(new List<List<InlineKeyboardButton>>
                                                                {
                                                                    new()
                                                                    {
                                                                        InlineKeyboardButton.WithCallbackData("Вывести инфу о доступных билетах", "1")
                                                                    },
                                                                    new()
                                                                    {
                                                                        InlineKeyboardButton.WithCallbackData("Мои оповещения", "2")
                                                                    }
                                                                });

        public static InlineKeyboardMarkup? GetTickets()
        {
            var tickets = FsService.GetTickets();
            if (tickets == null)
                return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
                                                {
                                                    new()
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("Вернуться в меню", "1-1")
                                                    }
                                                });
            var groupings      = tickets.GroupBy(_ => _.Date.Date);
            var        inlineKeyboard = new List<List<InlineKeyboardButton>>();
            foreach (var source in groupings)
            {
                var inlineKeyboardButtonListList = inlineKeyboard;
                var inlineKeyboardButtonList = new List<InlineKeyboardButton>();
                var shortDateString = source.Key.ToShortDateString();
                var interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(98, 1);
                interpolatedStringHandler1.AppendLiteral("https://widget.kassir.ru/?type=E&key=0d043285-33ff-bbbb-d1f0-4d379a98d494&domain=spb.kassir.ru&id=");
                ref var local = ref interpolatedStringHandler1;
                var @event = source.FirstOrDefault();
                var num = @event != null ? @event.Id : 0;
                local.AppendFormatted(num);
                var stringAndClear = interpolatedStringHandler1.ToStringAndClear();
                inlineKeyboardButtonList.Add(InlineKeyboardButton.WithUrl(shortDateString, stringAndClear));
                inlineKeyboardButtonListList.Add(inlineKeyboardButtonList);
                inlineKeyboard.Add(source.Select(c =>
                                                                              {
                                                                                  var                           text                       = c.Date.ToShortTimeString() ?? "";
                                                                                  var interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(98, 1);
                                                                                  interpolatedStringHandler2.AppendLiteral("https://widget.kassir.ru/?type=E&key=0d043285-33ff-bbbb-d1f0-4d379a98d494&domain=spb.kassir.ru&id=");
                                                                                  interpolatedStringHandler2.AppendFormatted(c.Id);
                                                                                  return InlineKeyboardButton.WithUrl(text, interpolatedStringHandler2.ToStringAndClear());
                                                                              }).ToList());
            }
            inlineKeyboard.Add(new List<InlineKeyboardButton>
                               {
        InlineKeyboardButton.WithCallbackData("Вернуться в меню", "1-1")
      });
            return new InlineKeyboardMarkup(inlineKeyboard);
        }

        public static InlineKeyboardMarkup GetAlerts(ChatWithUser user)
        {
            var inlineKeyboard = new List<List<InlineKeyboardButton>>();
            var inlineKeyboardButtonList1 = new List<InlineKeyboardButton>();
            inlineKeyboard.Add(inlineKeyboardButtonList1);
            for (var index = 0; index < 5 && user.AlarmDays.Count > index; ++index)
                inlineKeyboardButtonList1.Add(InlineKeyboardButton.WithCallbackData(GetDayWithDOW(user.AlarmDays[index]) ?? "", "0"));
            var inlineKeyboardButtonList2 = new List<InlineKeyboardButton>();
            inlineKeyboard.Add(inlineKeyboardButtonList2);
            for (var index = 5; index < 10 && user.AlarmDays.Count > index; ++index)
                inlineKeyboardButtonList2.Add(InlineKeyboardButton.WithCallbackData(GetDayWithDOW(user.AlarmDays[index]) ?? "", "0"));
            inlineKeyboard.Add(new List<InlineKeyboardButton>
                               {
        InlineKeyboardButton.WithCallbackData("Добавить оповещение", "2-2")
      });
            if (user.AlarmDays.Count > 0)
            {
                inlineKeyboard.Add(new List<InlineKeyboardButton>
                                   {
          InlineKeyboardButton.WithCallbackData("Удалить оповещение", "2-3")
        });
                inlineKeyboard.Add(new List<InlineKeyboardButton>
                                   {
          InlineKeyboardButton.WithCallbackData("Удалить все оповещения", "2-4")
        });
            }
            inlineKeyboard.Add(new List<InlineKeyboardButton>
                               {
        InlineKeyboardButton.WithCallbackData("Назад", "2-5")
      });
            return new InlineKeyboardMarkup(inlineKeyboard);
        }

        public static InlineKeyboardMarkup GetDaysForNewAlert()
        {
            var inlineKeyboard = new List<List<InlineKeyboardButton>>();
            var inlineKeyboardButtonList1 = new List<InlineKeyboardButton>();
            inlineKeyboard.Add(inlineKeyboardButtonList1);
            DateTime dateTime1;
            DefaultInterpolatedStringHandler interpolatedStringHandler;
            for (var index = 0; index < 5; ++index)
            {
                var inlineKeyboardButtonList2 = inlineKeyboardButtonList1;
                dateTime1 = DateTime.Now;
                dateTime1 = dateTime1.Date;
                var text = GetDayWithDOW(dateTime1.AddDays(index)) ?? "";
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
                interpolatedStringHandler.AppendLiteral("2-2-Date-");
                ref var local = ref interpolatedStringHandler;
                dateTime1 = DateTime.Now;
                dateTime1 = dateTime1.Date;
                var dateTime2 = dateTime1.AddDays(index);
                local.AppendFormatted(dateTime2);
                var inlineKeyboardButton = InlineKeyboardButton.WithCallbackData(text, interpolatedStringHandler.ToStringAndClear());
                inlineKeyboardButtonList2.Add(inlineKeyboardButton);
            }
            var inlineKeyboardButtonList3 = new List<InlineKeyboardButton>();
            inlineKeyboard.Add(inlineKeyboardButtonList3);
            for (var index = 5; index < 10; ++index)
            {
                var inlineKeyboardButtonList4 = inlineKeyboardButtonList3;
                dateTime1 = DateTime.Now;
                dateTime1 = dateTime1.Date;
                var text = GetDayWithDOW(dateTime1.AddDays(index)) ?? "";
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
                interpolatedStringHandler.AppendLiteral("2-2-Date-");
                ref var local = ref interpolatedStringHandler;
                dateTime1 = DateTime.Now;
                dateTime1 = dateTime1.Date;
                var dateTime3 = dateTime1.AddDays(index);
                local.AppendFormatted(dateTime3);
                var inlineKeyboardButton = InlineKeyboardButton.WithCallbackData(text, interpolatedStringHandler.ToStringAndClear());
                inlineKeyboardButtonList4.Add(inlineKeyboardButton);
            }
            inlineKeyboard.Add(new List<InlineKeyboardButton>
                               {
        InlineKeyboardButton.WithCallbackData("Назад", "2-2-1")
      });
            return new InlineKeyboardMarkup(inlineKeyboard);
        }

        public static InlineKeyboardMarkup GetAlertsForDelete(ChatWithUser user)
        {
            var inlineKeyboard = new List<List<InlineKeyboardButton>>();
            var inlineKeyboardButtonList1 = new List<InlineKeyboardButton>();
            inlineKeyboard.Add(inlineKeyboardButtonList1);
            DefaultInterpolatedStringHandler interpolatedStringHandler;
            for (var index = 0; index < 5 && user.AlarmDays.Count > index; ++index)
            {
                var inlineKeyboardButtonList2 = inlineKeyboardButtonList1;
                var text = GetDayWithDOW(user.AlarmDays[index]) ?? "";
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                interpolatedStringHandler.AppendLiteral("2-3-");
                interpolatedStringHandler.AppendFormatted(index);
                var inlineKeyboardButton = InlineKeyboardButton.WithCallbackData(text, interpolatedStringHandler.ToStringAndClear());
                inlineKeyboardButtonList2.Add(inlineKeyboardButton);
            }
            var inlineKeyboardButtonList3 = new List<InlineKeyboardButton>();
            inlineKeyboard.Add(inlineKeyboardButtonList3);
            for (var index = 5; index < 10 && user.AlarmDays.Count > index; ++index)
            {
                var inlineKeyboardButtonList4 = inlineKeyboardButtonList3;
                var text = GetDayWithDOW(user.AlarmDays[index]) ?? "";
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                interpolatedStringHandler.AppendLiteral("2-3-");
                interpolatedStringHandler.AppendFormatted(index);
                var inlineKeyboardButton = InlineKeyboardButton.WithCallbackData(text, interpolatedStringHandler.ToStringAndClear());
                inlineKeyboardButtonList4.Add(inlineKeyboardButton);
            }
            inlineKeyboard.Add(new List<InlineKeyboardButton>
                               {
        InlineKeyboardButton.WithCallbackData("Назад", "2-3-999")
      });
            return new InlineKeyboardMarkup(inlineKeyboard);
        }
    }
}
