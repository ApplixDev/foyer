using Abp.Web.Mvc.Views;

namespace Foyer.Web.Views
{
    public abstract class FoyerWebViewPageBase : FoyerWebViewPageBase<dynamic>
    {

    }

    public abstract class FoyerWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected FoyerWebViewPageBase()
        {
            LocalizationSourceName = FoyerConsts.LocalizationSourceName;
        }
    }
}