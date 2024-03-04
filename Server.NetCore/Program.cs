
using Implementation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Sila.Contracts;
using Tecan.Sila2.Cancellation;
using Tecan.Sila2.Server;

var startInfo = ServerConfigReader.ReadServerStartInformation(args);
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrelForSila2(startInfo, options => options.Protocols = HttpProtocols.Http2);


// it is important to call AddSila2Components in order to add these components that also have a MEF registration    
builder.Services.AddSila2Components();
builder.Services.AddSila2(startInfo);

builder.Services.AddSingleton<ITestService, TestService>();
builder.Services.AddSingleton<IFeatureProvider, SiLAGen.TestService.TestServiceProvider>();

builder.Services.AddSingleton<ICancelController, CancelControllerService>();
builder.Services.AddSingleton<IFeatureProvider, CancelControllerProvider>();

var app = builder.Build();
app.Services.InitializeLogging();
app.MapSila2();

app.Run();
