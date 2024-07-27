using JccModbus;
using JccModbus.Tasks;

namespace TestJccModbus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        static void TestTcpModbus()
        {
            JccVariableDef def = new JccVariableDef();
            def.ModbusDataType = ModbusDataType.Coils;
            def.DataType = JccDataType.Bool;
            def.VarId = "0";

            ModbusTcpJober modbusTcpJober = new ModbusTcpJober();
            modbusTcpJober.Register(def);

        }
    }
}
