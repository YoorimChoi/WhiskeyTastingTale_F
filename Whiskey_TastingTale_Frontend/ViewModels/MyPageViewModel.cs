using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    

    public class MyPageViewModel : INotifyPropertyChanged
    {
        private readonly UserState _userState;
        private readonly WhiskeyState _whiskeyState;

        private List<ReviewWhiskeyDTO> myReviews = new List<ReviewWhiskeyDTO>(); 
        private List<WishWhiskeyDTO> myWishs = new List<WishWhiskeyDTO>();

        public MyPageViewModel(UserState userSate, WhiskeyState whiskeyState)
        {
            _userState = userSate;
            _whiskeyState = whiskeyState;
        }

        public List<ReviewWhiskeyDTO> MyReviews
        {
            get
            {
                return myReviews;
            }
            set
            {
                myReviews = value;
                OnPropertyChanged(nameof(MyReviews));   
            }
        }

        public ReviewWhiskeyDTO MyReview
        {
            get
            {
                return myReviews.OrderBy(x=> x.updated_date).FirstOrDefault();
            }
        }

        public List<WishWhiskeyDTO> MyWishs
        {
            get
            {
                return myWishs;
            }
            set
            {
                myWishs = value;
                OnPropertyChanged(nameof(MyWishs));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadData()
        {
            var reviews_result = await RestApiHelper.Get(RestApiHelper.server_uri + "Review/user/" + _userState.UserId);
            if (reviews_result != null)
            {
                MyReviews = JsonConvert.DeserializeObject<List<ReviewWhiskeyDTO>>(reviews_result.ToString());
            }

            var wishs_result = await RestApiHelper.Get(RestApiHelper.server_uri + "Wish/user/" + _userState.UserId);
            if (wishs_result != null)
            {
                MyWishs = JsonConvert.DeserializeObject<List<WishWhiskeyDTO>>(wishs_result.ToString());
            }
        }

        public async Task ClickedReviewItem(ReviewWhiskeyDTO review)
        {
            _whiskeyState.Selected = null;
            var whiskey_result = await RestApiHelper.Get(RestApiHelper.server_uri + "Whiskey/" + review.whiskey_id);
            if (whiskey_result != null)
            {
                _whiskeyState.Selected = JsonConvert.DeserializeObject<Whiskey>(whiskey_result.ToString());
            }

        }

        public async Task ClickedWishItem(WishWhiskeyDTO wish)
        {
            _whiskeyState.Selected = null;
            var whiskey_result = await RestApiHelper.Get(RestApiHelper.server_uri + "Whiskey/" + wish.whiskey_id);
            if (whiskey_result != null)
            {
                _whiskeyState.Selected = JsonConvert.DeserializeObject<Whiskey>(whiskey_result.ToString());
            }

        }
    }
}
