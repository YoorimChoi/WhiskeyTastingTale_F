using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        private readonly SelectState _whiskeyState;
        private readonly SignalRService _signalRService;
        private readonly UserState _userState;
        private readonly RestApiHelper _helper; 

        private int notificationNum = 3;
        private bool isOpen = false;
        private bool isInit = false;

        private List<Notification> notifications = new List<Notification>();

        public NotificationViewModel(SelectState whiskeyState, RestApiHelper helper, UserState userState, IConfiguration configuration)
        {
            _whiskeyState = whiskeyState;
            _helper = helper;
            _userState = userState;

            _signalRService = new SignalRService(userState, configuration); 
            _signalRService.OnMessageReceived += RecieveSignal;
        }

        public Action OnStateChange { get; set; }

        public int NotificationNum{
            get => notificationNum;
            set
            {
                notificationNum = value;
                OnPropertyChanged(nameof(NotificationNum));
            }
        }

        public bool IsOpen
        {
            get => isOpen;
            set
            {
                isOpen = value;
                OnPropertyChanged(nameof(IsOpen));
            }
        }

        public List<Notification> Notifications
        {
            get => notifications;
            set
            {
                notifications = value;
                OnPropertyChanged(nameof(Notifications)); 

            }
        }


        private int page = 1;
        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                page = value;
                OnPropertyChanged(nameof(Page));
            }
        }

        private int totalPage = 1;
        public int TotalPage
        {
            get
            {
                return totalPage;
            }
            set
            {
                totalPage = value;
                OnPropertyChanged(nameof(TotalPage));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task initSignalR()
        {
            if (!isInit)
            {
                await _signalRService.StartConnectionAsync();
                isInit = true;
            }
        }

        public async Task LoadData()
        {
            var response = await _helper.Get(_helper.server_uri + "Notification/user/" + _userState.UserId + "?page=" + Page);
            var result = JsonConvert.DeserializeObject<NotificationPageDTO>(response.ToString());
            if (result != null && result.notifications != null)
            {
                Notifications = result.notifications;
                Page = result.page;
                TotalPage = result.totalPages; 
            }else
            {
                Notifications = new List<Notification>();
                Page = 1;
                TotalPage = 1;
            }
        }

        public void OpenNotificationList()
        {
            IsOpen = true;
        }

        public async Task ChangeNotificationRead(Notification notification)
        {
            var resnponse = await _helper.Post(_helper.server_uri + "Notification/read/" + notification.notification_id, null);

            notification.is_read = true;
            OnPropertyChanged(nameof(Notifications));
            OnStateChange?.Invoke();
        }

        public async Task ChangeAllRead()
        {
            foreach(var notification in Notifications)
            {
                var resnponse = await _helper.Post(_helper.server_uri + "Notification/read/" + notification.notification_id, null);
                notification.is_read = true;
            }

            OnPropertyChanged(nameof(Notifications));
            OnStateChange?.Invoke();
        }

        public async Task DeleteAll()
        {
            foreach (var notification in Notifications)
            {
                var resnponse = await _helper.Delete(_helper.server_uri + "Notification/delete/" + notification.notification_id);
            }

            if(Page != 1) Page = Page - 1;
            await LoadData(); 
        }

        public async Task<string> HandleItemClick(Notification notification)
        {
            IsOpen = false;
            await ChangeNotificationRead(notification);

            if (!string.IsNullOrEmpty(notification.notification_type) && notification.notification_type.Equals("addwhiskey"))
            {
                _whiskeyState.SelectedWhiskey = new Whiskey()
                {
                    whiskey_id = notification.related_entity_id?? 0
                };
            }

            return notification.target_url;
        }

        public void RecieveSignal(Notification message)
        {
            Notifications.Add(message);
            OnPropertyChanged(nameof(Notifications));
            OnStateChange?.Invoke(); 
        }

        public async Task PageChanged(int i)
        {
            Page = i;
            await LoadData();
        }
    }
}
