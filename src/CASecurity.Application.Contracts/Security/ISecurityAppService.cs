using System.Threading.Tasks;
using CAVerifierServer.Account;
using CAVerifierServer.IpWhiteList;

namespace CAVerifierServer;

public interface ISecurityAppService
{
    Task<ResponseResultDto<IpCheckResultDtos>> IsUserIpInWhiteListAsync(CheckUserIpRequestDto requestDto);
    Task AddIpToWhiteListAsync(AddUserIpToWhiteListRequestDto requestDto);
}