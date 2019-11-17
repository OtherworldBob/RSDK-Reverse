﻿using System;
using RSDK.Core.IO;

namespace RSDKv5
{
    [Serializable]
    public class NameIdentifier
    {
        /// <summary>
        /// the MD5 hash of the name in bytes
        /// </summary>
        private byte[] Hash;
        /// <summary>
        /// the name in plain text
        /// </summary>
        public String Name = null;

        public NameIdentifier(string name)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                Hash = md5.ComputeHash(new System.Text.ASCIIEncoding().GetBytes(name));
            }
            Name = name;
        }

        public NameIdentifier(byte[] hash)
        {
            Hash = hash;
        }

        internal NameIdentifier(RsdkReader reader)
        {
             Hash = reader.ReadBytes(16);
        }

        internal void Write(RsdkWriter writer)
        {
             writer.Write(Hash);
        }

        public string HashString()
        {
            return BitConverter.ToString(Hash).Replace("-", string.Empty).ToLower();
        }

        public override string ToString()
        {
            if (Name != null) return Name;
            return HashString();
        }
    }
}
