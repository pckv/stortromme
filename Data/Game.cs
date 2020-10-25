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
        public int CurrentPage { get; set; }
        public IList<Player> ReadyPlayers { get; set; }

        public event Action OnNextPage;
        public event Action OnGameEnded;

        public Game()
        {
            Books = new List<Book>();
            ReadyPlayers = new List<Player>();
            CurrentPage = -1;
        }

        public void PlayerReady(Player player)
        {
            ReadyPlayers.Add(player);
            if (AreAllPlayersReady())
            {
                NextPage();
                ReadyPlayers.Clear();
            }
        }

        public void SubmitPage(Book book)
        {
            book.Pages[CurrentPage].InProgress = false;
            if (AreAllPagesSubmitted())
            {
                NextPage();
            }
        }

        public bool HasGameEnded() => CurrentPage >= Settings.Pages;

        public Book GetCurrentBook(Player player)
        {
            return Books.FirstOrDefault(b => b.Pages[CurrentPage].Author == player);
        }

        public void NextPage()
        {
            CurrentPage++;

            if (HasGameEnded())
            {
                OnGameEnded?.Invoke();
            }
            else
            {
                if (CurrentPage == 0)
                {
                    CreateBooks();
                }
                else
                {
                    CreateNewPages();
                }

                OnNextPage?.Invoke();
            }
        }

        private bool AreAllPagesSubmitted()
        {
            return Books.All(b => !b.Pages[CurrentPage].InProgress);
        }

        private bool AreAllPlayersReady()
        {
            return Players.All(p => ReadyPlayers.Contains(p));
        }

        private ContentType GetCurrentPageType()
        {
            // First page is chosen after settings, and following pages are
            // based on the pattern
            return CurrentPage == 0 
                ? Settings.FirstPageType
                : Settings.PageTypePattern[(CurrentPage - 1) % Settings.PageTypePattern.Count];
        }

        private void CreateBooks()
        {
            foreach (var player in Players)
            {
                var book = new Book { Master = player };

                // Add the first page by the Master
                book.Pages.Add(new Page
                {
                    Author = book.Master,
                    ContentType = GetCurrentPageType(),
                });

                Books.Add(book);
            }
        }

        private void CreateNewPages()
        {
            // TODO: assign to random player using cool algorithm hehe
            var randomPlayers = Players.OrderBy(p => Guid.NewGuid()).ToList();

            // Create a new page for each book
            foreach (var (player, book) in randomPlayers.Zip(Books))
            {
                book.Pages.Add(new Page
                {
                    Author = player,
                    ContentType = GetCurrentPageType(),
                });
            }
        }
    }
}
