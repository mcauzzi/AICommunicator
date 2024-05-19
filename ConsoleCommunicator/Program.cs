using ConsoleCommunicator;
using Models.Configs;
using Serilog;
using Serilog.Formatting.Compact;
using ServiceImplementations;

var       builder = Host.CreateApplicationBuilder(args);
builder.Services.AddAiCommunicationServices();
builder.Services.AddSerilog(x => x.WriteTo.Async(x => x.Console(outputTemplate:
                                                                "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                                  .WriteTo.Async(x => x.File(new CompactJsonFormatter(), "./DockerStuff/Logs")));
builder.Services.Configure<AiCommunicatorConfig>(builder.Configuration.GetSection(nameof(AiCommunicatorConfig)));
builder.Services.AddHostedService<MainLoop>();
var host = builder.Build();
host.Run();