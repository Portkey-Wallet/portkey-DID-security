using System;
using CASecurity.Tokens.Dtos;
using Volo.Abp.EventBus;

namespace CASecurity.Tokens.Etos;

[EventName("UserTokenEto")]
public class UserTokenEto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool IsDisplay { get; set; }
    public bool IsDefault { get; set; }
    public int SortWeight { get; set; }
    public Token Token { get; set; }
}