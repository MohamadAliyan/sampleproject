using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.Web.Infrastructure
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null);

        Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string requestId = null);

        Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null);
    }
}
