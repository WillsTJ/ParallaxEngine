using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace BattleCARDS.Networking.NetSocket
{
    internal class NetSocketCommClient
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

        public NetSocketCommClient()
        {
            // Initialise a socket.
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            // Return the IP address of the local machine.
            // Used to help establish a socket.
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

            // Request remote host connection.
            socket.BeginConnect(new IPEndPoint(getIPAddress(), PORT_NUMBER), ConnectCallback, null);
        }

        private async void ConnectCallback(IAsyncResult result)
        {
            try
            {
                if (socket.Connected)
                {
                    string debugString = "Connected to the server!";
                    buffer = new byte[1028];
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecievedCallback, null);

                    // Create a data packet.
                    #region Initial Packet

                    byte[] packet = new byte[1028];
                    byte[] packetLength = BitConverter.GetBytes((ushort)packet.Length);
                    byte[] packetType = BitConverter.GetBytes((ushort)2000);

                    Array.Copy(packetLength, packet, 2);
                    Array.Copy(packetType, packet, 2);

                    socket.Send(packet);
                    #endregion

                }
                else
                {
                    MessageDialog errorDialog = new MessageDialog("Error, could not connect to the server.", "Connection error");
                    await errorDialog.ShowAsync();
                }
            }
            catch
            {

            }
        }

        private void RecievedCallback(IAsyncResult result)
        {
            int bufferLength = socket.EndReceive(result);
            byte[] packet = new byte[1028];
            Array.Copy(buffer, packet, packet.Length);

            // Handle packet.
            buffer = new byte[1028];
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecievedCallback, null);
        }
    }
}
