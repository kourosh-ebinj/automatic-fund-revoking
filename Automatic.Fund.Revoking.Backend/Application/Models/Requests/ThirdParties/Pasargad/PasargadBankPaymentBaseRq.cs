using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using Application.Enums;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    
    public abstract record PasargadBankPaymentBaseRq
    {
        public string TransactionId { get; set; } = "";
        public string TransactionDate => DateTime.Now.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));

    }
}
