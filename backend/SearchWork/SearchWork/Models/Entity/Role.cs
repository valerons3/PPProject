namespace SearchWork.Models.Entity
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public ICollection<User> RoleUsers { get; set; } = [];
    }
}
