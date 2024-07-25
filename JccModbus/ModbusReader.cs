using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    internal class ModbusTcpReader : IReadVariable
    {
        private readonly ModbusTcpServer modbus;
        public ModbusTcpReader(ModbusTcpServer server)
        {
            modbus = server;
        }

        bool IReadVariable.ReadCoil(string address)
        {
            return modbus.ReadCoil(address);
        }

        bool[] IReadVariable.ReadCoil(string address, ushort length)
        {
            return modbus.ReadCoil(address, length);
        }

        bool IReadVariable.ReadDiscrete(string address)
        {
            return modbus.ReadDiscrete(address);
        }

        bool[] IReadVariable.ReadDiscrete(string address, ushort length)
        {
            return modbus.ReadDiscrete(address, length);
        }


    }
}
