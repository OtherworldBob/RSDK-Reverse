using System.Collections.Generic;
using RSDK.Core.IO;

namespace RSDKvB
{
    public class StaticObject
    {

        public List<int[]> Data = new List<int[]>();

        public StaticObject()
        {
        }

        public StaticObject(string filepath) : this(new RsdkReader(filepath))
        {

        }

        public StaticObject(System.IO.Stream strm) : this(new RsdkReader(strm))
        {

        }

        public StaticObject(RsdkReader reader)
        {
            while(!reader.IsEof)
            {
                int size = reader.ReadInt32();
                int[] DataSet = new int[size];
                for (int i = 0; i < size; i++)
                {
                    DataSet[i] = reader.ReadInt32();
                }
                Data.Add(DataSet);
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


        public void Write(RsdkWriter writer)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                writer.Write(Data[i].Length);
                for (int p = 0; p < Data[i].Length; p++)
                {
                    writer.Write(Data[i][p]);
                }
            }
            writer.Close();
        }
    }
}
