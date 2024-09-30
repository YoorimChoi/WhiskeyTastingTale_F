using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        private string email;
        private string password;
        private readonly UserState _state;

        public LoginViewModel(UserState state)
        {
            _state = state;
        }


        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> RequestLogin()
        {
            
            var response = await RestApiHelper.Post(RestApiHelper.server_uri + "User/login",
                new User()
                {
                    email = Email,
                    password_hash = Password
                });

            if (response != null)
            {
                LoginResult token = JsonConvert.DeserializeObject<LoginResult>(response.ToString());
                _state.Token = token.token; 
                _state.Email = Email;
                return true; 
            }
            return false; 
        }
    }

}
