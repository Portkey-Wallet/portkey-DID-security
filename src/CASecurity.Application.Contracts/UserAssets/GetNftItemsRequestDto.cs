using System.ComponentModel.DataAnnotations;

namespace CASecurity.UserAssets;

public class GetNftItemsRequestDto : GetAssetsBase
{
    [Required][MinLength(1)] public string Symbol { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }
}