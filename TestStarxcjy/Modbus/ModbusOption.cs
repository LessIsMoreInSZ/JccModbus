using Starxcjy.DataAccess.PlcModBusAccess.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarxcjy.Modbus
{
    public class ModbusOption : IChannelOption
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public bool HasHeaderFooter { get; set; }
        public string ModBusDataAddress { get; set; }
    }
}
