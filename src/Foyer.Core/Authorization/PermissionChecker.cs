using Abp.Authorization;
using Foyer.Authorization.Roles;
using Foyer.Authorization.Users;

namespace Foyer.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
