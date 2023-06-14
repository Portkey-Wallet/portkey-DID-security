using System;
using System.Threading.Tasks;
using CASecurity.Tokens.Dtos;

namespace CASecurity.Tokens
{
    public interface IUserTokenAppService
    {
        Task<UserTokenDto> ChangeTokenDisplayAsync(bool isDisplay, Guid id);
        Task<UserTokenDto> AddUserTokenAsync(Guid userId,AddUserTokenInput input);
    }
}