using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class WhiskeyManagementViewMdoel : INotifyPropertyChanged
    {
        private readonly RestApiHelper _helper; 

        private List<Whiskey> whiskeys = new List<Whiskey>();
        private string searchString; 
        public WhiskeyManagementViewMdoel(RestApiHelper helepr)
        {
            _helper = helepr;
        }
        public List<Whiskey> Whiskeys
        {
            get => whiskeys; set
            {
                whiskeys = value;
                OnPropertyChanged(nameof(Whiskeys));
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public async Task LoadData()
        {
            var response = await _helper.Get(_helper.server_uri + "Whiskey/all");
            var result = JsonConvert.DeserializeObject<List<Whiskey>>(response.ToString()); 
            if(result != null) { whiskeys = result; }
            else result = new List<Whiskey>();
        }

        public async Task<bool> HandleDeleteRequest(Whiskey whiskey)
        {
            var response = await _helper.Delete(_helper.server_uri + "Whiskey/" + whiskey.whiskey_id);
            var result = JsonConvert.DeserializeObject<Whiskey>(response.ToString());
            if (result != null && whiskey.whiskey_id == result.whiskey_id)
            {
                Whiskeys.Remove(whiskey);
                return true;
            }
            else return false; 
        }

        public async Task<bool> InfoChanged(Whiskey whiskey)
        {
            var response = await _helper.Put(_helper.server_uri + "Whiskey", whiskey);
            var result = JsonConvert.DeserializeObject<Whiskey>(response.ToString());
            if (result != null && result.whiskey_id == whiskey.whiskey_id) return true;
            else return false;
        }

        public bool QuickFilterMethod(Whiskey x)
        {
            if (string.IsNullOrWhiteSpace(SearchString))
                return true;

            if (x.whiskey_name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
