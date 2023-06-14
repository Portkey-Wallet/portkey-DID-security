using System.Threading.Tasks;
using CASecurity.Account;
using CASecurity.IpWhiteList;

namespace CASecurity;

public interface ISecurityAppService
{
    Task<ResponseResultDto<IpCheckResultDtos>> IsUserIpInWhiteListAsync(CheckUserIpRequestDto requestDto);
    Task AddIpToWhiteListAsync(AddUserIpToWhiteListRequestDto requestDto);
}