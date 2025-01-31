using ItaTask.Domain.Enums;

namespace ItaTask.Application.DTOs.Persons;

public record RelatedPersonDto
{
    public int RelatedPersonId { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public RelationType RelationType { get; init; }
}
