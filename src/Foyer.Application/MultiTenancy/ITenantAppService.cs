using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Foyer.MultiTenancy.Dto;

namespace Foyer.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
