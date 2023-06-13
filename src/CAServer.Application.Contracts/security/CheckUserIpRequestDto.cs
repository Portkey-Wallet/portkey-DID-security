using System;
using System.Text.Json.Serialization;

namespace CAVerifierServer.Account;

public class CheckUserIpRequestDto
{
    [JsonPropertyName("ip")]
    public string Ip { get; set; }
    
   

}