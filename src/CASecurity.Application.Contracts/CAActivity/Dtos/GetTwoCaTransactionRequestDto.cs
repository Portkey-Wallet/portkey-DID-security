using System.Collections.Generic;
using CASecurity.UserAssets;
using Volo.Abp.Application.Dtos;

namespace CASecurity.CAActivity.Dtos;

public class GetTwoCaTransactionRequestDto : PagedResultRequestDto
{
    public List<CAAddressInfo> TargetAddressInfos { get; set; }

    public List<CAAddressInfo> CaAddressInfos { get; set; }
    public string ChainId { get; set; }
    public string Symbol { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}