using System.Net.Http;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.Model.Data;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;

namespace WPFTest.ApiServices
{
    public class ApiExerciseService : ApiWithRefreshTokenService, IApiExerciseService
    {
        public ApiExerciseService(IApiAuthenticationService authenticationService) 
            : base(authenticationService, StaticData.EXERCISE_ROUDE)
        {
        }

        public async Task<int> GetCountAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync("Count"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<int>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<List<LiteExercise>> GetByPageAsync(int page)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Page{page}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<LiteExercise>>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<ExercisesTasksFile> GetFileByIdAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"{id}/File/Task"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ExercisesTasksFile>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<FullExercise?> GetByIdAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"{id}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullExercise>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> AddExerciseAsync(NewExercise exercise)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PostAsJsonAsync("Add", exercise));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<ExerciseState?> ChangeIsLikedAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PutAsync($"{id}/IsLiked", null));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ExerciseState>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<ExerciseState?> GetLikesCountByIdAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"{id}/LikesCount"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ExerciseState>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> AddCommentAsync(int id, NewComment comment)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PostAsJsonAsync($"{id}/Comments/Add", comment));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<ICollection<FullComment>?> GetCommentsByIdAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"{id}/Comments"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<FullComment>>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public void Dispose()
        {
            StaticData.OnTokenChanged -= OnTokenChanged;
            _httpClient?.Dispose();
        }
    }
}