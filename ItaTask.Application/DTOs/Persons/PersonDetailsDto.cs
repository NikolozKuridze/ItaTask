namespace ItaTask.Application.DTOs.Persons;

public record PersonDetailsDto : PersonDto
{
    public string? ImagePath { get; init; }
    public List<PhoneNumberDto> PhoneNumbers { get; init; } = new();
    public List<RelatedPersonDto> RelatedPersons { get; init; } = new();
}