namespace PmeMemberApi.Common.Auth
{
    public static class Permissions
    {
        public static List<RolePermission> AllPermissions = new List<RolePermission>
        {
            new RolePermission{Id = "Permissions.Posts.View", Name = "View Posts"},
            new RolePermission{Id = "Permissions.Posts.Create", Name = "Create Posts"},
            new RolePermission{Id = "Permissions.Posts.Delete", Name = "Delete Posts"},
            new RolePermission{Id = "Permissions.Users.View", Name = "View Users"},
            new RolePermission{Id = "Permissions.Users.Create", Name = "Create Users"},
            new RolePermission{Id = "Permissions.Users.Delete", Name = "Delete Users"},
            new RolePermission{Id = "Permissions.Emails.Send", Name = "Send Emails"},
        };
    }
}
