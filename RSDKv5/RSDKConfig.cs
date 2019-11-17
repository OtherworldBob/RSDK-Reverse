using System;
using System.Collections.Generic;
using RSDK.Core.IO;

namespace RSDKv5
{
    public class RSDKConfig
    {

        public class Variable
        {
            public string Name = "";
            public string Type = "";
            public string Value = "";

            public Variable(string name = "", string type = "", string value = "")
            {
                Name = name;
                Type = type;
                Value = value;
            }

            public Variable(RsdkReader reader)
            {
                Name = reader.ReadString();
                Type = reader.ReadString();
                Value = reader.ReadString();
            }

            public void Write(RsdkWriter writer)
            {
                writer.Write(Name);
                writer.Write(Type);
                writer.Write(Value);
            }
        }

        public class Alias
        {
            public string Name = "";
            public string Value = "";

            public Alias()
            {
            }

            public Alias(RsdkReader reader)
            {
                Name = reader.ReadString();
                Value = reader.ReadString();
            }

            public void Write(RsdkWriter writer)
            {
                writer.Write(Name);
                writer.Write(Value);
            }
        }

        public List<Variable> Variables = new List<Variable>();
        public List<Alias> Aliases = new List<Alias>();

        public RSDKConfig()
        {

        }

        public RSDKConfig(string filename) : this(new RsdkReader(filename))
        {

        }

        public RSDKConfig(System.IO.Stream reader) : this(new RsdkReader(reader))
        {

        }

        public RSDKConfig(RsdkReader reader)
        {
            byte vcount = reader.ReadByte();

            for (int i = 0; i < vcount; i++)
            {
                Variables.Add(new Variable(reader));
                Console.WriteLine(Variables[i].Name + ", " + Variables[i].Type + ", " + Variables[i].Value);
            }

            byte acount = reader.ReadByte();

            for (int i = 0; i < acount; i++)
            {
                Aliases.Add(new Alias(reader));
                Console.WriteLine(Aliases[i].Name + ", " + Aliases[i].Value);
            }
            reader.Close();
        }

        public void Write(string filename)
        {
            using (var writer = new RsdkWriter(filename))
                Write(writer);
        }

        public void Write(System.IO.Stream stream)
        {
            using (var writer = new RsdkWriter(stream))
                Write(writer);
        }

        public void Write(RsdkWriter writer)
        {
            writer.Write((byte)Variables.Count);

            for (int i = 0; i < Variables.Count; i++)
            {
                Variables[i].Write(writer);
            }

            writer.Write((byte)Aliases.Count);

            for (int i = 0; i < Aliases.Count; i++)
            {
                Aliases[i].Write(writer);
            }
            writer.Close();
        }

    }
}
