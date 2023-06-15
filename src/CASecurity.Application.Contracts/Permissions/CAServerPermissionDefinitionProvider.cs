using CASecurity.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CASecurity.Permissions;

public class CASecurityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CASecurityPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(CASecurityPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CASecurityResource>(name);
    }
}
