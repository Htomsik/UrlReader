using UrlReader.Core.Services.StatisticService.Base;
using UrlReader.Core.Stores;

namespace UrlReader.Core.Services.StatisticService;

/// <summary>
///     Statistic service for ServiceUrlStore
/// </summary>
public class ServiceUrlStoreStatisticService : BaseUrlsStoreStatisticService
{
    public ServiceUrlStoreStatisticService(ServiceUrlStore urlsStore) : base(urlsStore)
    {
    }
}