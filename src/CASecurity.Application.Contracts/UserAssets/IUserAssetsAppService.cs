using System.Threading.Tasks;
using CASecurity.UserAssets.Dtos;

namespace CASecurity.UserAssets;

public interface IUserAssetsAppService
{
    Task<GetTokenDto> GetTokenAsync(GetTokenRequestDto requestDto);
}