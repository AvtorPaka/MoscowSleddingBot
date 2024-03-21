using Telegram.Bot;
using MoscowSleddingBot.Services;
using MoscowSleddingBot.Interfaces;
using MoscowSleddingBot.Log;
using MoscowSleddingBot.Additional;

// Creating an IHostBuilder object to build or knot
var builder = Host.CreateDefaultBuilder(args);

//Setting the application configuration
builder.ConfigureAppConfiguration((context, config) =>
{
    IHostEnvironment envir = context.HostingEnvironment;
    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
});

//Setting up the configuration of the services
builder.ConfigureServices((context, services) =>
{   
    //Adding a new http client - our bot
    string token = context.Configuration["MoscowSleddingBotToken"] ?? throw new Exception("Bot token is missing.");
    services.AddHttpClient("TelegramBotClient").AddTypedClient<ITelegramBotClient>(_ =>
    {
        return new TelegramBotClient(new TelegramBotClientOptions(token));
    });

    //Adding services
    services.AddTransient<ICallbackQueryService, CallbackQueryService>();
    services.AddTransient<IMessageService, MessageService>();
    services.AddScoped<UpdateHandler>();
    services.AddScoped<ReceiverService>();
    services.AddHostedService<PollingService>();
});

//Adding a logger
string dirToLogData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoggData");
builder.ConfigureLogging(loggingBuilder => {loggingBuilder.AddProvider(new CustomLoggingProvider(dirToLogData));});

//Creating an IHost object with bot services
using IHost host = builder.Build();

//Launching the node/host
await host.RunAsync();