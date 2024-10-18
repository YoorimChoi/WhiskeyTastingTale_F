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
        private readonly RestApiHelper _apiHelper; 

        private List<ReviewWhiskeyDTO> myReviews = new List<ReviewWhiskeyDTO>(); 
        private List<WishWhiskeyDTO> myWishs = new List<WishWhiskeyDTO>();

        public MyPageViewModel(UserState userSate, WhiskeyState whiskeyState, RestApiHelper apiHelper)
        {
            _userState = userSate;
            _whiskeyState = whiskeyState;
            _apiHelper = apiHelper;
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
        private int reviewPage = 1;
        public int ReviewPage
        {
            get
            {
                return reviewPage;
            }
            set
            {
                reviewPage = value;
                OnPropertyChanged(nameof(ReviewPage));
            }
        }

        private int reviewTotalPage = 1;
        public int ReviewTotalPage
        {
            get
            {
                return reviewTotalPage;
            }
            set
            {
                reviewTotalPage = value;
                OnPropertyChanged(nameof(ReviewTotalPage));
            }
        }

        private int reviewTotalCount = 1;
        public int ReviewTotalCount
        {
            get
            {
                return reviewTotalCount;
            }
            set
            {
                reviewTotalCount = value;
                OnPropertyChanged(nameof(ReviewTotalCount));
            }
        }
        private int wishPage = 1;
        public int WishPage
        {
            get
            {
                return wishPage;
            }
            set
            {
                wishPage = value;
                OnPropertyChanged(nameof(WishPage));
            }
        }

        private int wishTotalPage = 1;
        public int WishTotalPage
        {
            get
            {
                return wishTotalPage;
            }
            set
            {
                wishTotalPage = value;
                OnPropertyChanged(nameof(WishTotalPage));
            }
        }

        private int wishTotalCount = 1;
        public int WishTotalCount
        {
            get
            {
                return wishTotalCount;
            }
            set
            {
                wishTotalCount = value;
                OnPropertyChanged(nameof(WishTotalCount));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadData()
        {
            await LoadReviewData();
            await LoadWishData();
        }

        public async Task ClickedReviewItem(ReviewWhiskeyDTO review)
        {
            _whiskeyState.Selected = null;
            var whiskey_result = await _apiHelper.Get(_apiHelper.server_uri + "Whiskey/id/" + review.whiskey_id);
            if (whiskey_result != null)
            {
                _whiskeyState.Selected = JsonConvert.DeserializeObject<Whiskey>(whiskey_result.ToString());
            }

        }

        public async Task ClickedWishItem(WishWhiskeyDTO wish)
        {
            _whiskeyState.Selected = null;
            var whiskey_result = await _apiHelper.Get(_apiHelper.server_uri + "Whiskey/id/" + wish.whiskey_id);
            if (whiskey_result != null)
            {
                _whiskeyState.Selected = JsonConvert.DeserializeObject<Whiskey>(whiskey_result.ToString());
            }

        }

        public async Task LoadReviewData()
        {
            var reviews_result = await _apiHelper.Get(_apiHelper.server_uri + "Review/user?page=" + ReviewPage);
            if (!string.IsNullOrEmpty(reviews_result.ToString()))
            {
                var result = JsonConvert.DeserializeObject<ReviewWhiskeyPageDTO>(reviews_result.ToString());
                MyReviews = result.reviews;
                ReviewTotalCount = result.totalCount;
                ReviewTotalPage = result.totalPages;

            }
        }

        public async Task LoadWishData()
        {
            var wishs_result = await _apiHelper.Get(_apiHelper.server_uri + "Wish/user?page=" + WishPage);
            if (!string.IsNullOrEmpty(wishs_result.ToString()))
            {
                var result = JsonConvert.DeserializeObject<WishWhiskeyPageDTO>(wishs_result.ToString());
                MyWishs = result.wishs;
                WishTotalCount = result.totalCount;
                WishTotalPage = result.totalPages;
            }
        }

        public async Task ReviewPagesChanged(int page)
        {
            ReviewPage = page; 
            await LoadReviewData();
        }

        public async Task WishPagesChanged(int page)
        {
            WishPage = page;
            await LoadWishData();
        }

        public async Task<bool> ClickedDeleteReview(ReviewWhiskeyDTO review)
        { 
            var response = await _apiHelper.Delete(_apiHelper.server_uri + "Review/" + review.review_id);
            var result = JsonConvert.DeserializeObject<Review>(response.ToString());
            if (review.review_id == result.review_id)
            {
                await LoadReviewData();
                return true;
            }
            else return false;
        }
    }
}
