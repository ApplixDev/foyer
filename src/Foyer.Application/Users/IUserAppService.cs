using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Foyer.Roles.Dto;
using Foyer.Users.Dto;

namespace Foyer.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();
    }
}