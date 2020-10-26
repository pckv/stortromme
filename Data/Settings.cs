using System;
using System.Collections.Generic;

namespace broken_picturephone_blazor.Data
{
    public class Settings
    {
        public bool ShouldUpdatePagesDymanically { get; set; }
        private int maxPlayers;
        public int MaxPlayers 
        { 
            get {
                return maxPlayers;
            }
            set {
                maxPlayers = value;
                OnSettingsUpdated?.Invoke();
            }
        }

        private int pages;
        public int Pages
        { 
            get {
                return pages;
            }
            set {
                ShouldUpdatePagesDymanically = false;
                pages = value;
                OnSettingsUpdated?.Invoke();
            }
        }

        public ContentType FirstPageType { get; set; }
        public IList<ContentType> PageTypePattern { get; set; }

        public event Action OnSettingsUpdated;

        public Settings()
        {
            ShouldUpdatePagesDymanically = true;
            maxPlayers = 10;
            pages = 1;
            PageTypePattern = new List<ContentType>();

            // TODO: add this to settings in lobby
            PageTypePattern.Add(ContentType.Text);
        }

        public void UpdateDynamicPages(int pages)
        {
            this.pages = pages;
            OnSettingsUpdated?.Invoke();
        }
    }
}