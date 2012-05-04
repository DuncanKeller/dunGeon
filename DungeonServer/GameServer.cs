using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace DungeonServer
{
    class GameServer
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private List<TcpClient> clients = new List<TcpClient>();
        private int newKey = 0;

        public GameServer()
        {
           this.tcpListener = new TcpListener(IPAddress.Any, 3000);
           this.listenThread = new Thread(new ThreadStart(BeginListen));
           this.listenThread.Start();
        }

        private void BeginListen()
        {
            // start listening
            tcpListener.Start();

            while (true)
            {
                // grab dat client
                TcpClient client = tcpListener.AcceptTcpClient();

                // send 'em off on his own little thread
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        private void HandleClient(object client)
        {
            // cast client
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream stream = tcpClient.GetStream();
            clients.Add(tcpClient);

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

                for(int i = 0; i < clients.Count; i++)
                {
                    Send(data, i);
                }
            }

            tcpClient.Close();
            clients.Remove(tcpClient);
        }

        public void Send(string data, int id)
        {
            NetworkStream clientStream = clients[id].GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(data);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

    }
}
