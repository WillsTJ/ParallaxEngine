using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Net.Sockets;
using JustConnect;
using System.Net;
using Windows.UI.Popups;
using System.Runtime.CompilerServices;

using BattleCARDS.Networking.NetSocket;
using Windows.Storage.Pickers;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;
using Telegram.Bot.Types;
using BattleCARDS.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BattleCARDS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Decalre a port number. Hard-coded for quick testing.
        /// </summary>
        private const int PORT_NUMBER = 2002;

        /// <summary>
        /// Declare a socket class.
        /// </summary>
        private static Socket socket;

        /// <summary>
        /// This class implements logic for the sockets client.
        /// </summary>
        private Client mainClient;

        /// <summary>
        /// This class implements logic for the sockets server.
        /// </summary>
        private Server mainServer;

        public Model.Draw draw;
        public Model.Update update;
        public Model.Physics physics;
        public View.LoadAnimatedResources loadAnimatedResources;
        public Controllers.ResourceController resourceController;
        public Controllers.InputParser inputParser;
        public ContentPipeline contentPipeline = new ContentPipeline();
        private List<Button> contentButtonList = new List<Button>();

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            this.inputParser = new Controllers.InputParser(this);
            this.physics = new Model.Physics();

            this.loadAnimatedResources = new View.LoadAnimatedResources(this);
            this.draw = new Model.Draw(this.loadAnimatedResources.CanvasBitmapList, this);
            this.update = new Model.Update(this);

            this.AnimatedCanvas.CreateResources += new TypedEventHandler<CanvasAnimatedControl, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs>(this.loadAnimatedResources.GenerateResources);
            this.AnimatedCanvas.Draw += new TypedEventHandler<ICanvasAnimatedControl, CanvasAnimatedDrawEventArgs>(draw.AnimatedDraw);
            this.AnimatedCanvas.Update += new TypedEventHandler<ICanvasAnimatedControl, CanvasAnimatedUpdateEventArgs>(update.AnimatedUpdate);

            this.resourceController = new Controllers.ResourceController(this);

            this.contentButtonList.Add(this.ObjectTB1);
            this.contentButtonList.Add(this.ObjectTB2);
            this.contentButtonList.Add(this.ObjectTB3);
            this.contentButtonList.Add(this.ObjectTB4);
            this.contentButtonList.Add(this.ObjectTB5);
            this.contentButtonList.Add(this.ObjectTB6);
            this.contentButtonList.Add(this.ObjectTB7);
            this.contentButtonList.Add(this.ObjectTB8);
            this.contentButtonList.Add(this.ObjectTB9);
            this.contentButtonList.Add(this.ObjectTB10);
            this.contentButtonList.Add(this.ObjectTB11);

            for (int index = 0; index < this.contentButtonList.Count; index++)
            {
                this.contentButtonList[index].Click += new RoutedEventHandler(this.ObjectTB_click);
            }

            this.UpdateContentPipelineVisual();
        }

        private void ObjectTB_click(object sender, object e)
        {
            Button buttonRef = new Button();
            buttonRef = sender as Button;

            // Highlight the Rect in focus.
            foreach(Rect rect in this.contentPipeline.contentRectList)
            {
                if (rect == ((Rect)buttonRef.Content))
                {
                    this.draw.rectToHighlight = rect;
                }
            }

        }

        private void UpdateContentPipelineVisual()
        {
            for (int index= 0; index < this.contentPipeline.contentRectList.Count; index++)
            {
                this.contentButtonList[index].Content = this.contentPipeline.contentRectList[index];
            }
        }

        public void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            // Send key-down information to the 'Controller' to parse.
            this.inputParser.ParseKeyInput(e.VirtualKey);
        }

        public void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            // Send key-up information to the 'Controller' to parse.
            this.inputParser.ParseKeyInput(Windows.System.VirtualKey.Escape);
        }

        public async void GameLoopThreadBridge()
        {
            await this.AnimatedCanvas.RunOnGameLoopThreadAsync(() => { UpdateDebugMonitor(this.draw.Bit.BoundingRect.X.ToString()); });
        }

        public void UpdateDebugMonitor(string debugString)
        {

        }


        public void InitialiseServer()
        {
            mainServer = new Server();
        }

        public void InitialiseClient()
        {
            mainClient = new Client();
        }


        //  Library-abstraction version.
        private class SimpleClientServer
        {
            /// <summary>
            /// Create a server object.
            /// </summary>
            private JustConnect.Tcp.Server server;

            /// <summary>
            /// Create a client object.
            /// </summary>
            private JustConnect.Tcp.Client client;

            /// <summary>
            /// Declare a byte buffer for low-level sockets comms.
            /// </summary>
            private static byte[] buffer { get; set; }

            private void Initialise()
        {
            this.client = new JustConnect.Tcp.Client();
            this.server = new JustConnect.Tcp.Server();
            this.server.Port = PORT_NUMBER;

            this.client.Received += new JustConnect.Tcp.ClientReceivedHandler(Client_InboundMessage);
            this.server.Accepted += new JustConnect.Tcp.ServerAcceptedHandler(Server_ConnectionEstablished);
            this.server.Received += new JustConnect.Tcp.ServerReceivedHandler(Server_InboundMessage);
            this.server.Log += new JustConnect.Tcp.ServerLogHandler(Server_Log);

            InitialiseClientServerCommsTest();
        }
        private void InitialiseClientServerCommsTest()
        {
            this.InitialiseServer();
            this.InitialiseNetClient();

            this.client.Send("Hello, world! From the client!");
        }

        private void Server_Log(string log)
        {
            string logBuffer = log;
        }

        private void Client_InboundMessage(string sender)
        {
            string buffer = string.Empty;
            buffer = sender;
        }

        private void Server_InboundMessage(string message, Socket client)
        {
            // An inbound message from a client has been recieved by the server.
            string serverMsgRecieved = message;
        }

        private void Server_ConnectionEstablished(Socket socket)
        {

        }

        private void InitialiseServer()
        {
            this.server.Start();
        }

        private void InitialiseNetClient()
        {
            client.Port = PORT_NUMBER;

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

            void connectToSocket()
            {
                // Target the local host machine IP address.
                this.client.Connect(getIPAddress());
            }

            try
            {
                connectToSocket();
            }
            catch
            {
                // Re-try connection.
                connectToSocket();
            }
        }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Close/release any resources.
        }

        private void UpdateNetworkLogButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the UI visual with debug information regarding the nerwork.
            //this.MessageRecievedTB.Text = PacketHandler.packetHandlerDebugLog;
        }

        private void MainVB_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Send key-down information to the 'Controller' to parse.
            this.inputParser.ParseKeyInput(e.Key);
        }

        private void MainViewportSP_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Send key-down information to the 'Controller' to parse.
            this.inputParser.ParseKeyInput(e.Key);
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Send key-down information to the 'Controller' to parse.
            this.inputParser.ParseKeyInput(e.Key);
        }

        private void LoadResourcesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadResources()
        {
            this.AnimatedCanvas.CreateResources += new TypedEventHandler<CanvasAnimatedControl, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs>(this.loadAnimatedResources.GenerateResources);   
        }

        private void DrawSceneButton_Click(object sender, RoutedEventArgs e)
        {
            this.AnimatedCanvas.Draw += new TypedEventHandler<ICanvasAnimatedControl, CanvasAnimatedDrawEventArgs>(draw.AnimatedDraw);
            this.AnimatedCanvas.Update += new TypedEventHandler<ICanvasAnimatedControl, CanvasAnimatedUpdateEventArgs>(update.AnimatedUpdate);
        }
    }
}
