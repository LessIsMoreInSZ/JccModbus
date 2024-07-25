using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    internal interface IReadVariable
    {
        bool ReadDiscrete(string address);

        bool[] ReadDiscrete(string address, ushort length);

        bool ReadCoil(string address);

        bool[] ReadCoil(string address, ushort length);
    }
}
