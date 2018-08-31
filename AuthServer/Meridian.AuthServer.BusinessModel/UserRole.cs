namespace Meridian.AuthServer.BusinessModel
{
    public class UserRole
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int RoleID { get; set; }
        public int RoleType { get; set; }
        public string RoleName { get; set; }
    }
}
