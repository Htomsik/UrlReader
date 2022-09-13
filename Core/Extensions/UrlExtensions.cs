using System;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Extensions;

/// <summary>
///     Exstensions for url type
/// </summary>
public static class UrlExtensions
{
    #region Methods

    /// <summary>
    ///     Checking url status
    /// </summary>
    /// <param name="url">Url type</param>
    /// <returns>True if path is alive</returns>
    /// <exception cref="ArgumentNullException">If url or Url.Pat is null</exception>
    public static async Task<bool> IsAlive(this Url url)
    {
        if (url is null)
            throw new ArgumentNullException(nameof(url));

        if (url.Path is null)
            throw new ArgumentNullException(nameof(url.Path));
        
        var client = new HttpClient();

        HttpResponseMessage response;
        
        try
        {
             response = await client.GetAsync(url.Path);
        }
        catch
        {
            return false;
        }
        
        return response.IsSuccessStatusCode;
    }

    #endregion
  
}