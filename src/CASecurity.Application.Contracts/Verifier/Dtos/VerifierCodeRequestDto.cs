using System;

namespace CASecurity.Verifier.Dtos;

public class VerifierCodeRequestDto
{
    public string Type { get; set; }
    public string GuardianIdentifier { get; set; }
    public Guid VerifierSessionId{ get; set; }
    public string VerifierId{ get; set; }
    
    public string ChainId { get; set; }

}