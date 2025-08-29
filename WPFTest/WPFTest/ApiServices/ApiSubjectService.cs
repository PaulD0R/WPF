using System.Net;
using System.Net.Http;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Subject;
using WPFTest.Services;

namespace WPFTest.ApiServices
{
    public class ApiSubjectService : ApiWithRefreshTokenService, IApiSubjectService
    {
        public ApiSubjectService(IApiAuthenticationService authenticationService)
            : base(authenticationService, StaticData.SUBJECT_ROUDE)
        {
        }

        public async Task<ICollection<LiteSubject>> GetAllAsync()
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync(""));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<LiteSubject>>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<FullSubject?> GetByIdAsync(int id)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.GetAsync($"{id}"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<FullSubject>();

            var error = await response.Content.ReadAsStringAsync();
            throw string.IsNullOrEmpty(error)
                ? new ApiException(response.StatusCode)
                : new ApiException(error);
        }

        public async Task<bool> AddSubjectAsync(NewSubject subject)
        {
            var response = await ExecuteRequestWithTokenRefreshAsync(() => _httpClient.PostAsJsonAsync("Add", subject));

            if (response.IsSuccessStatusCode)
                return true;

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