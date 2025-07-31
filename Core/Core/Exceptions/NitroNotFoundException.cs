using System;

namespace Core.Exceptions;

public class CustomNotFoundException : ExceptionBase
{
    public CustomNotFoundException() { }

    public CustomNotFoundException(string? message) : base(message) { }

    public CustomNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    public CustomNotFoundException(string? message, params object[] parameters) : base(message, parameters) { }
    
}
