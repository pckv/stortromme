namespace stortromme.Data
{
    public class Presenter
    {
        public Book Book { get; set; }
        public int DisplayPages { get; set; }
        public bool Flipped { get; set; }

        public Presenter(Book book)
        {
            Book = book;
            DisplayPages = 1;
            Flipped = false;
        }
    }
}