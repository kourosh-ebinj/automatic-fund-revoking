using System;

namespace Core.Exceptions;

[Serializable]
public sealed class CustomForbiddenException : ExceptionBase
{
    public CustomForbiddenException() { }
    public CustomForbiddenException(string? message) : base(message) { }
    public CustomForbiddenException(string? message, Exception? inner) : base(message, inner) { }
    public CustomForbiddenException(string? message, params object[] parameters) : base(message, parameters) { }
}
