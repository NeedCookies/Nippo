namespace Domain.Entities.Identity
{
    /*
    public class AppRole : IdentityRole<string>
    {
        public AppRole() { }

        public AppRole(string roleName)
        {
            Name = roleName;
            NormalizedName = roleName.ToUpperInvariant();
        }
    }
    */
    public enum Role
    {
        User = 1,
        Author = 2,
        Admin = 3
    }
}
