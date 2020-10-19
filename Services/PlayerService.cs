using broken_picturephone_blazor.Data;

namespace broken_picturephone_blazor.Services {
    public class PlayerService {
        public Player us { get; set; }
        public Lobby lobby { get; set; }

        public void Clear()
        {
            this.us = null;
            this.lobby = null;
        }
    }
}