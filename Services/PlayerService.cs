using broken_picturephone_blazor.Data;

namespace broken_picturephone_blazor.Services {
    public class PlayerService {
        public Player Player { get; set; }
        public Lobby Lobby { get; set; }
        public bool WasKicked { get; set; }

        public void Clear()
        {
            this.Player = null;
            this.Lobby = null;
        }
    }
}