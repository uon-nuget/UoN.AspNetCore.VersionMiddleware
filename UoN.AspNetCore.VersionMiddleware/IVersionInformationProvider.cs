using System.Threading.Tasks;

namespace UoN.AspNetCore.VersionMiddleware
{
    public interface IVersionInformationProvider
    {
        Task<string> GetVersionInformationAsync();
    }
}
