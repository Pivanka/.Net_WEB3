using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class BookStoreDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1, Title = "1984", Author = "Goerge Orwell",
                    Cover =
                        "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1327144697l/3744438.jpg",
                    Content =
                        "1984 is a dystopian novella by George Orwell published in 1949, which follows the life of Winston Smith, a low ranking member of 'the Party', who is frustrated by the omnipresent eyes of the party, and its ominous ruler Big Brother. 'Big Brother' controls every aspect of people's lives.",
                    Genre = Genre.Novel,
                    Price = 100
                },
                new Book
                {
                    Id = 2, Title = "The Little Prince", Author = "Antoine de Saint-Exupéry",
                    Cover = "https://images-na.ssl-images-amazon.com/images/I/71OZY035QKL.jpg",
                    Content =
                        "The Little Prince is an honest and beautiful story about loneliness, friendship, sadness, and love. The prince is a small boy from a tiny planet (an asteroid to be precise), who travels the universe, planet-to-planet, seeking wisdom. On his journey, he discovers the unpredictable nature of adults.",
                    Genre = Genre.Novella,
                    Price = 110
                },
                new Book
                {
                    Id = 3, Title = "Harry Potter", Author = "J. K. Rowling",
                    Cover =
                        "https://images.ctfassets.net/usf1vwtuqyxm/3d9kpFpwHyjACq8H3EU6ra/85673f9e660407e5e4481b1825968043/English_Harry_Potter_4_Epub_9781781105672.jpg?w=914&q=70&fm=jpg",
                    Content =
                        "The novels chronicle the lives of a young wizard, Harry Potter, and his friends Hermione Granger and Ron Weasley, all of whom are students at Hogwarts School of Witchcraft and Wizardry. The main story arc concerns Harry's struggle against Lord Voldemort, a dark wizard who intends to become immortal, overthrow the wizard governing body known as the Ministry of Magic and subjugate all wizards and Muggles (non-magical people).",
                    Genre = Genre.Novel,
                    Price = 150
                },
                new Book
                {
                    Id = 4, Title = "The Man Who Died Twice", Author = "Richard Osman",
                    Cover = "https://storage.googleapis.com/lr-assets/_nielsen/400/9780241988244.jpg",
                    Content =
                        "A fabulously twisty crime novel that also sits in our feel-good and humorous categories, what more could you ask for?",
                    Genre = Genre.Novel,
                    Price = 89
                },
                new Book
                {
                    Id = 5, Title = "Ugly Love", Author = "Colleen Hoover",
                    Cover = "https://storage.googleapis.com/lr-assets/_nielsen/400/9781471136726.jpg",
                    Content =
                        "When Tate Collins finds airline pilot Miles Archer passed out in front of her apartment door, it is definitely not love at first sight. They wouldn't even go so far as to consider themselves friends. But what they do have is an undeniable mutual attraction.He doesn't want love and she doesn't have time for a relationship, but their chemistry cannot be ignored.Once their desires are out in the open, they realize they have the perfect set - up, as long as Tate can stick to two rules:Never ask about the past and don't expect a future.Tate is determined that she can handle it, but when she realises that she can't, will she be able to say no to her sexy pilot when he lives just next door?",
                    Genre = Genre.Novel,
                    Price = 115
                },
                new Book
                {
                    Id = 6, Title = "The Hunger Games", Author = "Suzanne Collins",
                    Cover =
                        "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1586722975l/2767052.jpg",
                    Content =
                        "In the ruins of a place once known as North America lies the nation of Panem, a shining Capitol surrounded by twelve outlying districts. The Capitol is harsh and cruel and keeps the districts in line by forcing them all to send one boy and one girl between the ages of twelve and eighteen to participate in the annual Hunger Games, a fight to the death on live TV.",
                    Genre = Genre.Fiction,
                    Price = 90
                },
                new Book
                {
                    Id = 7, Title = "Divergent", Author = "Veronica Roth",
                    Cover =
                        "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1618526890l/13335037._SY475_.jpg",
                    Content =
                        "In Beatrice Prior's dystopian Chicago world, society is divided into five factions, each dedicated to the cultivation of a particular virtue—Candor (the honest), Abnegation (the selfless), Dauntless (the brave), Amity (the peaceful), and Erudite (the intelligent). On an appointed day of every year, all sixteen-year-olds must select the faction to which they will devote the rest of their lives. For Beatrice, the decision is between staying with her family and being who she really is—she can't have both. So she makes a choice that surprises everyone, including herself.",
                    Genre = Genre.Fantasy,
                    Price = 120
                },
                new Book
                {
                    Id = 8, Title = "Twilight", Author = "Stephenie Meyer",
                    Content =
                        "About three things I was absolutely positive.First, Edward was a vampire.Second, there was a part of him—and I didn't know how dominant that part might be—that thirsted for my blood.And third, I was unconditionally and irrevocably in love with him.",
                    Cover =
                        "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1361039443l/41865.jpg",
                    Genre = Genre.Fantasy,
                    Price = 85
                },
                new Book
                {
                    Id = 9, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald",
                    Content =
                        "The Great Gatsby, F. Scott Fitzgerald's third book, stands as the supreme achievement of his career. This exemplary novel of the Jazz Age has been acclaimed by generations of readers. The story of the fabulously wealthy Jay Gatsby and his love for the beautiful Daisy Buchanan, of lavish parties on Long Island at a time when The New York Times noted \"gin was the national drink and sex the national obsession,\" it is an exquisitely crafted tale of America in the 1920s.",
                    Cover =
                        "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1490528560l/4671._SY475_.jpg",
                    Genre = Genre.Fiction,
                    Price = 125
                },
                new Book
                {
                    Id = 10, Title = "The Diary of a Young Girl", Author = "Anne Frank",
                    Content =
                        "In 1942, with the Nazis occupying Holland, a thirteen-year-old Jewish girl and her family fled their home in Amsterdam and went into hiding.",
                    Cover =
                        "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1560816565l/48855.jpg",
                    Genre = Genre.Biography,
                    Price = 95
                }
            );
        }
    }
}
