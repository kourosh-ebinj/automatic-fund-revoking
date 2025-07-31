using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Responses.ThirdParties.Pasargad
{
    public record PasargadBaseRs<T> : ResultBase<T>
    {
        public PasargadBaseRs()
        {

        }

        [JsonPropertyName("Message")]
        public string ErrorMessage { get; set; }
        public int MessageId { get; set; }
        public bool HasError { get; set; }
        public int ErrorCode { get; set; }
        public string ReferenceNumber { get; set; }
        public int Count { get; set; }
        public int StatusCode { get; set; }

    }

    public record ResultBase<T> { 
        public ResultInnerBase<T> Result { get; set; }
    
    }

    public record ResultInnerBase<T>
    {
        public int RsCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T ResultData { get; set; }
    }

}

