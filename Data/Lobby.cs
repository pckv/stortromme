using System;
using System.Collections.Generic;
using System.Linq;

namespace broken_picturephone_blazor.Data
{
    public class Lobby
    {
        public string Name { get; set; }
        public IList<Player> Players { get; set; }

        public event Action OnLobbyUpdated;

        public Lobby()
        {
            Players = new List<Player>();
        }

        public Player AddPlayer(string name)
        {
            Player player = Players.FirstOrDefault(p => p.IsNameEqual(name));
            if (player == null) 
            {
                player = new Player {
                    Name = name,
                    IsConnected = true,
                };
            }
            else if (player.IsConnected)
            {
                // Error: found existing connected player
            }
            else
            {
                player.IsConnected = true;
            }

            Players.Add(player);
            OnLobbyUpdated?.Invoke();

            return player;
        }

        public void RemovePlayer(Player player)
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
                OnLobbyUpdated?.Invoke();
            }
        }
    }
}