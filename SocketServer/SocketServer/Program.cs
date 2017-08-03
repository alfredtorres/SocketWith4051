using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 6666));
            socket.Listen(4);
            socket.BeginAccept(new AsyncCallback((ar)=>
            {
                var client=socket.EndAccept(ar);
                client.Send(Encoding.Unicode.GetBytes("Hi im fine and you"+DateTime.Now.ToString()));
                //var timer = new System.Timers.Timer();
                //timer.Interval = 2000D;
                //timer.Enabled = true;
                //timer.Elapsed += (o, a) =>
                //    {
                //        client.Send(Encoding.Unicode.GetBytes("message send at" + DateTime.Now.ToString()));
                //    };
                //timer.Start();
                client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);


            }),null);
            Console.WriteLine("Server is ready!");
            Console.Read();
        }
        static byte[] buffer = new byte[1024];
        public static void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                var length = socket.EndReceive(ar);
                var message = Encoding.Unicode.GetString(buffer, 0, length);
                Console.WriteLine(message);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
