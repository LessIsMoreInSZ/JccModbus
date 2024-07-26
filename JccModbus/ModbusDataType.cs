using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public enum ModbusDataType
    {
        /// <summary>
        /// Discrete Inputs
        /// </summary>
        Discrete,

        Coils,

        InputRegisters,

        HoldRegisters

    }
}
