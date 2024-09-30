namespace Whiskey_TastingTale_Frontend.Services
{
    public class UserState
    {
        private string token = null;
        private string email = null; 
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                NotifyStateChanged();
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
