using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace DataLibrary;

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
            Console.WriteLine($"{ex.Message}\nПроизошла хуйня с джейсоном.");
            isConvCorrect = false;
            return new List<IceHillData>();
        }
    }

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
        catch (Exception)
        {
            isWriteCorrect = false;
            return Stream.Null;
        }
    }
}