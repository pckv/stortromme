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
        public event Action<Player> OnPlayerRemoved;

        public Lobby()
        {
            Players = new List<Player>();

            // Invoke OnLobbyUpdated for any more specific lobby update events
            OnPlayerRemoved += (_player) => OnLobbyUpdated?.Invoke();
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
                throw new PlayerExistsException();
            }
            else
            {
                player.IsConnected = true;
            }

            if (Players.Count == 0)
            {
                player.IsModerator = true;
            }

            Players.Add(player);
            OnLobbyUpdated?.Invoke();

            return player;
        }

        public void RemovePlayer(Player player)
        {
            // Skip if the player does not exist in the lobby
            if (!Players.Contains(player))
            {
                return;
            }

            Players.Remove(player);
            OnPlayerRemoved?.Invoke(player);

            // Assign the first player as moderator if all moderators were removed
            if (Players.Count > 0 && Players.All(p => !p.IsModerator))
            {
                Players.First().IsModerator = true;
                OnLobbyUpdated?.Invoke();
            }
        }

        public void KickPlayer(Player moderator, Player player)
        {
            if (moderator.IsModerator)
            {
                RemovePlayer(player);
            }
        }

        public void MakeModerator(Player moderator, Player player)
        {
            if (moderator.IsModerator)
            {
                player.IsModerator = true;
                OnLobbyUpdated?.Invoke();
            }
        }
    }

    public class PlayerExistsException : Exception
    {
    }
}