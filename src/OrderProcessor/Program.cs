using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<AspireLearning.Repository.MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddHostedService<AspireLearning.OrderProcessor.Worker>();

var host = builder.Build();
host.Run();
