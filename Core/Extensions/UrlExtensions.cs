﻿using System;
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
    #region IsAliveAsync :   Checking url status

    /// <summary>
    ///     Checking url status
    /// </summary>
    /// <param name="url">Url type</param>
    /// <param name="client">Http client</param>
    /// <returns>True if path is alive</returns>
    /// <exception cref="ArgumentNullException">If url or Url.Pat is null</exception>
    public static async Task<bool> IsAliveAsync(this Url url,CancellationToken canceltoken,HttpClient client = null)
    {
        //If Null - throwed exception
        NullChecker(url);
        
        client ??= new ();

        HttpResponseMessage response;
        
        try
        {
             response = await client.GetAsync(url.Path,canceltoken);
        }
        catch
        {
            return false;
        }
        
        return  response != null && response.IsSuccessStatusCode;
    }
    
    #endregion

    #region HtmlDownloadAsync :  Download html page in string format

    /// <summary>
    ///     Download html page in string format
    /// </summary>
    /// <param name="url">Url type</param>
    /// <param name="cancelToken">Cancceled operation token</param>
    /// <param name="client">Http client</param>
    /// <returns>Download Html page in string format</returns>
    public static async Task<string?> HtmlDownloadAsync(this Url url,CancellationToken cancelToken,HttpClient client = null)
    {
        //If Null - throwed exception
        NullChecker(url);
        
        client ??= new ();
        
        HttpResponseMessage? response = null;
        
        
        try
        {
            response = await client.GetAsync(url.Path ,cancelToken);
        }
        catch (TaskCanceledException ex)
        {
            //This ex generated by Timeout. Just ignore it
        }
 
      
        string? source = null;

        if(response != null && response.IsSuccessStatusCode)
            source = await response.Content.ReadAsStringAsync();
        
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
    private static void NullChecker(Url url)
    {
        if (url is null)
            throw new ArgumentNullException(nameof(url));

        if (url.Path is null)
            throw new ArgumentNullException(nameof(url.Path));
        
    }


    #endregion
   
}