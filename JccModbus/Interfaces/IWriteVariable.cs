using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus.Interfaces
{
    internal interface IWriteVariable
    {
        void WriteDiscrete(string address, bool data);

        void WriteDiscrete(string address, bool[] data);
    }
}
