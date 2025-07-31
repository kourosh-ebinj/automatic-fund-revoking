using System.Collections.Generic;

namespace WebCore.Dtos;

public record NotOkResultDto(
    string Message,
    string? Details = default,
    string? CorrelationId = default,
    IEnumerable<string> Parameters = default)
    : ResultBaseDto;
