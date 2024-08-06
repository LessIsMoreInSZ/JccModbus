using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Starxcjy.DataAccess.Core;
using Starxcjy.DataAccess.Core.Extensions;
using Starxcjy.DataAccess.PlcModBusAccess.Core.Models;
using TestStarxcjy.Modbus;


ServiceCollection serviceDescriptors = new ServiceCollection();
serviceDescriptors.AddTransient<IClientService, ClientService>();
serviceDescriptors.AddTransient<IServerService, ServerService>();
serviceDescriptors.AddTransient<TcpChannelClient>();
//serviceDescriptors.AddTransient<ModBusClientChannel<ModbusOption>>();
//serviceDescriptors.AddTransient<ModBusServerChannel<ModbusMessage, ModbusOption, ModbusService>>();
serviceDescriptors.AddLogging(d => d.AddConsole());
await ServerMode(serviceDescriptors);
await ClientMode(serviceDescriptors);
//await ModbusServerMode(serviceDescriptors);
//await ModbusClientMode(serviceDescriptors);
async static Task ServerMode(ServiceCollection serviceDescriptors)
{
    var p = serviceDescriptors.BuildServiceProvider();
    var d = p.GetRequiredService<IServerService>();
    var logger = p.GetRequiredService<ILogger<IClientService>>();
    await d.OpenAsync(new ServerServiceOption
    {
        ByChannel = Starxcjy.DataAccess.PlcModBusAccess.Core.Models.ByChannel.Tcp,
        Port = 1200,
        SerialPortDelayTimeSpan = 100,
        IsEnable = true,
    });
    d.OnMessage += D_OnMessage;
    d.ClientJoin += D_ClientJoin;
    d.ClientExit += D_ClientExit;
}

static void D_ClientExit(string obj)
{
    Console.WriteLine(obj);
}

static void D_ClientJoin(string obj)
{
    Console.WriteLine(obj);
}

static Task<Starxcjy.DataAccess.Core.Result> D_OnMessage(Starxcjy.DataAccess.PlcModBusAccess.Core.Models.ServerChannelCore<ServerServiceOption, ServerMessage, IServerService> arg1, ServerMessage[]? arg2, string? arg3)
{
    foreach (var item in arg2)
    {
        Console.WriteLine($"服务端收到数据:{item.Number}");
    }
    return Result.Accept("Roger That").AsTaskResult();
}

async static Task ClientMode(ServiceCollection serviceDescriptors)
{

    var p = serviceDescriptors.BuildServiceProvider();
    var d = p.GetRequiredService<IClientService>();
    var logger = p.GetRequiredService<ILogger<IClientService>>();
    var openresult = await d.OpenAsync(new ServiceClientOption
    {
        ByChannel = Starxcjy.DataAccess.PlcModBusAccess.Core.Models.ByChannel.Tcp,
        IpAddress = "127.0.0.1",
        ModBusDataAddress = "0",
        Port = 1200,
        AutoRetryCount = 3,
        HasHeaderFooter = false,
        ServiceIndex = 1,
        IsEnable = true
    });
    if (openresult.IsError)
    {
        logger.LogError(openresult.Message);
        return;
    }
    logger.LogInformation(openresult.Message);
    for (int i = 0; i < 100; i++)
    {
        var result = await d.SendPackageAsync(new ClientInfo
        {
            Number = (ushort)i,
        });
        if (result.IsError)
        {
            logger.LogError(result.Message);
        }
        await Task.Delay(200);
        logger.LogInformation(result.Message);
    }


    Console.ReadLine();
}

async static Task ModbusServerMode(ServiceCollection serviceDescriptors)
{
    var p = serviceDescriptors.BuildServiceProvider();
    var d = p.GetRequiredService<IServerService>();
    var logger = p.GetRequiredService<ILogger<IClientService>>();
    await d.OpenAsync(new ServerServiceOption
    {
        ByChannel = Starxcjy.DataAccess.PlcModBusAccess.Core.Models.ByChannel.ModBus,
        Port = 502,
        SerialPortDelayTimeSpan = 100,
        IsEnable = true,
    });
    d.OnMessage += D_OnMessage;
    d.ClientJoin += D_ClientJoin;
    d.ClientExit += D_ClientExit;
}

async static Task ModbusClientMode(ServiceCollection serviceDescriptors)
{

    var p = serviceDescriptors.BuildServiceProvider();
    var d = p.GetRequiredService<IClientService>();
    var logger = p.GetRequiredService<ILogger<IClientService>>();
    var openresult = await d.OpenAsync(new ServiceClientOption
    {
        ByChannel = Starxcjy.DataAccess.PlcModBusAccess.Core.Models.ByChannel.ModBus,
        IpAddress = "127.0.0.1",
        ModBusDataAddress = "0",
        Port = 502,
        AutoRetryCount = 3,
        HasHeaderFooter = false,
        ServiceIndex = 1,
        IsEnable = true
    });
    if (openresult.IsError)
    {
        logger.LogError(openresult.Message);
        return;
    }
    logger.LogInformation(openresult.Message);
    for (int i = 0; i < 100; i++)
    {
        var result = await d.SendPackageAsync(new ClientInfo
        {
            Number = (ushort)i,
        });
        if (result.IsError)
        {
            logger.LogError(result.Message);
        }
        await Task.Delay(200);
        logger.LogInformation(result.Message);
    }


    Console.ReadLine();
}
