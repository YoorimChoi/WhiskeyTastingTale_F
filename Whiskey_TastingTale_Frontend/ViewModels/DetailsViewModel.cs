using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Entities;
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

        private Whiskey selectedWhiskey = new Whiskey();
        private List<ReviewUserDTO> reviews = new List<ReviewUserDTO>();
        private ReviewUserDTO firstReview = new ReviewUserDTO();

        private Review writingReview = new Review();
        private Wish wishCheck;

        public Whiskey SelectedWhiskey
        {
            get => selectedWhiskey;
            set
            {
                selectedWhiskey = value;
                OnPropertyChanged(nameof(SelectedWhiskey));
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
            get { return writingReview.rating; }
            set
            {
                writingReview.rating = value;
                OnPropertyChanged(nameof(SelectedRating));
            }
        }
        public string ReviewText
        {
            get { return writingReview.review_text; }
            set
            {
                writingReview.review_text = value;
                OnPropertyChanged(nameof(ReviewText));
            }
        }
        public Wish WishCheck
        {
            get
            {
                return wishCheck;
            }
            set
            {
                wishCheck = value;
                OnPropertyChanged(nameof(WishCheck));
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
            SelectedWhiskey = _whiskeyState.Selected;
            var whiskey = await RestApiHelper.Get(RestApiHelper.server_uri + "Whiskey/" + SelectedWhiskey.whiskey_id);
            if (whiskey != null)
            {
                SelectedWhiskey = JsonConvert.DeserializeObject<Whiskey>(whiskey.ToString());
            }

            var review = await RestApiHelper.Get(RestApiHelper.server_uri + "Review/whiskey/" + SelectedWhiskey.whiskey_id);

            if (review != null)
            {
                var result = JsonConvert.DeserializeObject<ReviewUserPageDTO>(review.ToString());
                Reviews = result.reviews;
                FirstReview = Reviews.FirstOrDefault();
                TotalPage = result.totalPages;
                TotalCount = result.totalCount;
            }

            var wish_response = await RestApiHelper.Get(RestApiHelper.server_uri + "Wish/" + _userState.UserId + "/" + SelectedWhiskey.whiskey_id);

            if (wish_response != null)
            {
                WishCheck = JsonConvert.DeserializeObject<Wish>(wish_response.ToString());
            }
        }

        public async Task AddReview()
        {
            var response = await RestApiHelper.Post(RestApiHelper.server_uri + "Review/", new Review
            {
                user_id = _userState.UserId,
                whiskey_id = SelectedWhiskey.whiskey_id,
                rating = SelectedRating, 
                review_text = ReviewText
            });
        }

        public async Task changeWishCheck()
        {
            if (WishCheck != null)
            {
                var response = await RestApiHelper.Delete(RestApiHelper.server_uri + "Wish/" + WishCheck.wish_id);
                if (JsonConvert.DeserializeObject<Wish>(response.ToString()) != null)
                {
                    WishCheck = null;
                }
            }
            else
            {
                var response = await RestApiHelper.Post(RestApiHelper.server_uri + "Wish/", new Wish
                {
                    user_id = _userState.UserId,
                    whiskey_id = SelectedWhiskey.whiskey_id
                });
                if (response != null)
                {
                    WishCheck = JsonConvert.DeserializeObject<Wish>(response.ToString());
                }
            }

        }

        public async Task ReviewPagesChanged(int page)
        {
            Page = page;
            await LoadReviewData(); 
        }

        public async Task LoadReviewData()
        {
            var review = await RestApiHelper.Get(RestApiHelper.server_uri + "Review/whiskey/" + SelectedWhiskey.whiskey_id +"?page="+Page);

            if (review != null)
            {
                var result = JsonConvert.DeserializeObject<ReviewUserPageDTO>(review.ToString());
                Reviews = result.reviews;
                FirstReview = Reviews.FirstOrDefault();
                TotalPage = result.totalPages; 
                TotalCount = result.totalCount; 
            }

        }
    }

}
