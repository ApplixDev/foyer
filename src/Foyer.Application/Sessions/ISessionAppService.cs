using System.Threading.Tasks;
using Abp.Application.Services;
using Foyer.Sessions.Dto;

namespace Foyer.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
