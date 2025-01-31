using ItaTask.Domain.Enums;

namespace ItaTask.Application.DTOs.Persons;

public record PhoneNumberDto
{
    public PhoneType Type { get; init; }
    public string Number { get; init; } = default!;
}