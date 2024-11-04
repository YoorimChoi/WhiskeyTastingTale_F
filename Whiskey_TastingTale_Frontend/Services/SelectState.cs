using Microsoft.JSInterop;
using System.Text.Json;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Frontend.Services
{
    public class SelectState
    {
        private readonly IJSRuntime _js;

        public SelectState(IJSRuntime js)
        {
            _js = js;
        }
        private string searchWord; 
        private Whiskey selectedWhiskey;

        public Whiskey SelectedWhiskey
        {
            get => selectedWhiskey;
            set
            {
                selectedWhiskey = value;
                _ = SaveStateAsync(); 
                NotifyStateChanged();
            }
        }

        public string SearchWord
        {
            get => searchWord;
            set
            {
                searchWord = value;
                _ = SaveStateAsync();
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task SaveStateAsync()
        {
            var state = JsonSerializer.Serialize(new SelectStateModel { SearchWord = SearchWord, SelectedWhiskey= SelectedWhiskey });
            await _js.InvokeVoidAsync("sessionStorage.setItem", "selectState", state);
        }

        public async Task LoadStateAsync()
        {
            var stateJson = await _js.InvokeAsync<string>("sessionStorage.getItem", "selectState");
            if (!string.IsNullOrEmpty(stateJson))
            {
                var loadedState = JsonSerializer.Deserialize<SelectStateModel>(stateJson);
                if (loadedState != null)
                {
                    SelectedWhiskey = loadedState.SelectedWhiskey;
                    SearchWord = loadedState.SearchWord;
                }
            }
        }

        private class SelectStateModel
        {
            public Whiskey SelectedWhiskey { get; set; } = new Whiskey();
           
            public string SearchWord { get; set; }  
        }
    }

}
