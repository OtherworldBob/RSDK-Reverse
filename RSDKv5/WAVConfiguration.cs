using RSDK.Core.IO;

namespace RSDKv5
{
    public class WAVConfiguration
    {
        /// <summary>
        /// the name of the sound effect
        /// </summary>
        public string Name;

        public byte MaxConcurrentPlay;

        public WAVConfiguration()
        {
        }

        public WAVConfiguration(RsdkReader reader)
        {
            Name = reader.ReadRSDKString();
            MaxConcurrentPlay = reader.ReadByte();
        }

        public void Write(RsdkWriter writer)
        {
            writer.WriteRSDKString(Name);
            writer.Write(MaxConcurrentPlay);
        }
    }
}
