using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AppInfrastructure.Stores.DefaultStore;
using Microsoft.Extensions.Logging;


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
    
    #endregion
    
    #region TemporaryData

    /// <summary>
    ///     Count of separating string lines
    /// </summary>
    private int _separatingCollectionCount = 0;
    
    /// <summary>
    ///     Count of converted values
    /// </summary>
    private  int _convertedCount = 0;
    
    /// <summary>
    ///     Count of deserialized values
    /// </summary>
    private TCollection? _deserializedValues = new ();


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

    #region GetDataFromFile : Getting and processing some data from file

    public async Task GetDataFromFile(CancellationToken cancellationToken)
    {
        var noSerializedText = await _jsonFileService.GetDataFromFile();
        
        if (string.IsNullOrEmpty(noSerializedText))
        {
            _logger.LogError("Operation denied or file empty. Please take other file");
            return;
        }
        
        var loggerTimer = Stopwatch.StartNew();
        
        await foreach (var item in TextSeparator(noSerializedText).WithCancellation(cancellationToken))
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Operation denied. War operating {0},{1}",_convertedCount,_separatingCollectionCount);
                    break;
                }

                await JsonDeserialize(item,cancellationToken);

            }
            catch (Exception e)
            {
                _logger.LogError(e,"Invalid data format. value : {0}/{1}",_convertedCount,_separatingCollectionCount);
            }
        }
        
        if (_deserializedValues?.Count == 0 || (bool)_deserializedValues?.Equals(new TCollection()))
        {
            _logger.LogWarning("Failed deserialize data. Please take other file");
            return;
        }
        
        _logger.LogWarning("All data converted.Elapsed time: {0}s", loggerTimer.Elapsed.TotalSeconds);
        
        loggerTimer.Stop();
        
        _store.CurrentValue =  _deserializedValues;

        DisposingTemporaryValues();

    }

    #endregion

    #region TextSeparator :  Separating Json Array text on blocks

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

    #region JsonDeserialize : Deserialize string to json

    /// <summary>
    ///     deserialize module
    /// </summary>
    /// <param name="item">Text to deserialize</param>
    /// <param name="cancellationToken">Cancel opearation tokek</param>
    /// <exception cref="ArgumentNullException">If deserialized object is null</exception>
    private async Task JsonDeserialize(string item,CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            if (_deserializedValues?.Count == _convertedCount + 50)
            {
                _logger.LogInformation("Converting {0}/{1} value...",_deserializedValues.Count,_separatingCollectionCount);
                _convertedCount += 50;
            }

            TValue deserializedObject = default;
            
            deserializedObject = fastJSON.JSON.ToObject<TValue>(item);
            
            if (deserializedObject is null)
            {
                throw new ArgumentNullException(nameof(deserializedObject));
            }
            

            _deserializedValues ??= new();

            _deserializedValues?.Add(deserializedObject);
            
        },cancellationToken);

    }

    #endregion

    #region DisposingTemporaryValues : Set to default or null temporary data

    /// <summary>
    ///     Set to null temporary data
    /// </summary>
    private void DisposingTemporaryValues()
    {
        _convertedCount = 0;

        _separatingCollectionCount = 0;

        _deserializedValues = default(TCollection);
    }

    #endregion

    #endregion
    
}