using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Foyer.Configuration.Dto;

namespace Foyer.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : FoyerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
