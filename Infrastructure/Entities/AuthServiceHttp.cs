using Application.Abstractions.Services;

namespace Infrastructure.Entities
{
    public class AuthServiceHttp(
        IHttpClientFactory httpClientFactory
        ) : IAuthServiceHttp
    {
        public async Task<string> GetUserPermissionsAsync(Guid userId)
        {
            using var httpClient = httpClientFactory.CreateClient("authServ");

            var response = await httpClient.GetAsync(
                $"/get-user-permissions?userId={userId}");

            if (response.Content == null)
                return "";
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUserRoleAsync(Guid userId)
        {
            using var httpClient = httpClientFactory.CreateClient("authServ");

            var response = await httpClient.GetAsync(
                $"/get-user-role?userId={userId}");

            if (response.Content == null)
                return "";
            return await response.Content.ReadAsStringAsync();
        }
    }
}
