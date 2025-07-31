using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public record PasargadBankAccountNumberRq
    {
        public string ShebaNumber { get; set; }

    }
}
