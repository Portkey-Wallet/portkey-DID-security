using System.Threading.Tasks;
using Asp.Versioning;
using CASecurity.Account;
using CASecurity.IpWhiteList;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace CASecurity.Controllers;

[RemoteService]
[Area("app")]
[ControllerName("CASecurity")]
[Route("api/app/security")]
public class CaSecurityController : CASecurityControllerBase
{
    private readonly ISecurityAppService _securityAppService;

    public CaSecurityController(ISecurityAppService securityAppService)
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