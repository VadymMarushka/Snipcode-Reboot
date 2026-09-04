using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

// GroupResponseDto буде використовуватися для того,
// щоб показати інфу про групу, яку ми будемо дисплеїти на карточках,
// самі сніппети групи сюди не вантажимо, бо цих dto ми будемо отримувати багато,
// а клікати користувач буде на одиниці - тільки тоді будемо тягнути GroupDetailsResponseDto

public record GroupResponseDto(
    Guid Id,
    string Name,
    string? Description,
    Category Category,
    bool IsPublic,
    DateTime CreatedAt,
    Guid OwnerId,
    string OwnerUsername,
    int SnippetCount,
    IEnumerable<Technology> Technologies
);