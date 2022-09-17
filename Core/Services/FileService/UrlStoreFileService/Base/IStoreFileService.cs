using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.FileService.UrlStoreFileService;

/// <summary>
///     File service for stores
/// </summary>
public interface IStoreFileService
{
    /// <summary>
    ///     Getting and processing some data from file
    /// </summary>
    /// <param name="cancellationToken">Cancel operation token</param>
    /// <returns></returns>
    Task GetDataFromFile(CancellationToken cancellationToken);
}