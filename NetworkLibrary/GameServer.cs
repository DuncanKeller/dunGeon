using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace NetworkLibrary
{
    public class GameServer
    {
        Random rand = new Random();
        private TcpListener tcpListener;
        private Thread listenThread;
        private Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
        private int newKey = 0;
        private int seed;
        private bool runnning = true;

        private int maxPlayers = 4;
        private int readyPlayers = 0;

        private int mapType = 0;

        private int gameType = 0;

        public GameServer()
        {
            
        }

        public void Start()
        {
            tcpListener = new TcpListener(IPAddress.Any, 3000);
            listenThread = new Thread(new ThreadStart(BeginListen));
            listenThread.Start();
            GenerateSeed();

            while (runnning)
            {

            }

            tcpListener.Stop();
            //listenThread.Join();
        }
        
        /// <summary>
        /// Initialize game elements
        /// </summary>
        /// <param name="p">Max number of players</param>
        /// <param name="m">Map Type</param>
        /// <param name="gt">Game Type</param>
        public void InitGame(int p, int m, int gt)
        {
            maxPlayers = p;
            mapType = m;
            gameType = gt;
        }

        public int Seed
        {
            get { return seed; }
        }

        public void GenerateSeed()
        {
            seed = rand.Next(1024 * 4);
        }

        public void Reset()
        {
            readyPlayers = 0;
            GenerateSeed(); foreach (int id in clients.Keys)
            {
                Send("rand\n" + seed.ToString() + "!", id);
            }
        }

        public void Close()
        {
            runnning = false;
        }

        private void BeginListen()
        {
            // start listening
            tcpListener.Start();

            try
            {
                while (runnning)
                {
                    // grab dat client
                    TcpClient client = tcpListener.AcceptTcpClient();

                    // send 'em off on his own little thread
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(client);
                }
            }
            catch (SocketException e)
            {
                // server closed
            }
        }

        public void StartGame()
        {
            foreach (int id in clients.Keys)
            {
                Send("start!", id);
            }
        }

        public void SetControllers()
        {
            int count = 0;
            foreach (int id in clients.Keys)
            {
                Send("xbox" + "\n" + count.ToString() + "!", id);
                count++;
            }
        }

        private void HandleClient(object client)
        {
            // cast client
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream stream = tcpClient.GetStream();

            if (clients.Count >= 6)
            {
                string data = "maxed";
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(data);

                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                tcpClient.Close();
                return;
            }

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
              
                string[] commands = data.Split('\n');
                if (commands[0].Contains("ready"))
                {
                    readyPlayers++;
                    Console.WriteLine(readyPlayers + "ready"); 
                    if (readyPlayers == maxPlayers)
                    {
                        StartGame();
                        readyPlayers = 0;
                    }
                } else if (commands[0].Contains("reset"))
                {
                    Console.Write("reset");
                    Reset();
                }
                else
                {
                    foreach (var k in clients)
                    {
                        Send(data, k.Key);
                    }
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
            Console.Write(data);
            NetworkStream clientStream = clients[id].GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(data);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
    }
}
