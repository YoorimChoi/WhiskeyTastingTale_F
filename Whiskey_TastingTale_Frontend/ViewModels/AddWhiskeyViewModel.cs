using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Frontend.Services;
using static System.Net.WebRequestMethods;

namespace Whiskey_TastingTale_Frontend.ViewModels
{
    public class AddWhiskeyViewModel : INotifyPropertyChanged
    {
        private string name;
        private double? alcoholDegree;
        private string maker;
        private string details;
        private string uploadedImage;
        private IBrowserFile file;
        private string registerResult = string.Empty;
        private string uploadResult = string.Empty;

        private readonly RestApiHelper _helper;
        private readonly UserState _userState; 

        public AddWhiskeyViewModel(RestApiHelper helper, UserState userState)
        {
            _helper = helper;
            _userState = userState;
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public double? AlcoholDegree
        {
            get => alcoholDegree;
            set
            {
                alcoholDegree = value;
                OnPropertyChanged(nameof(AlcoholDegree));
            }
        }
        public string Maker
        {
            get => maker;
            set
            {
                maker = value;
                OnPropertyChanged(nameof(Maker));
            }
        }
        public string Details
        {
            get => details;
            set
            {
                details = value;
                OnPropertyChanged(nameof(Details));
            }
        }

        public string UploadedImage
        {
            get => uploadedImage; 
            set
            {
                uploadedImage = value;
                OnPropertyChanged(nameof(UploadedImage)); 
            }
        }


        public IBrowserFile File
        {
            get=> file;
            set
            {
                file = value;
                OnPropertyChanged(nameof(File));
            }
        }

        public string RegisterResult
        {
            get => registerResult;
            set
            {
                registerResult = value;
                OnPropertyChanged(nameof(RegisterResult));
            }
        }

        public string UploadResult
        {
            get => uploadResult;
            set
            {
                uploadResult = value;
                OnPropertyChanged(nameof(UploadResult));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            UploadedImage = string.Empty; // 이전 이미지 초기화
            File = e.File;
            if (File.Size > 370 * 200)
            {
                UploadedImage = await CompressImageAsync(File);
            }
            else
            {
                UploadedImage = await LoadImageAsync(File);
            }
        }
        private async Task<string> CompressImageAsync(IBrowserFile file)
        {
            try
            {
                using var stream = file.OpenReadStream(maxAllowedSize:10* 1024 * 1024);
                using var image = await SixLabors.ImageSharp.Image.LoadAsync(stream);

                var ratio = image.Width / 370;
                // 이미지 크기를 50%로 줄이기
                image.Mutate(x => x.Resize(image.Width / ratio, image.Height / ratio));

                // 압축 설정 (JPEG로 70% 화질)
                var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
                {
                    Quality = 70
                };

                using var ms = new MemoryStream();
                await image.SaveAsync(ms, encoder);
                ms.Seek(0, SeekOrigin.Begin);

                // Base64 인코딩
                var base64String = Convert.ToBase64String(ms.ToArray());
                UploadResult= string.Empty;
                return $"data:image/jpeg;base64,{base64String}";
            }
            catch
            {
                UploadResult = "파일 크기가 커서 업로드가 불가합니다."; 
                return string.Empty; 
            }
        }

        private async Task<string> LoadImageAsync(IBrowserFile file)
        {
            // 이미지 파일 크기만큼의 버퍼 생성
            var buffer = new byte[file.Size];

            // 최대 크기 10MB 제한을 둔 스트림으로 파일 읽기
            using (var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024))
            {
                await stream.ReadAsync(buffer, 0, (int)file.Size);
            }

            // Base64로 변환
            var base64String = Convert.ToBase64String(buffer);
            // 이미지 URL 생성
            UploadResult = string.Empty;
            return $"data:{file.ContentType};base64,{base64String}";

        }

        public async Task<bool> HandleRegister()
        {
            var image_index = string.Empty; 
            if (File != null)
            {
                var content = new MultipartFormDataContent();

                // 파일을 스트림으로 읽어들임
                var fileContent = new StreamContent(File.OpenReadStream(maxAllowedSize: 1024 * 1024)); // 15MB 제한
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(File.ContentType);

                // 파일 데이터 추가
                content.Add(fileContent, "file", File.Name);

                // API 호출하여 파일 업로드
                var response = await _helper.PostImage(_helper.server_uri + "File/upload", content);
                //var response = await Http.PostAsync("https://localhost:7299/api/upload", content);
                image_index = response.ToString();  
            }

            var request = new WhiskeyRequest
            {
                alcohol_degree = AlcoholDegree,
                is_accepted = false,
                is_completed = false,
                img_index = image_index,
                user_id = _userState.UserId, 
                details = Details, 
                maker = Maker, 
                name = Name
            };  
            var request_response = await _helper.Post(_helper.server_uri + "WhiskeyRequest", request);
            var result = JsonConvert.DeserializeObject<WhiskeyRequest>(request_response.ToString());

            if (result.request_id != 0) return true;
            else
            {
                RegisterResult = "서비스가 원활하지 않습니다. 등록에 실패하였습니다.";
                return false;
            }
        }

    }
}
