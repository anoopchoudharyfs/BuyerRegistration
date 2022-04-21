using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BuyerRegistration.Api.Services.IService
{
    public interface IRequestResponseLogger
    {
        Task LogResponse(MemoryStream memoryStream, Stream originalBodyStream, string routeName);
        Task LogRequest(HttpContext context, string routeName);
        bool IsExcludeLogRoute(string[] excludeLogRoute);
    }


}
