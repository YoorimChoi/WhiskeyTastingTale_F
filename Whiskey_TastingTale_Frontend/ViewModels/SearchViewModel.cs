﻿using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Whiskey_TastingTale_Backend.API.DTOs;
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

        private string searchWord = String.Empty;
        private List<Whiskey> searachResult = new List<Whiskey>();
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
            string url = RestApiHelper.server_uri + "Whiskey/name/"+ SearchWord + "?page=" + Page; 
            var response = await RestApiHelper.Get(url);
            
            var result = JsonConvert.DeserializeObject<WhiskeyPageDTO>(response.ToString());
            if (result.page != 0)
            {
                SearchResult = result.whiskeys;
                TotalPage = result.totalPages;
                TotalCount = result.totalCount;
            }
        }

        public async Task PageChanged(int i)
        {
            Page = i; 
            await LoadData(); 
        }

        public async Task ClickedItem(Whiskey product)
        {
            _whiskeyState.Selected = product; 
        }
    }
}
