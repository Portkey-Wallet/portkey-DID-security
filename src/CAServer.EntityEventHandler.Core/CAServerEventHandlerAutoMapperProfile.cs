using AutoMapper;
using CAServer.Chain;
using CAServer.Guardian;
using CAServer.Entities.Es;
using CAServer.Etos;
using CAServer.Etos.Chain;
using CAServer.Tokens.Etos;
using ContactAddress = CAServer.Entities.Es.ContactAddress;

namespace CAServer.EntityEventHandler.Core;

public class CAServerEventHandlerAutoMapperProfile : Profile
{
    public CAServerEventHandlerAutoMapperProfile()
    {
        CreateMap<ContactIndex, ContactCreateEto>().ReverseMap();
        CreateMap<ContactUpdateEto, ContactIndex>();
        CreateMap<ContactAddress, ContactAddressEto>().ReverseMap();
        CreateMap<UserTokenEto, UserTokenIndex>()
            .ForPath(t => t.Token.Id, m => m.MapFrom(u => u.Token.Id))
            .ForPath(t => t.Token.Symbol, m => m.MapFrom(u => u.Token.Symbol))
            .ForPath(t => t.Token.ChainId, m => m.MapFrom(u => u.Token.ChainId))
            .ForPath(t => t.Token.Decimals, m => m.MapFrom(u => u.Token.Decimals))
            .ForPath(t => t.Token.Address, m => m.MapFrom(u => u.Token.Address));
        CreateMap<UpdateCAHolderEto, CAHolderIndex>();
        CreateMap<DefaultToken, DefaultTokenInfo>();
        CreateMap<ChainCreateEto, ChainsInfoIndex>();
        CreateMap<ChainUpdateEto, ChainsInfoIndex>();
        CreateMap<ChainDeleteEto, ChainsInfoIndex>();
        CreateMap<GuardianEto, GuardianIndex>();
    }
}