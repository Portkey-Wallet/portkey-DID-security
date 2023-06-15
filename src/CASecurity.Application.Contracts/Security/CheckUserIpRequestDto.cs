using System;
using System.Text.Json.Serialization;

namespace CASecurity.Account;

public class CheckUserIpRequestDto
{
    public string Ip { get; set; }
}