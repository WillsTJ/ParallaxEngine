using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCARDS.Model
{
    public class Animation
    {
        public struct AnimationStruct
        {
            private int animationFrame;
            private int animationPerFrameIndex;
            private int frameIndexLowerRef;

            public int AnimationFrame
            {
                get { return this.animationFrame; }
                set { this.animationFrame = value; }
            }

            private int FrameIndexLowerRef
            {
                get { return this.frameIndexLowerRef; }
                set { this.frameIndexLowerRef = value; }
            }

            private int AnimationPerFrameIndex
            {
                get { return this.animationPerFrameIndex; }
                set { this.animationPerFrameIndex = value; }
            }


            /// <summary>
            /// Depracated.
            /// </summary>
            /// <param name="frameIndexLower"></param>
            public void InitAnimation(int frameIndexLower)
            {
                animationFrame = 0;
                animationPerFrameIndex = 0;
                frameIndexLowerRef = 0;

                this.AnimationFrame = frameIndexLower;
                this.FrameIndexLowerRef = frameIndexLower;
            }

            /// <summary>
            /// Animate a buffer of bitmap graphics kept in a CanvasBitmap List.
            /// </summary>
            /// <param name="spritesList"></param>
            /// <param name="animationsPerFrame"></param>
            /// <param name="frameIndexLower"></param>
            /// <param name="frameIndexUpper"></param>
            /// <param name="mainRef"></param>
            /// <returns></returns>
            public CanvasBitmap AnimateSprites(List<CanvasBitmap> spritesList, int animationsPerFrame, int frameLowerIndex, int frameIndexUpper)
            {
                if (AnimationFrame >= frameIndexUpper)
                {
                    AnimationPerFrameIndex = 0;
                    AnimationFrame = 0;
                    return spritesList[AnimationFrame];
                }

                if (AnimationPerFrameIndex < animationsPerFrame)
                {
                    AnimationPerFrameIndex += 1;
                    return spritesList[AnimationFrame];
                }
                else
                {
                    AnimationPerFrameIndex = 0;
                    AnimationFrame += 1;
                    return spritesList[AnimationFrame];
                }
            }
        }
    }
}
