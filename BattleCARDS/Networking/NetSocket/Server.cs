using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCARDS.Networking.NetSocket
{
    internal class Server
    {
        public static NetSocketComm NetSocketComm = new NetSocketComm();

        public Server()
        {
            InitialiseAuthentication();

            /*
            NetSocketComm.Bind();
            NetSocketComm.Listen();
            NetSocketComm.Accept();
            */
            /*while (true)
            {
                // This loop keeps the process active.
            }*/
        }

        public async void InitialiseAuthentication()
        {
            await RunListenerAndAccept();
        }

        public static async Task RunListenerAndAccept()
        {
            NetSocketComm.Bind();
            await NetSocketComm.Listen();
            NetSocketComm.Accept();
        }
    }
}
