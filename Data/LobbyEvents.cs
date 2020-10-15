using System;

namespace broken_picturephone_blazor.Data
{
    public class LobbyEvents
    {
        public event Action<Lobby> OnLobbyUpdated;
        public void UpdateLobby(Lobby lobby) => OnLobbyUpdated?.Invoke(lobby);
    }
}
