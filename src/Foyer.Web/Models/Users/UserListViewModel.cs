using System.Collections.Generic;
using Foyer.Roles.Dto;
using Foyer.Users.Dto;

namespace Foyer.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}