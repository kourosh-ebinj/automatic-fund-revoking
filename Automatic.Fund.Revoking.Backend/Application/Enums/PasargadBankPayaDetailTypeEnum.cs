using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Enums
{
    public enum PasargadBankPayaDetailTypeEnum : byte
    {
        [Description("واریز حقوق")]
        Hoghough = 1,

        [Description("امور بیمه خدمات")]
        BimehKhadamat = 2,

        [Description("امور درمانی")]
        BimehDarmani = 3,

        [Description("امور سرمایه‌گذاری و بورس")]
        SarmayegozariVaBourse = 4,

        [Description("امور ارزی در چهارچوب ضوابط و مقررات")]
        OmoureArzi = 5,

        [Description("پرداخت قرض و تادیه دیون (قرض‌الحسنه، بدهی و...)")]
        pardakhtGharz = 6,

        [Description("امور بازنشستگی")]
        AmoureBazneshastegi = 7,

        [Description("معاملات اموال منقول")]
        MoamelateAmvalemanghoul = 8,

        [Description("معاملات اموال غیرمنقول")]
        MoamelateAmvaleGheiremanghoul = 9,

        [Description("مدیریت نقدینگی")]
        ModiriateNaghdinegi = 10,

        [Description("عوارض گمرکی")]
        AvarezeGomroki = 11,

        [Description("تسویه مالیاتی")]
        TasviehMaliati = 12,

        [Description("سایر خدمات دولتی")]
        SayerKhadamateDolati = 13,

        [Description("تسهیلات و تعهدات")]
        TashilatVaTaahodat = 14,

        [Description("تودیع وثیقه")]
        TodieVasighe = 15,

        [Description("هزینه‌ی عمومی و امور روزمره")]
        HazinehOmoumiVaOmoureRoozmare = 16,

        [Description("کمک‌های خیریه")]
        KomakhayeKheirieh = 17,

        [Description("خرید کالا")]
        KharideKala = 18,

        [Description("خرید خدمات")]
        KharidehKhadamat = 19,

    }
}
