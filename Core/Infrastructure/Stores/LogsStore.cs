using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;

namespace Core.Infrastructure.Stores;

/// <summary>
///     Information log store
/// </summary>
internal sealed class LogsStore : BaseLazyCollectionRepository<ObservableCollection<string>, string>
{
    protected override bool addIntoEnumerable(string value)
    {
        CurrentValue?.Add(value);
        return true;
    }
}
