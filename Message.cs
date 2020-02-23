using System.Collections.Generic;

namespace em2txt
{
    public class Message
    {
        public string TalkBackgroundId { get; set; }
        public string VoiceId { get; set; }
        public int MessageGroup { get; set; }
        public int MessageId { get; set; }
        public List<int> Unknown { get; set; }
        public List<string> Entries { get; set; }
    }
}