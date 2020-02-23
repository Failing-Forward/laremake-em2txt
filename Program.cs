using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using IOExtension;

namespace em2txt
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            
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

                string filename = reader.ReadUTF8String();

                int messages_count = reader.ReadInt32();

                for (int i = 0; i < messages_count; i++) {
                    Message msg = new Message();

                    msg.Entries = new List<string>();
                    msg.Unknown = new List<int>();

                    msg.Unknown.Add(reader.ReadInt32()); // unk8
                    msg.Unknown.Add(reader.ReadInt32()); // unk9

                    msg.MessageGroup = reader.ReadInt32();
                    msg.MessageId = reader.ReadInt32();

                    msg.Unknown.Add(reader.ReadInt32()); // unk10
                    msg.Unknown.Add(reader.ReadInt32()); // unk11
                    msg.Unknown.Add(reader.ReadInt32()); // unk12
                    msg.Unknown.Add(reader.ReadInt32()); // unk13
                    msg.Unknown.Add(reader.ReadInt32()); // unk14
                    msg.Unknown.Add(reader.ReadInt32()); // unk15
                    msg.Unknown.Add(reader.ReadInt32()); // unk16

                    msg.VoiceId = reader.ReadUTF8String();
                    reader.Align(4);

                    if (reader.PeekChar() != 0) {
                        msg.TalkBackgroundId = reader.ReadUTF8String();
                        reader.Align(4);
                    } else {
                        msg.TalkBackgroundId = "Empty";
                        reader.Skip(4);
                    }

                    int entry_count = reader.ReadInt32();
                    for (int j = 0; j < entry_count; j++)
                    {
                        string text = reader.ReadUTF8String();
                        msg.Entries.Add(text.Remove(text.Length - 2).Replace(Environment.NewLine, "\\n"));
                        reader.Align(4);
                    }

                    Messages.Add(msg);
                }
            }

            File.WriteAllText("eventMessage.json", JsonConvert.SerializeObject(Messages, Formatting.Indented));
        }
    }
}
