using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Core.Abstractions;
using Core.Enums;
using Core.Exceptions;
using Core.Extensions;

namespace Core.Services;

public class Guard : IGuard
{
    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, message: default, innerException: default, errors: []);

    public void Assert(bool trueCondition, HttpStatusCode statusCode) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: default, innerException: default, errors: []);

    public void Assert(bool trueCondition, int statusCode) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: default, innerException: default, errors: []);

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, message: message, innerException: default, errors: []);

    public void Assert(bool trueCondition, HttpStatusCode statusCode, string message) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: default, errors: []);

    public void Assert(bool trueCondition, int statusCode, string message) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: default, errors: []);

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, params string[] errors) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, message: default, innerException: default, errors: errors);

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, IDictionary<string, object> errors) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, errors: errors, innerException: default, message: default);

    public void Assert(bool trueCondition, HttpStatusCode statusCode, params string[] errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: default, innerException: default, errors: errors);

    public void Assert(bool trueCondition, HttpStatusCode statusCode, IDictionary<string, object> errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, errors: errors, innerException: default, message: default);

    public void Assert(bool trueCondition, int statusCode, params string[] errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: default, innerException: default, errors: errors);

    public void Assert(bool trueCondition, int statusCode, IDictionary<string, object> errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: default, innerException: default, errors: errors);

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, Exception innerException) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, message: default, innerException: innerException, errors: []);

    public void Assert(bool trueCondition, HttpStatusCode statusCode, Exception innerException) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: default, innerException: innerException, errors: []);

    public void Assert(bool trueCondition, int statusCode, Exception innerException) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: default, innerException: innerException, errors: []);

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message, params string[] errors) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, message: message, innerException: default, errors: errors);

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message, IDictionary<string, object> errors) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, message: message, innerException: default, errors: errors);

    public void Assert(bool trueCondition, HttpStatusCode statusCode, string message, params string[] errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: default, errors: errors);

    public void Assert(bool trueCondition, HttpStatusCode statusCode, string message, IDictionary<string, object> errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: default, errors: errors);

    public void Assert(bool trueCondition, int statusCode, string message, params string[] errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: default, errors: errors);

    public void Assert(bool trueCondition, int statusCode, string message, IDictionary<string, object> errors) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: default, errors: errors);

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message, Exception innerException) =>
        Assert(trueCondition: trueCondition, exceptionCode: exceptionCode, message: message, innerException: innerException, errors: []);

    public void Assert(bool trueCondition, HttpStatusCode statusCode, string message, Exception innerException) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: innerException, errors: []);

    public void Assert(bool trueCondition, int statusCode, string message, Exception innerException) =>
        Assert(trueCondition: trueCondition, statusCode: statusCode, message: message, innerException: innerException, errors: []);

    public void Assert(bool trueCondition, int statusCode, string? message = default, Exception? innerException = default, params string[] errors)
    {
        switch (statusCode)
        {
            case 400:
                Assert(trueCondition, ExceptionCodeEnum.BadRequest, message, innerException, errors);
                break;
            case 401:
                Assert(trueCondition, ExceptionCodeEnum.Unauthorized, message, innerException, errors);
                break;
            case 403:
                Assert(trueCondition, ExceptionCodeEnum.Forbidden, message, innerException, errors);
                break;
            case 404:
                Assert(trueCondition, ExceptionCodeEnum.NotFound, message, innerException, errors);
                break;
            case 429:
                Assert(trueCondition, ExceptionCodeEnum.TooManyRequests, message, innerException, errors);
                break;
            default:
                Assert(trueCondition, ExceptionCodeEnum.InternalServerError, message, innerException, errors);
                break;
        }
    }

    public void Assert(bool trueCondition, int statusCode, string? message = default, Exception? innerException = default, IDictionary<string, object>? errors = default)
    {
        switch (statusCode)
        {
            case 400:
                Assert(trueCondition, ExceptionCodeEnum.BadRequest, message, innerException, errors);
                break;
            case 401:
                Assert(trueCondition, ExceptionCodeEnum.Unauthorized, message, innerException, errors);
                break;
            case 403:
                Assert(trueCondition, ExceptionCodeEnum.Forbidden, message, innerException, errors);
                break;
            case 404:
                Assert(trueCondition, ExceptionCodeEnum.NotFound, message, innerException, errors);
                break;
            case 429:
                Assert(trueCondition, ExceptionCodeEnum.TooManyRequests, message, innerException, errors);
                break;
            default:
                Assert(trueCondition, ExceptionCodeEnum.InternalServerError, message, innerException, errors);
                break;
        }
    }

    public void Assert(bool trueCondition, HttpStatusCode statusCode, string? message = default, Exception? innerException = default, params string[] errors)
    {
        switch (statusCode)
        {
            case HttpStatusCode.BadRequest:
                Assert(trueCondition, ExceptionCodeEnum.BadRequest, message, innerException, errors);
                break;
            case HttpStatusCode.Unauthorized:
                Assert(trueCondition, ExceptionCodeEnum.Unauthorized, message, innerException, errors);
                break;
            case HttpStatusCode.Forbidden:
                Assert(trueCondition, ExceptionCodeEnum.Forbidden, message, innerException, errors);
                break;
            case HttpStatusCode.NotFound:
                Assert(trueCondition, ExceptionCodeEnum.NotFound, message, innerException, errors);
                break;
            default:
                Assert(trueCondition, ExceptionCodeEnum.InternalServerError, message, innerException, errors);
                break;
        }
    }

    public void Assert(bool trueCondition, HttpStatusCode statusCode, IDictionary<string, object> errors, Exception? innerException = default, string message = default)
    {
        switch (statusCode)
        {
            case HttpStatusCode.BadRequest:
                Assert(trueCondition, ExceptionCodeEnum.BadRequest, message, innerException, errors);
                break;
            case HttpStatusCode.Unauthorized:
                Assert(trueCondition, ExceptionCodeEnum.Unauthorized, message, innerException, errors);
                break;
            case HttpStatusCode.Forbidden:
                Assert(trueCondition, ExceptionCodeEnum.Forbidden, message, innerException, errors);
                break;
            case HttpStatusCode.NotFound:
                Assert(trueCondition, ExceptionCodeEnum.NotFound, message, innerException, errors);
                break;
            case HttpStatusCode.TooManyRequests:
                Assert(trueCondition, ExceptionCodeEnum.TooManyRequests, message, innerException, errors);
                break;
            default:
                Assert(trueCondition, ExceptionCodeEnum.InternalServerError, message, innerException, errors);
                break;
        }
    }

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message = default, Exception? innerException = default, params string[] errors)
    {
        if (trueCondition)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? exceptionCode.ToDescription() : message;

        Exception exception = GetException(exceptionCode, message, innerException);

        if (errors is not null)
            foreach (var (value, index) in errors.Select((v, i) => (v, i)))
                exception.Data.Add(index, value);

        throw exception;
    }

    public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message, Exception? innerException, IDictionary<string, object> errors)
    {
        if (trueCondition)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? exceptionCode.ToDescription() : message;

        Exception exception = GetException(exceptionCode, message, innerException);

        if (errors is not null)
            foreach (var error in errors)
                exception.Data.Add(error.Key, error.Value);

        throw exception;
    }

    private static Exception GetException(ExceptionCodeEnum exceptionCode, string message, Exception? innerException) =>
        exceptionCode switch
        {
            ExceptionCodeEnum.BadRequest => new CustomBadRequestException(message, innerException),
            ExceptionCodeEnum.Unauthorized => new CustomUnauthorizedException(message, innerException),
            ExceptionCodeEnum.Forbidden => new CustomForbiddenException(message, innerException),
            ExceptionCodeEnum.NotFound => new CustomNotFoundException(message, innerException),
            ExceptionCodeEnum.TooManyRequests => new CustomTooManyRequestsException(message, innerException),
            _ => new CustomException(exceptionCode, message, innerException)
        };
}
