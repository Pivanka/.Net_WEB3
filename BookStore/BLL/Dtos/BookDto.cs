namespace BLL.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public int ReviewsNumber { get; set; }
    }
}
