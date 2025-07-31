using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public record PasargadBankDepositNumberAndNationalCodeCompatibilityRq
    {
        public string DepositNumber { get; set; }
        public string NationalCode{ get; set; }


    }
}
