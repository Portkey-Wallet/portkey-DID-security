using Volo.Abp.Settings;

namespace CASecurity.Settings;

public class CASecuritySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(CASecuritySettings.MySetting1));
    }
}
