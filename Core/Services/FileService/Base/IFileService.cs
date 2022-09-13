namespace Core.Services.FileService;

/// <summary>
///     File service for get some info from out
/// </summary>
/// <typeparam name="TValue">some generic value</typeparam>
public interface IFileService<TValue>
{
    TValue GetDataFromFile();
}