using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices
{
    public class ApiAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public ApiAuthenticationService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(StaticData.AUTHENTICATION_ROUDE)
            };
        }

        public async Task<Token> Signup(SignupPerson signupPerson)
        {
            var response = await _httpClient.PostAsJsonAsync("Signup", signupPerson);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Token>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiException(response.StatusCode);

            throw new ApiException(error);
            
        }

        public async Task<Token> Signin(SigninPerson signinPerson)
        {
            var response = await _httpClient.PostAsJsonAsync("Signin", signinPerson);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Token>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiException(response.StatusCode);

            throw new ApiException(error);
        }

        public async Task<Token> SigninWithToken(string token)
        {
            var response = await _httpClient.PostAsJsonAsync("RefreshToken", new {Token =  token});

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Token>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiException(response.StatusCode);

            throw new ApiException(error);
        }

        public async Task<bool> Logout()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "logout")
            {
                Headers = { { "Authorization", $"Bearer {StaticData.TOKEN}" } }
            };
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiException(response.StatusCode);

            throw new ApiException(error);
        }
    }
}
