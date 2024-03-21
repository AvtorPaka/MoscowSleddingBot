using Telegram.Bot;
using MoscowSleddingBot.Services;
using MoscowSleddingBot.Interfaces;
using MoscowSleddingBot.Log;
using MoscowSleddingBot.Additional;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    IHostEnvironment envir = context.HostingEnvironment;
    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
});

builder.ConfigureServices((context, services) =>
{   
    string token = context.Configuration["MoscowSleddingBotToken"] ?? throw new Exception("Bot token is missing.");
    services.AddHttpClient("TelegramBotClient").AddTypedClient<ITelegramBotClient>(_ =>
    {
        return new TelegramBotClient(new TelegramBotClientOptions(token));
    });

    services.AddTransient<ICallbackQueryService, CallbackQueryService>();
    services.AddTransient<IMessageService, MessageService>();
    services.AddScoped<UpdateHandler>();
    services.AddScoped<ReceiverService>();
    services.AddHostedService<PollingService>();
});

string dirToLogData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoggData");
builder.ConfigureLogging(loggingBuilder => {loggingBuilder.AddProvider(new CustomLoggingProvider(dirToLogData));});

using IHost host = builder.Build();

await host.RunAsync();