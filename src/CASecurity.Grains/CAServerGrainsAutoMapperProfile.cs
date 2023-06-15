using AElf.Types;
using AutoMapper;
using Google.Protobuf.Collections;
using Portkey.Contracts.CA;

namespace CASecurity.Grains;

public class CASecurityGrainsAutoMapperProfile : Profile
{
    public CASecurityGrainsAutoMapperProfile()
    {

        CreateMap<GetHolderInfoOutput, ValidateCAHolderInfoWithManagerInfosExistsInput>()
            .ForMember(d => d.LoginGuardians,
                opt => opt.MapFrom(e => new RepeatedField<Hash>
                    { e.GuardianList.Guardians.Where(g => g.IsLoginGuardian).Select(g => g.IdentifierHash).ToList() }))
            .ForMember(d => d.ManagerInfos, opt => opt.MapFrom(g => g.ManagerInfos))
            .ForMember(d => d.CaHash,
                opt => opt.MapFrom(g => g.CaHash));

    }
}