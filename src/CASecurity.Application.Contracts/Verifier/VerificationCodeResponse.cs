using System;
using System.Text.Json.Serialization;

namespace CASecurity.Verifier;

public class VerificationCodeResponse
{
    public string VerificationDoc{ get; set; }

    public string Signature{ get; set; }
}