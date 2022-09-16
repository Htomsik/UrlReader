using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AppInfrastructure.Stores.DefaultStore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Services.FileService.UrlStoreFileService;

/// <summary>
///     Json realizing for IStoreFileService
/// </summary>
/// <typeparam name="TCollection"></typeparam>
public class BaseJsonCollectionStoreFileService<TCollection,TValue> : IStoreFileService
where TCollection : ICollection<TValue>, new()
{

    #region Stores and Services

    private readonly IStore<TCollection> _store;

    private readonly IFileService<string> _jsonFileService;

    #endregion

    #region Properties and Fields

    private readonly ILogger _logger;

    /// <summary>
    ///     
    /// </summary>
    private int _separatingCollectionCount = 0;

    #endregion

    #region Constructors

    public BaseJsonCollectionStoreFileService(
        IStore<TCollection> store,
        IFileService<string> jsonFileService,
        ILogger logger)
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
    public async Task GetDataFromFile()
    {
        var noSerializedText = await _jsonFileService.GetDataFromFile();
        
        if (string.IsNullOrEmpty(noSerializedText))
        {
            _logger.LogError("Operation denied or file empty. Please take other file");
            return;
        }

        TCollection? deserCollection = new TCollection();
        
        int convertedCount = 0;
        
        var loggerTimer = Stopwatch.StartNew();
        
        await foreach (var item in TextSeparator(noSerializedText))
        {
            try
            {
                await Task.Run(() =>
                {
                    
                    if (deserCollection.Count == convertedCount + 50)
                    {
                        _logger.LogInformation("Converting {0}/{1} value...",deserCollection.Count,_separatingCollectionCount);
                        convertedCount += 50;
                    }
                    
                    var deserializedObject = fastJSON.JSON.ToObject<TValue>(item);
                  
                    if (deserializedObject is null)
                    {
                        throw new ArgumentNullException(nameof(deserializedObject));
                    }
                    
                    deserCollection.Add(deserializedObject);
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Invalid data format. value : {0}/{1}",convertedCount);
            }
        }
        
        if (deserCollection.Count == 0 || deserCollection.Equals(new TCollection()))
        {
            _logger.LogInformation("Failed deserialize data. Please take other file");
            return;
        }
        
        _logger.LogWarning("All data converted.Elapsed time: {0}s", loggerTimer.Elapsed.TotalSeconds);
        loggerTimer.Stop();
        
        _store.CurrentValue = deserCollection;
        
    }
    
    /// <summary>
    ///     Separating Json Array text on blocks
    /// </summary>
    /// <param name="text">Separating text</param>
    private async IAsyncEnumerable<string> TextSeparator(string text)
    {
        text = text.Replace("[", "");
        text = text.Replace("]", "");

        var separatingText = text.Split(',');

        _separatingCollectionCount = separatingText.Length;

        foreach (var item in separatingText)
        {
            yield return  item;
        }
    }

    #endregion
    
}