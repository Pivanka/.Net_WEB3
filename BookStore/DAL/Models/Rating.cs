using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Rating : BaseEntity
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }
    }
}
