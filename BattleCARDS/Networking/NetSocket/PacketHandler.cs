using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace BattleCARDS.Networking.NetSocket
{
    public static class PacketHandler
    {
        public static string packetHandlerDebugLog = string.Empty;

        public static void Handle(byte[] packet, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            packetHandlerDebugLog = "Packet recived! Length; " + packetLength.ToString() + ". Type: " + packetType;
        }
    }
}
