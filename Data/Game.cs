using System;
using System.Collections.Generic;
using System.Linq;

namespace broken_picturephone_blazor.Data
{
    public class Game
    {
        public Settings Settings { get; set; }
        public IList<Player> Players { get; set; }
        public IList<Book> Books { get; set; }

        private int currentPage = 0;

        public event Action<ContentType> OnNewPage;

        public Game()
        {
            Books = new List<Book>();
        }

        public Page GetCurrentPage(Player player)
        {
            return Books
                .Select(b => b.Pages[currentPage])
                .Where(p => p.Author == player)
                .FirstOrDefault();
        }

        public bool IsGameOver() => currentPage == Settings.Pages;

        public void FirstPages()
        {
            foreach (var player in Players)
            {
                var book = new Book { Master = player };
            }

            OnNewPage?.Invoke(ContentType.Text);
        }
    }
}