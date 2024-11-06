using Microsoft.JSInterop;
using System.Data;
using System.Text.Json;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Frontend.Services
{
    public class UserState
    {
        private readonly IJSRuntime _js;
        public UserState(IJSRuntime js)
        {
            _js = js;
        }

        private User user = new User()
        {
            user_id = -1,
            role = null,
            nickname = null,
            email = null
        }; 
        private string token = null;


        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                _ = SaveStateAsync(); // 상태 변경 시 저장
            }
        }

        public User User
        {
            get { return user; }
            set
            {
                user = value;
                _ = SaveStateAsync(); // 상태 변경 시 저장
                NotifyStateChanged();
            }
        }

        public string Email { get => this.User.email; }
        public int UserId { get => this.User.user_id; }
        public string Role { get => this.User.role; }
        public string Nickname { get => this.User.nickname; }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void init()
        {
            Token = null;
            User = new User()
            {
                user_id = -1,
                role = null,
                nickname = null,
                email = null
            };
        }

        public async Task SaveStateAsync()
        {
            var state = JsonSerializer.Serialize(new UserStateModel { user = user, token = token});
            await _js.InvokeVoidAsync("sessionStorage.setItem", "userState", state);
        }
        public async Task LoadStateAsync()
        {
            var stateJson = await _js.InvokeAsync<string>("sessionStorage.getItem", "userState");
            if (!string.IsNullOrEmpty(stateJson))
            {
                var loadedState = JsonSerializer.Deserialize<UserStateModel>(stateJson);
                if (loadedState != null)
                {
                    token = loadedState.token;
                    user = loadedState.user;
                }
            }
        }
        private class UserStateModel
        {
            public string token { get; set; }
            public User user { get; set; } = new User()
            {
                user_id = -1,
                role = null,
                nickname = null,
                email = null
            };
        }
    }

}
