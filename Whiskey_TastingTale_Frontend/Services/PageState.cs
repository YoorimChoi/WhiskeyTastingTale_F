using Microsoft.AspNetCore.Components;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Frontend.Services
{
    public class PageState
    {
        private Stack<string> priviousPages = new Stack<string>();
        public string PriviousPage
        {
            get
            {
                if (priviousPages.Count == 0) return "/"; 
                return priviousPages.Pop();
            }
            set
            {
                priviousPages.Push(value);
            }
        }

        public void init()
        {
            priviousPages.Clear(); 
        }
    }



       
}
