using System.Threading.Tasks;

namespace UoN.AspNetCore.VersionMiddleware
{
    /// <summary>
    /// Basic Interface for classes which are able to provide version information.
    /// </summary>
    public interface IVersionInformationProvider
    {
        /// <summary>
        /// Gets version information from the configured source.
        /// </summary>
        /// <returns>An object containing the acquired version information.</returns>
        Task<object> GetVersionInformationAsync();
    }
}
