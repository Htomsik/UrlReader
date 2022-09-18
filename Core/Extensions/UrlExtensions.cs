using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Extensions;

/// <summary>
///     Extensions for url type
/// </summary>
public static class UrlExtensions
{
   
    
    #region HtmlDownloadAsync :  Download html page in string format

    /// <summary>
    ///     Download html page in string format and set state to service url
    /// </summary>
    /// <param name="url">Url type</param>
    /// <param name="cancelToken">Canceled operation token</param>
    /// <param name="client">Http client</param>
    /// <exception cref="ArgumentNullException">If Url or Url.Path or client is null</exception>
    /// <returns>Download Html page in string format</returns>
    public static async Task<string?> HtmlDownloadAsync(this ServiceUrl url,CancellationToken cancelToken,HttpClient client)
    {
        //If Null - throw exception
        url.NullChecker();

        if (client is null)
        {
            throw new ArgumentNullException(nameof(client));
        }
        
        
        try
        {
            using (HttpResponseMessage response = await client.GetAsync(url.Path, cancelToken).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    url.State = UrlState.Alive;
                    return await response.Content.ReadAsStringAsync(cancelToken);
                }
            }
        }
        catch 
        {
            url.State = UrlState.Unknown;
            return null;
        }
        
        url.State = UrlState.NotAlive;
        return null;
    }
    
    #endregion

    #region NullChecker :  Check Url and Url.path of null

    /// <summary>
    ///     Check Url and Url.path of null
    /// </summary>
    /// <param name="url">Url type</param>
    /// <returns>True if is not null, else throw exception</returns>
    /// <exception cref="ArgumentNullException">If Url or Url.Path null</exception>
    public static void NullChecker(this Url url)
    {
        if (url is null)
            throw new ArgumentNullException(nameof(url));

        if (url.Path is null || string.IsNullOrEmpty(url.Path.Trim()))
            throw new ArgumentNullException(nameof(url));
        
    }


    #endregion
   
}