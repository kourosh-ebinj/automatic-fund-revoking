using System;
using System.Collections.Generic;
using Core.DomainValidation.Models;

namespace Core.Exceptions;

/// <summary>
/// This exception will be thrown when a domain validation error occurs
/// </summary>
[Serializable]
public sealed class CustomBadRequestException : ExceptionBase
{
    public const string ConstMessagesDomainValidationError = "پارامترهای ورودی صحیح نیست. ";

    public IEnumerable<DomainValidationError> ValidationErrors { get; set; }

    public CustomBadRequestException(params DomainValidationError[] validationErrors) :
        base(ConstMessagesDomainValidationError) => ValidationErrors = validationErrors;

    public CustomBadRequestException(string? message) : base(message ?? ConstMessagesDomainValidationError) { }

    public CustomBadRequestException(string? message, Exception? inner) : base(message, inner) { }
}
