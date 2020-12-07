using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace broken_picturephone_blazor.Data
{
    public class Settings
    {
        public bool ShouldUpdatePagesDymanically { get; set; }
        private int maxPlayers;
        public int MaxPlayers 
        { 
            get => maxPlayers;
            set {
                maxPlayers = value;
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
                pages = value;
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
            maxPlayers = 10;
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