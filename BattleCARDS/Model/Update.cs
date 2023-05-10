using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCARDS.Model
{
    public class Update
    {
        MainPage mainPageRef;

        public Update(MainPage mainPage)
        {
            mainPageRef = mainPage;
        }

        public async void AnimatedUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs e)
        {
            try
            {
                // Evaluate any user inputs.
                if (mainPageRef.inputParser.KeyDown == Windows.System.VirtualKey.W)
                {
                    // Render the bit to translate up.
                    //this.mainPageRef.draw.BitList[0].BoundingRect = new Windows.Foundation.Rect(this.mainPageRef.draw.BitList[0].XAxis += 2, this.mainPageRef.draw.BitList[0].YAxis, this.mainPageRef.draw.BitList[0].width, this.mainPageRef.draw.BitList[0].height);
                }

                if (mainPageRef.inputParser.KeyDown == Windows.System.VirtualKey.A)
                {
                    // Go left.
                }

                if (mainPageRef.inputParser.KeyDown == Windows.System.VirtualKey.S)
                {
                    // Go right.
                }

                if (mainPageRef.inputParser.KeyDown == Windows.System.VirtualKey.D)
                {
                    // Go down.
                }

                // Update debug information.
                // mainPageRef.GameLoopThreadBridge();
                //mainPageRef.UpdateDebugMonitor(mainPageRef.draw.BitList[0].XAxis.ToString() + ", " + mainPageRef.draw.BitList[0].XAxis.ToString()); 
            }
            catch
            {

            }
        }
    }
}
