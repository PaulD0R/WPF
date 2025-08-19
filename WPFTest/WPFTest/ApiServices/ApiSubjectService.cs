using System.Net;
using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Subject;
using WPFTest.Services;

namespace WPFTest.ApiServices
{
    public class ApiSubjectService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ApiAuthenticationService _authenticationService;

        public ApiSubjectService(ApiAuthenticationService authenticationService)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(StaticData.SUBJECT_ROUDE)
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

        public async Task<ICollection<LightSubject>> GetAllAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync(""));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<LightSubject>>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<FullSubject?> GetByIdAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"{id}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullSubject>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiExeption(response.StatusCode)
                : new ApiExeption(error);
        }

        public async Task<bool> AddSubjectAsync(NewSubject subject)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PostAsJsonAsync("Add", subject));

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