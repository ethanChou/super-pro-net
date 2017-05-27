using System;
using System.Net.Sockets;

namespace super_pro_net.Server
{
    public class UltraClient : UltraSocket
    {

        private string _ip;
        private int _port;
        /// <summary>
        /// 客户端主动请求服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public UltraClient(string ip = "127.0.0.1", int port = 8400)
        {
            this._ip = ip;
            this._port = port;
            this._Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //this._Socket.BeginConnect(ip, port, Connect_Async, _Socket);
            this._Socket.Connect(ip, port);
            this.SetSocket();
            this.ReceiveAsync();
        }

        private void Connect_Async(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            
            if (socket.Connected)
            {
              
                Console.WriteLine("Connect true");
                socket.EndConnect(ar);
            }
            else
            {
                Console.WriteLine("Connect false");
                
                socket.BeginConnect(_ip, _port, Connect_Async, socket);
            }
        }
        
        protected override void OnMessageReceived(MessageArgs e)
        {
            base.OnMessageReceived(e);
        }
    }
}
