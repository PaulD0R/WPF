using System.Net;
using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.Services;

namespace WPFTest.ApiServices.Interfaces
{
    public class ApiWithRefreshTokenService : IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected readonly IApiAuthenticationService _authenticationService;

        protected ApiWithRefreshTokenService(IApiAuthenticationService authenticationService, string roude)
        {
            _authenticationService = authenticationService;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(roude)
            };

            UpdateAuthorizationHeader();

            StaticData.OnTokenChanged += OnTokenChanged;
        }

        protected void UpdateAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + StaticData.TOKEN);
        }

        protected void OnTokenChanged(object sender, string newToken)
        {
            UpdateAuthorizationHeader();
        }

        protected async Task<bool> TryRefreshTokenAsync()
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

        protected async Task<HttpResponseMessage> ExecuteRequestWithTokenRefreshAsync(Func<Task<HttpResponseMessage>> requestFunc)
        {
            var response = await requestFunc();

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            if (!await TryRefreshTokenAsync())
                throw new ApiException(response.StatusCode);

            return await requestFunc();
        }

        public void Dispose()
        {
            StaticData.OnTokenChanged -= OnTokenChanged;
            _httpClient?.Dispose();
        }
    }
}
