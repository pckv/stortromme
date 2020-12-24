using System;
using System.Collections.Generic;
using System.Linq;

namespace stortromme.Data
{
    public class Game
    {
        public Settings Settings { get; set; }
        public IList<Player> Players { get; set; }
        public IList<Book> Books { get; set; }
        public int CurrentPage { get; set; }
        public IList<Player> ReadyPlayers { get; set; }

        private IList<IList<Player>> playerPattern;

        public event Action OnNextPage;
        public event Action OnPageSubmitted;
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
            else
            {
                OnPageSubmitted?.Invoke();
            }
        }

        public bool HasGameEnded() => CurrentPage >= Settings.Pages;

        public Book GetCurrentBook(Player player)
        {
            return Books.FirstOrDefault(b => b.Pages[CurrentPage].Author == player);
        }

        public bool IsPlayerDone(Player player)
        {
            return HasGameEnded() || !(GetCurrentBook(player)?.Pages[CurrentPage].InProgress ?? false);
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

        private void createPlayerPattern()
        {
            // Create a shifted list per player
            playerPattern = Enumerable.Range(0, Players.Count)
                .Select(i => (IList<Player>) Players.Skip(i).Concat(Players.Take(i)).ToList())
                .ToList();
            
            // Shuffle the list of patterns
            playerPattern = playerPattern.OrderBy(l => Guid.NewGuid()).ToList();

            // Transpose the player patterns to allow for previous page to be
            // by an unpredicted player
            playerPattern = Enumerable.Range(0, Players.Count)
                .Select(i => (IList<Player>) playerPattern.Select(l => l[i]).ToList())
                .ToList();

            // And finally shuffle the new list to enable the statement above
            playerPattern = playerPattern.OrderBy(l => Guid.NewGuid()).ToList();
        }

        private void CreateBooks()
        {
            createPlayerPattern();

            // Create the first page of every book
            foreach (var player in playerPattern[CurrentPage])
            {
                var book = new Book { Master = player };

                // Add the first page by the Master
                AddNewPage(book, author: player);

                Books.Add(book);
            }
        }

        private void CreateNewPages()
        {
            // Create a new page for each book
            for (var i = 0; i < Books.Count; i++)
            {
                AddNewPage(Books[i], playerPattern[CurrentPage % Players.Count][i]);
            }
        }
    }
}
