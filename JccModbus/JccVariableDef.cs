using System;
using System.Collections.Generic;
using System.Text;

namespace JccModbus
{
    public class JccVariableDef
    {
        private int strDataLength;
        /// <summary>
        /// Unique key for variable registration
        /// </summary>
        public string VarId {  get; set; }

        public JccDataType DataType { get; set; }

        public ModbusDataType ModbusDataType { get; set; }

        public ModbusProtocol Protocol { get; set; }

        public bool IsBit { get; set; }

        public int BitIndex { get; set; }

        public override int GetHashCode()
        {
            return $"{this.VarId}".GetHashCode();
        }

        public bool IsSameDef(JccVariableDef other)
        {
            return this.VarId.Equals(other.VarId);
        }

        public Type Value2DotNetType
        {
            get
            {
                switch (this.DataType)
                {
                    case JccDataType.Byte:
                        return typeof(byte);
                    case JccDataType.Int16:
                        return typeof(short);
                    case JccDataType.Int32:
                        return typeof(int);
                    case JccDataType.Int64:
                        return typeof(long);
                    case JccDataType.UInt16:
                        return typeof(ushort);
                    case JccDataType.UInt32:
                        return typeof(uint);
                    case JccDataType.UInt64:
                        return typeof(ulong);
                    case JccDataType.Float32:
                        return typeof(float);
                    case JccDataType.Float64:
                        return typeof(double);
                    case JccDataType.String:
                        return typeof(string);
                    case JccDataType.WString:
                        return typeof(byte[]);
                    case JccDataType.Bool:
                        return typeof(bool);
                    case JccDataType.SByte:
                        return typeof(sbyte);
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public int DataLength
        {
            get
            {
                if (this.IsBit)
                {
                    return 1;
                }
                if (this.DataType != JccDataType.String && this.DataType != JccDataType.WString)
                {
                    switch (this.DataType)
                    {
                        case JccDataType.Byte:
                            return 1;
                        case JccDataType.Int16:
                            return 2;
                        case JccDataType.Int32:
                            return 4;
                        case JccDataType.Int64:
                            return 8;
                        case JccDataType.UInt16:
                            if (this.IsBit)
                            {
                                return 1;
                            }
                            return 2;
                        case JccDataType.UInt32:
                            if (this.IsBit)
                            {
                                return 1;
                            }
                            return 4;
                        case JccDataType.UInt64:
                            return 8;
                        case JccDataType.Float32:
                            return 4;
                        case JccDataType.Float64:
                            return 8;
                        case JccDataType.Bool:
                            return 1;
                        case JccDataType.SByte:
                            return 1;
                    }
                    throw new ArgumentOutOfRangeException();
                }
                return this.strDataLength;
            }
            set
            {
                this.strDataLength = value;
            }
        }
    }
}
