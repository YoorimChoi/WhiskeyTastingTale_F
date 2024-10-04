namespace Whiskey_TastingTale_Frontend.Services
{
    public class UserState
    {
        private string token = null;
        private string email = null;
        private int user_id = -1;
        private string nickname = null;
        private string role = null; 

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

        public int UserId
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
                NotifyStateChanged();
            }
        }

        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
                NotifyStateChanged();
            }
        }

        public string Nickname
        {
            get
            {
                return nickname;
            }
            set
            {
                nickname = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void init()
        {
            Token = null;
            UserId = -1;
            Role = null;
            Nickname = null;
            Email = null; 
        }
    }

}
