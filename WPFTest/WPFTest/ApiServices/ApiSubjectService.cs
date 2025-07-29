using System.Net.Http;
using WPFTest.Data;
using WPFTest.MVVM.Model.Subject;

namespace WPFTest.ApiServices
{
    public class ApiSubjectService
    {
        private readonly HttpClient _httpClient;

        public ApiSubjectService(string baseApi)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseApi)
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + StaticData.TOKEN);
        }

        public async Task<ICollection<LightSubject>> GetAllAsync()
        {
            var subjects = new List<LightSubject>();    
            var response = await _httpClient.GetAsync("");

            if (!response.IsSuccessStatusCode)
            {
                return subjects;
            }

            subjects = await response.Content.ReadAsAsync<List<LightSubject>>();

            return subjects;
        }

        public async Task<FullSubject?> GetByIdAsync(int id)
        {
            FullSubject? subject = null;
            var response = await _httpClient.GetAsync($"{id}");

            if (!response.IsSuccessStatusCode)
            {
                return subject;
            }

            subject = await response.Content.ReadAsAsync<FullSubject>();
            
            return subject;
        }

        public async Task<string> AddSubjectAsync(NewSubject subject)
        {
            var error = string.Empty;
            var response = await _httpClient.PostAsJsonAsync("Add", subject);

            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode.ToString();
            }

            return error;
        }
    }
}
