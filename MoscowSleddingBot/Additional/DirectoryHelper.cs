using System.Runtime.InteropServices;
using System.Text;


namespace MoscowSleddingBot.Additional;

/// <summary>
/// An auxiliary class for working with directories
/// </summary>
public static class DirectoryHelper
{   
    /// <summary>
    /// An auxiliary class for getting the directory to the data from the environment variables
    /// </summary>
    /// <param name="envirVariableName">environment variable</param>
    /// <param name="addToPath">file name</param>
    /// <returns>The line with the formed directory</returns>
    public static string GetDirectoryFromEnvironment(string envirVariableName = "PathToLoadedData", string addToPath = "")
    {   
        string? pathToSmth;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { pathToSmth = Environment.GetEnvironmentVariable($"{envirVariableName}Win")!; }
        else { pathToSmth = Environment.GetEnvironmentVariable($"{envirVariableName}UNIX")!;}

        return Path.Combine(pathToSmth, addToPath);
    }
}