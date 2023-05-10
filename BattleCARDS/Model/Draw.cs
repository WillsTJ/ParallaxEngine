using BattleCARDS.Controllers;
using BattleCARDS.View;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BattleCARDS.Model
{
    public class Draw
    {
        MainPage mainPageRef;

        /// <summary>
        /// The main Rect() bounds for the background.
        /// </summary>
        private Rect rectbounds1;
        private Rect mapBGRect;
        private Rect player_1_Rect;
        private Rect player_2_Rect;
        private Rect ground_Rect;
        private Rect foregroundRect_0;
        private Rect npc_rect;
        public Rect rectToHighlight = new Rect(0, 0, 0, 0);
        private int npc_steps = 0;
        private int npc_state_int = 0;
        private const int npc_step_limit = 20;
        private bool npc_walk_direction = false;
        private bool debug = true;
        private double ground_X = 0;
        private double ground_Y = 1200;
        private const double playerRectWidth = 40;
        private const double playerRectHeight = 40;
        private const double playerDefaultX = 500;

        public Matrix4x4 groundPlaneMatrix4x4 = new Matrix4x4();
        private Transform3DEffect transformEffect;
        private CanvasBitmap canvasBitmapObjectTransform;

        public float yaw = 0f;
        public float pitch = 0f; // 1.35f;
        public float roll = 0;

        /// <summary>
        /// A List of list of type canvas-bitmap.
        /// This structure is useful as I can order multiple sprites in multiple spritesheets.
        /// </summary>
        private List<List<CanvasBitmap>> SpriteSheet;

        /// <summary>
        /// Access members that create animation functionality.
        /// </summary>
        private BattleCARDS.Model.Animation animation;

        /// <summary>
        /// Constuct this struct to access animation functionality.
        /// </summary>
        private Animation.AnimationStruct aniamtionStruct;

        /// <summary>
        /// This class accesses members for the bits, such as run-speed and other behaviours.
        /// </summary>
        private Bit bit;

        private int animationFrame;
        private int animationPerFrameIndex;
        private int frameIndexLowerRef;

        private List<int> animationFrames = new List<int>();
        private List<int> animationFramePerIndex = new List<int>();

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
        /// Access a list of typical frame-rates for the sprite animations.
        /// </summary>
        public enum FrameRates : int
        {
            OneFrame = 1,
            TwoFrames = 2,
            ThreeFrames = 3,
            FourFrames = 4,
            TwelveFrames = 12
        }

        public enum SpriteSheetReferenceIndexes : int
        {
            // Each index references the slot where the spritesheet is stored in the sprite sheet list.
            MainBackground = 0,
            SpaceBG = 1,
            Saturn = 2,
            Asset = 3,
            NPCWalkLeft = 4,
            NPCWalkRight = 5,
            GrassTexture = 6,
            MistTexture = 7
        }

        /// <summary>
        /// Stores the indexes for frame-limits. 
        /// Populate this list with values that represent the lower-upper indexes for frames
        /// to animate within the CanvasBitmap list.
        /// </summary>
        public List<int> frameIndexes;
        private List<Bit> bitList;

        public Draw(List<List<CanvasBitmap>> spriteSheet, MainPage mainPage)
        {
            this.mainPageRef = mainPage;
            this.SpriteSheet = spriteSheet;
            this.FrameIndexes = new List<int>();

            // Obtain indexes for lower-upper limits of frame-slots from the CanvasBitmap list.
            // Make sure all needed graphics resources are loaded before processing in this class.
            // Functionality may still flow, but I may get issues namely initialising indexes.
            this.ObtainFrameIndexes();
            this.GenerateRects();

            if (this.debug == true)
            {
                this.mainPageRef.contentPipeline.PopulateContentPipelineRects(this.rectbounds1);
                this.mainPageRef.contentPipeline.PopulateContentPipelineRects(this.player_1_Rect);
            }


        }

        public List<Bit> BitList
        {
            get
            {
                return this.bitList;
            }
            set
            {
                this.bitList = value;
            }

        }

        public Bit Bit
        {
            get
            {
                return this.bit;
            }
            set
            {
                this.bit = value;
            }

        }

        /// <summary>
        /// Stores the indexes for frame-limits. 
        /// Populate this list with values that represent the lower-upper indexes for frames
        /// to animate within the CanvasBitmap list.
        /// </summary>
        public List<int> FrameIndexes
        {
            get
            {
                return this.frameIndexes;
            }
            set
            {
                this.frameIndexes = value;
            }

        }

        public BattleCARDS.Model.Animation Animation
        {
            get
            {
                return this.animation;
            }
            set
            {
                this.animation = value;
            }
        }

        /// <summary>
        /// Construct the bounding rects for the graphical assets on-screen.
        /// </summary>
        public void GenerateRects()
        {
            this.rectbounds1 = new Rect(0, 0, 1040, 680); // Background rect
            this.mapBGRect = new Rect(0, 0, 4000, 4000); // Map BG rect
            this.player_1_Rect = new Rect(playerDefaultX, 600, playerRectWidth, playerRectHeight);
            this.player_2_Rect = new Rect(0, 0, 40, 40);
            this.npc_rect = new Rect(0, 0, 40, 40);
            this.ground_Rect = new Rect(0, 0, 40, 40);
            this.foregroundRect_0 = new Rect(0, 0, 700, 2000);

            this.aniamtionStruct = new Animation.AnimationStruct();
        }

        /// <summary>
        /// Obtain frame-indexes to help control sprite sheet animations.
        /// Consider initialising this functionality by calling this method
        /// straight after all CanvasBitmap objects are loaded into memory.
        /// </summary>
        public void ObtainFrameIndexes() // May need to deprecate this function, as my collections hold multiple assets in a single List<>.
        {
            // This function NEEDS to be called AFTER ALL CanvasBitmaps are loaded in memory.

            int animationIndex = 0;


            // First, loop through the list holding the INDIVIDUAL SPRITESHEETS.
            for (int index = 0; index < this.SpriteSheet.Count; index++)
            {
                // Obtain the count values needed for parsing the animations.
                // (In this case, the number of frames in a sprite sheet is used for the "UPPER LIMIT")
                this.FrameIndexes.Add(this.SpriteSheet[index].Count);

                this.animationFramePerIndex.Add(animationIndex);
                this.animationFrames.Add(animationIndex);
            }

            // Store a particular background to be matrix4x4 transformed.
            // This could be a ground-graphic, or some effect, etc.
           // this.canvasBitmapObjectTransform = SpriteSheet[(int)SpriteSheetReferenceIndexes.GrassTexture][0]; // Target a grass texture.


        }

        /// <summary>
        /// The main Draw() loop for the game logic. Main render-logic goes here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void AnimatedDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs e)
        {
            
            try
            {
                if (this.mainPageRef.loadAnimatedResources.resourcesLoaded == true) // Only draw when sprites are loaded
                {
                    // Impose physics on respective objects in the scene.
                    physics();

                    // Render-call flow.
                    drawBackgrounds();
                    drawForegrounds();
                    animateSprites();
                    checkDebugActions();

                    void physics()
                    {
                        // Check for player state- namely LEFT/RIGHT movement.
                        // Check if the player collides with a parallax zone on the left.
                        if (this.player_1_Rect.X <= this.mainPageRef.physics.parallaxZoneLeft.X && this.mainPageRef.inputParser.state == Controllers.InputParser.PlayerState.Left)
                        {
                            // Trigger parallax scrolling.
                            // Background layers.
                            this.rectbounds1.X = this.mainPageRef.physics.TranslateBackground(this.rectbounds1.X, this.mainPageRef.physics.backgroundScrollSpeed, (int)this.mainPageRef.inputParser.state);
                            this.mapBGRect.X = this.mainPageRef.physics.TranslateBackground(this.mapBGRect.X, this.mainPageRef.physics.backgroundScrollSpeedSlow, (int)this.mainPageRef.inputParser.state);


                            this.ground_X = this.mainPageRef.physics.TranslateBackground(ground_X, 12, (int)this.mainPageRef.inputParser.state);
                            this.yaw += 0.002f;

                            // Offset all other assets on the scene against the parallaxing effect.
                            this.npc_rect.X = this.mainPageRef.physics.OffsetParallax(this.npc_rect.X, (int)this.mainPageRef.inputParser.state);
                        }
                        else
                        {
                            // Check for left-movement.
                            if (this.mainPageRef.inputParser.state == Controllers.InputParser.PlayerState.Left)
                            {
                                // Normally translate the player across the scene to the left, without triggering parallax scrolling.
                                this.player_1_Rect.X = this.mainPageRef.physics.TranslatePlayer(this.player_1_Rect.X, (int)this.mainPageRef.inputParser.state);
                            }
                        }

                        // Check if the player collides with a parallax zone on the right.
                        if (this.player_1_Rect.X >= this.mainPageRef.physics.parallaxZoneRight.X && this.mainPageRef.inputParser.state == Controllers.InputParser.PlayerState.Right)
                        {
                            // Trigger parallax scrolling.
                            // Background layers.
                            this.rectbounds1.X = this.mainPageRef.physics.TranslateBackground(this.rectbounds1.X, this.mainPageRef.physics.backgroundScrollSpeed, (int)this.mainPageRef.inputParser.state);
                            this.mapBGRect.X = this.mainPageRef.physics.TranslateBackground(this.mapBGRect.X, this.mainPageRef.physics.backgroundScrollSpeedSlow, (int)this.mainPageRef.inputParser.state);


                            this.ground_X = this.mainPageRef.physics.TranslateBackground(ground_X, 12, (int)this.mainPageRef.inputParser.state);
                            this.yaw -= 0.002f;

                            // Offset all other assets on the scene against the parallaxing effect.
                            this.npc_rect.X = this.mainPageRef.physics.OffsetParallax(this.npc_rect.X, (int)this.mainPageRef.inputParser.state);
                        }
                        else
                        {
                            // Check for right-movement.
                            if (this.mainPageRef.inputParser.state == Controllers.InputParser.PlayerState.Right)
                            {
                                // Translate the player across the scene to the left.
                                this.player_1_Rect.X = this.mainPageRef.physics.TranslatePlayer(this.player_1_Rect.X, (int)this.mainPageRef.inputParser.state);
                            }
                        }
                    }


                    void animateSprites()
                    {
                        // Draw the parallax collision zone- for debugging (see why this isn't drawing)
                        // e.DrawingSession.DrawRectangle(this.mainPageRef.physics.parallaxZoneLeft, Windows.UI.Color.FromArgb(255, 90, 40, 20));
                        void drawNPC()
                        {
                            this.npc_rect = new Rect(this.npc_rect.X, 700, 620, 740);

                            if (this.npc_steps < npc_step_limit && this.npc_walk_direction == false)
                            {
                                // Animate the NPC walking left.
                                e.DrawingSession.DrawImage(this.AnimateSprites(this.SpriteSheet[(int)SpriteSheetReferenceIndexes.NPCWalkLeft], (int)FrameRates.FourFrames, 0, 4, (int)SpriteSheetReferenceIndexes.NPCWalkLeft), this.npc_rect);
                                this.npc_rect.X = this.mainPageRef.physics.TranslatePlayer(this.npc_rect.X, this.npc_state_int);

                                this.npc_steps++;
                                return;
                            }

                            if (this.npc_steps == npc_step_limit && this.npc_walk_direction == false)
                            {
                                e.DrawingSession.DrawImage(this.AnimateSprites(this.SpriteSheet[(int)SpriteSheetReferenceIndexes.NPCWalkLeft], (int)FrameRates.FourFrames, 0, 4, (int)SpriteSheetReferenceIndexes.NPCWalkLeft), this.npc_rect);
                                this.npc_rect.X = this.mainPageRef.physics.TranslatePlayer(this.npc_rect.X, this.npc_state_int);


                                this.npc_walk_direction = !npc_walk_direction;
                                this.npc_steps = 0;
                                this.npc_state_int = 1;
                            }

                            if (this.npc_steps < npc_step_limit && this.npc_walk_direction == true)
                            {
                                // Animate the NPC walking right.
                                e.DrawingSession.DrawImage(this.AnimateSprites(this.SpriteSheet[(int)SpriteSheetReferenceIndexes.NPCWalkRight], (int)FrameRates.FourFrames, 0, 4, (int)SpriteSheetReferenceIndexes.NPCWalkRight), this.npc_rect);
                                this.npc_rect.X = this.mainPageRef.physics.TranslatePlayer(this.npc_rect.X, this.npc_state_int);

                                this.npc_steps++;
                                return;
                            }

                            if (this.npc_steps == npc_step_limit && this.npc_walk_direction == true)
                            {
                                e.DrawingSession.DrawImage(this.AnimateSprites(this.SpriteSheet[(int)SpriteSheetReferenceIndexes.NPCWalkRight], (int)FrameRates.FourFrames, 0, 4, (int)SpriteSheetReferenceIndexes.NPCWalkRight), this.npc_rect);
                                this.npc_rect.X = this.mainPageRef.physics.TranslatePlayer(this.npc_rect.X, this.npc_state_int);

                                this.npc_walk_direction = !npc_walk_direction;
                                this.npc_steps = 0;
                                this.npc_state_int = 0;
                            }

                        }
                        

                    }
                    void drawForegrounds()
                    {
                        // This assigment isn't too optimal.
                        this.canvasBitmapObjectTransform = SpriteSheet[(int)SpriteSheetReferenceIndexes.Saturn][0];

                        // Transform pass 1. Set the transform.
                        transformEffect = new Transform3DEffect()
                        {
                            Source = this.canvasBitmapObjectTransform,
                            InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                            TransformMatrix = Matrix4x4.CreateFromYawPitchRoll(this.yaw, this.pitch, this.roll),
                        };

                        // Apply the yaw/pitch/roll matrix transform.
                        groundPlaneMatrix4x4 = Matrix4x4.CreateFromYawPitchRoll(this.yaw, this.pitch, this.roll);
                        transformEffect.TransformMatrix = groundPlaneMatrix4x4;

                        // Draw the matrix transform graphic.
                        e.DrawingSession.DrawImage(transformEffect, 0f, 0f, this.foregroundRect_0);
                    }

                    void drawBackgrounds()
                    {
                        // e.DrawingSession.DrawImage(this.AnimateSprites(this.SpriteSheet[(int)SpriteSheetReferenceIndexes.SpaceBG], (int)FrameRates.OneFrame, 0, 0, (int)SpriteSheetReferenceIndexes.MainBackground), this.mapBGRect);
                        e.DrawingSession.DrawImage(this.AnimateSprites(this.SpriteSheet[(int)SpriteSheetReferenceIndexes.SpaceBG], (int)FrameRates.OneFrame, 0, 0, (int)SpriteSheetReferenceIndexes.MainBackground), this.rectbounds1);
                    }

                    void checkDebugActions()
                    {
                        if (this.mainPageRef.inputParser.state == Controllers.InputParser.PlayerState.Bypass_Pass_Bitmaps_To_Debug)
                        {
                            // Pass the loaded bitmap graphic file paths to a target directory.
                            // These bitmap filepaths should be targeted by the frame debug tool.
                            // Important to inspect file-paths (and possibly canvas bitmap information)
                            // from run-time, as there might be discrepancies after the program has loaded
                            // against the intended frames.
                            ResourceController.SerialiseAssets.PassFrameFilePaths(this.mainPageRef.loadAnimatedResources.SpriteLocalfilePathList);
                        }

                        if (this.debug == true)
                        {
                            try
                            {
                                Windows.UI.Color highlightColor = new Windows.UI.Color();
                                highlightColor.R = 220;
                                highlightColor.G = 20;
                                highlightColor.B = 20;
                                highlightColor.A = 80;

                                e.DrawingSession.DrawRectangle(rectToHighlight, highlightColor, 40f);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public CanvasBitmap AnimateSprites(List<CanvasBitmap> spritesList, int animationsPerFrame, int frameLowerIndex, int frameIndexUpper, int animationSheetIndex)
        {
            if (animationFrames[animationSheetIndex] >= frameIndexUpper)
            {
                animationFramePerIndex[animationSheetIndex] = 0;
                animationFrames[animationSheetIndex] = 0;
                return spritesList[animationFrames[animationSheetIndex]];
            }

            if (animationFramePerIndex[animationSheetIndex] < animationsPerFrame)
            {
                animationFramePerIndex[animationSheetIndex] += 1;
                return spritesList[animationFrames[animationSheetIndex]];
            }
            else
            {
                animationFramePerIndex[animationSheetIndex] = 0;
                animationFrames[animationSheetIndex] += 1;
                return spritesList[animationFrames[animationSheetIndex]];
            }
        }
    }
}
