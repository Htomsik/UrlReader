using System;
using System.Net.Http;
using AppInfrastructure.Stores.DefaultStore;

namespace Core.Stores;

/// <summary>
///     Store for HttpClient
/// </summary>
public class HttpClientStore : BaseLazyStore<HttpClient>
{
    public override HttpClient CurrentValue => (HttpClient)_currentValue.Value;

    public HttpClientStore() => _currentValue = new Lazy<object?>(() => new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(4)
    });
}
   