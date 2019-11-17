﻿using RSDK.Core.IO;

namespace RSDKv5
{
    public class Palette
    {
        /// <summary>
        /// how many columns in the palette
        /// </summary>
        public const int MAX_PALETTE_COLUMNS = 0x10;
        public const int PALETTE_COLUMNS = 0x10;
        /// <summary>
        /// how many colours per column
        /// </summary>
        public const int COLORS_PER_COLUMN = 0x10;
        /// <summary>
        /// how many colours in total
        /// </summary>
        public const int PALETTE_COLORS = 0x100;

        /// <summary>
        /// our array of colours in the palette
        /// </summary>
        public PaletteColor[][] Colors = new PaletteColor[MAX_PALETTE_COLUMNS][];

        public Palette(int pc = 0)
        {
            int palColumns = pc;

            Colors = new PaletteColor[palColumns][];
            for (int i = 0; i < palColumns; i++)
            {
                Colors[i] = new PaletteColor[COLORS_PER_COLUMN];
                for (int j = 0; j < COLORS_PER_COLUMN; ++j)
                { Colors[i][j] = new PaletteColor(); }
            }
        }

        public Palette(RsdkReader reader)
        {
            ushort columns_bitmap = reader.ReadUInt16();
            for (int i = 0; i < PALETTE_COLUMNS; ++i)
            {
                if ((columns_bitmap & (1 << i)) != 0)
                {
                    Colors[i] = new PaletteColor[COLORS_PER_COLUMN];
                    for (int j = 0; j < COLORS_PER_COLUMN; ++j)
                        Colors[i][j] = new PaletteColor(reader);
                }
            }
        }

        public void Write(RsdkWriter writer)
        {
            ushort columns_bitmap = 0;
            for (int i = 0; i < PALETTE_COLUMNS; ++i)
                if (Colors[i] != null)
                    columns_bitmap |= (ushort)(1 << i);
            writer.Write(columns_bitmap);

            foreach (PaletteColor[] column in Colors)
                if (column != null)
                    foreach (PaletteColor color in column)
                        color.Write(writer);
        }
    }
}
