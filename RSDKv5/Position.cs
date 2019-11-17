﻿using System;
using System.Text;
using RSDK.Core.IO;

namespace RSDKv5
{
    [Serializable]
    public struct Position
    {
        [Serializable]
        public struct Value
        {
            public Value(short high = 0, ushort low = 0)
            {
                Low = low;
                High = high;
            }
            /// <summary>
            /// High value
            /// </summary>
            public short High;
            /// <summary>
            /// Low value
            /// </summary>
            public ushort Low;
        };

        /// <summary>
        /// Xpos values
        /// </summary>
        public Value X;
        /// <summary>
        /// Ypos values
        /// </summary>
        public Value Y;
        /// <summary>
        /// Zpos values
        /// </summary>
        public Value Z;

        public Position(short x = 0, short y = 0, short z = 0)
        {
            X = new Value(x);
            Y = new Value(y);
            Z = new Value(y);
        }

        internal Position(RsdkReader reader, bool vec3 = false) : this()
        {
            Read(reader, vec3);
        }

        internal void Read(RsdkReader reader, bool vec3 = false)
        {
            X.Low = reader.ReadUInt16();
            X.High = reader.ReadInt16();

            Y.Low = reader.ReadUInt16();
            Y.High = reader.ReadInt16();

            if (vec3)
            {
                Z.Low = reader.ReadUInt16();
                Z.High = reader.ReadInt16();
            }
        }

        internal void Write(RsdkWriter writer, bool vec3 = false)
        {
            writer.Write(X.Low);
            writer.Write(X.High);

            writer.Write(Y.Low);
            writer.Write(Y.High);

            if (vec3)
            {
                writer.Write(Z.Low);
                writer.Write(Z.High);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("X: ");
            sb.Append(X.High);
            if (0 != X.Low) sb.Append($"[{X.Low}]");
            sb.Append(", Y: ");
            sb.Append(Y.High);
            if (0 != Y.Low) sb.Append($"[{Y.Low}]");
            return sb.ToString();
        }
    }
}
