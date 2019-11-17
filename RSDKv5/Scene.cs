﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using RSDK.Core.IO;

namespace RSDKv5
{
    [Serializable]
    public class Scene
    {   
        /// <summary>
        /// the file's signature
        /// </summary>
        public static readonly byte[] MAGIC = new byte[] { (byte)'S', (byte)'C', (byte)'N', (byte)'\0' };
        public static bool readTilesOnly;

        /// <summary>
        /// metadata stuff for RSDK Scene editor
        /// </summary>
        public SceneEditorMetadata EditorMetadata;

        /// <summary>
        /// the layers in this scene
        /// </summary>
        public List<SceneLayer> Layers = new List<SceneLayer>();
        /// <summary>
        /// the object types in this scene
        /// </summary>
        public List<SceneObject> Objects = new List<SceneObject>();


        public static AttributeValue FallbackValue = new AttributeValue(AttributeTypes.ENUM);


        public Scene()
        {
            EditorMetadata = new SceneEditorMetadata();
        }

        public Scene(string filename) : this(new RsdkReader(filename))
        {

        }

        public Scene(RsdkReader reader)
        {
            if (!readTilesOnly)
            {
                // Load scene
                if (!reader.ReadBytes(4).SequenceEqual(MAGIC))
                    throw new Exception("Invalid scene file header magic");

                EditorMetadata = new SceneEditorMetadata(reader);

                byte layers_count = reader.ReadByte();
                for (int i = 0; i < layers_count; ++i)
                    Layers.Add(new SceneLayer(reader));

                byte objects_count = reader.ReadByte();
                for (int i = 0; i < objects_count; ++i)
                    Objects.Add(new SceneObject(reader));
            }
            else
            {
                // Load scene
                if (!reader.ReadBytes(4).SequenceEqual(MAGIC))
                    throw new Exception("Invalid scene file header magic");

                EditorMetadata = new SceneEditorMetadata(reader);

                byte layers_count = reader.ReadByte();
                for (int i = 0; i < layers_count; ++i)
                    Layers.Add(new SceneLayer(reader));

                /*byte objects_count = reader.ReadByte();
                for (int i = 0; i < objects_count; ++i)
                    Objects.Add(new SceneObject(reader));*/
            }
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

        internal void Write(RsdkWriter writer)
        {
            writer.Write(MAGIC);

            EditorMetadata.Write(writer);

            writer.Write((byte)Layers.Count);
            foreach (SceneLayer layer in Layers)
                layer.Write(writer);

            writer.Write((byte)Objects.Count);
            foreach (SceneObject obj in Objects)
                obj.Write(writer);
        }
    }
}
