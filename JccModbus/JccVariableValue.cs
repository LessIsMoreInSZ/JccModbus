using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public class JccVariableValue : ICloneable
    {
        public JccVariableDef Def { get; set; }

        public object Value { get; set; }

        public JccVariableValue() { }

        public JccVariableValue(JccVariableDef def, object value)
        {
            this.Def = def;
            this.Value = value;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}{2}{3}", new object[]
            {
                this.Def.VarId,
                this.Def.IsBit,
                this.Def.BitIndex,
                this.Value
            }).GetHashCode();
        }

        public object Clone()
        {
            return new JccVariableValue
            {
                Def = this.Def,
                Value = this.Value
            };
        }
    }
}
