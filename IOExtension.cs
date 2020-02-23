using System.IO;
using System.Text;

namespace IOExtension
{
    public static class IOExtension
    {
        public static string ReadUTF8StringLen(this BinaryReader reader, int length)
        {
            byte[] bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ReadUTF8String(this BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(reader.ReadInt32());
            return Encoding.UTF8.GetString(bytes);
        }

        public static void Align(this BinaryReader reader, int align)
        {
            while(reader.BaseStream.Position % align != 0)
            {
                reader.BaseStream.Position++;
            }
        }

        public static void Skip(this BinaryReader reader, int count)
        {
            reader.BaseStream.Position += count;
        }

        public static void writeUTF8(this BinaryWriter writer, string line)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(line);
            writer.Write(bytes);
        }

        public static void writeUTF8Line(this BinaryWriter writer, string line)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(line + "\n");
            writer.Write(bytes);
        }
    }
}