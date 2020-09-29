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

            var rfidpackets = new List<byte[]>();
            rfidpackets.Add(packetcreator.Create("54erbye45y45y"));
            rfidpackets.Add(packetcreator.Create("rthdrthhdrtdb", "23425352.43545.23"));
            rfidpackets.Add(packetcreator.Create("42352346354", "bsdb43597jb45987hj", "drjfgd98rjte98d"));
            rfidpackets.Add(packetcreator.Create("876576456344"));
            rfidpackets.Add(packetcreator.Create("2342345645675678"));
            var rfidemulator = new Emulator("127.0.0.1:2112"); // rfu610
            rfidemulator.Start(); // 10.0.0.4:2112

            var scannerpackets = new List<byte[]>();
            scannerpackets.Add(packetcreator.Create("8976fd86678678"));
            scannerpackets.Add(packetcreator.Create("875674f57467467"));
            scannerpackets.Add(packetcreator.Create("765756v756"));
            scannerpackets.Add(packetcreator.Create("3564564v5764565"));
            scannerpackets.Add(packetcreator.Create("765863464567b8578"));

            var scanneremulator = new Emulator("127.0.0.1:2113"); // lector
            scanneremulator.Start(); // 10.0.0.6:2112

            bool rfid = true;

            Console.WriteLine("<space> toggle RFID/scanner");
            Console.WriteLine("<esc> close sockets and exits");
            Console.WriteLine();
            Console.WriteLine("Mode: " + (rfid ? "RFID" : "Scanner"));

            List<byte[]> currentpackets = rfid ? rfidpackets : scannerpackets;
            DisplayChoices(currentpackets);

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.Escape)
                {
                    // gracefully shut down both emulators
                    rfidemulator.Dispose();
                    scanneremulator.Dispose();
                    break;
                }
                if (key.Key == ConsoleKey.Spacebar)
                {
                    rfid = !rfid;
                    currentpackets = rfid ? rfidpackets : scannerpackets;
                    Console.WriteLine();
                    Console.WriteLine("Mode: " + (rfid ? "RFID" : "Scanner"));
                    DisplayChoices(currentpackets);
                    continue;
                }

                if (key.Key > ConsoleKey.D0 && key.Key <= ConsoleKey.D0 + currentpackets.Count)
                    (rfid ? rfidemulator : scanneremulator).Send(currentpackets[key.Key - ConsoleKey.D0 - 1]); // 10.0.0.4:2112
            }
        }

        private static void DisplayChoices(List<byte[]> packets)
        {
            var packetdecoder = new PacketDecoder();
            for (int i = 0; i < packets.Count; i++)
                Console.WriteLine($"{i + 1}. {packetdecoder.Decode(packets[i])}");
        }
    }
}
