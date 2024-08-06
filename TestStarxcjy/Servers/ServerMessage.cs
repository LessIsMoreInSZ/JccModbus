// See https://aka.ms/new-console-template for more information
using Starxcjy.DataAccess.Core;
using Starxcjy.DataAccess.Core.Extensions;
using Starxcjy.DataAccess.PlcModBusAccess.Core.Models;
using System.Buffers;
using System.ComponentModel;

public class ServerMessage : DetailInfoCore<ServerMessage>
{
    public ServerMessage()
    {

    }
    [Description("接收次数")]
    public ushort Number { get; set; }
    public override ResultData<ServerMessage> ReadFrame(ref SequenceReader<byte> reader, PlcPackageInfo<ServerMessage> frame)
    {
        if (reader.TryReadUInt16BigEndian(out var n))
        {
            Number = n;
            return ResultData<ServerMessage>.Accept("完毕",this);
        }
        else
        {
            return UnknownPackage;
        }
    }
}
