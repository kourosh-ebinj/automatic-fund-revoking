using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums.Messaging.MessageDispatcher
{
    public enum MessageType : byte
    {
        [Description("پیامک")]
        SMS = 1,

        [Description("ایمیل")]
        Email = 2,

        [Description("")]
        Rest = 3,

        [Description("نوتیف درون برنامه")]
        Signal = 4,

        [Description("")]
        MessageBroker = 5,

        [Description("تلگرام")]
        Telegram = 6,

        [Description("واتسآپ")]
        Whatsapp = 7,

    }
}
