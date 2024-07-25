using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public class ModbusTcpServerConnectParam:ModbusConnectParam
    {
        public ModbusTcpServerConnectParam(int port)
        {
            this.port = port;
        }
    }
}
