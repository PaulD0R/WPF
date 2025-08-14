using System.Net.Http;
using WPFTest.Data;
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
            int count = 0;
            var response = await _httpClient.GetAsync("Count");

            if (response.IsSuccessStatusCode)
            {
                count = await response.Content.ReadAsAsync<int>();
            }

            return count;
        }

        public async Task<List<LightExercise>> GetByPageAsync(int page)
        {
            var exercises = new List<LightExercise>();
            var response = await _httpClient.GetAsync($"Page{page}");

            if (response.IsSuccessStatusCode)
            {
                exercises = await response.Content.ReadAsAsync<List<LightExercise>>();
            }

            return exercises;
        }

        public async Task<ExercisesTasksFile?> GetFileByIdAsync(int id)
        {
            ExercisesTasksFile? file = null;
            var response = await _httpClient.GetAsync($"{id}/File/Task");

            if (response.IsSuccessStatusCode)
            {
                file = await response.Content.ReadAsAsync<ExercisesTasksFile>();
            }

            return file;
        }

        public async Task<FullExercise?> GetByIdAsync(int id)
        {
            FullExercise? exercise = null;
            var response = await _httpClient.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
            {
                exercise = await response.Content.ReadAsAsync<FullExercise>();
            }

            return exercise;
        }

        public async Task<string> AddExerciseAsync(NewExercise exercise)
        {
            var error = string.Empty;
            var response = await _httpClient.PostAsJsonAsync("Add", exercise);

            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode.ToString();
            }

            return error;
        }

        public async Task<ExerciseState?> ChangeIsLikedAsync(int id)
        {
            var response = await _httpClient.PutAsync($"{id}/IsLiked", null);

            if (!response.IsSuccessStatusCode) return null;

            var isLicked = await response.Content.ReadAsAsync<ExerciseState>();
            return isLicked;
        }

        public async Task<ExerciseState?> GetLikesCountByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{id}/LikesCount");

            if (!response.IsSuccessStatusCode) return null;

            var likesCount = await response.Content.ReadAsAsync<ExerciseState>();
            return likesCount;
        }

        public async Task<bool> AddCommentAsync(int id, NewComment comment) 
        {
            var response = await _httpClient.PostAsJsonAsync($"{id}/Comments/Add", comment);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<ICollection<FullComment>?> GetCommentsByIdAsync(int id)
        {
            List<FullComment>? comments = null;
            var response = await _httpClient.GetAsync($"{id}/Comments");

            if (response.IsSuccessStatusCode)
            {
                comments = await response.Content.ReadAsAsync<List<FullComment>>();
            }

            return comments;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
