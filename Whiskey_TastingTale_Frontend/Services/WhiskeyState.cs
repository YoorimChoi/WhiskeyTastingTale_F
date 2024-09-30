using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Frontend.Services
{
    public class WhiskeyState
    {
        private Whiskey selected;

        public Whiskey Selected
        {
            get => selected;
            set
            {
                selected = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
