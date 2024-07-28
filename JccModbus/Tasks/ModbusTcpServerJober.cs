using HslCommunication.ModBus;
using JccModbus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JccModbus.Tasks
{
    public class ModbusTcpJober : IConnect, IVariableChange
    {
        private ModbusTcpServer modbusTcpServer;

        private ModbusTcpServerConnectParam modbusTcpServerConnectParam = new ModbusTcpServerConnectParam();

        public ModbusTcpServerConnectParam ModbusTcpServerConnectParam
        {
            get
            {
                return modbusTcpServerConnectParam;
            }
            set
            {
                modbusTcpServerConnectParam = value;
            }
        }

        private ModbusConnectStatus modbusConnectStatus = ModbusConnectStatus.Disconnected;

        private readonly object plcLock = new object();

        private int cycleInterval = 50;

        public List<string> readPoolKeys = new List<string>();

        private readonly Dictionary<string, JccVariableValue> readPool = new Dictionary<string, JccVariableValue>();

        private readonly Dictionary<string, JccVariableDefError> errorDefs = new Dictionary<string, JccVariableDefError>();

        #region Realize IConnect IVariableChange
        //public ModbusConnectStatus ConnectionStatus;

        public Action<JccVariableDef, JccVariableValue> OnVariableChange { get; set; }
        public Action<ModbusConnectStatus> OnConnectStatusChange { get; set; }
        public Action<List<JccVariableDefError>> OnVariableDefError { get; set; }

        ModbusConnectStatus IConnect.ConnectionStatus => throw new NotImplementedException();

        /// <summary>
        /// copy the readPool
        /// </summary>
        /// <returns></returns>
        public List<JccVariableDef> GetVariables()
        {
            object obj = plcLock;
            List<JccVariableValue> list;
            lock (obj)
            {
                list = readPool.Values.ToList();
            }
            List<JccVariableDef> list2 = new List<JccVariableDef>();
            foreach (JccVariableValue ltVariableValue in list)
            {
                list2.Add(ltVariableValue.Def);
            }
            return list2;
        }

        public void CheckVariableDef(JccVariableDef def)
        {
            if (def == null) throw new ArgumentNullException("JccVariableDef");
            if (string.IsNullOrWhiteSpace(def.VarId))
            {
                throw new ArgumentNullException(def.VarId + " is invalid");
            }
        }

        public void Connect()
        {
            CheckAndReconnect();

            // 20240728 jyj 假设默认连接成功
            modbusConnectStatus = ModbusConnectStatus.Connected;
        }

        private void CheckParameters()
        {
            if (ModbusTcpServerConnectParam.port == 0)
            {
                throw new ArgumentException("EndPointUrl is needed.");
            }
        }

        private void CheckAndReconnect()
        {
            CheckParameters();
            modbusTcpServer = new ModbusTcpServer();
            modbusTcpServer.ServerStart(modbusTcpServerConnectParam.port, true);
            Task.Run(new Action(ReadVariable));

        }

        private async void ReadVariable()
        {
            while (true)
            {
                if (modbusConnectStatus != ModbusConnectStatus.Connected)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    object obj = plcLock;
                    List<JccVariableDefError> errorList = new List<JccVariableDefError>();

                    //if (this.readPool.Count <= 0) { continue; }

                    lock (obj)
                    {
                        for (int i = 0; i < readPoolKeys.Count; ++i)
                        {
                            IReadVariable readVariable = new ModbusTcpReader(modbusTcpServer);

                            switch (readPool[readPoolKeys[i]].Def.ModbusDataType)
                            {
                                case ModbusDataType.Coils:
                                    {
                                        bool newValue = readVariable.ReadCoil(readPoolKeys[i]);
                                        if(readPool[readPoolKeys[i]].Value!=null&&bool.Parse(readPool[readPoolKeys[i]].Value.ToString())!= newValue)
                                        {
                                            Action<JccVariableDef, JccVariableValue> onVariableChange = this.OnVariableChange;
                                            if (onVariableChange == null)
                                            {
                                                return;
                                            }
                                            readPool[readPoolKeys[i]].Value = newValue;
                                            onVariableChange(readPool[readPoolKeys[i]].Def, readPool[readPoolKeys[i]]);

                                            //Task.Run(delegate ()
                                            //{
                                               
                                            //});
                                        }
                                        readPool[readPoolKeys[i]].Value = newValue;
                                        break;
                                    }
                                case ModbusDataType.Discrete:
                                    {
                                        bool isCoil = readVariable.ReadDiscrete(readPoolKeys[i]);
                                        readPool[readPoolKeys[i]].Value = isCoil;
                                        break;
                                    }
                                case ModbusDataType.InputRegisters:
                                    break;
                                case ModbusDataType.HoldRegisters:
                                    break;
                            }


                        }
                    }

                    await Task.Delay(1000);
                }
            }
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public JccVariableValue Read(JccVariableDef varDef)
        {
            throw new NotImplementedException();
        }

        public List<JccVariableValue> ReadMany(List<JccVariableDef> varDefs)
        {
            throw new NotImplementedException();
        }

        public void WriteMany(Dictionary<JccVariableDef, JccVariableValue> values)
        {
            throw new NotImplementedException();
        }

        public void Write(JccVariableDef varDef, JccVariableValue value)
        {
            throw new NotImplementedException();
        }

        public JccVariableValue Get(JccVariableDef varDef)
        {
            throw new NotImplementedException();
        }

        public void Register(JccVariableDef varDef)
        {
            CheckVariableDef(varDef);
            object obj = plcLock;
            lock (obj)
            {
                if (readPool.ContainsKey(varDef.VarId))
                {
                    throw new Exception("duplicate register");
                }
                readPool.Add(varDef.VarId, new JccVariableValue(varDef, null));
                readPoolKeys.Add(varDef.VarId);
            }
        }

        public void UnRegister(JccVariableDef varDef)
        {
            CheckVariableDef(varDef);
            object obj = plcLock;
            lock (obj)
            {
                if (!readPool.ContainsKey(varDef.VarId))
                {
                    throw new Exception("duplicate register");
                }
                readPool.Remove(varDef.VarId);
                readPoolKeys.Remove(varDef.VarId);
            }
        }
        #endregion
    }
}
