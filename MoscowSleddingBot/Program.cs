using Telegram.Bot;
using MoscowSleddingBot.Services;
using MoscowSleddingBot.Interfaces;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    IHostEnvironment envir = context.HostingEnvironment;
    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
});

builder.ConfigureServices((context, services) =>
{   
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

using IHost host = builder.Build();

await host.RunAsync();