using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace RFID_Sick_Reader
{
    public class Reader : IDisposable
    {
        private readonly string ipendpoint;
        private TcpClient tcpclient = new TcpClient();

        public Reader(string ipendpoint)
        {
            this.ipendpoint = ipendpoint;
        }

        public void Read()
        {
            byte[] buffer = new byte[4096];
            while (true)
            {
                try
                {
                    if (!tcpclient.Connected)
                        tcpclient.Connect(IPEndPoint.Parse(ipendpoint));
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    Console.Error.WriteLine("Could not connect");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }

                try
                {
                    int received = tcpclient.Client.Receive(buffer);
                    if (received == 0) // they closed their send
                    {
                        Console.WriteLine($"{DateTime.Now} - {tcpclient.Client.RemoteEndPoint} - They closed their send");
                        tcpclient.Close();
                        tcpclient = new TcpClient();
                        continue;
                    }
                    byte[] data = buffer.ToList().Take(received).ToArray();
                    string stringdata = System.Text.Encoding.ASCII.GetString(data);

                    foreach (Match m in Regex.Matches(stringdata, @"\u0002(.+?)\u0003"))
                    {
                        string epc = m.Groups[1].Value;
                        Console.WriteLine($"{DateTime.Now} - {tcpclient.Client.RemoteEndPoint} - Received EPC: {epc}");
                    }
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Console.Error.WriteLine($"{DateTime.Now} - {tcpclient.Client.RemoteEndPoint} - They disconnected");
                    tcpclient.Close();
                    tcpclient = new TcpClient();
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
        }

        public void Dispose()
        {
            tcpclient.Close();
        }
    }
}
