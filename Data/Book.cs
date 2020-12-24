using System.Collections.Generic;

namespace stortromme.Data
{
    public class Book
    {
        public Player Master { get; set; }
        public IList<Page> Pages { get; set; }

        public Book()
        {
            Pages = new List<Page>();
        }
    }
}