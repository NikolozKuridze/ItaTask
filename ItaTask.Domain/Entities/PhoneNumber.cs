using System.ComponentModel.DataAnnotations;
using ItaTask.Domain.Enums;

namespace ItaTask.Domain.Entities;

public class PhoneNumber : EntityBase
{
    public required int PersonId { get; set; }
    public required PhoneType Type { get; set; }
    
    [StringLength(50, MinimumLength = 4)]
    public required string Number { get; set; }

    public virtual Person? Person { get; set; }
}