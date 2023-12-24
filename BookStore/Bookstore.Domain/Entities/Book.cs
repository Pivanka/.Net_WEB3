using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Domain.Base;
using BookStore.Domain.Shared;

namespace BookStore.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string Cover { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public Genre Genre { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal Price { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
}

