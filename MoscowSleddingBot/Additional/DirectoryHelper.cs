using System.Runtime.InteropServices;
using System.Text;


namespace MoscowSleddingBot.Additional;

public static class DirectoryHelper
{
    public static string GetDirectoryFromEnvironment(string envirVariableName = "PathToLoadedData", string addToPath = "")
    {   
        string? pathToSmth;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { pathToSmth = Environment.GetEnvironmentVariable($"{envirVariableName}Win")!; }
        else { pathToSmth = Environment.GetEnvironmentVariable($"{envirVariableName}UNIX")!;}

        return Path.Combine(pathToSmth, addToPath);
    }
}