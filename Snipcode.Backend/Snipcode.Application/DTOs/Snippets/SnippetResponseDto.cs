using Snipcode.Application.DTOs.Groups;
using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Snippets;

// Це єдиний dto для сніпеттів, я не бачу сенсу робити окремий SnippetDetailsResponse,
// бо на картці ми так будемо показувати майже все, що показуємо у Details
public record SnippetResponseDto(
    Guid Id,
    string Title,
    string? Description,
    string CodeContent,
    Technology Technology,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid AuthorId,
    string AuthorUsername,
    List<string> Tags,
    GroupSummaryDto? Group
);