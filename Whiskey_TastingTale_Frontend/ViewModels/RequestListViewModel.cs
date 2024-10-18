using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class RequestListViewModel : INotifyPropertyChanged
    {
        private List<WhiskeyRequest> requestList = new List<WhiskeyRequest>();
        private string deleteResult = string.Empty; 

        private readonly RestApiHelper _helper;
        private readonly WhiskeyState _whiskeyState; 
        public RequestListViewModel(RestApiHelper helper, WhiskeyState whiskeyState)
        {
            _helper = helper;
            _whiskeyState = whiskeyState; 
        }

        public List<WhiskeyRequest> RequestList
        {
            get { return requestList; }
            set
            {
                requestList = value;
                OnPropertyChanged(nameof(RequestList));
            }
        }

        public string DeleteResult
        {
            get => deleteResult; 
            set
            {
                deleteResult = value;
                OnPropertyChanged(nameof(DeleteResult)); 
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task LoadData()
        {
            var response = await _helper.Get(_helper.server_uri + "WhiskeyRequest/user");
            var result = JsonConvert.DeserializeObject<List<WhiskeyRequest>>(response.ToString()); 
            if(result!= null) RequestList = result;

        }

        public async Task<bool> RequestChanged(WhiskeyRequest request)
        {
            var response = await _helper.Put(_helper.server_uri + "WhiskeyRequest", request); 
            var result = JsonConvert.DeserializeObject<WhiskeyRequest>(response.ToString());
            if (result != null && result.request_id == request.request_id) return true;
            else return false; 
        }

        public async Task<bool> HandleDeleteRequest(WhiskeyRequest request)
        {
            var response = await _helper.Delete(_helper.server_uri + "WhiskeyRequest/" + request.request_id);
            var result = JsonConvert.DeserializeObject<WhiskeyRequest>(response.ToString());
            if (result != null && result.request_id == request.request_id)
            {
                requestList.Remove(request); 
                return true;
            }
            else
            {
                DeleteResult = request.name + "의 요청 취소가 실패하였습니다.";
                return false;
            }
        }
        public void HandleMoveWhiskeyInfo(WhiskeyRequest request)
        {
            _whiskeyState.Selected = new Whiskey() { whiskey_id = request.whiskey_id ?? -1 }; 
        }
    }
}
