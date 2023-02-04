using System.Threading.Tasks;

namespace UrlReader.Core.Services.FileService.Base;

/// <summary>
///     File service for get some info from out
/// </summary>
/// <typeparam name="TValue">some generic value</typeparam>
public interface IFileService<TValue>
{
    Task<TValue> GetDataFromFile();
}