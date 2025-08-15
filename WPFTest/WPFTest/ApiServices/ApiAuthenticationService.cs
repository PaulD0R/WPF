using System.Net.Http;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices
{
    public class ApiAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public ApiAuthenticationService(string baseAddress)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        public async Task<Person?> Signup(SignupPerson signupPerson)
        {
            var response = await _httpClient.PostAsJsonAsync("Signup", signupPerson);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Person>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
            
        }

        public async Task<Person?> Signin(SigninPerson signinPerson)
        {
            var response = await _httpClient.PostAsJsonAsync("Signin", signinPerson);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Person>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }
    }
}
