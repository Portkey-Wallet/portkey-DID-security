using System.Threading.Tasks;
using CAServer.Controllers;
using CAVerifierServer.Account;
using CAVerifierServer.IpWhiteList;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace CAVerifierServer.Controllers;


[RemoteService]
[Area("app")]
[ControllerName("CASecurity")]
[Route("api/app/security")]
public class CaSecurityControllerBase : CASecurityControllerBase
{
    private readonly ISecurityAppService _securityAppService;

    public CaSecurityControllerBase(ISecurityAppService securityAppService)
    {
        _securityAppService = securityAppService;
    }

    [HttpPost]
    [Route("isUserIpInWhiteList")]
    public async Task<ResponseResultDto<IpCheckResultDtos>> IsUserIpInWhiteList(CheckUserIpRequestDto requestDto)
    {
        return await _securityAppService.IsUserIpInWhiteListAsync(requestDto);
    }

    [HttpPost]
    [Route("addIpToWhiteList")]
    public async Task AddIpToWhiteList(AddUserIpToWhiteListRequestDto requestDto)
    {
        await _securityAppService.AddIpToWhiteListAsync(requestDto);
    }
}