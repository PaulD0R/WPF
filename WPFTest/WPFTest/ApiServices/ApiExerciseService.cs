using System.Net;
using System.Net.Http;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.Model.Data;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;
using WPFTest.Services;

namespace WPFTest.ApiServices
{
    public class ApiExerciseService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ApiAuthenticationService _authenticationService;

        public ApiExerciseService(ApiAuthenticationService authenticationService)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(StaticData.EXERCISE_ROUDE)
            };

            UpdateAuthorizationHeader();
            _authenticationService = authenticationService;

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
                throw new ApiException(response.StatusCode);

            return await requestFunc();
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

        public async Task<List<LightExercise>> GetByPageAsync(int page)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"Page{page}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<LightExercise>>();

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