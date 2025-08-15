using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
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
            var response = await _httpClient.GetAsync("");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<LightSubject>>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<FullSubject?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullSubject>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<bool> AddSubjectAsync(NewSubject subject)
        {
            var response = await _httpClient.PostAsJsonAsync("Add", subject);

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }
    }
}
