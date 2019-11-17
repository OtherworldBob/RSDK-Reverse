using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RSDK.Core.IO
{
    public class RsdkReader : BinaryReader
    {
        public RsdkReader(Stream stream) : base(stream) { }

        public RsdkReader(string file) : base(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read)) { }

        public string GetFilename()
        {
            var fileStream = BaseStream as FileStream;
            return fileStream.Name;
        }

        public byte[] ReadBytes(long count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException($"requested {count} bytes, while only non-negative int32 amount of bytes possible");
            return ReadBytes((ulong)count);
        }

        public byte[] ReadBytes(ulong count)
        {
            if (count > int.MaxValue)
                throw new ArgumentOutOfRangeException($"requested {count} bytes, but only values up to {int.MaxValue} are possible");
            int cnt = (int)count;
            byte[] bytes = base.ReadBytes(cnt);
            if (bytes.Length < cnt)
                throw new EndOfStreamException($"requested {count} bytes, but got only {bytes.Length} bytes");
            return bytes;
        }

        public bool IsEof
        {
            get { return BaseStream.Position >= BaseStream.Length; }
        }

        public void Seek(long position, SeekOrigin org)
        {
            BaseStream.Seek(position, org);
        }

        public long Pos
        {
            get { return BaseStream.Position; }
        }

        public long Size
        {
            get { return BaseStream.Length; }
        }

        public uint ReadUInt32BE()
        {
            byte[] bytes = ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public string ReadRSDKString()
        {
            return new UTF8Encoding().GetString(ReadBytes(ReadByte()));
        }

        public string ReadRSDKUnicodeString()
        {
            return new UnicodeEncoding().GetString(ReadBytes(ReadUInt16() * 2));
        }
        
        public byte[] ReadCompressed()
        {
            uint compresedSize = ReadUInt32();
            uint uncompressedSize = ReadUInt32BE();
            var compressedBytes = ReadBytes(compresedSize - 4);
            using (var inStream = new MemoryStream(compressedBytes))
                return GetDecompressedData(inStream);
        }
        
        public byte[] ReadCompressedRaw()
        {
            var compressedBytes = ReadBytes(BaseStream.Length);
            using (var inStream = new MemoryStream(compressedBytes))
                return GetDecompressedData(inStream);
        }

        private byte[] GetDecompressedData(MemoryStream compressedSteam)
        {
            using (var outMemoryStream = new MemoryStream())
            using (var decompress = new GZipStream(compressedSteam, CompressionMode.Decompress))
            {
                decompress.CopyTo(outMemoryStream);
                return outMemoryStream.ToArray();
            }
        }
        
        public RsdkReader GetCompressedStream()
        {
            return new RsdkReader(new MemoryStream(ReadCompressed()));
        }

        public RsdkReader GetCompressedStreamRaw()
        {
            return new RsdkReader(new MemoryStream(ReadCompressedRaw()));
        }
    }
}
