using Snipcode.Application.DTOs.Snippets;
using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

// GroupDetailsResponseDto буде використовуватись для того,
// щоб коли юзер вибрав групу наприклад у списку груп він
// міг переглянути сніпетти у ній + загальну інфу про неї.
public record GroupDetailResponseDto(
    Guid Id,
    string Name,
    string? Description,
    Category Category,
    bool IsPublic,
    DateTime CreatedAt,
    Guid OwnerId,
    string OwnerUsername,
    List<SnippetResponseDto> Snippets,
    IEnumerable<Technology> Technologies
);