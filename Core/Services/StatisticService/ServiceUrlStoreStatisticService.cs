using Core.Stores;

namespace Core.Services.StatisticService;

/// <summary>
///     Statistic service for ServiceUrlStore
/// </summary>
public class ServiceUrlStoreStatisticService : BaseUrlsStoreStatisticService
{
    public ServiceUrlStoreStatisticService(ServiceUrlStore urlsStore) : base(urlsStore)
    {
    }
}