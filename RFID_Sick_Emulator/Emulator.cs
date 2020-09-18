using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RFID_Sick_Emulator
{
    public class Emulator
    {
        private List<byte[]> packets = new List<byte[]>(); // to emulate
        public Emulator(List<byte[]> packets)
        {
            this.packets = packets;
        }

        public void Send(string ipaddress)
        {
            var localep = IPEndPoint.Parse(ipaddress);
            var listener = new TcpListener(localep);
            Console.WriteLine("Listening to " + localep);
            listener.Start();
            var client = listener.AcceptTcpClient();
            Console.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - Client connected from IP address: {client.Client.RemoteEndPoint}");
            while (true)
            {
                foreach (var packet in packets)
                {
                    try
                    {
                        Console.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - Sending packet: " + BitConverter.ToString(packet).Replace("-", ""));
                        client.Client.Send(packet);
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        Console.Error.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - They disconnected: {client.Client.RemoteEndPoint}");
                        Console.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - Listening to: {localep}");
                        client = listener.AcceptTcpClient();
                        Console.WriteLine($"{DateTime.Now} - {client.Client.LocalEndPoint} - Client connected from IP address: {client.Client.RemoteEndPoint}");
                    }
                    System.Threading.Thread.Sleep(5000);
                }
                //client.Close(); // for testing graceful shutdown on client
                //break;
            }
        }
    }
}
