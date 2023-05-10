using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BattleCARDS.Model
{
    public class Physics
    {
        // Player position coordinates.
        private double player1_x;
        private double player1_y;

        private double player2_x;
        private double player2_y;

        // This Rect defines the bounds for a ground-plane on the scene.
        public Rect parallaxZoneLeft;
        public Rect parallaxZoneRight;
        public Rect groundPlaneRect;
        private double downForce = 12;
        private double runSpeed = 14;
        public double backgroundScrollSpeed = 0.4;
        public double backgroundScrollSpeedSlow = 0.2;

        public Physics()
        {
            // Initialise default position values for player 1.
            this.InitialisePlayer1Position();
            this.InitialiseSceneBounds();
        }

        public void InitialiseSceneBounds()
        {
            this.groundPlaneRect = new Rect(0, 1470, 720, 1080);
            this.parallaxZoneLeft = new Rect(200, 0, 40, 100);
            this.parallaxZoneRight = new Rect(580, 0, 40, 100); // 2200
        }

        public void InitialisePlayer1Position()
        {
            this.Player1_X = 80;
            this.Player1_Y = 80;

            this.Player2_X = 2000;
            this.Player2_Y = 1400;
        }

        public double Gravity(double yPosition)
        {
            return yPosition += this.downForce;
        }

        public double Upthrust(double yPosition)
        {
            return yPosition -= this.downForce;
        }

        public double OffsetParallax(double xPosition, int state)
        {
            if (state == 0) // Offset objects by translating them to the right of the scene.
            {
                return xPosition += this.backgroundScrollSpeed; // Scroll backgorund to the right.
            }

            if (state == 1) // Offset objects by translating them to the left of the scene.
            {
                return xPosition -= this.backgroundScrollSpeed; // Scroll backgorund to the left.
            }

            return xPosition;
        }

        public double TranslateBackground(double xPosition, double speed, int state)
        {
            if (state == 0) // Player running to the left.
            {
                return xPosition += speed; // Scroll backgorund to the right.
            }

            if (state == 1) // Player running to the right.
            {
                return xPosition -= speed; // Scroll backgorund to the left.
            }

            return xPosition;
        }

        public double TranslatePlayer(double xPosition, int state)
        {
            if (state == 0)
            {
                return xPosition -= this.runSpeed;
            }

            if (state == 1)
            {
                return xPosition += this.runSpeed;
            }

            return xPosition;
        }

        public double Player1_X
        {
            get
            {
                return this.player1_x;
            }
            set
            {
                this.player1_x = value;
            }
        }

        public double Player1_Y
        {
            get
            {
                return this.player1_y;
            }
            set
            {
                this.player1_y = value;
            }
        }



        public double Player2_X
        {
            get
            {
                return this.player2_x;
            }
            set
            {
                this.player2_x = value;
            }
        }

        public double Player2_Y
        {
            get
            {
                return this.player2_y;
            }
            set
            {
                this.player2_y = value;
            }
        }
    }
}
