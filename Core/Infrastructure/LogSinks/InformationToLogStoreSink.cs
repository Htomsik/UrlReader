using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using Serilog.Core;
using Serilog.Events;

namespace Core.Infrastructure.LogSinks;

/// <summary>
///     Serilog sink to LogStore
/// </summary>
public sealed class InformationToLogStoreSink : ILogEventSink
{
    #region Properties and Fileds

    private readonly IStore<ObservableCollection<string>>  _infoLogStore;

    #endregion

    #region Constructors

    public InformationToLogStoreSink(IStore<ObservableCollection<string>> infoLogStore)
    {
        _infoLogStore = infoLogStore;
    }

    #endregion

    #region Methods
    
    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level == LogEventLevel.Information)
            _infoLogStore.CurrentValue.Add(logEvent.RenderMessage());
    }
    
    #endregion
    
}