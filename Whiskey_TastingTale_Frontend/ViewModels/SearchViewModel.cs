using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly WhiskeyState _whiskeyState; 
        public SearchViewModel(WhiskeyState whiskeyState)
        {
            _whiskeyState = whiskeyState;
        }

        private string searchWord;
        private List<Whiskey> searachResult = new List<Whiskey>() {
            new Whiskey()
            {
                whiskey_name = "name",
                alcohol_degree = 70.3, 
                img_index = "images/icon.png", 
                details = "내맘대로적을거임 짜증남 프론트", 
                maker = "나당나당뀨뀨스키핑"
            },
            new Whiskey()
            {
                whiskey_name = "name1",
                alcohol_degree = 70.3,
                img_index = "images/icon.png",
                details = "내맘대로적을거임 짜증남 프론트",
                maker = "나당나당뀨뀨스키핑"
            },
            new Whiskey()
            {
                whiskey_name = "name2",
                alcohol_degree = 70.3,
                img_index = "images/icon.png",
                details = "내맘대로적을거임 짜증남 프론트",
                maker = "나당나당뀨뀨스키핑"
            },
            new Whiskey()
            {
                whiskey_name = "name3",
                alcohol_degree = 70.3,
                img_index = "images/icon.png",
                details = "내맘대로적을거임 짜증남 프론트",
                maker = "나당나당뀨뀨스키핑"
            }

        } ;
        public List<Whiskey> SearchResult {
            get => searachResult; 
            set
            {
                searachResult = value;
                OnPropertyChanged(nameof(SearchResult));
            }
        }

        public string SearchWord
        {
            get => searchWord;
            set
            {
                searchWord = value;
                OnPropertyChanged(nameof(SearchWord));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadData()
        {

            var response = await RestApiHelper.Get(RestApiHelper.server_uri + "Whiskey/name/"+SearchWord);
               

            if (response != null)
            {
                SearchResult = JsonConvert.DeserializeObject<List<Whiskey>>(response.ToString());
            }
        }

        public async Task ClickedItem(Whiskey product)
        {
            _whiskeyState.Selected = product; 
        }
    }
}
