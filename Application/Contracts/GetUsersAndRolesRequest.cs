namespace Application.Contracts
{
    public record GetUsersAndRolesRequest
    (
        string userId,
        string userName,
        string role
    );
}
