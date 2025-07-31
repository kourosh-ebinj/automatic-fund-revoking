using System;

namespace Core.Exceptions;

public sealed class CustomTooManyRequestsException : ExceptionBase
{
    public CustomTooManyRequestsException() { }

    public CustomTooManyRequestsException(string? message) : base(message) { }

    public CustomTooManyRequestsException(string? message, Exception? innerException) : base(message, innerException) { }

    public CustomTooManyRequestsException(string? message, params object[] parameters) : base(message, parameters) { }
}
