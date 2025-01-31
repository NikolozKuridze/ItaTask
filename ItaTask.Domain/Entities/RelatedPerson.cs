using ItaTask.Domain.Enums;

namespace ItaTask.Domain.Entities;

public class RelatedPerson : EntityBase
{
    public required int PersonId { get; set; }
    public required int RelatedPersonId { get; set; }
    public required RelationType RelationType { get; set; }

    public virtual Person? MainPerson { get; set; }
    public virtual Person? RelatedTo { get; set; }
}