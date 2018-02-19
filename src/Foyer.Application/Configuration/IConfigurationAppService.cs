using System.Threading.Tasks;
using Abp.Application.Services;
using Foyer.Configuration.Dto;

namespace Foyer.Configuration
{
    public interface IConfigurationAppService: IApplicationService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}