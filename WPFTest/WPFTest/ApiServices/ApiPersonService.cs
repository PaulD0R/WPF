using System.Net.Http;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Data;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices
{
    public class ApiPersonService : ApiWithRefreshTokenService, IApiPersonService
    {
        public ApiPersonService(IApiAuthenticationService authenticationService)
            : base(authenticationService, StaticData.PERSON_ROUDE)
        {
        }

        public async Task<PrivatePerson?> GetPrivateAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync("Me"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PrivatePerson>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<FullPerson?> GetPersonByIdAsync(string id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Id/{id}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullPerson>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<FullPerson?> GetPersonByNameAsync(string name)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Name/{name}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullPerson>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool?> GetIsLickedAsync(int exerciseId)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Me/IsLicked/{exerciseId}"));

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadAsAsync<PersonData>()).IsLiked;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<ImageNewFile?> ChangePrivateImageAsync(ImageNewFile newImage)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PutAsJsonAsync("Me/Image/Change", newImage));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ImageNewFile>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> DeletePrivateImageAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.DeleteAsync("Me/Image/Delete"));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.DeleteAsync($"Me/Comments/Delete/{id}"));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public void Dispose()
        {
            StaticData.OnTokenChanged -= OnTokenChanged;
            _httpClient?.Dispose();
        }
    }
}