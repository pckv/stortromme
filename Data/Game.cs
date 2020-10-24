using System.Collections.Generic;

namespace broken_picturephone_blazor.Data
{
    public class Game
    {
        public Settings Settings { get; set; }
        public IList<Player> Players { get; set; }
        public IList<Book> Books { get; set; }

        public Game()
        {
            Books = new List<Book>();
        }
    }
}