using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        private readonly WhiskeyState _whiskeyState; 
        public DetailsViewModel(WhiskeyState whiskeyState)
        {
            _whiskeyState = whiskeyState;
        }

        private Whiskey selected = new Whiskey();
        private List<Review> reviews = new List<Review>();
        private Review firstReview = new Review(); 

        public Whiskey Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public Review FirstReview
        {
            get => firstReview;
            set
            {
                firstReview = value;
                OnPropertyChanged(nameof(FirstReview));
            }
        }

        public List<Review> Reviews
        {
            get => reviews;
            set
            {
                reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadData()
        {
            Selected = _whiskeyState.Selected;
            var response = await RestApiHelper.Get(RestApiHelper.server_uri + "Review/" + Selected.whiskey_id);

            if (response != null)
            {
                Reviews = JsonConvert.DeserializeObject<List<Review>>(response.ToString());
                FirstReview = Reviews.FirstOrDefault(); 
            }
        }
    }

}
