// See https://aka.ms/new-console-template for more information
using Starxcjy.DataAccess.PlcModBusAccess.Core.Services;

public class ServerService:ServerAutoChannelService<ServerMessage,ServerServiceOption, IServerService>, IServerService
{
    /// <summary>
    /// 服务通道
    /// </summary>
    /// <param name="serviceProvider1"></param>
    public ServerService(IServiceProvider serviceProvider1) : base(serviceProvider1)
    {

    }
}
