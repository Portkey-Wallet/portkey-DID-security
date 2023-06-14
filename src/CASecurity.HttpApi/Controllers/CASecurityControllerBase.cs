using CASecurity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace CASecurity.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class CASecurityControllerBase : AbpControllerBase
{
    protected CASecurityControllerBase()
    {
        LocalizationResource = typeof(CASecurityResource);
    }
}