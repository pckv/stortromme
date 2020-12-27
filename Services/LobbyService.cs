using System;
using System.Collections.Generic;
using System.Linq;
using stortromme.Data;

namespace stortromme.Services
{
    public class LobbyService 
    {
        private IList<Lobby> lobbies;

        public LobbyService()
        {
            lobbies = new List<Lobby>();
        }

        public (Lobby, Player) CreateOrJoinLobby(string lobbyName, string playerName)
        {
            Lobby lobby = lobbies.FirstOrDefault(l => l.Name == lobbyName);
            if (lobby == null)
            {
                lobby = new Lobby { Name = lobbyName, Settings = new Settings() };
                lobbies.Add(lobby);
            }

            // Throw when a connected player with the same name is already in
            // the lobby
            if (lobby.HasConnectedPlayer(playerName))
            {
                throw new PlayerExistsException();
            }

            Player player = lobby.AddPlayer(playerName);
            return (lobby, player);
        }

        public void LeaveLobby(Player player, Lobby lobby)
        {
            lobby.RemovePlayer(player);
            CleanupLobby(lobby);
        }

        public void DisonnectLobby(Player player, Lobby lobby)
        {
            lobby.DisconnectPlayer(player);
            CleanupLobby(lobby);
        }

        private void CleanupLobby(Lobby lobby)
        {
            if (lobby.Players.Count == 0 || lobby.Players.All(p => !p.IsConnected))
            {
                lobbies.Remove(lobby);
            }
        }

        public LobbyStats Stats => new LobbyStats
        {
            Lobbies = lobbies.Count,
            Players = lobbies.Sum(l => l.Players.Count),
        };
    }

    public class LobbyStats
    {
        public int Lobbies { get; set; }
        public int Players { get; set; }
    }
}