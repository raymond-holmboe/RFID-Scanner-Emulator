using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RFID_Sick_Emulator
{
    public class PacketDecoder
    {
        public string Decode(byte[] packet)
        {
            string strpacket = Encoding.ASCII.GetString(packet);
            strpacket = strpacket.Replace("\u0002", "<STX>").Replace("\u0003", "<ETX>");
            return strpacket;
        }
    }
}
