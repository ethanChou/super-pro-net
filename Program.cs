using System;
using System.Text;
using super_pro_net.Server;

namespace super_pro_net
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //http://www.jdon.com/concurrent/nio%D4%AD%C0%ED%D3%A6%D3%C3.htm
            //http://www.cnblogs.com/dolphin0520/p/3919162.html
            //https://www.ibm.com/developerworks/cn/java/l-niosvr/index.html
            UltraClient client = new UltraClient();
            byte[] buf = Encoding.UTF8.GetBytes("hello world");
            for (int i = 1; i < 1000001; i++)
            {
                client.Send(new Message(i, (int)DateTime.Now.ToFileTime(), (byte)0, (byte)1, buf));
            }
            Console.ReadLine();
        }
    }
}
