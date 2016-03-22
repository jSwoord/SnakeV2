﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeV2
{
    class Client
    {
        public bool nameOk = false;
        private TcpClient client;
        public void Start()
        {
            Console.WriteLine("Enter server IP");
            string serverIP = Console.ReadLine();

            client = new TcpClient(serverIP, 5000);

            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();

            Thread senderThread = new Thread(Send);
            senderThread.Start();

            listenerThread.Join();
            senderThread.Join();

        }

        private void Send()
        {

            string message = "";
            try
            {
                NetworkStream streamer = client.GetStream();

                while (!nameOk)
                {
                    Console.Write("Enter your name: ");
                    message = Console.ReadLine();

                    BinaryWriter writer = new BinaryWriter(streamer);
                    writer.Write(message);
                    writer.Flush();
                    Thread.Sleep(200);
                }
                while (true)
                {
                Console.WriteLine("Du är inne");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Listen()
        {
            string[] message;
            try
            {
                while (!nameOk)
                {
                    NetworkStream streamer = client.GetStream();
                    message = new BinaryReader(streamer).ReadString().Split(';');
                    if (message[0] == "0")
                        nameOk = true;
                    else
                        Console.WriteLine(message[1]);
                }
                while (true)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}