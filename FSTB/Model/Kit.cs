using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FSTB.Model
{
    [JsonObject]
    public class Kit
    {
        public List<EventBucket> eventBuckets { get; set; }
    }

    [JsonObject]
    public class EventBucket
    {
        public List<Event> events { get; set; }
    }

    [JsonObject]
    public class Event
    {
        public int Id { get; set; }

        public DateTimeOffset BeginsAt { get; set; }

        public DateTime Date
        {
            get
            {
                var beginsAt = BeginsAt;
                var       dateTime = beginsAt.DateTime;
                beginsAt = BeginsAt;
                var offset = beginsAt.Offset;
                return dateTime - offset;
            }
        }
    }

    [JsonObject]
    public class ChatWithUser
    {
        public long UserId { get; set; }

        [JsonIgnore]
        public int LastMessageId { get; set; }

        public ChatId Chat { get; set; }

        public string Name { get; set; }

        public string? LName { get; set; }

        public string? UName { get; set; }

        public List<DateTime> AlarmDays { get; set; } = new();

        public ChatWithUser()
        {
        }

        public ChatWithUser(User user)
        {
            UserId = user.Id;
            Name   = user.FirstName;
            LName  = user.LastName;
            UName  = user.Username;
            Chat   = new ChatId(UserId);
        }
    }

    [JsonObject]
    public class FsReply
    {
        public Kit Kit { get; set; }
    }
}
