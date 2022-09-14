using System;
using AppInfrastructure.Stores.DefaultStore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Services.FileService.UrlStoreFileService;

/// <summary>
///     Json realizing for IStoreFileService
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class BaseJsonStoreFileService<TValue> : IStoreFileService
{

    #region Stores and Services

    private readonly IStore<TValue> _store;

    private readonly IFileService<string> _jsonFileService;

    #endregion

    #region Properties and Fields

    private readonly ILogger _logger;

    #endregion

    #region Constructors

    public BaseJsonStoreFileService(
        IStore<TValue> store,
        IFileService<string> jsonFileService,
        ILogger<BaseJsonStoreFileService<TValue>> logger)
    {
        #region Stores and Services Initializing

        _store = store;
        
        _jsonFileService = jsonFileService;

        #endregion

        #region Properties and Fields Initalizing

        _logger = logger;

        #endregion
    }


    #endregion

    #region Methods

    public void GetDataFromFile()
    {
        var noSerializedText = _jsonFileService.GetDataFromFile();

        if (string.IsNullOrEmpty(noSerializedText))
        {
            _logger.LogInformation("Operation denied or file empty. Please take other file");
            return;
        }
        
        TValue? deserializedValue = default;
        
        try
        {
            deserializedValue = JsonConvert.DeserializeObject<TValue>(noSerializedText);
        }
        catch (Exception e)
        {
            _logger.LogError("Invalid data format. Please take other file",e);
            return;
        }

        if (deserializedValue is null || deserializedValue.Equals(default(TValue)))
        {
            _logger.LogInformation("Failed deserialize data. Please take other file");
            return;
        }
        
        _store.CurrentValue = deserializedValue;
        
    }

    #endregion
    
}