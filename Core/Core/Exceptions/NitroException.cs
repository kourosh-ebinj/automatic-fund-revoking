using System;
using System.Collections.Generic;
using Core.Enums;

namespace Core.Exceptions;

public class CustomException : ExceptionBase
{
    public CustomException() { }
    
    public CustomException(ExceptionCodeEnum errorCode, string? customMessage = null, Exception? innerException = null) : base(customMessage, innerException)
    {
        ErrorCode = errorCode;
        CustomMessage = customMessage;
    }

    public ExceptionCodeEnum ErrorCode { get; set; }

    public string CustomMessage { get; set; }

    public object Value { get; set; }

}
