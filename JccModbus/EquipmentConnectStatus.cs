using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public enum EquipmentConnectStatus
    {
        Connected,
        Closing,
        Disconnected,
        Reconnecting,
        ConnectAbnormal
    }
}
