using System.ComponentModel.DataAnnotations;

namespace CASecurity.Verifier;

public class VerifierServerBase
{
    [Required]
    public string Type { get; set; }
    
    [Required]
    public string GuardianIdentifier{ get; set; }
    
    [Required]
    public string VerifierId { get; set; }    
    

    
}