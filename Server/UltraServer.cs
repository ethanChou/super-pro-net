using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace super_pro_net.Server
{
    public class UltraServer
    {
        private IPEndPoint _ip;
        private Socket _serverSocket;
        private volatile bool IsInit = false;
        List<UltraSocket> _clientSockets = new List<UltraSocket>();

        /// <summary>
        /// 初始化服务器
        /// </summary>
        public UltraServer(string ip = "0.0.0.0", int port = 8400)
        {
            IsInit = true;
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip), port);
            this._ip = localEP;
            try
            {
                Console.WriteLine(string.Format("Start Server {0}:{1} ", ip, port));
                this._serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._serverSocket.Bind(this._ip);
                this._serverSocket.Listen(5000);
                SocketAsyncEventArgs sea = new SocketAsyncEventArgs();
                sea.Completed += new EventHandler<SocketAsyncEventArgs>(this.AcceptAsync_Async);
                this.AcceptAsync(sea);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                this.Dispose();
            }
        }

        private void AcceptAsync(SocketAsyncEventArgs args)
        {
            if (IsInit)
            {
                if (!this._serverSocket.AcceptAsync(args))
                {
                    AcceptAsync_Async(this, args);
                }
            }
            else
            {
                if (args != null)
                {
                    args.Dispose();
                }
            }
        }

        private void AcceptAsync_Async(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                var socket = new UltraSocket(args.AcceptSocket);
                _clientSockets.Add(socket);
            }

            args.AcceptSocket = null;

            this.AcceptAsync(args);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (IsInit)
            {
                IsInit = false;
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        /// <param name="flag1"></param>
        protected virtual void Dispose(bool flag1)
        {
            if (flag1)
            {
                if (_serverSocket != null)
                {
                    try
                    {
                        Console.WriteLine(string.Format("Stop Server {0}:{1} ", this._ip.Address.ToString(),
                            this._ip.Port));
                        _serverSocket.Close();
                        _serverSocket.Dispose();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

      
    }
}
