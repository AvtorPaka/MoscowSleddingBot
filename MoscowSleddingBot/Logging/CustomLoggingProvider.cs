namespace MoscowSleddingBot.Logging;

/// <summary>
/// Provider class for a custom logger
/// </summary>
public class CustomLoggingProvider: ILoggerProvider
{
    public void Dispose() {}

    private string DirToLog {get; init;}

    public CustomLoggingProvider(string dirToLog)
    {
        DirToLog = dirToLog;
    }

    public ILogger CreateLogger(string ntu)
    {
        return new CustomLogger(DirToLog);
    }
}