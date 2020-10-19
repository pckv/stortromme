using broken_picturephone_blazor.Data;

namespace broken_picturephone_blazor.Services {
    public class PlayerService {
        public Player player { get; set; }
        public Lobby lobby { get; set; }

        public void Clear()
        {
            this.player = null;
            this.lobby = null;
        }
    }
}