using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public interface IVariableChange
    {
        void CheckVariableDef(JccVariableDef def);

        void Connect();

        void Disconnect();

        JccVariableValue Read(JccVariableDef varDef);

        List<JccVariableValue> ReadMany(List<JccVariableDef> varDefs);

        void WriteMany(Dictionary<JccVariableDef, JccVariableValue> values);

        void Write(JccVariableDef varDef, JccVariableValue value);

        JccVariableValue Get(JccVariableDef varDef);

        void Register(JccVariableDef varDef);

        void UnRegister(JccVariableDef varDef);
    }
}
