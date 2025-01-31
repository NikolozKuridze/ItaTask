using System.ComponentModel.DataAnnotations;
using ItaTask.Domain.Enums;

namespace ItaTask.Domain.Entities;

public class Person : EntityBase
{
    [StringLength(50, MinimumLength = 2)] public required string FirstName { get; set; }

    [StringLength(50, MinimumLength = 2)] public required string LastName { get; set; }

    public required Gender Gender { get; set; }

    [StringLength(11, MinimumLength = 11)]
    [RegularExpression("^[0-9]*$")]
    public required string PersonalNumber { get; set; }

    public required DateTime BirthDate { get; set; }

    public required int CityId { get; set; }

    public string? ImagePath { get; set; }

    public virtual City City { get; set; } = null!;
    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    public virtual ICollection<RelatedPerson> RelatedPersons { get; set; } = new List<RelatedPerson>();
    public string FirstNameKey => $"Person_{Id}_FirstName";
    public string LastNameKey => $"Person_{Id}_LastName";
}