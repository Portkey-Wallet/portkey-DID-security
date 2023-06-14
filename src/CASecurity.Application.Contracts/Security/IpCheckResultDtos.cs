using System;
using System.Text.Json.Serialization;

namespace CASecurity.IpWhiteList;

public class IpCheckResultDtos
{
    
    [JsonPropertyName("isInWhiteList")]
    public bool IsInWhiteList{ get; set; }
    
}