using System;
using System.Collections.Generic;
using System.Linq;

namespace stortromme.Data
{
    public class Lobby
    {
        public string Name { get; set; }
        public IList<Player> Players { get; set; }
        public Settings Settings { get; set; }
        public Game Game { get; set; }
        public GameState State { get; set; }

        public event Action OnLobbyUpdated;
        public event Action<Player, bool> OnPlayerRemoved;
        public event Action OnGameStarted;

        public Lobby()
        {
            Players = new List<Player>();
            State = GameState.InLobby;

            // Invoke OnLobbyUpdated for any more specific lobby update events
            OnPlayerRemoved += (_player, wasKicked) => OnLobbyUpdated?.Invoke();
        }

        public void StartGame()
        {
            Game = new Game(Players, Settings); 

            Game.OnGameEnded += OnGameEnded;
            State = GameState.Started;
            OnGameStarted?.Invoke();
        }

        public void ReturnToLobby()
        {
            Game = null;
            State = GameState.InLobby;
            OnLobbyUpdated?.Invoke();
        }

        public void OnGameEnded()
        {
            State = GameState.Presenting;
            Game.OnGameEnded -= OnGameEnded;
        }

        public bool HasConnectedPlayer(string name)
        {
            return Players.Any(p => p.IsNameEqual(name) && p.IsConnected);
        }

        private void UpdatePages()
        {
            if (Settings.ShouldUpdatePagesDymanically)
            {
                Settings.UpdateDynamicPages(Players.Count);
            }
        }

        public Player AddPlayer(string name)
        {
            // Assign to existing player or create a new one. This is presumed 
            // no connected player is already in the lobby with the same name.
            Player player = Players.FirstOrDefault(p => p.IsNameEqual(name));

            // Only allow joining if the game is not in progress, or this player
            // is reconnecting
            if ((player == null || player.IsConnected) && Game != null)
            {
                throw new GameInProgressException();
            }

            if (player == null)
            {
                player = new Player { 
                    Name = name,
                    IsModerator = Players.Count == 0,
                };
                Players.Add(player);
            }

            UpdatePages();
            
            player.IsConnected = true;

            OnLobbyUpdated?.Invoke();
            return player;
        }

        public void RemovePlayer(Player player, bool wasKicked = false)
        {
            Players.Remove(player);
            OnPlayerRemoved?.Invoke(player, wasKicked);

            UpdatePages();

            // Assign the first player as moderator if all moderators were removed
            if (Players.Count > 0 && Players.All(p => !p.IsModerator))
            {
                Players.First().IsModerator = true;
            }

            OnLobbyUpdated?.Invoke();
        }

        public void KickPlayer(Player player)
        {
            RemovePlayer(player, wasKicked: true);
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

        public void LobbyUpdated()
        {
            OnLobbyUpdated?.Invoke();
        }
    }
}
