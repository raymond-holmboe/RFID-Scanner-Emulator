using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RFID_Sick_Emulator
{
    public class Emulator : IDisposable
    {
        private readonly string ep;
        private TcpClient client;
        private TcpListener listener;

        public Emulator(string ep)
        {
            this.ep = ep;
        }

        public void Start()
        {
            var localep = IPEndPoint.Parse(ep);
            listener = new TcpListener(localep);
            listener.Start();
            Connect();
        }

        private void Connect()
        {
            client = listener.AcceptTcpClient();
            Console.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - Listening to: {ep}");
            Console.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - Client connected from IP address: {client.Client.RemoteEndPoint}");
        }

        public void Send(byte[] packet)
        {
            try
            {
                Console.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - Sending packet: " + BitConverter.ToString(packet).Replace("-", ""));
                client.Client.Send(packet);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
            {
                Console.Error.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - They disconnected: {client.Client.RemoteEndPoint}");
                Connect();
            }
            //client.Close(); // for testing graceful shutdown on client
            //break;
        }

        public void Dispose()
        {
            client?.Close();
        }
    }
}
