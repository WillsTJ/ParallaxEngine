using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows;
using Windows.Gaming.Input;
using Windows.UI.Xaml;

namespace BattleCARDS.Controllers
{
    public class InputParser
    {
        /// <summary>
        /// A global reference to the key pressed down.
        /// </summary>
        private Windows.System.VirtualKey keyDown;

        private MainPage mainPageRef;

        /// <summary>
        /// Indicate the state of the player- particularly player-direction movement.
        /// </summary>
        public enum PlayerState : int
        {
            Left,
            Right,
            Up,
            Down,
            Idle,
            Bypass_Pass_Bitmaps_To_Debug,
            Set_Status_To_Server,
            Set_Status_To_Client
        }

        /// <summary>
        /// The current state of the player, this mainly refers to the direction of the player across the grid.
        /// </summary>
        public PlayerState state;

        public InputParser(MainPage page)
        {
            this.KeyDown = new Windows.System.VirtualKey();
            state = new PlayerState();
            state = PlayerState.Idle;

            mainPageRef = page;
        }

        /// <summary>
        /// A global reference to the key pressed down.
        /// </summary
        public Windows.System.VirtualKey KeyDown
        {
            get
            {
                return this.keyDown;
            }

            set
            {
                this.keyDown = value;
            }
        }

        /// <summary>
        /// Change the state of the player based on the keyboard button pressed.
        /// </summary>
        /// <param name="pressedKey"></param>
        public void ParseKeyInput(Windows.System.VirtualKey pressedKey)
        {
            this.KeyDown = pressedKey;

            if (this.KeyDown == Windows.System.VirtualKey.W)
            {
                // 'Go up' - pass the 'W' key to global scope.
                this.KeyDown = Windows.System.VirtualKey.W;
                this.state = PlayerState.Up;
                return;
            }

            if (this.KeyDown == Windows.System.VirtualKey.A)
            {
                // 'Go up' - pass the 'W' key to global scope.
                this.KeyDown = Windows.System.VirtualKey.A;
                this.state = PlayerState.Left;
                return;
            }

            if (this.KeyDown == Windows.System.VirtualKey.S)
            {
                // 'Go up' - pass the 'W' key to global scope.
                this.KeyDown = Windows.System.VirtualKey.S;
                this.state = PlayerState.Down;
                return;
            }

            if (this.KeyDown == Windows.System.VirtualKey.D)
            {
                // 'Go up' - pass the 'W' key to global scope.
                this.KeyDown = Windows.System.VirtualKey.D;
                this.state = PlayerState.Right;
                return;
            }

            if (this.KeyDown == Windows.System.VirtualKey.Escape)
            {
                // Character state set to 'idle'.
                this.state = PlayerState.Idle;
            }

            if (this.KeyDown == Windows.System.VirtualKey.F1)
            {
                // Character state set to 'idle'.
                this.state = PlayerState.Bypass_Pass_Bitmaps_To_Debug;
            }

            if (this.KeyDown == Windows.System.VirtualKey.F2)
            {
                // Set the program to a server-state via LAN.
                this.state = PlayerState.Set_Status_To_Server;
                this.mainPageRef.InitialiseServer();
            }

            if (this.KeyDown == Windows.System.VirtualKey.F3)
            {
                // Set the program to a server-state via LAN.
                this.state = PlayerState.Set_Status_To_Client;
                this.mainPageRef.InitialiseClient();
            }
        }
    }
}
