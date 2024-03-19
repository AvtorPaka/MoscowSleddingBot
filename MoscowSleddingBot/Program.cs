using Telegram.Bot;
using MoscowSleddingBot.Services;
using MoscowSleddingBot.Interfaces;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(configBuilder =>
    configBuilder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true));

// Logging configuration.
// builder.ConfigureLogging(loggingBuilder =>
// {
//     var loggerPath = PathGetter.Get("var");
//     loggingBuilder.AddProvider(new FileLoggerProvider(loggerPath));
// });

builder.ConfigureServices((context, services) =>
{
    // string token = context.Configuration["MoscowSleddingBotToken"] ?? throw new Exception("Bot token wasnt found.");
    string token = Environment.GetEnvironmentVariable("TOKEN")!;

    services.AddHttpClient("TelegramBotClient")
        .AddTypedClient<ITelegramBotClient>(_ => new TelegramBotClient(new TelegramBotClientOptions(token)));

    services.AddTransient<IMessageService, MessageService>();
    services.AddTransient<ICallbackQueryService, CallbackQueryService>();
    services.AddScoped<UpdateHandler>();
    services.AddScoped<ReceiverService>();
    services.AddHostedService<PollingService>();
});

IHost host = builder.Build();

await host.RunAsync();