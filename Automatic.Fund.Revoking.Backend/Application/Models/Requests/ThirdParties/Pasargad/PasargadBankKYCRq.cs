using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public record PasargadBankKYCRq
    {
        public string ScApiKeyKYC { get; set; }
        public string NationalCode { get; set; }
        public string Iban { get; set; }
        public string BirthDate { get; set; }

    }
}
