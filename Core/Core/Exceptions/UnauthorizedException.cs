using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public ExceptionCodeEnum ErrorCode { get; set; }

        public string CustomMessage { get; set; }

        public object Value { get; set; }

        public UnauthorizedException() { }
        public UnauthorizedException(string message) : base(message) { }
        public UnauthorizedException(string message, params object[] parameters) : base(message)
        {
            foreach (var parameter in parameters)
                Data.Add(nameof(parameter), parameter);

        }
        public UnauthorizedException(ExceptionCodeEnum errorCode,
                                  string customMessage = null,
                                  IDictionary<string, string> data = null,
                                  Exception innerException = null) : base(customMessage, innerException)
        {
            ErrorCode = errorCode;
            CustomMessage = customMessage;

            if (data is not null)
                foreach (var item in data)
                    this.Data.Add(item.Key, item.Value);

        }
        public UnauthorizedException(string message, Exception inner) : base(message, inner) { }

    }
}
