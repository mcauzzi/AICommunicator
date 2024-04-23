using AnythingLLMCommunicator;
using Serilog;
using Serilog.Formatting.Compact;
using SerilogTracing;
using ServiceImplementations;
using ServiceInterfaces;

var       builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<ISpeech, AzureSpeech>();
builder.Services.AddScoped<IWebApiCommunicator, WebApiCommunicator>();
builder.Services.AddSerilog(x => x.WriteTo.Async(x => x.Console(outputTemplate:
                                                                "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                                  .WriteTo.Async(x => x.File(new CompactJsonFormatter(), "./DockerStuff/Logs")));
builder.Services.AddHostedService<MainLoop>();

var host = builder.Build();
host.Run();