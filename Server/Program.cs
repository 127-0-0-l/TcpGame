using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Server
{
    class Program
    {
        private static object _lock = new object();
        
        private static (TcpClient playerRed, TcpClient playerBlue) clients;

        static void Main()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 5000);
            tcpListener.Start();
            Console.WriteLine("listen");

            clients.playerRed = tcpListener.AcceptTcpClient();
            new Thread(new ThreadStart(()=> { HandleClient(clients.playerRed); })).Start();
            Console.WriteLine("Red player connected");
            Send(clients.playerRed, "red");

            clients.playerBlue = tcpListener.AcceptTcpClient();
            new Thread(new ThreadStart(()=> { HandleClient(clients.playerBlue); })).Start();
            Console.WriteLine("Blue player connected");
            Send(clients.playerBlue, "blue");
            Send(clients.playerRed, "start");
        }

        private static void HandleClient(TcpClient client)
        {
            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int length = stream.Read(buffer, 0, buffer.Length);

                if (length == 0)
                    break;

                string data = Encoding.Unicode.GetString(buffer, 0, length);
                
                if(client == clients.playerRed)
                {
                    Send(clients.playerBlue, data);
                }
                else if(client == clients.playerBlue)
                {
                    Send(clients.playerRed, data);
                }
                
                Console.WriteLine(data);
            }

            if (client == clients.playerRed)
            {
                Console.WriteLine("Blue player disconnected");
            }
            else if (client == clients.playerBlue)
            {
                Console.WriteLine("Blue player disconnected");
            }

            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public static void Send(TcpClient client, string data)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(data);

            lock (_lock)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
