using System;

namespace CAVerifierServer.IpWhiteList;

public class AddUserIpToWhiteListRequestDto
{

    public Guid UserId { get; set; }

    public string UserIp { get; set; }
}