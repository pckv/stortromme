using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace stortromme.Data
{
    public class Settings
    {
        public const int MAX_PLAYERS = 64;

        public bool ShouldUpdatePagesDymanically { get; set; }
        private int maxPlayers;
        public int MaxPlayers 
        { 
            get => maxPlayers;
            set {
                maxPlayers = Math.Clamp(value, 1, MAX_PLAYERS);
                OnSettingsUpdated?.Invoke();
            }
        }

        private int pages;
        public int Pages
        { 
            get => pages;
            set
            {
                ShouldUpdatePagesDymanically = false;
                pages = Math.Clamp(value, 1, MAX_PLAYERS);
                OnSettingsUpdated?.Invoke();
            }
        }

        private ContentType firstPageType;
        public ContentType FirstPageType { 
            get => firstPageType;
            set
            {
                firstPageType = value;
                OnSettingsUpdated?.Invoke();
            }
        }
        public ObservableCollection<ContentType> PageTypePattern { get; }
        private void pageTypePatternChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnSettingsUpdated?.Invoke();
        }

        public event Action OnSettingsUpdated;

        public Settings()
        {
            ShouldUpdatePagesDymanically = true;
            maxPlayers = MAX_PLAYERS;
            pages = 1;
            PageTypePattern = new ObservableCollection<ContentType>();
            PageTypePattern.CollectionChanged += pageTypePatternChanged;

            // TODO: add this to settings in lobby
            FirstPageType = ContentType.Text;
            PageTypePattern.Add(ContentType.Image);
        }

        public void UpdateDynamicPages(int pages)
        {
            this.pages = pages;
            OnSettingsUpdated?.Invoke();
        }
    }
}