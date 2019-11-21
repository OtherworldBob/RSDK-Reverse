using RSDK.Core.Data.Interface;
using RSDK.Core.IO;

namespace RSDK.Core.Data
{
    internal class PaletteColor : IPaletteColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public PaletteColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public PaletteColor(RsdkReader reader)
        {
            Read(reader);
        }

        public void Read(RsdkReader reader)
        {
            R = reader.ReadByte();
            G = reader.ReadByte();
            B = reader.ReadByte();
        }

        public void Write(RsdkWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
        }
    }
}
