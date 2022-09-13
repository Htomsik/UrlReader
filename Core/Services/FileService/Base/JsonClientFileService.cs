using System;
using System.IO;
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
    
    public JsonClientFileService(ILogger<JsonClientFileService> logger)
    {
        _logger = logger;
    }
    
    public string GetDataFromFile()
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

            using (StreamReader reader = new StreamReader(fileStream))
            {
                fileContent = reader.ReadToEnd();
            }
        }

        return fileContent;
    }
   
}