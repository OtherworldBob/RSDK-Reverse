using System.Collections.Generic;
using RSDK.Core.IO;

namespace RSDKvRS
{
    /* StageConfig Equivelent */
    public class Zoneconfig
    {
        public class ObjectData
        {
            /// <summary>
            /// the filepath to the script
            /// </summary>
            public string FilePath;
            /// <summary>
            /// the spritesheet ID for the object
            /// </summary>
            public byte SpriteSheetID;
        }

        /// <summary>
        /// the stageconfig palette (index 96-128)
        /// </summary>
        public Palette StagePalette = new Palette();
        /// <summary>
        /// a list of sheets to add to the global list
        /// </summary>
        public List<string> ObjectSpritesheets = new List<string>();
        /// <summary>
        /// the list of stage objects
        /// </summary>
        public List<ObjectData> Objects = new List<ObjectData>();
        /// <summary>
        /// the list of the stage music tracks
        /// </summary>
        public List<string> Music = new List<string>();
        /// <summary>
        /// the list of Stage SoundFX
        /// </summary>
        public List<string> SoundFX = new List<string>();

        public Zoneconfig()
        {

        }

        public Zoneconfig(string filename) : this(new RsdkReader(filename))
        {

        }

        public Zoneconfig(System.IO.Stream stream) : this(new RsdkReader(stream))
        {

        }

        public Zoneconfig(RsdkReader reader)
        {
            StagePalette.Read(reader, 2);

            this.ReadObjectsSpriteSheets(reader);

            this.ReadObjectsNames(reader);

            this.ReadWAVConfiguration(reader);

            this.ReadOGGConfiguration(reader);

            reader.Close();
        }

        internal void ReadObjectsSpriteSheets(RsdkReader reader)
        {
            byte objectSheets_count = reader.ReadByte();

            for (int i = 0; i < objectSheets_count; ++i)
            { ObjectSpritesheets.Add(reader.ReadRSDKString()); }
        }

        internal void WriteObjectsSpriteSheets(RsdkWriter writer)
        {
            writer.Write((byte)ObjectSpritesheets.Count);
            foreach (string name in ObjectSpritesheets)
                writer.WriteRSDKString(name);
        }

        internal void ReadObjectsNames(RsdkReader reader)
        {
            byte objects_count = reader.ReadByte();

            for (int i = 0; i < objects_count; ++i)
            {
                ObjectData OD = new ObjectData();
                OD.FilePath = reader.ReadRSDKString();
                Objects.Add(OD);
            }

            for (int i = 0; i < objects_count; i++)
            {
                Objects[i].SpriteSheetID = reader.ReadByte();
            }
        }

        internal void WriteObjectsNames(RsdkWriter writer)
        {
            writer.Write((byte)Objects.Count);

            for (int i = 0; i < Objects.Count; ++i)
            {
                writer.WriteRSDKString(Objects[i].FilePath);
            }

            for (int i = 0; i < Objects.Count; i++)
            {
                writer.Write(Objects[i].SpriteSheetID);
            }
        }

        internal void ReadWAVConfiguration(RsdkReader reader)
        {
            byte SoundFX_count = reader.ReadByte();

            for (int i = 0; i < SoundFX_count; ++i)
            { SoundFX.Add(reader.ReadString()); }
        }

        internal void WriteWAVConfiguration(RsdkWriter writer)
        {
            writer.Write((byte)SoundFX.Count);
            foreach (string wav in SoundFX)
                writer.Write(wav);
        }

        internal void ReadOGGConfiguration(RsdkReader reader)
        {
            byte Music_count = reader.ReadByte();

            for (int i = 0; i < Music_count; ++i)
            { Music.Add(reader.ReadString()); }
        }

        internal void WriteOGGConfiguration(RsdkWriter writer)
        {
            writer.Write((byte)Music.Count);
            foreach (string mus in Music)
                writer.Write(mus);
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
            StagePalette.Write(writer);

            WriteObjectsSpriteSheets(writer);

            WriteObjectsNames(writer);

            WriteWAVConfiguration(writer);

            WriteOGGConfiguration(writer);

            writer.Close();
        }

    }
}
