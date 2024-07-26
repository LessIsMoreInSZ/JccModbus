using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JccModbus
{
    public  class ModbusTcpJober:IConnect, IVariableChange
    {
        private ModbusTcpServer modbusTcpServer;

        private ModbusTcpServerConnectParam modbusTcpServerConnectParam;

        public ModbusTcpServerConnectParam ModbusTcpServerConnectParam
        {
            get
            {
                return this.modbusTcpServerConnectParam;
            }
            set
            {
                this.modbusTcpServerConnectParam = value;
            }
        }

        private ModbusConnectStatus modbusConnectStatus = ModbusConnectStatus.Disconnected;

        private readonly object plcLock = new object();

        private int cycleInterval = 50;

        public List<string> readPoolKeys = new List<string>();

        private readonly Dictionary<string, JccVariableValue> readPool = new Dictionary<string, JccVariableValue>();

        private readonly Dictionary<string, JccVariableDefError> errorDefs = new Dictionary<string, JccVariableDefError>();

        #region Realize IConnect IVariableChange
        public ModbusConnectStatus ConnectionStatus => throw new NotImplementedException();

        public Action<JccVariableDef, JccVariableValue> OnVariableChange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<ModbusConnectStatus> OnConnectStatusChange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<List<JccVariableDefError>> OnVariableDefError { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// copy the readPool
        /// </summary>
        /// <returns></returns>
        public List<JccVariableDef> GetVariables()
        {
            object obj = this.plcLock;
            List<JccVariableValue> list;
            lock (obj)
            {
                list = this.readPool.Values.ToList<JccVariableValue>();
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
            if(def== null) throw new ArgumentNullException("JccVariableDef");
            if (string.IsNullOrWhiteSpace(def.VarId))
            {
                throw new ArgumentNullException(def.VarId + " is invalid");
            }
        }

        public void Connect()
        {
            this.CheckAndReconnect();
        }

        private void CheckParameters()
        {
            if (this.ModbusTcpServerConnectParam.port==0)
            {
                throw new ArgumentException("EndPointUrl is needed.");
            }
        }

        private void CheckAndReconnect()
        {
            CheckParameters();
            modbusTcpServer = new ModbusTcpServer();
            modbusTcpServer.ServerStart(modbusTcpServerConnectParam.port, true);
            Task.Run(new Action(this.ReadVariable));

        }

        private void ReadVariable()
        {
            while(true)
            {
                if (this.modbusConnectStatus != ModbusConnectStatus.Connected)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    object obj = this.plcLock;
                    List<JccVariableDefError> errorList = new List<JccVariableDefError>();

                    //if (this.readPool.Count <= 0) { continue; }

                    lock (obj)
                    {
                        for(int i=0;i<readPoolKeys.Count;i++) 
                        {
                            IReadVariable readVariable = new ModbusTcpReader(this.modbusTcpServer);

                            switch (readPool[readPoolKeys[i]].Def.ModbusDataType)
                            {
                                case ModbusDataType.Coils:
                                    {
                                        bool isCoil = readVariable.ReadCoil(readPoolKeys[i]);
                                        readPool[readPoolKeys[i]].Value = isCoil;
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
            this.CheckVariableDef(varDef);
            object obj = this.plcLock;
            lock (obj)
            {
                if (this.readPool.ContainsKey(varDef.VarId))
                {
                    throw new Exception("duplicate register");
                }
                this.readPool.Add(varDef.VarId, new JccVariableValue(varDef, null));
                this.readPoolKeys.Add(varDef.VarId);
            }
        }

        public void UnRegister(JccVariableDef varDef)
        {
            this.CheckVariableDef(varDef);
            object obj = this.plcLock;
            lock (obj)
            {
                if (!this.readPool.ContainsKey(varDef.VarId))
                {
                    throw new Exception("duplicate register");
                }
                this.readPool.Remove(varDef.VarId);
                this.readPoolKeys.Remove(varDef.VarId);
            }
        }
        #endregion
    }
}
