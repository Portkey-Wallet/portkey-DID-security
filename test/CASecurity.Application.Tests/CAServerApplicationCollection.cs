using CASecurity.MongoDB;
using Xunit;

namespace CASecurity;

[CollectionDefinition(CASecurityTestConsts.CollectionDefinitionName)]
public class CASecurityApplicationCollection
{
    public const string CollectionDefinitionName = "CASecurity collection";
}
