using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Core.Services.FileService;

/// <summary>
///     Json openFileDialog service
/// </summary>
public sealed class JsonClientFileService : IFileService<string>
{

    #region Properties and Fields

    private readonly ILogger _logger;

    private readonly Lazy<OpenFileDialog> _openFileDialog;

    #endregion
    
    #region Constructors

    public JsonClientFileService(ILogger<JsonClientFileService> logger )
    {
        _logger = logger;

        _openFileDialog = new(() => new OpenFileDialog()
        {
            InitialDirectory = "c:\\",
            Filter = "Json Files|*.json;*.txt",
            FilterIndex = 2,
            RestoreDirectory = true
        });

    }

    #endregion
    
    
    public async Task<string> GetDataFromFile()
    {
        string fileContent = string.Empty;

        if (_openFileDialog.Value.ShowDialog() == true)
        {
            using (var fileStream = _openFileDialog.Value.OpenFile())
            {
                _logger.LogInformation("File have {0} symbols",fileStream.Length);
            
                await foreach (var line in  GetData(fileStream).ConfigureAwait(false))
                {
                    fileContent += line;
                }
            }
            
        }
        
        return fileContent.Trim();
    }
    
    /// <summary>
    ///     Getting data from file
    /// </summary>
    /// <param name="stream">File stream</param>
    private async IAsyncEnumerable<string> GetData(Stream stream)
    {
        byte[] buffer = new byte[stream.Length];

        await stream.ReadAsync(buffer, 0, buffer.Length);

        yield return  Encoding.Default.GetString(buffer);
    }
}