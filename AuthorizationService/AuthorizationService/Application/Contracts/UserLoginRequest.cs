using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Application.Contracts
{
    public record UserLoginRequest(
        [Required] string email,
        [Required] string password
        );
}
