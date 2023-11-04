using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_interface.Models
{
   public class ChatConversation
    {
        public string ContactName { get; set; } 

        public string RecievedMessage { get; set; }

        public string MsgRecievedOn{ get; set; }

        public string SentMessage { get; set; }
        public string MsgSentOn { get; set; }
        public bool IsMessageRecieved { get; set; } 
        public bool MessageContainsReply { get; set; }
    }
}
