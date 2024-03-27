using System.Runtime.InteropServices;
using System.Text;


namespace MoscowSleddingBot.Additional;

/// <summary>
/// An auxiliary class for working with directories
/// </summary>
public static class DirectoryHelper
{   
    /// <summary>
    /// An auxiliary class for getting the directory to the data
    /// </summary>
    /// <param name="envirVariableName">environment variable</param>
    /// <param name="addToPath">file name</param>
    /// <returns>The line with the formed directory</returns>
    public static string GetDirectoryFromEnvironment(string envirVariableName = "PathToLoadedData", string addToPath = "")
    {   
        string? pathToSmth = envirVariableName switch
        {
            "PathToLoadedData" => "./LoadedData/",
            "OriginalFilePath" => "./Assets/ice-hills.csv",
            "PathToLoggData" => "./var/",
            _ => "../"
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { pathToSmth.Replace('/', '\\'); }

        return Path.Combine(pathToSmth, addToPath);
    }
}