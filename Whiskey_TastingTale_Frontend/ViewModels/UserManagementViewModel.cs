using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class UserManagementViewModel : INotifyPropertyChanged
    {
        private readonly RestApiHelper _helper; 

        private List<User> users = new List<User>();
        private string searchString;
        private string searchOption = "전체";

        public UserManagementViewModel(RestApiHelper helper)
        {
            _helper = helper;
        }

        public List<User> Users
        {
            get => users;
            set {
                users = value;
                OnPropertyChanged(nameof(Users)); 
            }
        }

        public string SearchString
        {
            get => searchString;
            set
            {
                searchString = value;
                OnPropertyChanged(nameof(SearchString)); 
            }
        }


        public string SearchOption
        {
            get => searchOption;
            set
            {
                searchOption = value;
                OnPropertyChanged(nameof(SearchOption));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
        
        public async Task LoadData()
        {
            var response = await _helper.Get(_helper.server_uri + "User");
            if (response == null)
            {
                Users.Clear();
            }else
            {
                var result = JsonConvert.DeserializeObject<List<User>>(response.ToString());
                if (result != null && result.Count != 0) Users = result; 
                else Users.Clear();
            }
        }

        public async Task IsActiveChanged(User user)
        {
            var response = await _helper.Put(_helper.server_uri + "User", user);
            var result = JsonConvert.DeserializeObject<User>(response.ToString());
            if (result != null && result.user_id == user.user_id)
            {
                user.is_active = !user.is_active;

            }
        }

        public bool QuickFilterMethod(User x)
        {
            if (string.IsNullOrWhiteSpace(SearchString))
                return true;

            if ((SearchOption == "전체" || SearchOption == "닉네임") && x.nickname.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if ((SearchOption == "전체" || SearchOption == "이메일") && x.email.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
