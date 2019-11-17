using System.IO;
using RSDK.Core.IO;

namespace RSDKvRS
{
    class SaveFiles
    {
        class SaveData
        {
            /// <summary>
            /// what level the player is upto
            /// </summary>
            byte CurrentLevel;
            /// <summary>
            /// how many chaos emeralds the player has
            /// </summary>
            byte EmeraldCount;
            /// <summary>
            /// How many lives the player has
            /// </summary>
            byte Lives;

            public SaveData(Stream stream) : this(new RsdkReader(stream))
            {
            }

            public SaveData(string file) : this(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
            }

            internal SaveData(RsdkReader reader)
            {
                CurrentLevel = reader.ReadByte();
                EmeraldCount = reader.ReadByte();
                Lives = reader.ReadByte();
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
                writer.Write(CurrentLevel);
                writer.Write(EmeraldCount);
                writer.Write(Lives);
            }
        }

        SaveData[] Saves = new SaveData[10];

        public SaveFiles(Stream stream) : this(new RsdkReader(stream))
        {
        }

        public SaveFiles(string file) : this(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
        }

        internal SaveFiles(RsdkReader reader)
        {
            for (int i = 0; i < 10; i++)
            {
                Saves[i] = new SaveData(reader);
            }
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
            for (int i = 0; i < 10; i++)
            {
                Saves[i].Write(writer);
            }
        }
    }
}
