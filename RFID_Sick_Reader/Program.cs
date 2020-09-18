using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RFID_Sick_Reader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lector scanner (imager/laser) has 10.0.0.6 and RFU610 scanner (rfid) has 10.0.0.4
            ThreadPool.QueueUserWorkItem((s) =>
            {
                using (var rfidreader = new Reader("127.0.0.1:2112")) 
                    rfidreader.Read();
            });

            ThreadPool.QueueUserWorkItem((s) =>
            {
                using (var scannerreader = new Reader("127.0.0.1:2113"))
                    scannerreader.Read();
            });

            Console.ReadLine();

        }
    }
}
