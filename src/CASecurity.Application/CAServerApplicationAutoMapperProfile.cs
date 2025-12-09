using System;
using AutoMapper;
using CASecurity.CAAccount.Dtos;
using CASecurity.CAActivity.Dto;
using CASecurity.CAActivity.Dtos;
using CASecurity.CAActivity.Provider;
using CASecurity.Guardian;
using CASecurity.UserAssets.Dtos;
using CASecurity.UserAssets.Provider;
using CASecurity.Verifier;
using CASecurity.Verifier.Dtos;
using Portkey.Contracts.CA;

namespace CASecurity;

public class CASecurityApplicationAutoMapperProfile : Profile
{
    public CASecurityApplicationAutoMapperProfile()
    {

        CreateMap<VerificationSignatureRequestDto, VierifierCodeRequestInput>();
        
        // user assets
        CreateMap<IndexerTransactionFee, TransactionFee>();
        
        CreateMap<IndexerTokenInfo, Token>()
            .ForMember(t => t.Balance, m => m.MapFrom(f => f.Balance.ToString()))
            .ForMember(t => t.Symbol, m => m.MapFrom(f => f.TokenInfo == null ? null : f.TokenInfo.Symbol))
            .ForMember(t => t.Decimals, m => m.MapFrom(f => f.TokenInfo == null ? new decimal() : f.TokenInfo.Decimals))
            .ForMember(t => t.TokenContractAddress,
                m => m.MapFrom(f =>
                    f.TokenInfo == null || f.TokenInfo.TokenContractAddress.IsNullOrEmpty()
                        ? null
                        : f.TokenInfo.TokenContractAddress));
        
        CreateMap<IndexerNftInfo, NftItem>()
            .ForMember(t => t.Balance, m => m.MapFrom(f => f.Balance.ToString()))
            .ForMember(t => t.Symbol, m => m.MapFrom(f => f.NftInfo == null ? null : f.NftInfo.Symbol))
            .ForMember(t => t.Alias, m => m.MapFrom(f => f.NftInfo == null ? null : f.NftInfo.TokenName))
            .ForMember(t => t.TokenContractAddress,
                m => m.MapFrom(f => f.NftInfo == null ? null : f.NftInfo.TokenContractAddress))
            .ForMember(t => t.ImageUrl,
                m => m.MapFrom(f =>
                    f.NftInfo == null ? null : f.NftInfo.ImageUrl));
        
        // user activity
        CreateMap<IndexerTransaction, GetActivityDto>()
            .ForMember(t => t.TransactionType, m => m.MapFrom(f => f.MethodName))
            .ForMember(t => t.NftInfo, m => m.MapFrom(f => f.NftInfo == null ? null : new NftDetail()))
            .ForMember(t => t.Symbol,
                m => m.MapFrom(f =>
                    f.TokenInfo == null ? (f.NftInfo == null ? null : f.NftInfo.Symbol) : f.TokenInfo.Symbol))
            .ForMember(t => t.Decimals,
                m => m.MapFrom(f =>
                    f.TokenInfo == null
                        ? (f.NftInfo == null ? null : f.NftInfo.Decimals.ToString())
                        : f.TokenInfo.Decimals.ToString()))
            .ForMember(t => t.Timestamp, m => m.MapFrom(f => f.Timestamp.ToString()))
            .ForMember(t => t.FromAddress,
                m => m.MapFrom(f =>
                    f.TransferInfo == null
                        ? f.FromAddress
                        : f.TransferInfo.FromCAAddress ?? f.TransferInfo.FromAddress))
            .ForMember(t => t.ToAddress, m => m.MapFrom(f => f.TransferInfo == null ? "" : f.TransferInfo.ToAddress))
            .ForMember(t => t.Amount,
                m => m.MapFrom(f => f.TransferInfo == null ? "" : f.TransferInfo.Amount.ToString()))
            .ForMember(t => t.FromChainId,
                m => m.MapFrom(f => f.ChainId))
            .ForMember(t => t.ToChainId, m => m.MapFrom(f => f.TransferInfo == null ? "" : f.TransferInfo.ToChainId));

        CreateMap<VerifierServerInput, SendVerificationRequestInput>();
        CreateMap<SendVerificationRequestInput, VerifierCodeRequestDto>();

        CreateMap<Portkey.Contracts.CA.ManagerInfo, ManagerInfoDto>()
            .ForMember(t => t.Address, m => m.MapFrom(f => f.Address.ToBase58()));
        CreateMap<Portkey.Contracts.CA.Guardian, GuardianDto>()
            .ForMember(t => t.IdentifierHash, m => m.MapFrom(f => f.IdentifierHash.ToHex()))
            .ForMember(t => t.VerifierId, m => m.MapFrom(f => f.VerifierId.ToHex()))
            .ForMember(t => t.Type, m => m.MapFrom(f => (GuardianIdentifierType)(int)f.Type));

        CreateMap<Portkey.Contracts.CA.GuardianList, GuardianListDto>();

        CreateMap<GetHolderInfoOutput, GuardianResultDto>()
            .ForMember(t => t.CaHash, m => m.MapFrom(f => f.CaHash.ToHex()))
            .ForMember(t => t.CaAddress, m => m.MapFrom(f => f.CaAddress.ToBase58()));

        CreateMap<CASecurity.Entities.Es.ContactAddress, UserContactAddressDto>();
        
        CreateMap<TransactionsDto, IndexerTransactions>()
            .ForMember(t => t.CaHolderTransaction, m => m.MapFrom(f => f.TwoCaHolderTransaction));

    }
}