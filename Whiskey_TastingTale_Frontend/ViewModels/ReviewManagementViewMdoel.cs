using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class ReviewManagementViewMdoel : INotifyPropertyChanged
    {

        private readonly RestApiHelper _helper; 

        private List<ReviewUserWhiskeyDTO> reviews;
        private int page = 1;
        private int pageSize = 10;
        private int totalPage = 1;
        private string searchOption;
        private string searchString; 

        public ReviewManagementViewMdoel(RestApiHelper helper)
        {
            _helper = helper;
        }

        public List<ReviewUserWhiskeyDTO> Reviews
        {
            get => reviews;
            set
            {
                reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        public int Page
        {
            get => page;
            set
            {
                page = value;
                OnPropertyChanged(nameof(Page));
            }
        }

        public int TotalPage
        {
            get => totalPage;
            set
            {
                totalPage = value;
                OnPropertyChanged(nameof(TotalPage));
            }
        }

        public string SearchOption
        {
            get => searchOption;
            set
            {
                searchOption = value;
                OnPropertyChanged(nameof(SearchOption));
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadData()
        {
            var response = await _helper.Get(_helper.server_uri + "Review/all?page="+Page);
            var result = JsonConvert.DeserializeObject<ReviewUserWhiskeyPageDTO>(response.ToString()); 
            if(result!= null && result.reviews != null)
            {
                Reviews = result.reviews; 
                Page = result.page;
                TotalPage = result.totalPages;
            }
        }

        public async Task LoadDataBySearch()
        {
            var response = await _helper.Get(_helper.server_uri + "Review/search/"+SearchOption+"/"+ SearchString+"?page="+Page);
            var result = JsonConvert.DeserializeObject<ReviewUserWhiskeyPageDTO>(response.ToString());
            if (result != null && result.reviews != null)
            {
                Reviews = result.reviews;
                Page = result.page;
                TotalPage = result.totalPages;
            }else
            {
                Reviews = new List<ReviewUserWhiskeyDTO>();
                Page = 1; 
                TotalPage = 1; 
            }
        }

        public async Task PageChanged(int i)
        {
            Page = i;
            if (!String.IsNullOrEmpty(SearchString) && !String.IsNullOrEmpty(SearchOption)) await LoadDataBySearch();
            else await LoadData();
        }

        public async Task HandleChangedSearchString(KeyboardEventArgs args)
        {
            if (args.Key == "Enter" && !String.IsNullOrEmpty(SearchString) && !String.IsNullOrEmpty(SearchOption) )
            {
                await LoadDataBySearch(); 
            }
        }

        public async Task SearchClear()
        {
            SearchOption = String.Empty;
            SearchString = String.Empty;
            await LoadData();
        }

        public async Task<bool> HandleDeleteReview(ReviewUserWhiskeyDTO review)
        {
            var response = await _helper.Delete(_helper.server_uri + "Review/" + review.review_id);
            var result = JsonConvert.DeserializeObject<Review>(response.ToString());
            if (review.review_id == result.review_id)
            {
                if (!String.IsNullOrEmpty(SearchString) && !String.IsNullOrEmpty(SearchOption)) await LoadDataBySearch();
                else await LoadData();
                return true;
            }
            else return false;
        }
    }
}
