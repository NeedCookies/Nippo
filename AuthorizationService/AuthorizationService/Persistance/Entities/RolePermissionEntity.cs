namespace AuthorizationService.Persistance.Entities
{
    /// <summary>
    /// This is class for many to many relationshib in DB
    /// </summary>
    public class RolePermissionEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}