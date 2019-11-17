using RSDK.Core.IO;

namespace RSDKv5
{
    public class PaletteColor
    {

        /// <summary>
        /// Colour Red Value
        /// </summary>
        public byte R;
        /// <summary>
        /// Colour Green Value
        /// </summary>
        public byte G;
        /// <summary>
        /// Colour Blue Value
        /// </summary>
        public byte B;

        public PaletteColor(byte R = 0, byte G = 0, byte B = 0)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        internal PaletteColor(RsdkReader reader)
        {
            this.Read(reader);
        }

        internal void Read(RsdkReader reader)
        {
            R = reader.ReadByte();
            G = reader.ReadByte();
            B = reader.ReadByte();
        }

        internal void Write(RsdkWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
        }
    }
}
