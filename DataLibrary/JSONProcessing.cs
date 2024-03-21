using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace DataLibrary;

/// <summary>
/// A class for working with Json files
/// </summary>
public class JSONProcessing
{
    private string? PathToWriteData { get; set; }

    private static JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        AllowTrailingCommas = true
    };

    public JSONProcessing(string? path = null)
    {
        PathToWriteData = path;
    }

    /// <summary>
    /// A method for reading data from a json file into a list of objects
    /// </summary>
    /// <param name="streamLoad"></param>
    /// <param name="isConvCorrect"></param>
    /// <returns></returns>
    public List<IceHillData> Read(FileStream streamLoad, out bool isConvCorrect)
    {
        try
        {
            List<IceHillData> lstWithData = new List<IceHillData>();
            lstWithData = JsonSerializer.Deserialize<List<IceHillData>>(streamLoad, options)!;

            isConvCorrect = true;
            return lstWithData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nError occured while handling JSON Read.");
            isConvCorrect = false;
            return new List<IceHillData>();
        }
    }

    /// <summary>
    /// A method for writing data from a list of class objects to a file in json format
    /// </summary>
    /// <param name="lstWithData"></param>
    /// <param name="isWriteCorrect"></param>
    /// <returns></returns>
    public Stream Write(List<IceHillData> lstWithData, out bool isWriteCorrect)
    {
        if (PathToWriteData == null)
        {
            isWriteCorrect = false;
            return Stream.Null;
        }
        try
        {
            using Stream fs = File.Open(PathToWriteData, FileMode.Create, FileAccess.ReadWrite);
            JsonSerializer.Serialize<List<IceHillData>>(fs, lstWithData, options);

            isWriteCorrect = true;
            return fs;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nError occured while handling JSON Write.");
            isWriteCorrect = false;
            return Stream.Null;
        }
    }
}