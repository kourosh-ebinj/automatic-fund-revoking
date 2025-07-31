using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.Enums;
using Application.Models.Responses.ThirdParties.Pasargad;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public record PasargadBankInternalPaymentRq : PasargadBankPaymentBaseRq
    {
        public string ScApiKeyInternal { get; set; }
        public string SourceAccount { get; set; }
        public double SourceAmount { get; set; }
        public string SourceComment { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PasargadBankDocumentItemTypeEnum DocumentItemType { get; set; }
        
        public string TransferBillNumber { get; set; }
        public IEnumerable<Creditor> Creditors { get; set; }

    }

    public record Creditor {
        public string DestinationAccount { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))] 
        public PasargadBankDocumentItemTypeEnum DocumentItemType { get; set; }
        
        public double DestinationAmount { get; set; }
        public string DestinationComment { get; set; }

    }
}
