using System;

namespace CASecurity.Tokens.Dtos;

public class GetTokenHistoryPriceInput
{
    public string Symbol { get; set; }
    public DateTime DateTime { get; set; }
}