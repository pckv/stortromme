using System;
using System.Collections.Generic;
using System.Linq;

namespace broken_picturephone_blazor.Data
{
    public class LobbyService 
    {
        private IList<Lobby> lobbies;

        public LobbyService()
        {
            lobbies = new List<Lobby>();
        }

        public Lobby CreateOrUseLobby(string name)
        {
            Lobby lobby = lobbies.FirstOrDefault(l => l.Name == name);
            if (lobby == null)
            {
                lobby = new Lobby { Name = name };
                lobbies.Add(lobby);
            }

            return lobby;
        }

        public void LeaveLobby(Player player, Lobby lobby)
        {
            lobby.RemovePlayer(player);

            // Remove lobby from service when empty
            if (lobby.Players.Count == 0)
            {
                lobbies.Remove(lobby);
            }
        }
    }
}