using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}