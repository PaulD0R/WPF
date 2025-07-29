using System.Net.Http;
using WPFTest.Data;
using WPFTest.MVVM.Model.Data;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices
{
    public class ApiPersonService
    {
        private readonly HttpClient _httpClient;

        public ApiPersonService(string baseAdres)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAdres)
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + StaticData.TOKEN);
        }

        public async Task<PrivatePerson?> GetPrivate()
        {
            PrivatePerson? person = null;
            var response = await _httpClient.GetAsync("Me");

            if (response.IsSuccessStatusCode)
            {
                person = await response.Content.ReadAsAsync<PrivatePerson>();
            }

            return person;
        }

        public async Task<FullPerson?> GetPersonById(string id)
        {
            FullPerson? person = null;
            var response = await _httpClient.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
            {
                person = await response.Content.ReadAsAsync<FullPerson>();
            }

            return person;
        }

        public async Task<bool?> GetIsLickedAsync(int exerciseId)
        {
            var response = await _httpClient.GetAsync($"Me/IsLicked/{exerciseId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var personData = await response.Content.ReadAsAsync<PersonData>();
            return personData.IsLiked;
        }

        public async Task<ImageNewFile?> ChangePrivateImage(ImageNewFile newImage)
        {
            ImageNewFile? image = null;
            var response = await _httpClient.PutAsJsonAsync("Me/Image/Change", newImage);

            if (response.IsSuccessStatusCode)
            {
                image = await response.Content.ReadAsAsync<ImageNewFile>();
            }

            return image;
        }

        public async Task<bool> DeletePrivateImage()
        {
            var response = await _httpClient.DeleteAsync("Me/Image/Delete");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
