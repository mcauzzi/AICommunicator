using AnythingLLMCommunicator;
using Serilog;
using Serilog.Formatting.Compact;
using ServiceImplementations;
using ServiceImplementations.Configs;
using ServiceInterfaces;

var       builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<ISpeech, AzureSpeech>();
builder.Services.AddScoped<ILLMWebApiCommunicator, ServiceImplementations.AnythingLLMCommunicator>();
builder.Services.AddSerilog(x => x.WriteTo.Async(x => x.Console(outputTemplate:
                                                                "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                                  .WriteTo.Async(x => x.File(new CompactJsonFormatter(), "./DockerStuff/Logs")));

builder.Services.Configure<AnythingLLMConfig>(builder.Configuration.GetSection(nameof(AnythingLLMConfig)));

builder.Services.AddHostedService<MainLoop>();

var host = builder.Build();
host.Run();