using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace BattleCARDS.Networking.NetSocket
{
    internal class NetSocketComm
    {
        /// <summary>
        /// Declare a byte buffer for low-level sockets comms.
        /// </summary>
        private static byte[] buffer { get; set; }

        /// <summary>
        /// Decalre a port number. Hard-coded for quick testing.
        /// </summary>
        private const int PORT_NUMBER = 2002;

        /// <summary>
        /// Declare a socket class.
        /// </summary>
        private static Socket socket;

        private void AuthenticateConnection()
        {
           

            try
            {
                // Initialise a socket.
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                
               
                //socket.Connect(localEndPoint);
            }
            catch (Exception ex)
            {
                MessageDialog errorLog = new MessageDialog("Error connceting to a server! Please check socket credentials. Original exception: " + ex.ToString());
            }
        }

        public void Bind()
        {
            IPAddress getIPAddress()
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip;
                    }
                }

                return null;
            }

            AuthenticateConnection();

            // Bind the socket.
            socket.Bind(new IPEndPoint(getIPAddress(), PORT_NUMBER));

        }

        public Task Listen() // void
        {
            // Listen on the socket for incomming connection requests,
            socket.Listen(500);

            Task.Delay(100);
            return Task.CompletedTask;
        }

        public void Accept()
        {
            socket.BeginAccept(this.AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSocket = socket.EndAccept(result);

            buffer = new byte[1028];
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecievedOnConnectionCallback, clientSocket);
            Accept();
        }

        private void RecievedOnConnectionCallback(IAsyncResult result)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket = (Socket)result.AsyncState;// as Socket;
            try
            {
                int bufferSize = clientSocket.EndReceive(result);

                byte[] packet = new byte[bufferSize];
                Array.Copy(buffer, packet, packet.Length);

                PacketHandler.Handle(packet, clientSocket);


                // Handle the packet.
                buffer = new byte[1028];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecievedOnConnectionCallback, clientSocket);
            }
            catch
            {
                // Attempt to re-initialise the socket class.
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket = (Socket)result.AsyncState;

                int bufferSize = clientSocket.EndReceive(result);

                byte[] packet = new byte[bufferSize];
                Array.Copy(buffer, packet, packet.Length);

                PacketHandler.Handle(packet, clientSocket);


                // Handle the packet.
                buffer = new byte[1028];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecievedOnConnectionCallback, clientSocket);
            }
            finally
            {

                

            }
        }
    }
}
