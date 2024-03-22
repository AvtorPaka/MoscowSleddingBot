namespace MoscowSleddingBot.Logging;

/// <summary>
/// Custom logger class
/// </summary>
public class CustomLogger : ILogger
{
    private string DirToLog {get; init;}

    public CustomLogger(string dirToLog)
    {
        DirToLog = dirToLog;
    }

    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    private static readonly object _lock = new object();

    /// <summary>
    /// The method of logging information to a file
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="eventId"></param>
    /// <param name="state"></param>
    /// <param name="exception"></param>
    /// <param name="formatter"></param>
    /// <typeparam name="TState"></typeparam>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState,Exception?,string> formatter)
    {
        lock(_lock)
        {
            string pathToCurLog = $"{DirToLog}log_{DateTime.Now.ToString("dd-MM-yyyy")}.txt";
            File.AppendAllLines(pathToCurLog, new string[]{$"{state}"});
        }
    }
}