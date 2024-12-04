namespace Application.Abstractions.Services
{
    /// <summary>
    /// Represents service to get data from auth service
    /// </summary>
    public interface IAuthServiceHttp
    {
        /// <summary>
        /// Get string with user permissions
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserPermissionsAsync(Guid userId);
        /// <summary>
        /// Get string with user role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserRoleAsync(Guid userId);
    }
}
