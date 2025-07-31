using System;

namespace Core.Exceptions;

public class CustomUnauthorizedException : ExceptionBase
{
    public CustomUnauthorizedException() { }

    public CustomUnauthorizedException(string message) : base(message) { }

    public CustomUnauthorizedException(string? message, Exception? innerException) : base(message, innerException) { }

    public CustomUnauthorizedException(string? message, params object[] parameters) : base(message, parameters) { }
}
