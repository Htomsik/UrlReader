using System;
using System.Diagnostics;

using Microsoft.Extensions.Logging;


namespace Core.Services.AppInfrastructure;

/// <summary>
///     Exception observer. Sink to logger
/// </summary>
internal sealed class GlobalExceptionHandler : IObserver<Exception>
{
    #region Properties and Fileds

    private readonly ILogger _logger;

    #endregion

    #region Constructors

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        #region Properties and Fileds Initializing

        _logger = logger;

        #endregion
       
    }

    #endregion
    
    
    #region Methods

    public void OnCompleted()
    {
        if(Debugger.IsAttached)
            Debugger.Break();
    }
    
    public void OnError(Exception error)
    {
        if(Debugger.IsAttached) Debugger.Break();
        _logger.LogCritical(error, "{0}:{1}", error.Source, error.Message);
    }

    public void OnNext(Exception value)
    {
        if (Debugger.IsAttached) Debugger.Break();

        _logger.LogCritical(value, "{0}:{1}", value.Source, value.Message);
    }
}
    #endregion
