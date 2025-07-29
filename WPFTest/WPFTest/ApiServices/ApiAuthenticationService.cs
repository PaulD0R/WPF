using System.Net.Http;
using WPFTest.Data;
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
            Person? person = null;
            var response = await _httpClient.PostAsJsonAsync("Signup", signupPerson);

            if (response.IsSuccessStatusCode)
            {
                person = await response.Content.ReadAsAsync<Person>();
            }

            return person;
        }

        public async Task<Person?> Signin(SigninPerson signinPerson)
        {
            Person? person = null;
            var response = await _httpClient.PostAsJsonAsync("Signin", signinPerson);

            if (response.IsSuccessStatusCode)
            {
                person = await response.Content.ReadAsAsync<Person>();
            }

            return person;
        }
    }
}
