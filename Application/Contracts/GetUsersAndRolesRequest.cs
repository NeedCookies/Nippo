namespace Application.Contracts
{
    public record GetUsersAndRolesRequest
    (
        string UserId,
        string UserName,
        string Role
    );
}
