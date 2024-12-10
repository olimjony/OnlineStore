namespace OnlineStore.Domain.Constants;

public static class Permissions
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return
        [
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete"
        ];
    }

    public static class UserPermissions
    {
        public const string View = "Permissions.User.View";
        public const string Create = "Permissions.User.Create";
        public const string Edit = "Permissions.User.Edit";
        public const string Delete = "Permissions.User.Delete";

    }


    public static class SellerPermissions
    {
        public const string View = "Permissions.Seller.View";
        public const string Create = "Permissions.Seller.Create";
        public const string Edit = "Permissions.Seller.Edit";
        public const string Delete = "Permissions.Seller.Delete";
    }

}
