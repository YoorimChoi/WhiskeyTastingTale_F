using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        private string email;
        private string password;
        private string loginResult; 
        private readonly UserState _state;
        private readonly RestApiHelper _apiHelper; 

        public LoginViewModel(UserState state, RestApiHelper apiHelper)
        {
            _state = state;
            _apiHelper = apiHelper;
        }

        public string LoginResult
        {
            get => loginResult; 
            set
            {
                loginResult = value;
                OnPropertyChanged(nameof(LoginResult)); 
            }
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
            var salt_response = await _apiHelper.Get(_apiHelper.server_uri + "User/salt/" + Email);
            string salt = salt_response.ToString();

            var password_hash = HashPassword(Password, salt); 

            var response = await _apiHelper.Post(_apiHelper.server_uri + "User/login",
                new User()
                {
                    email = Email,
                    password_hash = password_hash
                });

            LoginDTO result = JsonConvert.DeserializeObject<LoginDTO>(response.ToString());
            if (!string.IsNullOrEmpty(result.token))
            {
                _state.Token = result.token;
                _state.Email = result.email;
                _state.UserId = result.user_id;
                _state.Nickname = result.nickname;
                _state.Role = result.role;
                return true;
            }
            LoginResult = "이메일 혹은 비밀번호가 잘못되었습니다.";
            return false; 
        }

        public string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Concat(password, salt);
                var saltedPasswordByte = System.Text.Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordByte);

                return Convert.ToBase64String(hashBytes);
            }
        }
    }

}
