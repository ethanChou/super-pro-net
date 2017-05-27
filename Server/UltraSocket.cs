using System;
using System.Net.Sockets;

namespace super_pro_net.Server
{
    public  class UltraSocket
    {
        //封装socket
        internal Socket _Socket;
        //回调
        private AsyncCallback _revCallback;
        //接受数据的缓冲区
        private byte[] _buffers;
        //标识是否已经释放
        private volatile bool IsDispose;
        //10K的缓冲区空间
        private int _bufferSize = 5 * 1024;
        //收取消息状态码
        private SocketError _receiveError;
        //发送消息的状态码
        private SocketError _senderError;
        //每一次接受到的字节数
        private int _receiveSize = 0;
        //接受空消息次数
        private byte _zeroCount = 0;
        //消息解析器
        private readonly Protocol _proto = new Protocol();

        protected UltraSocket()
        {
        }

        /// <summary>
        /// 这个是服务器收到有效链接初始化
        /// </summary>
        /// <param name="socket"></param>
        public UltraSocket(Socket socket)
        {
            this._Socket = socket;
            this.SetSocket();
            this.ReceiveAsync();
        }

        public void SetSocket()
        {
            this._revCallback = new AsyncCallback(this.Receive);
            this.IsDispose = false;

            this._Socket.ReceiveBufferSize = this._bufferSize;
            this._Socket.SendBufferSize = this._bufferSize;
            this._buffers = new byte[this._bufferSize];
        }

        /// <summary>
        /// 关闭并释放资源
        /// </summary>
        public void Close()
        {
            if (!this.IsDispose)
            {
                this.IsDispose = true;
                try
                {
                    try
                    {
                        this._Socket.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                    IDisposable disposable = this._Socket;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                    this._buffers = null;
                    GC.SuppressFinalize(this);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 递归接收消息方法
        /// </summary>
        internal void ReceiveAsync()
        {
            try
            {
                if (!this.IsDispose && this._Socket.Connected)
                {
                    this._Socket.BeginReceive(this._buffers, 0, this._bufferSize, SocketFlags.None, out _senderError,
                        this._revCallback, this);
                    CheckSocketError(_receiveError);
                }
            }
            catch (SocketException)
            {
                this.Close();
            }
            catch (ObjectDisposedException)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 发送消息方法
        /// </summary>
        public int Send(Message msg)
        {
            int size = 0;
            try
            {
                if (!this.IsDispose)
                {
                    byte[] buffer = _proto.Encode(msg);
                    size = this._Socket.Send(buffer, 0, buffer.Length, SocketFlags.None, out _senderError);
                    CheckSocketError(_senderError);
                }
            }
            catch (ObjectDisposedException)
            {
                this.Close();
            }
            catch (SocketException)
            {
                this.Close();
            }
            msg.Dispose();
            return size;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MessageArgs> MessageReceived;

        /// <summary>
        /// 接收消息回调函数
        /// </summary>
        /// <param name="iar"></param>
        private void Receive(IAsyncResult iar)
        {
            if (!this.IsDispose)
            {
                try
                {
                    //接受消息
                    _receiveSize = _Socket.EndReceive(iar, out _receiveError);
                    //检查状态码
                    if (!CheckSocketError(_receiveError) && SocketError.Success == _receiveError)
                    {
                        //判断接受的字节数
                        if (_receiveSize > 0)
                        {
                            byte[] rbuff = new byte[_receiveSize];
                            Array.Copy(this._buffers, rbuff, _receiveSize);
                            var msgs = _proto.Decode(rbuff, _receiveSize);
                            foreach (var msg in msgs)
                            {
                                this.OnMessageReceived(new MessageArgs(msg));
                            }
                            //重置连续收到空字节数
                            _zeroCount = 0;
                            //继续开始异步接受消息
                            ReceiveAsync();
                        }
                        else
                        {
                            _zeroCount++;
                            if (_zeroCount == 5)
                            {
                                this.Close();
                            }
                        }
                    }
                }
                catch (SocketException)
                {
                    this.Close();
                }
                catch (ObjectDisposedException)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 错误判断
        /// </summary>
        /// <param name="socketError"></param>
        /// <returns></returns>
        private bool CheckSocketError(SocketError socketError)
        {
            switch ((socketError))
            {
                case SocketError.SocketError:
                case SocketError.VersionNotSupported:
                case SocketError.TryAgain:
                case SocketError.ProtocolFamilyNotSupported:
                case SocketError.ConnectionAborted:
                case SocketError.ConnectionRefused:
                case SocketError.ConnectionReset:
                case SocketError.Disconnecting:
                case SocketError.HostDown:
                case SocketError.HostNotFound:
                case SocketError.HostUnreachable:
                case SocketError.NetworkDown:
                case SocketError.NetworkReset:
                case SocketError.NetworkUnreachable:
                case SocketError.NoData:
                case SocketError.OperationAborted:
                case SocketError.Shutdown:
                case SocketError.SystemNotReady:
                case SocketError.TooManyOpenSockets:
                    this.Close();
                    return true;
            }
            return false;
        }

        protected virtual void OnMessageReceived(MessageArgs e)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, e);
            }
        }
    }
}
