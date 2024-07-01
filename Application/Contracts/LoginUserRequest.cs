namespace Application.Contracts
{
    public record LoginUserRequest
    (
        string UserName,
        string Password
    );
}
