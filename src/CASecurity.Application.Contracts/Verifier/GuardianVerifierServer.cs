using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Volo.Abp.Domain.Entities;

namespace CASecurity.Verifier;

public class GuardianVerifierServer
{
    public string Name { get; set; }

    public List<string> VerifierAddress { get; set; }
    
    public string ImageUrl { get; set; }

    public RepeatedField<string> EndPoints { get; set; }

}