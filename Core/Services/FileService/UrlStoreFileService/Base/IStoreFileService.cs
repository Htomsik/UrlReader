using System.Threading.Tasks;

namespace Core.Services.FileService.UrlStoreFileService;

/// <summary>
///     File service for stores
/// </summary>
public interface IStoreFileService
{
    Task GetDataFromFile();
}