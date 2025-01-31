using System.ComponentModel.DataAnnotations;

namespace ItaTask.Domain.Entities;

public class EntityBase
{
    [Key] public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}