using System;
using System.Linq;

namespace Core.Exceptions;

public abstract class ExceptionBase : Exception
{
    protected ExceptionBase() { }

    protected ExceptionBase(string? message): base(message) { }

    protected ExceptionBase(string? message, Exception? innerException) : base(message, innerException) { }

    protected ExceptionBase(string? message, params object[] parameters) : base(message)
    {
        foreach (var (value, index) in parameters.Select((v , i) => (v , i)))
            Data.Add(index, value);
    }
}
