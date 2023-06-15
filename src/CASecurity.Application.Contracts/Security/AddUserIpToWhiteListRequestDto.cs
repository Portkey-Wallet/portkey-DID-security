using System;

namespace CASecurity.IpWhiteList;

public class AddUserIpToWhiteListRequestDto
{

    public Guid UserId { get; set; }

    public string UserIp { get; set; }
}