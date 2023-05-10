using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BattleCARDS.Model
{
    public class Bit
    {
        // Run direction.
        public enum RunDirection : int
        {
            Left,
            Up,
            Right,
            Down
        }

        public RunDirection direction;

        // Run speed.
        private double runSpeed;

        // Position members.
        private double xAxis;
        private double yAxis;


        // Bounding rect.
        private Rect boudningRect;

        // Dimensions
        public double height = 240;
        public double width = 240;


        /// <summary>
        /// This class should be constructed when the game needs to spawn a bit.
        /// </summary>
        public Bit()
        {
            this.direction = new RunDirection();
            this.direction = RunDirection.Left;
            this.RunSpeed = 4;
            this.boudningRect = new Rect(XAxis, YAxis, width, height);
        }

        public double RunSpeed
        {
            get
            {
                return this.runSpeed;
            }
            set
            {
                this.runSpeed = value;
            }
        }

        public double XAxis
        {
            get
            {
                return this.xAxis;
            }
            set
            {
                this.xAxis = value;
            }
        }

        public double YAxis
        {
            get
            {
                return this.yAxis;
            }
            set
            {
                this.yAxis = value;
            }
        }

        public Rect BoundingRect
        {
            get
            {
                return this.boudningRect;
            }
            set
            {
                this.boudningRect = value;
            }
        }


    }
}
