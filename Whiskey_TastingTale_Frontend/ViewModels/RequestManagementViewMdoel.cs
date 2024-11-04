using Newtonsoft.Json;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Frontend.Pages;
using Whiskey_TastingTale_Frontend.Services;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class RequestManagementViewMdoel
    {
        private readonly RestApiHelper _helper;
        private readonly SelectState _whiskeyState; 

        private List<WhiskeyRequestUserDTO> requests;
        private string handleRequestResult; 

        public RequestManagementViewMdoel(RestApiHelper helper, SelectState whiskeyState)
        {
            _helper = helper;
            _whiskeyState = whiskeyState;
        }

        public List<WhiskeyRequestUserDTO> Requests
        {
            get => requests; 
            set
            {
                requests = value;
                OnPropertyChanged(nameof(Requests)); 
            }
        }

        public string HandleReqeustResult
        {
            get => handleRequestResult; 
            set
            {
                handleRequestResult = value;
                OnPropertyChanged(nameof(HandleReqeustResult)); 
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task LoadData()
        {
            var response = await _helper.Get(_helper.server_uri + "WhiskeyRequest/all");
            var result = JsonConvert.DeserializeObject<List<WhiskeyRequestUserDTO>>(response.ToString());
            if (result != null) Requests = result;
        }

        public void HandleMoveWhiskeyInfo(WhiskeyRequestUserDTO request)
        {
            _whiskeyState.SelectedWhiskey = new Whiskey() { whiskey_id = request.whiskey_id ?? -1 };
        }

        public async Task HandleAccept(WhiskeyRequestUserDTO request)
        {
            var response = await _helper.Post(_helper.server_uri + "Whiskey",
                new Whiskey()
                {
                    whiskey_name = request.name,
                    img_index = request.img_index,
                    alcohol_degree = request.alcohol_degree, 
                    maker = request.maker,
                    details = request.details
                });
            var result = JsonConvert.DeserializeObject<Whiskey>(response.ToString());
            if (result != null && result.whiskey_id != 0)
            {
                request.whiskey_id = result.whiskey_id;
                request.is_accepted = true;
                request.is_completed = true; 

                var request_response = await _helper.Put(_helper.server_uri + "WhiskeyRequest", request);
                var request_result = JsonConvert.DeserializeObject<WhiskeyRequest>(response.ToString());

                if (request_result != null && request_result.whiskey_id == result.whiskey_id)
                {
                    HandleReqeustResult = string.Empty; 
                }else
                {
                    HandleReqeustResult = "요청이 처리되지 못했습니다. 서비스 상태를 확인해주세요.";
                }
            }
        }

        public async Task HandleDecline(WhiskeyRequestUserDTO request)
        {
           
            request.is_accepted = false;
            request.is_completed = true;

            var request_response = await _helper.Put(_helper.server_uri + "WhiskeyRequest", request);
            var request_result = JsonConvert.DeserializeObject<WhiskeyRequest>(request_response.ToString());

            if (request_result != null && request_result.request_id == request.request_id)
            {
                HandleReqeustResult = string.Empty;
            }
            else
            {
                HandleReqeustResult = "요청이 처리되지 못했습니다. 서비스 상태를 확인해주세요.";
            }
        }
    }
}
