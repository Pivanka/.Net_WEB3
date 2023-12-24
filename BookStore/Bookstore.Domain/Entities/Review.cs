using BookStore.Domain.Base;

namespace BookStore.Domain.Entities;

public class Review : BaseEntity
{
    public string Message { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}

