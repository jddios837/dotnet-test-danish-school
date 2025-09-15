using System.Text.Json;

namespace Malte.Clean.Data.Storage;

public class JsonStorageService
{
    private readonly string _dataDirectory;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _fileLock;

    public JsonStorageService(string dataDirectory = "data")
    {
        _dataDirectory = dataDirectory;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _fileLock = new SemaphoreSlim(1, 1);

        // Ensure data directory exists
        if (!Directory.Exists(_dataDirectory))
        {
            Directory.CreateDirectory(_dataDirectory);
        }
    }

    public async Task<T?> ReadAsync<T>(string fileName) where T : class
    {
        var filePath = Path.Combine(_dataDirectory, fileName);

        await _fileLock.WaitAsync();
        try
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var jsonContent = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(jsonContent, _jsonOptions);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to deserialize JSON from file: {fileName}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to read file: {fileName}", ex);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task WriteAsync<T>(string fileName, T data) where T : class
    {
        var filePath = Path.Combine(_dataDirectory, fileName);

        await _fileLock.WaitAsync();
        try
        {
            var jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(filePath, jsonContent);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to write file: {fileName}", ex);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> FileExistsAsync(string fileName)
    {
        await _fileLock.WaitAsync();
        try
        {
            var filePath = Path.Combine(_dataDirectory, fileName);
            return File.Exists(filePath);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task DeleteAsync(string fileName)
    {
        var filePath = Path.Combine(_dataDirectory, fileName);

        await _fileLock.WaitAsync();
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete file: {fileName}", ex);
        }
        finally
        {
            _fileLock.Release();
        }
    }
}