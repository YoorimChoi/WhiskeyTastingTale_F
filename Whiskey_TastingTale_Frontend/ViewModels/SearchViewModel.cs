using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly UserState _userState;
        private readonly SelectState _selectState;
        private readonly RestApiHelper _apiHelper; 
        public SearchViewModel(SelectState selectState, RestApiHelper apiHelper, UserState userState)
        {
            _selectState = selectState;
            _userState = userState;
            _apiHelper = apiHelper;
        }

        private List<Whiskey> searachResult = new List<Whiskey>();
        public List<Whiskey> SearchResult {
            get => searachResult; 
            set
            {
                searachResult = value;
                OnPropertyChanged(nameof(SearchResult));
            }
        }

        private bool isConnectedWithServer = false;

        public bool IsConnectedWithServer
        {
            get => isConnectedWithServer;
            set
            {
                isConnectedWithServer = value;
                OnPropertyChanged(nameof(IsConnectedWithServer));
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

        private int totalCount = 1;
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
            set
            {
                totalCount = value;
                OnPropertyChanged(nameof(TotalCount));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadData()
        {
            string url = _apiHelper.server_uri + "Whiskey/name/"+ _selectState.SearchWord + "?page=" + Page; 
            var response = await _apiHelper.Get(url);
            if (!response.ToString().StartsWith("[ERR]"))
            {
                var result = JsonConvert.DeserializeObject<WhiskeyPageDTO>(response.ToString());
                if (result != null && result.page != 0)
                {
                    SearchResult = result.whiskeys;
                    TotalPage = result.totalPages;
                    TotalCount = result.totalCount;
                }
                IsConnectedWithServer = true;
            }
            else
            {
                IsConnectedWithServer = false;
                SearchResult = new List<Whiskey>();
                TotalPage = 1;
                TotalCount = 1;
                Page = 1; 
            }
        }

        public async Task PageChanged(int i)
        {
            Page = i; 
            await LoadData(); 
        }

        public async Task ClickedItem(Whiskey product)
        {
            _selectState.SelectedWhiskey = product; 
        }
    }
}
