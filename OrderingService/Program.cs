using Docker.DotNet;
using Docker.DotNet.Models;
using OrderingService.Database;
using OrderingService.Facade;
using OrderingService.Services;

var builder = WebApplication.CreateBuilder(args);

#region start db

var _dockerClient = new DockerClientConfiguration().CreateClient();

ImagesCreateParameters imagesCreateParameters = new ImagesCreateParameters()
{
    FromImage = "mcr.microsoft.com/mssql/server",
    Tag = "2022-CU12-ubuntu-22.04"
};

await _dockerClient.Images.CreateImageAsync(imagesCreateParameters, null, new Progress<JSONMessage>());

string containerName = $"OrderDB";

int retryCount = 0;

var environmentVariables = new List<string>
{
    "ACCEPT_EULA=Y",
    "MSSQL_SA_PASSWORD=oR!FzqF?5Eo#56gk"
};

do
{
    try
    {
        await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
        {
            Image = "mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04",
            HostConfig = new HostConfig()
            {
                DNS = new[] { "8.8.8.8", "8.8.4.4" },
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        "1433/tcp",
                        new List<PortBinding>
                        {
                            new PortBinding
                            {
                                HostPort = "1433"
                            }
                        }
                    }
                }
            },
            Name = containerName,
            Env = environmentVariables,
            ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    {"1433/tcp", new EmptyStruct()},
                }
        });

        retryCount = 10;
    } catch (DockerApiException)
    {
        try
        {
            await _dockerClient.Containers.RemoveContainerAsync(containerName, new ContainerRemoveParameters() { Force = true });
        } catch
        {
            retryCount++;
        }
        
    }
} while (retryCount < 3);

if (retryCount != 10)
{

    Console.WriteLine("Retry count exceeded. Could not create a container, please prune container with name OrderDB");
    Environment.Exit(1);
}

await _dockerClient.Containers.StartContainerAsync(containerName, new ContainerStartParameters());

#endregion


// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<IOrderFacade, OrderFacade>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<OrderService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
