using JccModbus;
using JccModbus.Tasks;

namespace TestJccModbus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestTcpModbus();
            Console.ReadLine();
        }

        static void TestTcpModbus()
        {
            

            ModbusTcpJober modbusTcpJober = new ModbusTcpJober();
            modbusTcpJober.ModbusTcpServerConnectParam.port = 502;
            modbusTcpJober.Connect();
            //modbusTcpJober.Register(def);

            JccVariableDef def = new JccVariableDef();
            def.ModbusDataType = ModbusDataType.Coils;
            def.DataType = JccDataType.Bool;
            def.VarId = "0";


            JccEventDispathcher jccEventDispathcher = new JccEventDispathcher(modbusTcpJober);
            jccEventDispathcher.RegisterVar(def, OnDataChange);

        }

        private static void OnDataChange(JccVariableDef def, JccVariableValue value)
        {
            try
            {
                //Console.WriteLine(value.Value);
                Console.WriteLine($"address:{def.VarId}+value:{value.Value}");
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
