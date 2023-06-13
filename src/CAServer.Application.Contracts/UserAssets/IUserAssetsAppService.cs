using System.Threading.Tasks;
using CAServer.UserAssets.Dtos;

namespace CAServer.UserAssets;

public interface IUserAssetsAppService
{
    Task<GetTokenDto> GetTokenAsync(GetTokenRequestDto requestDto);
}