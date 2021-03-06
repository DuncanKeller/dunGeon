﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace MultiDungeon
{
    class Client
    {
        static TcpClient server;

        public static bool connected = false;
        static ASCIIEncoding encoder = new ASCIIEncoding();
        static int size = 32;

        public static void Connect(string ip)
        {
            server = new TcpClient();

            server.Connect(ip, 3000);

            Thread clientThread = new Thread(new ParameterizedThreadStart(Run));
            clientThread.Start();
            connected = true;
        }

        private static void Run(object sender)
        {
            NetworkStream stream = server.GetStream();

            byte[] message = new byte[size];
            int bytesRead;
            ASCIIEncoding encoder = new ASCIIEncoding();
            while (true)
            {
                bytesRead = 0;
                string data = "";
                List<string> commands = new List<string>();
                try
                {
                    bytesRead = stream.Read(message, 0, size);

                    data = encoder.GetString(message, 0, bytesRead);

                    while (data[data.Length - 1] != '!')
                    {
                        bytesRead = stream.Read(message, 0, size);
                        data += encoder.GetString(message, 0, bytesRead);
                    }

                    //while (data[data.Length - 1] != '!')
                    //{
                    //    bytesRead = stream.Read(message, 0, size);
                    //    data += encoder.GetString(message, 0, bytesRead);

                    //    while (!data.Contains('!'))
                    //    {
                    //        bytesRead = stream.Read(message, 0, size);
                    //        data += encoder.GetString(message, 0, bytesRead);
                    //    }

                    //    string parseMe = data.Substring(0, data.LastIndexOf("!"));
                    //    ParseAndRecieve(parseMe);

                    //    data = data.Substring(data.LastIndexOf("!"));

                    //}

                    //while (!data.Contains('!'))
                    //{

                    //    bytesRead = stream.Read(message, 0, size);
                    //    data += encoder.GetString(message, 0, bytesRead);

                    //    string parseMe = data.Substring(0, data.LastIndexOf("!"));
                    //    ParseAndRecieve(parseMe);

                    //    data = data.Substring(data.LastIndexOf("!"), data.Length);
                    //}

                    ParseAndRecieve(data);

                    stream.Flush();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message, MessageType.urgent);
                    //Console.WriteLine(e.stackTrace);
                    break;
                }

                if (bytesRead == 0)
                {
                    // nothing to read, disconnect
                    break;
                }

            }

            Close();
        }

        public static void ParseAndRecieve(string data)
        {
            string[] datum = data.Split('!');
            World.RecieveData(datum);
        }

        public static void Close()
        {
            if (server != null)
            {
                if (server.Connected)
                {
                    NetworkStream clientStream = null;
                    try
                    {
                        clientStream = server.GetStream();
                        ASCIIEncoding encoder = new ASCIIEncoding();
                        byte[] b = encoder.GetBytes("");
                        clientStream.Write(b, 0, 0);
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        server.Close();
                        clientStream.Flush();
                        clientStream.Close();
                        connected = false;
                    }

                }
            }
        }

        public static void Send(string data)
        {
            data += "!";
            NetworkStream clientStream = server.GetStream();

            byte[] buffer = encoder.GetBytes(data);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        public static void SendPosition(int id, int x, int y, double a)
        {
            /*
            NetworkStream clientStream = server.GetStream();

            byte[] buffer = encoder.GetBytes("p");

            clientStream.Write(buffer, 0, buffer.Length);

            buffer = BitConverter.GetBytes(id);

            clientStream.Write(buffer, 0, buffer.Length);

            buffer = BitConverter.GetBytes(x);

            clientStream.Write(buffer, 0, buffer.Length);

            buffer = BitConverter.GetBytes(y);

            clientStream.Write(buffer, 0, buffer.Length);

            buffer = BitConverter.GetBytes(a);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            */
        }

        public static void SendBullet(int id)
        {
            /*
            NetworkStream clientStream = server.GetStream();

            byte[] buffer = encoder.GetBytes("b");
            clientStream.Write(buffer, 0, buffer.Length);

            buffer = BitConverter.GetBytes(id);
            clientStream.Write(buffer, 0, buffer.Length);

            clientStream.Flush();
            */
        }
    }
}