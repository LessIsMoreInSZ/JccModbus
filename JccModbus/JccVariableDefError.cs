using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public class JccVariableDefError
    {
        public JccVariableDef Def { get; set; }

        public Exception Error { get; set; }
    }
}
