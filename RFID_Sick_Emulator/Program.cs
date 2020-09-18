using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RFID_Sick_Emulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var packetcreator = new PacketCreator();

            ThreadPool.QueueUserWorkItem((s) =>
            {
                var rfidpackets = new List<byte[]>();
                rfidpackets.Add(packetcreator.Create("54erbye45y45y"));
                rfidpackets.Add(packetcreator.Create("rthdrthhdrtdb", "23425352.43545.23"));
                rfidpackets.Add(packetcreator.Create("42352346354", "bsdb43597jb45987hj", "drjfgd98rjte98d"));
                rfidpackets.Add(packetcreator.Create("876576456344"));
                rfidpackets.Add(packetcreator.Create("2342345645675678"));

                var rfidemulator = new Emulator(rfidpackets); // rfu610
                rfidemulator.Send("127.0.0.1:2112"); // 10.0.0.4:2112
            });

            ThreadPool.QueueUserWorkItem((s) =>
            {
                var scannerpackets = new List<byte[]>();
                scannerpackets.Add(packetcreator.Create("8976fd86678678"));
                scannerpackets.Add(packetcreator.Create("875674f57467467"));
                scannerpackets.Add(packetcreator.Create("765756v756"));
                scannerpackets.Add(packetcreator.Create("3564564v5764565"));
                scannerpackets.Add(packetcreator.Create("765863464567b8578"));

                var scanneremulator = new Emulator(scannerpackets); // lector
                scanneremulator.Send("127.0.0.1:2113"); // 10.0.0.6:2112
            });

            Console.ReadLine();
        }
    }
}
