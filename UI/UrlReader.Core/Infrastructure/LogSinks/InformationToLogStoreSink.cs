using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;
using Serilog.Core;
using Serilog.Events;

namespace UrlReader.Core.Infrastructure.LogSinks;

/// <summary>
///     Serilog sink to LogStore
/// </summary>
public sealed class InformationToLogStoreSink : ILogEventSink
{
    #region Properties and Fileds

    private readonly ICollectionRepository<ObservableCollection<string>,string>  _infoLogStore;

    #endregion

    #region Constructors

    public InformationToLogStoreSink(ICollectionRepository<ObservableCollection<string>,string> infoLogStore)
    {
        _infoLogStore = infoLogStore;
    }

    #endregion

    #region Methods
    
    public void Emit(LogEvent logEvent)
    {
        //  if (logEvent.Level == LogEventLevel.Information)
        
        if (_infoLogStore.CurrentValue.Count > 50)
        {
            _infoLogStore.CurrentValue = new ObservableCollection<string>();
        }
        
        _infoLogStore.AddIntoEnumerable(logEvent.RenderMessage());
    }
    
    #endregion
    
}