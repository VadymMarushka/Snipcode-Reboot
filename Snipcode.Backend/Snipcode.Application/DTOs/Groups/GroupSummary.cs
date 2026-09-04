using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

// GroupSummaryDto - це просто dto для SnippetResponseDto,
// щоб не тягнути всю інйу про групу, коли потрібна тільки назва, id і категорія
public record GroupSummaryDto(
    Guid Id,
    string Name,
    Category Category
);