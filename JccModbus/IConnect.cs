using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    internal interface IConnect
    {
        ModbusConnectStatus ConnectionStatus { get; }

        List<JccVariableDef> GetVariables();

        Action<JccVariableDef, JccVariableValue> OnVariableChange { get; set; }

        Action<ModbusConnectStatus> OnConnectStatusChange { get; set; }

        Action<List<JccVariableDefError>> OnVariableDefError { get; set; }

        void CheckVariableDef(JccVariableDef def);

        void Connect();

        void Disconnect();
    }
}
