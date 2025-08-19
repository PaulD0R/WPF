using System.Net;
using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Data;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.Model.Person;
using WPFTest.Services;

namespace WPFTest.ApiServices
{
    public class ApiPersonService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ApiAuthenticationService _authenticationService;

        public ApiPersonService(ApiAuthenticationService authenticationService)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(StaticData.PERSON_ROUDE)
            };

            _authenticationService = authenticationService;
            UpdateAuthorizationHeader();

            StaticData.OnTokenChanged += OnTokenChanged;
        }

        private void UpdateAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + StaticData.TOKEN);
        }

        private void OnTokenChanged(object sender, string newToken)
        {
            UpdateAuthorizationHeader();
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            var token = TokenStorageService.LoadRefreshToken();
            if (string.IsNullOrEmpty(token)) return false;

            var tokens = await _authenticationService.SigninWithToken(token);
            if (tokens?.Jwt == null || tokens.RefreshToken == null) return false;

            StaticData.TOKEN = tokens.Jwt;
            TokenStorageService.SaveRefreshToken(tokens.RefreshToken);
            UpdateAuthorizationHeader();
            return true;
        }

        private async Task<HttpResponseMessage> ExecuteRequestWithTokenRefreshAsync(Func<Task<HttpResponseMessage>> requestFunc)
        {
            var response = await requestFunc();

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            if (!await TryRefreshTokenAsync())
                throw new ApiExeption(response.StatusCode);

            return await requestFunc();
        }

        public async Task<PrivatePerson?> GetPrivateAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync("Me"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PrivatePerson>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<FullPerson?> GetPersonByIdAsync(string id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Id/{id}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullPerson>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<FullPerson?> GetPersonByNameAsync(string name)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Name/{name}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullPerson>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<bool?> GetIsLickedAsync(int exerciseId)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Me/IsLicked/{exerciseId}"));

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadAsAsync<PersonData>()).IsLiked;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<ImageNewFile?> ChangePrivateImageAsync(ImageNewFile newImage)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PutAsJsonAsync("Me/Image/Change", newImage));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ImageNewFile>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<bool> DeletePrivateImageAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.DeleteAsync("Me/Image/Delete"));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.DeleteAsync($"Me/Comments/Delete/{id}"));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public void Dispose()
        {
            StaticData.OnTokenChanged -= OnTokenChanged;
            _httpClient?.Dispose();
        }
    }
}