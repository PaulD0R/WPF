using System.Net.Http;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices
{
    public class ApiAdminService : ApiWithRefreshTokenService, IApiAdminService
    {
        public ApiAdminService(IApiAuthenticationService authenticationService) 
            : base(authenticationService, StaticData.ADMIN_ROUDE)
        {
        }

        public async Task<ICollection<Person>> GetUsersAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync("GetAll"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ICollection<Person>>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<ICollection<LiteComment>> GetCommentsAsync(string userId)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"{userId}/Comments"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ICollection<LiteComment>>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> UpdateUserAsync(string userId, UpdatePerson updatePerson)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PutAsJsonAsync($"{userId}", updatePerson));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> ChangeRoleAsync(string userId ,string role)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PutAsJsonAsync($"{userId}/ChangeRole", new { Role = role }));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.DeleteAsync($"{userId}"));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.DeleteAsync($"Comments/{commentId}"));

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }
    }
}
