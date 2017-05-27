using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace super_pro_net
{
    public class Message:IDisposable
    {
        /**
	    * LEN 10
	    */
        public static int LEN = 10;

        /**
         * 消息id
         */
        private int id;

        /**
         * 消息时间戳
         */
        private int timestamp;

        /**
         * 消息类型 
         */
        private byte type;

        /**
         * 标记，一条数据分成多个Message，0 or 1
         */
        private byte marker;


        /**
         * 数据
         */
        private byte[] data;

        public Message(int id, int tstamp, byte type, byte mk, byte[] data)
        {
            this.id = id;
            this.timestamp = tstamp;
            this.type = type;
            this.marker = mk;
            this.data = data;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public byte DType
        {
            get { return type; }
            set { type = value; }
        }

        public byte Marker
        {
            get { return marker; }
            set { marker = value; }
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public void Dispose()
        {
            data = null;
        }
    }
}
