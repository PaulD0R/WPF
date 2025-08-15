using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.Model.Data;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;

namespace WPFTest.ApiServices
{
    public class ApiExerciseService : IDisposable
    {
        private readonly HttpClient _httpClient;

        public ApiExerciseService(string baseApi)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseApi)
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + StaticData.TOKEN);
        }

        public async Task<int> GetCountAsync()
        {
            var response = await _httpClient.GetAsync("Count");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<int>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<List<LightExercise>> GetByPageAsync(int page)
        {
            var response = await _httpClient.GetAsync($"Page{page}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<LightExercise>>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<ExercisesTasksFile?> GetFileByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{id}/File/Task");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ExercisesTasksFile>();
            
            throw new ApiExeption(response.StatusCode);
        }

        public async Task<FullExercise?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullExercise>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<bool> AddExerciseAsync(NewExercise exercise)
        {
            var response = await _httpClient.PostAsJsonAsync("Add", exercise);

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<ExerciseState?> ChangeIsLikedAsync(int id)
        {
            var response = await _httpClient.PutAsync($"{id}/IsLiked", null);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ExerciseState>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<ExerciseState?> GetLikesCountByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{id}/LikesCount");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ExerciseState>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<bool> AddCommentAsync(int id, NewComment comment) 
        {
            var response = await _httpClient.PostAsJsonAsync($"{id}/Comments/Add", comment);

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public async Task<ICollection<FullComment>?> GetCommentsByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{id}/Comments");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<FullComment>>();

            var error = await response.Content.ReadAsStringAsync();
            if (error == null || error == string.Empty) throw new ApiExeption(response.StatusCode);

            throw new ApiExeption(error);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
