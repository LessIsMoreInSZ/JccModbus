using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public enum ModbusConnectStatus
    {
        Connected,
 
        Closing,

        Disconnected,

        Reconnecting,

        ConnectAbnormal
    }
}
