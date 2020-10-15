using System.Collections.Generic;

namespace broken_picturephone_blazor.Data
{
    public class Lobby
    {
        public string Name { get; set; }
        public IList<Player> Players { get; set; }

        public Lobby()
        {
            Players = new List<Player>();
        }
    }
}