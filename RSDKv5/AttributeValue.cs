﻿using System;
using RSDK.Core.IO;

namespace RSDKv5
{
    [Serializable]
    public class AttributeValue
    {
        /// <summary>
        /// the uint8 value of the attribute
        /// </summary>
        byte value_uint8;
        /// <summary>
        /// the uint16 value of the attribute
        /// </summary>
        ushort value_uint16;
        /// <summary>
        /// the uint32 value of the attribute
        /// </summary>
        uint value_uint32;
        /// <summary>
        /// the int8 value of the attribute
        /// </summary>
        sbyte value_int8;
        /// <summary>
        /// the int16 value of the attribute
        /// </summary>
        short value_int16;
        /// <summary>
        /// the int32 value of the attribute
        /// </summary>
        int value_int32;
        /// <summary>
        /// the var value of the attribute
        /// </summary>
        int value_enum;
        /// <summary>
        /// the bool value of the attribute
        /// </summary>
        bool value_bool;
        /// <summary>
        /// the string value of the attribute
        /// </summary>
        string value_string = string.Empty; // default to empty string, null causes many problems
        /// <summary>
        /// the vector2 value of the attribute
        /// </summary>
        Position value_vector2;
        /// <summary>
        /// the vector3 value of the attribute
        /// </summary>
        Position value_vector3;
        /// <summary>
        /// the colour value of the attribute
        /// </summary>
        Color value_color;

        public AttributeTypes Type;

        Position no_position = new Position(0, 0);

        private void CheckType(AttributeTypes type)
        {
            if (type != Type)
            {
                //throw new Exception("Unexpected value type.");

                switch (type)
                {
                    case AttributeTypes.UINT8:
                        value_uint8 = 0;
                        break;
                    case AttributeTypes.UINT16:
                        value_uint16 = 0;
                        break;
                    case AttributeTypes.UINT32:
                        value_uint32 = 0;
                        break;
                    case AttributeTypes.INT8:
                        value_int8 = 0;
                        break;
                    case AttributeTypes.INT16:
                        value_int16 = 0;
                        break;
                    case AttributeTypes.INT32:
                        value_int32 = 0;
                        break;
                    case AttributeTypes.ENUM:
                        value_enum = 0;
                        break;
                    case AttributeTypes.BOOL:
                        value_bool = false;
                        break;
                    case AttributeTypes.COLOR:
                        value_color = Color.EMPTY;
                        break;
                    case AttributeTypes.VECTOR2:
                    case AttributeTypes.VECTOR3:
                        value_vector2 = no_position;
                        break;
                    case AttributeTypes.STRING:
                        value_string = string.Empty;
                        break;
                    default:
                        throw new Exception("Unexpected value type.");

                }
            }
        }
        public byte ValueUInt8
        {
            get { CheckType(AttributeTypes.UINT8); return value_uint8; }
            set { CheckType(AttributeTypes.UINT8); value_uint8 = value; }
        }
        public ushort ValueUInt16
        {
            get { CheckType(AttributeTypes.UINT16); return value_uint16; }
            set { CheckType(AttributeTypes.UINT16); value_uint16 = value; }
        }
        public uint ValueUInt32
        {
            get { CheckType(AttributeTypes.UINT32); return value_uint32; }
            set { CheckType(AttributeTypes.UINT32); value_uint32 = value; }
        }
        public sbyte ValueInt8
        {
            get { CheckType(AttributeTypes.INT8); return value_int8; }
            set { CheckType(AttributeTypes.INT8); value_int8 = value; }
        }
        public short ValueInt16
        {
            get { CheckType(AttributeTypes.INT16); return value_int16; }
            set { CheckType(AttributeTypes.INT16); value_int16 = value; }
        }
        public int ValueInt32
        {
            get { CheckType(AttributeTypes.INT32); return value_int32; }
            set { CheckType(AttributeTypes.INT32); value_int32 = value; }
        }
        public int ValueEnum
        {
            get { CheckType(AttributeTypes.ENUM); return value_enum; }
            set { CheckType(AttributeTypes.ENUM); value_enum = value; }
        }
        public bool ValueBool
        {
            get { CheckType(AttributeTypes.BOOL); return value_bool; }
            set { CheckType(AttributeTypes.BOOL); value_bool = value; }
        }
        public string ValueString
        {
            get { CheckType(AttributeTypes.STRING); return value_string; }
            set { CheckType(AttributeTypes.STRING); value_string = value; }
        }
        public Position ValueVector2
        {
            get { CheckType(AttributeTypes.VECTOR2); return value_vector2; }
            set { CheckType(AttributeTypes.VECTOR2); value_vector2 = value; }
        }
        public Position ValueVector3
        {
            get { CheckType(AttributeTypes.VECTOR3); return value_vector3; }
            set { CheckType(AttributeTypes.VECTOR3); value_vector3 = value; }
        }
        public Color ValueColor
        {
            get { CheckType(AttributeTypes.COLOR); return value_color; }
            set { CheckType(AttributeTypes.COLOR); value_color = value; }
        }

        public AttributeValue()
        {
            Type = 0;
        }

        public AttributeValue(AttributeTypes type)
        {
            Type = type;
        }

        public AttributeValue Clone()
        {
            AttributeValue n = new AttributeValue(Type);

            n.value_uint8 = value_uint8;
            n.value_uint16 = value_uint16;
            n.value_uint32 = value_uint32;
            n.value_int8 = value_int8;
            n.value_int16 = value_int16;
            n.value_int32 = value_int32;
            n.value_enum = value_enum;
            n.value_bool = value_bool;
            n.value_string = value_string;
            n.value_vector2 = value_vector2;
            n.value_color = value_color;

            return n;
        }

        internal AttributeValue(RsdkReader reader, AttributeTypes type)
        {
            Type = type;
            Read(reader);
        }

        internal void Read(RsdkReader reader)
        {
            switch (Type)
            {
                case AttributeTypes.UINT8:
                    value_uint8 = reader.ReadByte();
                    break;
                case AttributeTypes.UINT16:
                    value_uint16 = reader.ReadUInt16();
                    break;
                case AttributeTypes.UINT32:
                    value_uint32 = reader.ReadUInt32();
                    break;
                case AttributeTypes.INT8:
                    value_int8 = reader.ReadSByte();
                    break;
                case AttributeTypes.INT16:
                    value_int16 = reader.ReadInt16();
                    break;
                case AttributeTypes.INT32:
                    value_int32 = reader.ReadInt32();
                    break;
                case AttributeTypes.ENUM:
                    value_enum = reader.ReadInt32();
                    break;
                case AttributeTypes.BOOL:
                    value_bool = reader.ReadUInt32() != 0;
                    break;
                case AttributeTypes.STRING:
                    value_string = reader.ReadRSDKUnicodeString();
                    break;
                case AttributeTypes.VECTOR2:
                    value_vector2 = new Position(reader);
                    break;
                case AttributeTypes.VECTOR3:
                    value_vector2 = new Position(reader);
                    break;
                case AttributeTypes.COLOR:
                    value_color = new Color(reader);
                    break;
            }
        }

        internal void Write(RsdkWriter writer)
        {
            switch (Type)
            {
                case AttributeTypes.UINT8:
                    writer.Write(value_uint8);
                    break;
                case AttributeTypes.UINT16:
                    writer.Write(value_uint16);
                    break;
                case AttributeTypes.UINT32:
                    writer.Write(value_uint32);
                    break;
                case AttributeTypes.INT8:
                    writer.Write(value_int8);
                    break;
                case AttributeTypes.INT16:
                    writer.Write(value_int16);
                    break;
                case AttributeTypes.INT32:
                    writer.Write(value_int32);
                    break;
                case AttributeTypes.ENUM:
                    writer.Write(value_enum);
                    break;
                case AttributeTypes.BOOL:
                    writer.Write((uint)(value_bool ? 1 : 0));
                    break;
                case AttributeTypes.STRING:
                    writer.WriteRSDKUnicodeString(value_string);
                    break;
                case AttributeTypes.VECTOR2:
                    value_vector2.Write(writer);
                    break;
                case AttributeTypes.VECTOR3:
                    value_vector2.Write(writer,true);
                    break;
                case AttributeTypes.COLOR:
                    value_color.Write(writer);
                    break;
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case AttributeTypes.UINT8:
                    return value_uint8.ToString();
                case AttributeTypes.UINT16:
                    return value_uint16.ToString();
                case AttributeTypes.UINT32:
                    return value_uint32.ToString();
                case AttributeTypes.INT8:
                    return value_int8.ToString();
                case AttributeTypes.INT16:
                    return value_int16.ToString();
                case AttributeTypes.INT32:
                    return value_int32.ToString();
                case AttributeTypes.ENUM:
                    return value_enum.ToString();
                case AttributeTypes.BOOL:
                    return value_bool.ToString();
                case AttributeTypes.STRING:
                    return value_string.ToString();
                case AttributeTypes.VECTOR2:
                    return value_vector2.ToString();
                case AttributeTypes.VECTOR3:
                    return value_vector3.ToString();
                case AttributeTypes.COLOR:
                    return value_color.ToString();
                default:
                    return "Unhandled Type for ToString()";
            }
        }
    }
}
