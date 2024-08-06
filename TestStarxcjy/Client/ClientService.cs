// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using Starxcjy.DataAccess.Communications.Core.IMessage;
using Starxcjy.DataAccess.PlcModBusAccess.Core.Services;
public class ClientService:ClientAutoChannelService<ServiceClientOption, ClientInfo, IClientService>, IClientService
{
    /// <summary>
    /// 发送数据客户端服务
    /// </summary>
    /// <param name="logger"></param>
    public ClientService(ILogger<IClientService> logger) : base(logger)
    {

    }
}
