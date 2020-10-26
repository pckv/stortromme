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

        private void AddNewPage(Book book, Player author)
        {
            book.Pages.Add(new Page
            {
                Author = author,
                ContentType = GetCurrentPageType(),
                PageNumber = CurrentPage,
            });
        }

        private void CreateBooks()
        {
            foreach (var player in Players)
            {
                var book = new Book { Master = player };

                // Add the first page by the Master
                AddNewPage(book, author: player);

                Books.Add(book);
            }
        }

        private Player GetAuthorForBook(Book book, IList<Player> assignedPlayers)
        {
            var previousAuthors = book.Pages
                .Reverse()
                .Take(Players.Count - 1)
                .Select(p => p.Author)
                .ToList();

            return Players
                .Where(p => !previousAuthors.Contains(p) && !assignedPlayers.Contains(p))
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefault();
        }

        private void CreateNewPages()
        {
            var assignedPlayers = new List<Player>();

            // Create a new page for each book
            foreach (var book in Books)
            {
                var author = GetAuthorForBook(book, assignedPlayers);
                assignedPlayers.Add(author);;

                AddNewPage(book, author);
            }
        }
    }
}
