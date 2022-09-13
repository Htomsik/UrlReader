using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.ParserService.UrlStoreParser;

/// <summary>
///     Parse service for store
/// </summary>
public interface IStoreParser<TParameter>
{
    /// <summary>
    ///     Parse all items from Istore
    /// </summary>
    /// <param name="parameter">Parsing parameter</param>
    /// <param name="cancelToken">Cancel operation token</param>
    Task Parse(TParameter parameter,CancellationToken? cancelToken = null);
}