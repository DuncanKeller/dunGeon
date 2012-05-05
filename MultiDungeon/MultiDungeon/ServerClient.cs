using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MultiDungeon
{
    class ServerClient
    {
        static TcpClient server;
        public static bool connected = false;

        public static void Connect()
        {
            server = new TcpClient();
            //IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("Viridian.csh.rit.edu"), 3000);
            server.Connect("Viridian.csh.rit.edu", 3000);

            Thread clientThread = new Thread(new ParameterizedThreadStart(Run));
            clientThread.Start();
            connected = true;
        }

        private static void Run(object sender)
        {
            NetworkStream stream = server.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = stream.Read(message, 0, 4096);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //Console.WriteLine(e.stackTrace);
                }

                if (bytesRead == 0)
                {
                    // nothing to read, disconnect
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                string data = encoder.GetString(message, 0, bytesRead);

                Console.WriteLine(data);
                World.RecieveData(data);
            }

            Close();
        }

        public static void Close()
        {
           
            if(server.Connected) 
            {
                 
                 NetworkStream clientStream = server.GetStream();
                 ASCIIEncoding encoder = new ASCIIEncoding();
                 byte[] b = encoder.GetBytes("");
                 clientStream.Write(b, 0, 0);
                 server.Close();
                 clientStream.Flush();
                 clientStream.Close();
                 connected = false;
            }
            
        }

        public static void Send(string data)
        {
            NetworkStream clientStream = server.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(data);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

    }
}
