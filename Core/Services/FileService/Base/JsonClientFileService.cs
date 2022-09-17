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

    #endregion
    
    #region Constructors

    public JsonClientFileService(ILogger<JsonClientFileService> logger )
    {
        _logger = logger;
    }

    #endregion
    
    public async Task<string> GetDataFromFile()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();

        openFileDialog.InitialDirectory = "c:\\";
        openFileDialog.Filter = "Json Files|*.json;*.txt";
        openFileDialog.FilterIndex = 2;
        openFileDialog.RestoreDirectory = true;

        string fileContent = string.Empty;

        if (openFileDialog.ShowDialog() == true)
        {
            var fileStream = openFileDialog.OpenFile();
            
            _logger.LogInformation("File have {0} symbols",fileStream.Length);
            
            await foreach (var line in  GetDataLines(fileStream).ConfigureAwait(false))
            {
                fileContent += line;
            }
            
        }
        
        return fileContent.Trim();
    }
    
    private async IAsyncEnumerable<string> GetDataLines(Stream stream)
    {
        byte[] buffer = new byte[stream.Length];

        await stream.ReadAsync(buffer, 0, buffer.Length);
             
        yield return  Encoding.Default.GetString(buffer);
        
    }
}