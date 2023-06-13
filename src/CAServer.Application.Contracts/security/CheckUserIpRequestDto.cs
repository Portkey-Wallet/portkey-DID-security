using System;
using System.Text.Json.Serialization;

namespace CAVerifierServer.Account;

public class CheckUserIpRequestDto
{
    public string Ip { get; set; }
}