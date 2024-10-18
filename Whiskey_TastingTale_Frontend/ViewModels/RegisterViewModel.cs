using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Security.Cryptography;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly RestApiHelper _apiHelper; 
        private string email;
        private string password;
        private string nickname;
        private string emailDuplicationCheck = string.Empty;
        private string nicknameDuplicationCheck = string.Empty;
        private string registerCheck = string.Empty;

        public RegisterViewModel(RestApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public string RegisterCheck
        {
            get => registerCheck;
            set
            {
                registerCheck = value;
                OnPropertyChanged(nameof(RegisterCheck)); 
            }
        }
        public string EmailDuplicationCheck
        {
            get => emailDuplicationCheck;
            set
            {
                emailDuplicationCheck = value;
                OnPropertyChanged(nameof(EmailDuplicationCheck));
            }
        }


        public string NicknameDuplicationCheck
        {
            get => nicknameDuplicationCheck;
            set
            {
                nicknameDuplicationCheck = value;
                OnPropertyChanged(nameof(NicknameDuplicationCheck));
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

        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> HandleRegister()
        {
            if(!NicknameDuplicationCheck.Equals("확인 완료") && !EmailDuplicationCheck.Equals("확인 완료"))
            {
                RegisterCheck = "중복 확인이 필요합니다.";
                return false; 
            }


            var salt = GenerateSalt();
            var passwordHash = HashPassword(Password, salt);

            User user = new User()
            {
                email = Email,
                password_hash = passwordHash,
                nickname = Nickname,
                role = "user",
                is_active = true,
                salt = salt
            };

            var url = _apiHelper.server_uri + "User/register";
            var response = await _apiHelper.Post(url, user);
            var result = JsonConvert.DeserializeObject<User>(response.ToString());

            if (result.user_id != null) return true;
            else
            {
                RegisterCheck = "서비스가 원활하지 못하니, 나중에 시도해주세요.";
                return false;
            }
        }

        public async Task CheckDuplicationEmail()
        {
            var url = _apiHelper.server_uri + "User/duplication/email?email=" + Email;
            var response = await _apiHelper.Get(url);
            var result = JsonConvert.DeserializeObject<bool>(response.ToString());
            if (result == true) EmailDuplicationCheck = "확인 완료";
            else EmailDuplicationCheck = "실패"; 

        }
        public async Task CheckDuplicationNickname()
        {
            var url = _apiHelper.server_uri + "User/duplication/nickname?nickname=" + Nickname;
            var response = await _apiHelper.Get(url);
            var result = JsonConvert.DeserializeObject<bool>(response.ToString());
            if (result == true) NicknameDuplicationCheck = "확인 완료";
            else NicknameDuplicationCheck = "실패";
        }

        public string GenerateSalt(int size = 32)
        {
            var saltBytes = new byte[size];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
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
