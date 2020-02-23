using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace em2txt
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Message> Messages = new List<Message>();

            using(BinaryReader reader = new BinaryReader(File.OpenRead(args[0])))
            {
                int unk1 = reader.ReadInt32();
                int unk2 = reader.ReadInt32();
                int unk3 = reader.ReadInt32();
                int unk4 = reader.ReadInt32();
                int unk5 = reader.ReadInt32();
                int unk6 = reader.ReadInt32();
                int unk7 = reader.ReadInt32();

                string filename = ReadUTF8String(reader, reader.ReadInt32());

                int messages_count = reader.ReadInt32();

                for (int i = 0; i < messages_count; i++) {
                    Message msg = new Message();

                    msg.Entries = new List<string>();
                    msg.Unknown = new List<int>();

                    msg.Unknown.Add(reader.ReadInt32()); // unk8
                    msg.Unknown.Add(reader.ReadInt32()); // unk9

                    msg.MessageGroup = reader.ReadInt32();
                    msg.MessageId = reader.ReadInt32();

                    msg.Unknown.Add(reader.ReadInt32()); // unk12
                    msg.Unknown.Add(reader.ReadInt32()); // unk13
                    msg.Unknown.Add(reader.ReadInt32()); // unk14
                    msg.Unknown.Add(reader.ReadInt32()); // unk15
                    msg.Unknown.Add(reader.ReadInt32()); // unk16
                    msg.Unknown.Add(reader.ReadInt32()); // unk17
                    msg.Unknown.Add(reader.ReadInt32()); // unk18

                    msg.VoiceId = ReadUTF8String(reader, reader.ReadInt32());
                    Align(reader, 4);

                    if (reader.PeekChar() != 0) {
                        msg.TalkBackgroundId = ReadUTF8String(reader, reader.ReadInt32());
                        Align(reader, 4);
                    } else {
                        msg.TalkBackgroundId = "Empty";
                        Skip(reader, 4);
                    }

                    int entry_count = reader.ReadInt32();

                    for (int j = 0; j < entry_count; j++)
                    {
                        string text = ReadUTF8String(reader, reader.ReadInt32());
                        msg.Entries.Add(text.Remove(text.Length - 2).Replace(Environment.NewLine, "\\n"));
                        Align(reader, 4);
                    }

                    Messages.Add(msg);
                }
            }

            string json = JsonConvert.SerializeObject(Messages, Formatting.Indented);

            File.WriteAllText("eventMessage.json", json);

            /*using(BinaryWriter writer = new BinaryWriter(File.OpenWrite("out.txt")))
            {
                foreach(Message msg in Messages)
                {
                    writeUTF8Line(writer, msg.VoiceId);
                    writeUTF8Line(writer, msg.TalkBackgroundId);
                    writeUTF8Line(writer, Convert.ToString(msg.MessageGroup));
                    writeUTF8Line(writer, Convert.ToString(msg.MessageId));
                    writeUTF8Line(writer, String.Join("|", msg.unk));

                    foreach(string entry in msg.Entries)
                    {
                        writeUTF8Line(writer, entry);
                    }

                    writeUTF8Line(writer, "&&&");
                }
            }*/
        }

        static string ReadUTF8String(BinaryReader reader, int length)
        {
            byte[] bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        static void Align(BinaryReader reader, int align)
        {
            while(reader.BaseStream.Position % align != 0)
            {
                reader.BaseStream.Position++;
            }
        }

        static void Skip(BinaryReader reader, int count)
        {
            reader.BaseStream.Position += count;
        }

        static void writeUTF8(BinaryWriter writer, string line)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(line);
            writer.Write(bytes);
        }

        static void writeUTF8Line(BinaryWriter writer, string line)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(line + "\n");
            writer.Write(bytes);
        }
    }
}
