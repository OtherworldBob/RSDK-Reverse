using RSDK.Core.IO;

namespace RSDKv2
{
    public class TextFile
    {

        byte[] TextFileInfo = new byte[1];

        public TextFile()
        {

        }

        public TextFile(RsdkReader reader)
        {
            reader.ReadBytes(reader.BaseStream.Length);
            reader.Close();
        }

        public void Write(RsdkWriter writer)
        {
            writer.Write(TextFileInfo);
            writer.Close();
        }

    }
}
