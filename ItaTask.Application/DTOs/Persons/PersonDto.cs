using ItaTask.Application.DTOs.Localization;
using ItaTask.Domain.Entities;
using ItaTask.Domain.Enums;

namespace ItaTask.Application.DTOs.Persons;

public record PersonDto
{
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Gender Gender { get; init; }
    public string PersonalNumber { get; init; } = default!;
    public required CityDto City { get; set; }
    public DateTime BirthDate { get; init; }
}