using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Kimmidoll
{
    public class ByteBuffer
    {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;

        public ByteBuffer()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public ByteBuffer(byte[] data, int length)
        {
            stream = new MemoryStream(data, 0, length);
            reader = new BinaryReader(stream);
        }

        public void Close()
        {
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();

            stream.Close();
            writer = null;
            reader = null;
            stream = null;
        }

        public void WriteChar(char v)
        {
            writer.Write(v);
        }

        public void WriteShort(short v)
        {
            writer.Write(v);
        }

        public void WriteInt(int v)
        {
            writer.Write(v);
        }

        public void WriteLong(long v)
        {
            writer.Write(v);
        }

        public void WriteString(string v)
        {
            v += '\0';
            writer.Write(v.ToCharArray());
        }

        public void WriteBytes(byte[] v)
        {
            writer.Write(v);
        }

        public void WriteMd5()
        {
            long length = stream.Length;
            writer.Seek(0, 0);
            writer.Write((int)length + 16);
            writer.Seek((int)length, 0);

            string sign = "NzrWj";
            writer.Write(sign.ToCharArray());

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(stream.ToArray(), 0, (int)stream.Length);

            stream.SetLength(length);
            writer.Write(hash);
        }

        public char ReadChar()
        {
            return reader.ReadChar();
        }

        public short ReadShort()
        {
            return reader.ReadInt16();
        }

        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public long ReadLong()
        {
            return reader.ReadInt64();
        }

		public ushort ReadUShort()
		{
			return reader.ReadUInt16();
		}

		public uint ReadUInt()
		{
			return reader.ReadUInt32();
		}

		public ulong ReadULong()
		{
			return reader.ReadUInt64();
		}

        public string ReadString(int packageLength)
        {
            byte[] temp = stream.ToArray();
            long index = stream.Position;
            for (; index < stream.Length; index++)
            {
                if (temp[index] == '\0')
                    break;
            }
            string strReturn = Encoding.Default.GetString(temp, (int)stream.Position, (int)(index - stream.Position));
            stream.Position = index + 1;
            return strReturn;
        }

        public byte[] ToBytes()
        {
            writer.Flush();
            return stream.ToArray();
        }

        public void Flush()
        {
            writer.Flush();
        }  
    }
}
