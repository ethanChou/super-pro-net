using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace super_pro_net
{
    public class Protocol
    {
        private List<byte> remainBuffer = new List<byte>(2);
       
        private  int ConstLenght = 8;

        /// <summary>
        /// 默认大端解析
        /// </summary>
        public static bool IsBigEndian = false;

        /// <summary>
        ///  包头 assic # 35
        /// </summary>
        private const byte HEAD = 0x23;
        /// <summary>
        /// 版本 0x01
        /// </summary>
        private const byte VERSION = 0x01;

        /// <summary>
        /// 保留
        /// </summary>
        private const byte RESERVE0 = 0x00;

        /// <summary>
        /// 保留
        /// </summary>
        private const byte RESERVE1 = 0x00;

        public byte[] Encode(Message msg)
        {
            MemoryStream ms = new MemoryStream();

            BinaryWriter bw = new BinaryWriter(ms, new UTF8Encoding());
            byte[] msgBuffer = msg.Data;

            //封装包头
            bw.Write(HEAD);
            bw.Write(VERSION);
            bw.Write(RESERVE0);
            bw.Write(RESERVE1);

            // 包协议
            if (msgBuffer != null)
            {
                bw.Write((msgBuffer.Length + 10));
                bw.Write(msg.Id);
                bw.Write(msg.Timestamp);
                bw.Write(msg.DType);
                bw.Write(msg.Marker);
                bw.Write(msgBuffer);
            }
            else
            {
                bw.Write((Int32)0);
            }
            bw.Close();
            ms.Close();
            bw.Dispose();
            ms.Dispose();
            return ms.ToArray();
        }
        
        public List<Message> Decode(byte[] buff, int len)
        {
            //拷贝本次的有效字节
            byte[] _b = new byte[len];
            Array.Copy(buff, 0, _b, 0, _b.Length);
            buff = _b;
            if (this.remainBuffer.Count > 0)
            {
                //拷贝之前遗留的字节
                this.remainBuffer.AddRange(_b);
                buff = this.remainBuffer.ToArray();
                this.remainBuffer.Clear();
                this.remainBuffer = new List<byte>(2);
            }
            List<Message> list = new List<Message>();
            try
            {
                int pos = 0;
                byte[] _buff;
            Label_00983:
                if ((buff.Length - pos) < ConstLenght)
                {
                    _buff = new byte[buff.Length - pos];
                    Array.Copy(buff, pos, _buff, 0, _buff.Length);
                    this.remainBuffer.AddRange(_buff);
                    return list;
                }
                short hd = buff[pos];
                pos += 1;
                short ver = buff[pos];
                pos += 1;
                short mm = BitConverter.ToInt16(buff, pos);
                pos += 2;
                if (!(hd == HEAD && ver == VERSION))
                {
                    pos -= 3;
                    goto Label_00983;
                }
                int offset = BitConverter.ToInt32(buff, pos);
                pos = pos + 4;
                if (offset <= (buff.Length - pos))
                {
                    int msgID = BitConverter.ToInt32(buff, pos);
                    pos += 4;

                    int timestamp = BitConverter.ToInt32(buff, pos);
                    pos += 4;

                    byte type = buff[pos];
                    pos += 1;

                    byte maker = buff[pos];
                    pos += 1;

                    _buff = new byte[offset - 10];
                    Array.Copy(buff, pos, _buff, 0, _buff.Length);
                    pos = pos + _buff.Length;
                    list.Add(new Message(msgID, timestamp, type, maker, _buff));
                    if ((buff.Length - pos) > 0)
                    {
                        goto Label_00983;
                    }
                }
                else
                {
                    pos -= ConstLenght;
                    _buff = new byte[buff.Length - pos];
                    Array.Copy(buff, pos, _buff, 0, _buff.Length);
                    this.remainBuffer.AddRange(_buff);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _b = null;
            }
            return list;
        }
    }
}
