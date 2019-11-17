using System.IO;
using RSDK.Core.IO;

namespace RSDKv5
{
    public class StageConfig : CommonConfig
    {
        /// <summary>
        /// the path to this file
        /// </summary>
        public string FilePath;

        /// <summary>
        /// whether or not we use the global objects in this stage
        /// </summary>
        public bool LoadGlobalObjects;

        public StageConfig(string filename) : this(new RsdkReader(filename))
        {
            FilePath = filename;
        }

        public StageConfig()
        {
            for (int i = 0; i < Palettes.Length; i++)
            {
                Palettes[i] = new Palette();
            }
        }

        public StageConfig(Stream stream) : this(new RsdkReader(stream))
        {

        }

        public StageConfig(RsdkReader reader)
        {
            base.ReadMagic(reader);

            LoadGlobalObjects = reader.ReadBoolean();

            base.ReadCommonConfig(reader);
        }

        public void Write(string filename)
        {
            using (var writer = new RsdkWriter(filename))
                this.Write(writer);
        }

        public void Write(Stream stream)
        {
            using (var writer = new RsdkWriter(stream))
                this.Write(writer);
        }

        public void Write(RsdkWriter writer)
        {
            base.WriteMagic(writer);

            writer.Write(LoadGlobalObjects);
            base.WriteCommonConfig(writer);

        }
    }
}
