using Microsoft.AspNetCore.SignalR.Client;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Frontend.Services
{

    public class SignalRService : IAsyncDisposable
    {
        private readonly UserState _userState;
        private readonly IConfiguration _configuration;
        private string server_uri; 

        private HubConnection _connection;

        public event Action<Notification> OnMessageReceived; // 메시지가 수신되었을 때 발생하는 이벤트

        public SignalRService(UserState userState, IConfiguration configuration)
        {
            _userState = userState; 
            _configuration = configuration;
            server_uri = _configuration["BackendSettings:ServerUrl"];
        }
        public async Task StartConnectionAsync()
        {
            var hubUrl = server_uri + "notificationHub"; 
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.Headers["user_id"] = _userState.UserId.ToString(); // 헤더에 userId 추가
                })
                .WithAutomaticReconnect()
                .Build();


            _connection.ServerTimeout = TimeSpan.FromSeconds(30);
            _connection.HandshakeTimeout = TimeSpan.FromSeconds(15);
            _connection.KeepAliveInterval = TimeSpan.FromSeconds(15);

            // 서버로부터 ReceiveNotification 메시지를 수신할 때 이벤트 발생
            _connection.On<Notification>("ReceiveNotification", (message) =>
            {
                OnMessageReceived?.Invoke(message); // 메시지가 오면 구독자에게 알림
            });

            await _connection.StartAsync();
            Console.WriteLine("SignalR 서버에 연결되었습니다.");
        }

        // 메시지를 서버로 전송
        public async Task SendMessageAsync(string message)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.SendAsync("SendNotification", message);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
            }
        }
    }

}
