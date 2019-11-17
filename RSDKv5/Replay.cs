using System.Collections.Generic;
using RSDK.Core.IO;

namespace RSDKv5
{
    public class Replay
    {
        public List<Position> positions = new List<Position>();
        public Replay()
        {

        }

        public Replay(RsdkReader reader)
        {
            var creader = reader.GetCompressedStreamRaw();
            //creader.BaseStream.Position = 0x84;

            while (creader.BaseStream.Position + 8 < creader.BaseStream.Length)
            {
                positions.Add(new Position(creader));
            }

            creader.Close();
        }

        public void Write(RsdkWriter writer)
        {

        }

    }
}
