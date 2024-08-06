// See https://aka.ms/new-console-template for more information
using Starxcjy.DataAccess.Core;
using Starxcjy.DataAccess.Core.Extensions;
using Starxcjy.DataAccess.PlcModBusAccess.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;

public class ClientInfo:PackageDataModel<ServiceClientOption>
{
    public ClientInfo()
    {

    }
    [Description("发射次数")]
    public ushort Number { get; set; }

    public override ResultData<byte[]> ConvertToPackage(ServiceClientOption uploadPackageServiceOption)
    {
        List<byte> bytes = new List<byte>();
        bytes.AddRange(Number.FromUInt16BigEndian());
        return ResultData<byte[]>.Accept("编码成功", bytes.ToArray());
    }
}
