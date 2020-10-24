using System;
using System.Collections.Generic;
using System.Linq;

namespace broken_picturephone_blazor.Data
{
    public class Lobby
    {
        public string Name { get; set; }
        public IList<Player> Players { get; set; }
        public Settings Settings { get; set; }
        public Game Game { get; set; }

        public event Action OnLobbyUpdated;
        public event Action<Player> OnPlayerRemoved;
        public event Action<Player> OnPlayerKicked;
        public event Action OnGameStarted;

        public Lobby()
        {
            Players = new List<Player>();

            // Invoke OnLobbyUpdated for any more specific lobby update events
            OnPlayerRemoved += (_player) => OnLobbyUpdated?.Invoke();
        }

        public void StartGame()
        {
            Game = new Game 
            { 
                Settings = Settings,
                Players = Players,
            };
            OnGameStarted?.Invoke();
        }

        public bool HasConnectedPlayer(string name)
        {
            return Players.Any(p => p.IsNameEqual(name) && p.IsConnected);
        }

        public Player AddPlayer(string name)
        {
            // Assign to existing player or create a new one. This is presumed 
            // no connected player is already in the lobby with the same name.
            Player player = Players.FirstOrDefault(p => p.IsNameEqual(name));
            if (player == null)
            {
                player = new Player { 
                    Name = name,
                    IsModerator = Players.Count == 0,
                };
                Players.Add(player);
            }
                        
            player.IsConnected = true;

            OnLobbyUpdated?.Invoke();
            return player;
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
            OnPlayerRemoved?.Invoke(player);

            // Assign the first player as moderator if all moderators were removed
            if (Players.Count > 0 && Players.All(p => !p.IsModerator))
            {
                Players.First().IsModerator = true;
                OnLobbyUpdated?.Invoke();
            }
        }

        public void KickPlayer(Player player)
        {
            OnPlayerKicked?.Invoke(player);
            RemovePlayer(player);
        }

        public void DisconnectPlayer(Player player)
        {
            player.IsConnected = false;
            OnLobbyUpdated?.Invoke();
        }

        public void MakeModerator(Player player)
        {
            player.IsModerator = true;
            OnLobbyUpdated?.Invoke();
        }

        public void UpdateSettings(Settings settings)
        {
            this.Settings = settings;
            OnLobbyUpdated?.Invoke();
        }
    }
}