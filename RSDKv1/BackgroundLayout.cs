﻿using System.Collections.Generic;
using RSDK.Core.IO;

namespace RSDKv1
{
    /* Background Layout */
    public class BGLayout
    {

        public class BGLayer
        {
            /// <summary>
            /// the array of Chunks IDs for the Layer
            /// </summary>
            public ushort[][] MapLayout { get; set; }

            /// <summary>
            /// Layer Width
            /// </summary>
            public byte width = 0;
            /// <summary>
            /// Layer Height
            /// </summary>
            public byte height = 0;
            /// <summary>
            /// the deform value
            /// set it to 1 for Horizontal parallax, 2 for Vertical parallax, 3 for "3D Sky", anything else for nothing
            /// (aka "behaviour")
            /// </summary>
            public byte Deform;
            /// <summary>
            /// how fast the Layer moves while the player is moving
            /// </summary>
            public byte RelativeSpeed;
            /// <summary>
            /// how fast the layer moves while the player isn't moving
            /// </summary>
            public byte ConstantSpeed;

            /// <summary>
            /// a list of Line positions
            /// </summary>
            public byte[] LineIndexes;

            public BGLayer()
            {
                width = height = 1;
                Deform = RelativeSpeed = ConstantSpeed = 0;
                LineIndexes = new byte[height * 128];
                MapLayout = new ushort[height][];
                for (int m = 0; m < height; m++)
                {
                    MapLayout[m] = new ushort[width];
                }
            }

            public BGLayer(byte w, byte h)
            {
                width = w;
                height = h;
                Deform = RelativeSpeed = ConstantSpeed = 0;
                LineIndexes = new byte[height * 128];
                MapLayout = new ushort[height][];
                for (int m = 0; m < height; m++)
                {
                    MapLayout[m] = new ushort[width];
                }
            }

            public BGLayer(RsdkReader reader)
            {
                width = reader.ReadByte();
                height = reader.ReadByte();
                Deform = reader.ReadByte();
                RelativeSpeed = reader.ReadByte();
                ConstantSpeed = reader.ReadByte();

                byte[] buf = new byte[3];
                bool finished = false;
                int cnt = 0;
                int loop = 0;

                LineIndexes = new byte[height * 128];

                while (!finished)
                {
                    buf[0] = reader.ReadByte();
                    if (buf[0] == 255)
                    {
                        buf[1] = reader.ReadByte();
                        if (buf[1] == 255)
                        {
                            finished = true;
                            break;
                        }
                        else
                        {
                            buf[2] = (byte)(reader.ReadByte() - 1);
                            loop = 0;

                            while (loop < buf[2] && !reader.IsEof)
                            {
                                LineIndexes[cnt++] = buf[1];
                                loop++;
                            }
                        }
                    }
                    else
                    {
                        LineIndexes[cnt++] = buf[0];
                    }
                }

                MapLayout = new ushort[height][];
                for (int m = 0; m < height; m++)
                {
                    MapLayout[m] = new ushort[width];
                }
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        MapLayout[y][x] = reader.ReadByte();
                    }
                }
            }

            public void Write(RsdkWriter writer)
            {
                writer.Write(width);
                writer.Write(height);
                writer.Write(Deform);
                writer.Write(RelativeSpeed);
                writer.Write(ConstantSpeed);

                // Output data
                int l = 0;
                int cnt = 0;

                for (int x = 0; x < LineIndexes.Length; x++)
                {
                    if (LineIndexes[x] != l && x > 0)
                    {
                        rle_write(writer, l, cnt);
                        cnt = 0;
                    }
                    l = LineIndexes[x];
                    cnt++;
                }

                rle_write(writer, l, cnt);

                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);

                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        writer.Write((byte)(MapLayout[h][w]));
                    }
                }

            }

            private static void rle_write(RsdkWriter file, int pixel, int count)
            {
                if (count <= 2)
                {
                    for (int y = 0; y < count; y++)
                        file.Write((byte)pixel);
                }
                else
                {
                    while (count > 0)
                    {
                        file.Write((byte)0xFF);

                        file.Write((byte)pixel);

                        file.Write((byte)((count > 253) ? 254 : (count + 1)));
                        count -= 253;
                    }
                }
            }

        }

        public class ScrollInfo
        {
            /// <summary>
            /// how fast the line moves while the player is moving
            /// </summary>
            public byte RelativeSpeed; //Known as "Speed" in Taxman's Editor
            /// <summary>
            /// How fast the line moves without the player moving
            /// </summary>
            public byte ConstantSpeed; //Known as "Scroll" in Taxman's Editor
            /// <summary>
            /// Behaviour Value, controls special line FX
            /// </summary>
            public byte Deform; //Known as "Deform" in Taxman's Editor, Same here!

            public ScrollInfo()
            {
                RelativeSpeed = 0;
                ConstantSpeed = 0;
                Deform = 0;
            }

            public ScrollInfo(byte r, byte c, byte d)
            {
                RelativeSpeed = r;
                ConstantSpeed = c;
                Deform = d;
            }

            public ScrollInfo(RsdkReader reader)
            {
                RelativeSpeed = reader.ReadByte();
                ConstantSpeed = reader.ReadByte();
                Deform = reader.ReadByte();
            }

            public void Write(RsdkWriter writer)
            {
                writer.Write(RelativeSpeed);
                writer.Write(ConstantSpeed);
                writer.Write(Deform);
            }
        }

        /// <summary>
        /// A list of Horizontal Line Scroll Values
        /// </summary>
        public List<ScrollInfo> HLines = new List<ScrollInfo>();
        /// <summary>
        /// A list of Vertical Line Scroll Values
        /// </summary>
        public List<ScrollInfo> VLines = new List<ScrollInfo>();
        /// <summary>
        /// A list of Background layers
        /// </summary>
        public List<BGLayer> Layers = new List<BGLayer>();

        public BGLayout()
        {

        }

        public BGLayout(string filename) : this(new RsdkReader(filename)) { }

        public BGLayout(System.IO.Stream stream) : this(new RsdkReader(stream)) { }

        public BGLayout(RsdkReader reader)
        {
            byte layerCount = reader.ReadByte();

            byte HLineCount = reader.ReadByte();

            for (int i = 0; i < HLineCount; i++)
            {
                ScrollInfo p = new ScrollInfo(reader);
                HLines.Add(p);
            }

            byte VLineCount = reader.ReadByte();

            for (int i = 0; i < VLineCount; i++)
            {
                ScrollInfo p = new ScrollInfo(reader);
                VLines.Add(p);
            }

            for (int i = 0; i < layerCount; i++) //Read BG Layers
            {
                Layers.Add(new BGLayer(reader));
            }

            //Console.WriteLine("Max Pos " + reader.BaseStream.Length + ", cur pos " + reader.Pos);
            reader.Close();

        }

        public void Write(string filename)
        {
            using (var writer = new RsdkWriter(filename))
                this.Write(writer);
        }

        public void Write(System.IO.Stream stream)
        {
            using (var writer = new RsdkWriter(stream))
                this.Write(writer);
        }

        internal void Write(RsdkWriter writer)
        {
            // Save Data
            writer.Write((byte)Layers.Count);

            writer.Write((byte)HLines.Count);

            for (int i = 0; i < HLines.Count; i++)
            {
                HLines[i].Write(writer);
            }

            writer.Write((byte)VLines.Count);

            for (int i = 0; i < VLines.Count; i++)
            {
                VLines[i].Write(writer);
            }

            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].Write(writer);
            }

            writer.Close();

        }
    }
}
