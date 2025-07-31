using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;

namespace Core.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public ExceptionCodeEnum ErrorCode { get; set; }

        public string CustomMessage { get; set; }

        public object Value { get; set; }

        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, params object[] parameters) : base(message)
        {
            foreach (var parameter in parameters)
                Data.Add(nameof(parameter), parameter);

        }
        public NotFoundException(ExceptionCodeEnum errorCode,
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
        public NotFoundException(string message, Exception inner) : base(message, inner) { }

    }
}
