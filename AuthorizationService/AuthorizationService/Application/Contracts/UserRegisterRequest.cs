using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Application.Contracts
{
    public record UserRegisterRequest(
        [Required] string firstName,
        [Required] string lastName,
        [Required] DateOnly birthDate,
        [Required] string email,
        [Required] string password
        );
}
