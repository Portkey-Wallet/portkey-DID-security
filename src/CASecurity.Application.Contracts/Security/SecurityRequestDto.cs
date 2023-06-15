using System;

namespace CASecurity.security;

public class SecurityRequestDto
{
    public string UserIp { get; set; }
    
    public Guid UserId { get; set; }
}