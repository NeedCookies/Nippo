using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Update
{
    public record UpdateUserInfoRequest (
        string? FirstName,
        string? LastName,
        string? UserName,
        string? PhoneNumber,
        string? BirthDate,
        IFormFile? UserPictureFile
    );
}
