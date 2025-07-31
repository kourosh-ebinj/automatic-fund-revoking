using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Application.Enums
{
    public enum RayanStatusEnum : byte
    {
        [Description("پیش نویس")]
        DRAFT = 0,

        [Description("انتظار تایید")]
        WAIT = 1,

        [Description("تایید شده")]
        CONFIRMED = 2,

        [Description("رد شده")]
        REJECTED = 3,

        [Description("حذف شده")]
        DELETE = 4,

        [Description("رد شده توسط مدیر")]
        REJECTED_BY_ADMIN = 5,
    }
}
