using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

public record GroupSummaryDto(
    Guid Id,
    string Name,
    Category Category
);