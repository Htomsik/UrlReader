using System;
using System.Net;
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
    #region CheckState :   Checking url status

    /// <summary>
    ///     Checking url status
    /// </summary>
    /// <param name="url">Url type</param>
    /// <param name="cancelToken">Canceled operation token</param>
    /// <param name="client">Http client</param>
    /// <returns>UrlState.Alive is Alive , UrlState.NotAlive is NotAlive, and  UrlState.Unknown if timeout ends</returns>
    /// <exception cref="ArgumentNullException">If url or Url.Pat is null</exception>
    public static async Task<UrlState> CheckState(this Url url,CancellationToken cancelToken,HttpClient client = null)
    {
       
        url.NullChecker();
        
        client ??= new ();
        
        HttpResponseMessage response;
        
        try
        {
             response = await client.GetAsync(url.Path,cancelToken).ConfigureAwait(false);
        }
        catch
        {
            return UrlState.Unknown;
        }
        
        return  response.IsSuccessStatusCode ? UrlState.Alive : UrlState.NotAlive;
    }
    
    #endregion

    #region HtmlDownloadAsync :  Download html page in string format

    /// <summary>
    ///     Download html page in string format
    /// </summary>
    /// <param name="url">Url type</param>
    /// <param name="cancelToken">Canceled operation token</param>
    /// <param name="client">Http client</param>
    /// <returns>Download Html page in string format</returns>
    public static async Task<string?> HtmlDownloadAsync(this Url url,CancellationToken cancelToken,HttpClient client = null)
    {
        //If Null - throw exception
        url.NullChecker();
        
        client ??= new ();
        
        HttpResponseMessage? response = null;

        try
        {
            response = await client.GetAsync(url.Path, cancelToken).ConfigureAwait(false);
        }
        catch 
        {
            return null;
        }
       
        
        string? source = null;

        if((bool)response?.IsSuccessStatusCode)
            source = await response.Content.ReadAsStringAsync(cancelToken);
        
        return source;
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