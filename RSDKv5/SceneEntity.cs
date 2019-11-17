﻿using System;
using System.Collections.Generic;
using System.Linq;
using RSDK.Core.IO;

namespace RSDKv5
{
    [Serializable]
    public class SceneEntity : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// what slotID this entity is in the level
        /// </summary>
        public ushort SlotID;
        /// <summary>
        /// the entities' position
        /// </summary>
        public Position Position;

        /// <summary>
        /// the entity type global data
        /// </summary>
        public SceneObject Object;
        
        private string extObjName = null;


        /// <summary>
        /// a list of all the attribute values for this entity
        /// </summary>
        public List<AttributeValue> Attributes = new List<AttributeValue>();
        /// <summary>
        /// the attribute values list sorted by the attribute names
        /// </summary>
        public DictionaryWithDefault<string, AttributeValue> attributesMap = new DictionaryWithDefault<string, AttributeValue>(RSDKv5.Scene.FallbackValue);


        public SceneEntity(SceneObject obj, ushort slotID)
        {
            Object = obj;
            SlotID = slotID;
            foreach (AttributeInfo attribute in Object.Attributes)
            {
                Attributes.Add(new AttributeValue(attribute.Type));
                attributesMap[attribute.Name.ToString()] = Attributes.Last();
            }
        }


        public SceneEntity(SceneEntity other, ushort slotID)
        {
            Object = other.Object;
            SlotID = slotID;
            Position = other.Position;

            foreach (AttributeInfo attribute in Object.Attributes)
            {
                Attributes.Add(other.GetAttribute(attribute.Name).Clone());
                attributesMap[attribute.Name.ToString()] = Attributes.Last();
            }
        }

        internal SceneEntity(RsdkReader reader, SceneObject obj)
        {
            Object = obj;
            SlotID = reader.ReadUInt16();
            Position = new Position(reader);

            foreach (AttributeInfo attribute in Object.Attributes)
            {
                Attributes.Add(new AttributeValue(reader, attribute.Type));
                attributesMap[attribute.Name.ToString()] = Attributes.Last();
            }
        }

        public AttributeValue GetAttribute(string name)
        {
            return attributesMap[name];
        }

        public bool AttributeExists(string name, AttributeTypes type)
        {
            if (attributesMap.ContainsKey(name)) {
                if (attributesMap[name].Type != type) return false;
                else return true;
            } 
            else return false;
        }

        public AttributeValue GetAttribute(NameIdentifier name)
        {
            return GetAttribute(name.ToString());
        }

        internal void Write(RsdkWriter writer)
        {
            writer.Write(SlotID);
            Position.Write(writer);

            foreach (AttributeValue attribute in Attributes)
                attribute.Write(writer);
        }

        public void AddAttributeToObject(string name, AttributeTypes type)
        {
            Console.WriteLine("Attempted to add attribute of name \"" + name + "\" to entity \"" + Object.Name + "\"");
            Object.AddAttribute(name, type);
        }

        public void PrepareForExternalCopy()
        {
            // Clear the Object reference to avoid dragging along more data than necessary
            // The Object reference will be reassigned upon pasting
            if (Object != null)
                extObjName = Object.Name.Name;
            Object = null;
        }

        public bool IsExternal()
        {
            return (Object == null && extObjName != null);
        }

        public static SceneEntity FromExternal(SceneEntity oldEntity, List<SceneObject> objs, ushort slotID)
        {
            // Search every Object in this Scene
            foreach (SceneObject obj in objs)
            {
                // If this Entity is supposed to be of that Object...
                if (oldEntity.extObjName == obj.Name.Name)
                {
                    // Make the Entity with that Object
                    SceneEntity newEntity = new SceneEntity(obj, slotID);
                    newEntity.Position = oldEntity.Position;

                    // Check each AttributeInfo in the Object
                    foreach (AttributeInfo attInfo in obj.Attributes)
                    {
                        // Is there a matching AttributeValue in the old Entity that can be copied?
                        if (attInfo.Name.Name != null && oldEntity.attributesMap.ContainsKey(attInfo.Name.Name))
                        {
                            AttributeValue oldAttVal = oldEntity.attributesMap[attInfo.Name.Name];
                            AttributeValue newAttVal = newEntity.attributesMap[attInfo.Name.Name];

                            // Make sure they share the same type or else problems will ensue
                            if (oldAttVal.Type == newAttVal.Type)
                            {
                                // Reassign the value
                                switch (newAttVal.Type)
                                {
                                    case AttributeTypes.INT8:
                                        newAttVal.ValueInt8 = oldAttVal.ValueInt8;
                                        break;

                                    case AttributeTypes.INT16:
                                        newAttVal.ValueInt16 = oldAttVal.ValueInt16;
                                        break;

                                    case AttributeTypes.INT32:
                                        newAttVal.ValueInt32 = oldAttVal.ValueInt32;
                                        break;

                                    case AttributeTypes.UINT8:
                                        newAttVal.ValueUInt8 = oldAttVal.ValueUInt8;
                                        break;

                                    case AttributeTypes.UINT16:
                                        newAttVal.ValueUInt16 = oldAttVal.ValueUInt16;
                                        break;

                                    case AttributeTypes.UINT32:
                                        newAttVal.ValueUInt32 = oldAttVal.ValueUInt32;
                                        break;

                                    case AttributeTypes.ENUM:
                                        newAttVal.ValueEnum = oldAttVal.ValueEnum;
                                        break;

                                    case AttributeTypes.BOOL:
                                        newAttVal.ValueBool = oldAttVal.ValueBool;
                                        break;

                                    case AttributeTypes.COLOR:
                                        newAttVal.ValueColor = oldAttVal.ValueColor;
                                        break;

                                    case AttributeTypes.VECTOR2:
                                        newAttVal.ValueVector2 = oldAttVal.ValueVector2;
                                        break;

                                    case AttributeTypes.VECTOR3:
                                        newAttVal.ValueVector3 = oldAttVal.ValueVector3;
                                        break;

                                    case AttributeTypes.STRING:
                                        newAttVal.ValueString = oldAttVal.ValueString;
                                        break;
                                }
                            }
                        }
                    }
                    
                    return newEntity;
                }
            }

            // No matching Object found, so nothing is created
            // Be sure to handle the null return or else a null Entity might get left floating around
            return null;
        }

        [Serializable]
        public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
        {
            TValue _default;
            public TValue DefaultValue
            {
                get { return _default; }
                set { _default = value; }
            }
            public DictionaryWithDefault() : base() { }
            public DictionaryWithDefault(TValue defaultValue) : base()
            {
                _default = defaultValue;
            }
            public new TValue this[TKey key]
            {
                get
                {
                    TValue t;
                    return base.TryGetValue(key, out t) ? t : _default;
                }
                set { base[key] = value; }
            }
        }
    }
}
