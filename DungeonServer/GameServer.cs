using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Microsoft.Xna.Framework.Net;

namespace DungeonServer
{
    class GameServer
    {
        Random rand = new Random();
        private TcpListener tcpListener;
        private Thread listenThread;
        private Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
        private int newKey = 0;
        private int seed;

        public GameServer()
        {
           this.tcpListener = new TcpListener(IPAddress.Any, 3000);
           this.listenThread = new Thread(new ThreadStart(BeginListen));
           this.listenThread.Start();
           GenerateSeed();
        }

        public int Seed
        {
            get { return seed; }
        }

        public void GenerateSeed()
        {
            seed = rand.Next(1024 * 4);
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

        public  void StartGame()
        {
            int counter = 0;
            int players = clients.Count;
            int numTeams = 2;

            Dictionary<int, int> idToTeamNum = new Dictionary<int, int>();

            foreach (int id in clients.Keys)
            {
                int teamNum = counter / numTeams;
                idToTeamNum.Add(id, teamNum);
                counter++;
            }

            foreach (int id in clients.Keys)
            {
                foreach (int player in idToTeamNum.Keys)
                {
                    Send("start\n" + player.ToString() + "\n" + (idToTeamNum[player]) + "!", id);
                }
            }
        }

        private void HandleClient(object client)
        {
            // cast client
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream stream = tcpClient.GetStream();
            int key = rand.Next(1023);

            while (clients.ContainsKey(key))
            {
                key = rand.Next(100);
            }

            clients.Add(key, tcpClient);

            Send("id\n" + key.ToString() + "!", key);

            Send("rand\n" + seed.ToString() + "!", key);

            Console.WriteLine("Client " + key + " connected");

            byte[] message = new byte[32];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = stream.Read(message, 0, 32);
                    stream.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //Console.WriteLine(e.stackTrace);
                }

                if (bytesRead == 0)
                {
                    Console.WriteLine("Client " + key + " disconnected");
                    ClientDisconnected(key);
                    // nothing to read, disconnect
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                string data = encoder.GetString(message, 0, bytesRead);
                /*
                int index = 0;

                string code = encoder.GetString(message, index, encoder.GetByteCount("p"));
                index += encoder.GetByteCount(code);

                if (code == "p")
                {
                    int id = BitConverter.ToInt32(message, index);
                    index += sizeof(Int32);
                    double x = BitConverter.ToDouble(message, index);
                    index += sizeof(Double);
                    double y = BitConverter.ToDouble(message, index);
                    int poop = 4;
                }
                else
                {
                    
                }

                */

                //Console.WriteLine(data);

                foreach (var k in clients)
                {
                    Send(data, k.Key);
                }
            }

            tcpClient.Close();
            clients.Remove(key);
        }

        public void ClientDisconnected(int id)
        {
            foreach (var k in clients)
            {
                if (k.Key != id)
                {
                    Send("disconnect\n" + id.ToString() + "!", k.Key);
                }
            }
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
