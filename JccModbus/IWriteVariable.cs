using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    internal interface IWriteVariable
    {
        void WriteDiscrete(string address, bool data);

        void WriteDiscrete(string address, bool[] data);
    }
}
