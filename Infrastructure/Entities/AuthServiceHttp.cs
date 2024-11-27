using Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Infrastructure.Entities
{
    internal class AuthServiceHttp(
        IConfiguration configurations,
        IHttpClientFactory httpClientFactory
        ) : IAuthServiceHttp
    {
        public async Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            using var httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(
                $"{configurations["AuthServiceUrl"]}/get-user-permissions?userId={userId}");

            if (response.Content == null)
                return new HashSet<Permission>();
            return await response.Content.ReadFromJsonAsync<HashSet<Permission>>();
        }
    }
}
