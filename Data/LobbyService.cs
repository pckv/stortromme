using System.Collections.Generic;
using System.Linq;

namespace broken_picturephone_blazor.Data
{
    public class LobbyService 
    {
        public IList<Lobby> lobbies;

        public LobbyService()
        {
            lobbies = new List<Lobby>();
        }

        public Lobby CreateOrJoinLobby(string playerName, string lobbyName)
        {
            Lobby lobby = lobbies.FirstOrDefault(l => l.Name == lobbyName);
            if (lobby == null)
            {
                lobby = new Lobby { Name = lobbyName };
                lobbies.Add(lobby);
            }
            
            Player player = lobby.Players.FirstOrDefault(p => p.IsNameEqual(playerName));
            if (player == null) 
            {
                player = new Player {
                    Name = playerName,
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

            lobby.Players.Add(player);

            return lobby;
        }
    }
}