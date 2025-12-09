using System;

namespace CASecurity.UserAssets;

public class GetTokenRequestDto : GetAssetsBase
{
    public Guid UserId { get; set; }
}