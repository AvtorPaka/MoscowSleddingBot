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
        // Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //removes "" with \" not the best solution
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
            Console.WriteLine($"{ex.Message}\nError occured while handling JSON Read.");
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
            //trying to fix the \u0022 symbol

            // TextEncoderSettings encoderSettings = new TextEncoderSettings();
            // encoderSettings.AllowCharacters('\u0022');
            // encoderSettings.AllowRanges(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);

            // JsonSerializerOptions optionsWrite = new JsonSerializerOptions
            // {
            //     WriteIndented = true,
            //     PropertyNameCaseInsensitive = true,
            //     Encoder = JavaScriptEncoder.Create(encoderSettings),
            //     AllowTrailingCommas = true
            // };

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