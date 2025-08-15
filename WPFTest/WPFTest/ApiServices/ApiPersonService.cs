using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
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

        public async Task<PrivatePerson?> GetPrivateAsync()
        {
            var response = await _httpClient.GetAsync("Me");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PrivatePerson>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<FullPerson?> GetPersonByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"Id/{id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullPerson>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<FullPerson?> GetPersonByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync($"Name/{name}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullPerson>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<bool?> GetIsLickedAsync(int exerciseId)
        {
            var response = await _httpClient.GetAsync($"Me/IsLicked/{exerciseId}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadAsAsync<PersonData>()).IsLiked;

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<ImageNewFile?> ChangePrivateImageAsync(ImageNewFile newImage)
        {
            var response = await _httpClient.PutAsJsonAsync("Me/Image/Change", newImage);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ImageNewFile>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<bool> DeletePrivateImageAsync()
        {
            var response = await _httpClient.DeleteAsync("Me/Image/Delete");

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Me/Comments/Delete/{id}");

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }
    }
}
