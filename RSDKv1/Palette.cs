using RSDK.Core.IO;

namespace RSDKv1
{
    public class Palette
    {
        /// <summary>
        /// how many colours for each row (always 16)
        /// </summary>
        public const int COLORS_PER_COLUMN = 0x10;

        /// <summary>
        /// an array of the colours
        /// </summary>
        public PaletteColour[][] Colors;

        public Palette(int pc = 2)
        {
            int palColumns = pc;

            Colors = new PaletteColour[palColumns][];
            for (int i = 0; i < palColumns; i++)
            {
                Colors[i] = new PaletteColour[COLORS_PER_COLUMN];
                for (int j = 0; j < COLORS_PER_COLUMN; ++j)
                { Colors[i][j] = new PaletteColour(); }
            }
        }

        public Palette(RsdkReader r)
        {
            Read(r);
        }

        public Palette(RsdkReader r, int palcols)
        {
            Read(r, palcols);
        }

        public void Read(RsdkReader reader, int Columns)
        {
            int palColumns = Columns;

            Colors = new PaletteColour[palColumns][];
            for (int i = 0; i < palColumns; i++)
            {
                Colors[i] = new PaletteColour[COLORS_PER_COLUMN];
                for (int j = 0; j < COLORS_PER_COLUMN; ++j)
                { Colors[i][j] = new PaletteColour(reader); }
            }
        }

        public void Read(RsdkReader reader)
        {
            int palColumns = ((int)reader.BaseStream.Length / 8) / 6;

            Colors = new PaletteColour[palColumns][];
            for (int i = 0; i < palColumns; i++)
            {
                Colors[i] = new PaletteColour[COLORS_PER_COLUMN];
                for (int j = 0; j < COLORS_PER_COLUMN; ++j)
                { Colors[i][j] = new PaletteColour(reader);}
            }
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
            int palColumns = Colors.Length/16;
            int c = 0;

            foreach (PaletteColour[] column in Colors)
                if (column != null && c < 32)
                    foreach (PaletteColour color in column)
                    { color.Write(writer); c++; }
        }

    }
}
