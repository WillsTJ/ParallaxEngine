using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace BattleCARDS.Networking.NetSocket
{
    internal class Client
    {
        private static NetSocketCommClient netSocketCommClient = new NetSocketCommClient();

        public Client()
        {
            netSocketCommClient.Connect();
            
            /*while (true)
            {
                // This loop keeps the process active.
            }*/

        }

    }
}
