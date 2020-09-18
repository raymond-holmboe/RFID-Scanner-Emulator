using System;
using System.Collections.Generic;
using System.Text;

namespace RFID_Sick_Emulator
{
    public class PacketCreator
    {
        public byte[] Create(params string[] epcs)
        {
            char STX = '\u0002';
            char ETX = '\u0003';
            var packet = new List<byte>();
            foreach (var epc in epcs)
            {
                var data = Encoding.ASCII.GetBytes(STX + epc + ETX);
                packet.AddRange(data);
            }
            return packet.ToArray();
        }
    }
}
