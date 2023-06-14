using Volo.Abp.Application.Dtos;

namespace CASecurity.Tokens.Dtos;

public class GetUserTokenListInput : PagedResultRequestDto
{
    public string Filter { get; set; }
    public bool IsDisplay { get; set; }

}