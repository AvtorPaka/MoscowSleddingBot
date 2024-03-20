using Telegram.Bot;
using MoscowSleddingBot.Services;
using MoscowSleddingBot.Interfaces;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    IHostEnvironment envir = context.HostingEnvironment;
    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
});

// builder.ConfigureLogging(loggingBuilder =>
// {
//     var loggerPath = PathGetter.Get("var");
//     loggingBuilder.AddProvider(new FileLoggerProvider(loggerPath));
// });

builder.ConfigureServices((context, services) =>
{   
    // string token = Environment.GetEnvironmentVariable("MoscowSleddingBotToken")!;
    string token = context.Configuration["MoscowSleddingBotToken"] ?? throw new Exception("Bot token wasnt found.");
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

IHost host = builder.Build();

await host.RunAsync();