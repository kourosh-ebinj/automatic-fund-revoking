using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public record PasargadBankAccountBalanceRq 
    {
        public string ScApiKeyAccountBalance { get; set; }
        public string DepositNumber { get; set; }
        public string ShebaNumber { get; set; }
    }
}
