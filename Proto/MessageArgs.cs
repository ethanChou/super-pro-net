using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace super_pro_net
{
    public class MessageArgs : EventArgs
    {

        private Message message;

        public MessageArgs(Message msg)
        {
            this.message = msg;
        }

        public Message Message1
        {
            get { return message; }
            set { message = value; }
        }
    }
}
