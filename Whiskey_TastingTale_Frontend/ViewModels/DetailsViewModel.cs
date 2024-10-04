using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        private readonly WhiskeyState _whiskeyState;
        private readonly UserState _userState;

        public DetailsViewModel(WhiskeyState whiskeyState, UserState userState)
        {
            _whiskeyState = whiskeyState;
            _userState = userState;
        }

        private Whiskey selected = new Whiskey();
        private List<ReviewUserDTO> reviews = new List<ReviewUserDTO>();
        private ReviewUserDTO firstReview = new ReviewUserDTO();
        private int selectedRating = 0;
        private string review = String.Empty; 

        public Whiskey Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public ReviewUserDTO FirstReview
        {
            get => firstReview;
            set
            {
                firstReview = value;
                OnPropertyChanged(nameof(FirstReview));
            }
        }

        public List<ReviewUserDTO> Reviews
        {
            get => reviews;
            set
            {
                reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        public int SelectedRating
        {
            get { return selectedRating; }
            set
            {
                selectedRating = value;
                OnPropertyChanged(nameof(SelectedRating));
            }
        }

        public string Review
        {
            get { return review; }
            set
            {
                review = value;
                OnPropertyChanged(nameof(Review));
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
            var whiskey = await RestApiHelper.Get(RestApiHelper.server_uri + "Whiskey/" + Selected.whiskey_id);
            if (whiskey != null)
            {
                Selected = JsonConvert.DeserializeObject<Whiskey>(whiskey.ToString());
            }

            var response = await RestApiHelper.Get(RestApiHelper.server_uri + "Review/whiskey/" + Selected.whiskey_id);

            if (response != null)
            {
                Reviews = JsonConvert.DeserializeObject<List<ReviewUserDTO>>(response.ToString());
                FirstReview = Reviews.FirstOrDefault(); 
            }
        }

        public async Task AddReview()
        {
            var response = await RestApiHelper.Post(RestApiHelper.server_uri + "Review/", new Review
            {
                user_id = _userState.UserId,
                whiskey_id = Selected.whiskey_id,
                rating = SelectedRating, 
                review_text = Review
            });
        }
    }

}
