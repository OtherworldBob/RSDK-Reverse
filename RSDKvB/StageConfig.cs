using System.Collections.Generic;
using RSDK.Core.IO;

namespace RSDKvB
{
    public class StageConfig
    {
        /// <summary>
        /// the stageconfig palette (index 96-128)
        /// </summary>
        public Palette StagePalette = new Palette();
        /// <summary>
        /// the list of Stage SoundFX paths
        /// </summary>
        public List<string> SoundFX = new List<string>();
        /// <summary>
        /// a list of names for each SFX file
        /// </summary>
        public List<string> SfxNames = new List<string>();
        /// <summary>
        /// a list of names for each script
        /// </summary>
        public List<string> ObjectsNames = new List<string>();
        /// <summary>
        /// A list of the script filepaths for the stage-specific objects
        /// </summary>
        public List<string> ScriptPaths = new List<string>();
        /// <summary>
        /// whether or not to load the global objects in this stage
        /// </summary>
        public bool LoadGlobalScripts = false;

        public StageConfig()
        {

        }

        public StageConfig(string filename) : this(new RsdkReader(filename))
        {

        }

        public StageConfig(System.IO.Stream stream) : this(new RsdkReader(stream))
        {

        }

        public StageConfig(RsdkReader reader)
        {
            LoadGlobalScripts = reader.ReadBoolean();

            StagePalette.Read(reader, 2);

            this.ReadWAVConfiguration(reader);

            this.ReadObjectsNames(reader);

            reader.Close();
        }

        internal void ReadObjectsNames(RsdkReader reader)
        {
            byte objects_count = reader.ReadByte();

            for (int i = 0; i < objects_count; ++i)
            { ObjectsNames.Add(reader.ReadRSDKString()); }

            for (int i = 0; i < objects_count; ++i)
            { ScriptPaths.Add(reader.ReadRSDKString());}
        }

        internal void WriteObjectsNames(RsdkWriter writer)
        {
            writer.Write((byte)ObjectsNames.Count);

            foreach (string name in ObjectsNames)
                writer.WriteRSDKString(name);

            foreach (string srcname in ScriptPaths)
                writer.WriteRSDKString(srcname);
        }

        internal void ReadWAVConfiguration(RsdkReader reader)
        {
            byte SoundFX_count = reader.ReadByte();

            for (int i = 0; i < SoundFX_count; ++i)
            { SfxNames.Add(reader.ReadRSDKString()); }

            for (int i = 0; i < SoundFX_count; ++i)
            { SoundFX.Add(reader.ReadString()); }
        }

        internal void WriteWAVConfiguration(RsdkWriter writer)
        {
            writer.Write((byte)SoundFX.Count);

            foreach (string wavname in SfxNames)
                writer.WriteRSDKString(wavname);

            foreach (string wav in SoundFX)
                writer.Write(wav);
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

        public void Write(RsdkWriter writer)
        {
            writer.Write(LoadGlobalScripts);

            StagePalette.Write(writer);

            WriteWAVConfiguration(writer);

            WriteObjectsNames(writer);

            writer.Close();

        }

    }
}
