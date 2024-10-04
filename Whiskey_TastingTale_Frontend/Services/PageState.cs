using Microsoft.AspNetCore.Components;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Frontend.Services
{
    public class PageState
    {
        private string priviousPage = String.Empty; 
        public string PriviousPage
        {
            get
            {
                return priviousPage;
            }
            set
            {
                priviousPage = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

    }


       
}
