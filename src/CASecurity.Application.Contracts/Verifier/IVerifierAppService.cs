using System.Threading.Tasks;
using CASecurity.Dtos;
using CASecurity.Verifier.Dtos;

namespace CASecurity.Verifier;

public interface IVerifierAppService
{
    public Task<VerifierServerResponse> SendVerificationRequestAsync(SendVerificationRequestInput input);

    public Task<VerificationCodeResponse> VerifyCodeAsync(VerificationSignatureRequestDto signatureRequestDto);
    public Task<VerificationCodeResponse> VerifyGoogleTokenAsync(VerifyTokenRequestDto requestDto);
    public Task<VerificationCodeResponse> VerifyAppleTokenAsync(VerifyTokenRequestDto requestDto);
    public Task<long> CountVerifyCodeInterfaceRequestAsync(string userIpAddress);
    public Task<bool> GuardianExistsAsync(string guardianIdentifier);
}