using System;
using System.Collections.Generic;
using System.Net;
using Core.Enums;

namespace Core.Abstractions;

public interface IGuard
{
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode);
    void Assert(bool trueCondition, HttpStatusCode statusCode);
    void Assert(bool trueCondition, int statusCode);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message);
    void Assert(bool trueCondition, HttpStatusCode statusCode, string message);
    void Assert(bool trueCondition, int statusCode, string message);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, params string[] errors);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, IDictionary<string, object> errors);
    void Assert(bool trueCondition, HttpStatusCode statusCode, params string[] errors);
    void Assert(bool trueCondition, HttpStatusCode statusCode, IDictionary<string, object> errors);
    void Assert(bool trueCondition, int statusCode, params string[] errors);
    void Assert(bool trueCondition, int statusCode, IDictionary<string, object> errors);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, Exception innerException);
    void Assert(bool trueCondition, HttpStatusCode statusCode, Exception innerException);
    void Assert(bool trueCondition, int statusCode, Exception innerException);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message, params string[] errors);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message, IDictionary<string, object> errors);
    void Assert(bool trueCondition, HttpStatusCode statusCode, string message, params string[] errors);
    void Assert(bool trueCondition, HttpStatusCode statusCode, string message, IDictionary<string, object> errors);
    void Assert(bool trueCondition, int statusCode, string message, params string[] errors);
    void Assert(bool trueCondition, int statusCode, string message, IDictionary<string, object> errors);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message, Exception innerException);
    void Assert(bool trueCondition, HttpStatusCode statusCode, string message, Exception innerException);
    void Assert(bool trueCondition, int statusCode, string message, Exception innerException);
    void Assert(bool trueCondition, int statusCode, string? message = default, Exception? innerException = default, params string[] errors);
    void Assert(bool trueCondition, HttpStatusCode statusCode, string? message = default, Exception? innerException = default, params string[] errors);
    void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string? message = default, Exception? innerException = default, params string[] errors);
}
