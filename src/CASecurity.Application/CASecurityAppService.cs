using System;
using System.Collections.Generic;
using System.Text;
using CASecurity.Localization;
using Volo.Abp.Application.Services;

namespace CASecurity;

/* Inherit your application services from this class.
 */
public abstract class CASecurityAppService : ApplicationService
{
    protected CASecurityAppService()
    {
        LocalizationResource = typeof(CASecurityResource);
    }
}
