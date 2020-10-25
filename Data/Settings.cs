using System.Collections.Generic;

namespace broken_picturephone_blazor.Data
{
    public class Settings
    {
        public int MaxPlayers { get; set; }
        public int Pages { get; set; }
        public ContentType FirstPageType { get; set; }
        public IList<ContentType> PageTypePattern { get; set; }

        public Settings()
        {
            PageTypePattern = new List<ContentType>();

            // TODO: add this to settings in lobby
            PageTypePattern.Add(ContentType.Text);
        }
    }
}