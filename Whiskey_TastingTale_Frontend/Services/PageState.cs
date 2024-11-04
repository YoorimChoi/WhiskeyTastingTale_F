using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Frontend.Services
{
    public class PageState
    {
        private readonly IJSRuntime _js;

        public PageState(IJSRuntime js)
        {
            _js = js;
        }

        private Stack<string> priviousPages = new Stack<string>();
        public string PriviousPage
        {
            get
            {
                string page = null;
                if (priviousPages.Count == 0) page =  "/";
                else page = priviousPages.Pop();
                _ = SaveStateAsync();
                return page;
            }
            set
            {
                priviousPages.Push(value);
                _ = SaveStateAsync(); 
            }
        }

        public void init()
        {
            priviousPages.Clear(); 
        }

        public async Task SaveStateAsync()
        {
            var state = JsonSerializer.Serialize(new PageStateModel { PriviousPages = priviousPages.ToList() });
            await _js.InvokeVoidAsync("sessionStorage.setItem", "pageState", state);
        }

        public async Task LoadStateAsync()
        {
            var stateJson = await _js.InvokeAsync<string>("sessionStorage.getItem", "pageState");
            if (!string.IsNullOrEmpty(stateJson))
            {
                var loadedState = JsonSerializer.Deserialize< PageStateModel> (stateJson);
                if (loadedState != null)
                {
                    priviousPages = new Stack<string>(loadedState.PriviousPages);
                }
            }
        }

        public class PageStateModel
        {
            public List<string> PriviousPages { get; set; } = new List<string>();
        }

    }




}
