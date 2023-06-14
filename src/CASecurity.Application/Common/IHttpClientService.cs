using System.Threading.Tasks;

namespace CASecurity.Common;

public interface IHttpClientService
{
    Task<T> GetAsync<T>(string url);
}