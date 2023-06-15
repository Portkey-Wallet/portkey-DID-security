using System.Collections.Generic;
using System.Threading.Tasks;
using CASecurity.CAActivity.Dto;
using CASecurity.CAActivity.Dtos;
using Volo.Abp.Application.Services;

namespace CASecurity.CAActivity;

public interface IUserActivityAppService:IApplicationService
{
    Task<GetActivitiesDto> GetTwoCaTransactionsAsync(GetTwoCaTransactionRequestDto request);
    Task<GetActivitiesDto> GetActivitiesAsync(GetActivitiesRequestDto request);
    Task<GetActivityDto> GetActivityAsync(GetActivityRequestDto request);
}