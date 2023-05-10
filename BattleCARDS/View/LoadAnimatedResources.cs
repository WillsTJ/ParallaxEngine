using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BattleCARDS.View
{
    public class LoadAnimatedResources
    {
        public bool resourcesLoaded = false;

        /// <summary>
        /// Stores bitmap information that can be used for a sprite.
        /// </summary>
        private CanvasBitmap canvasBitmapObject;

        /// <summary>
        /// Holds a list of bitmap objects.
        /// </summary>
        private List<List<CanvasBitmap>> canvasBitmapList;

        /// <summary>
        /// Pass the number of frames for the sprite sheet.
        /// </summary>
        private int spriteFrameCount;

        /// <summary>
        /// Pass the file path of the sprite.
        /// </summary>
        private string spriteLocalfilePath;

        /// <summary>
        /// Pass a list of the file paths of sprites/sheets.
        /// </summary>
        private List<List<string>> spriteLocalfilePathList;

        private MainPage mainPageRef;

        public enum SpritesheetCounts : int
        {
            // Set the index, depending on how many image files you want to load for a group of assets.
            SpritesheetCount = 4, // Set the number of sprite-sheets to load via a loop functionality.
            TestTrainingStageAssetsIndex = 1
        }

        public enum SpritesheetFrameCounts : int
        {
            // Add the sprite frame-indexes for individual sprite-sheet animations.
            ArcherIdleFrames = 4,
            ArcherRunRightFrames = 5,
            ArcherRunLEftFrames = 5,
            NPCWalkLeftFrames = 5,
            NPCWalkRightFrames = 5,

            ArcherFullSpritesheet = 14,
            SamuraiFullSpritesheet = 10,
            Level1Background = 1,
            GrassTextureFrameCount = 1,
            TrainingStageBackground = 1,
            GameplayGrid = 1,
            MistTextureIndex = 1,
            BitAnimationFrames = 9
        }

        public LoadAnimatedResources(MainPage mainPage)
        {
            this.mainPageRef = mainPage;

            // Construct the (outer) canvas bitmap list.
            this.CanvasBitmapList = new List<List<CanvasBitmap>>();

            // Construct the NESTED canvas bitmap list.
            this.CanvasBitmapList.Add(new List<CanvasBitmap>());

            this.spriteLocalfilePathList = new List<List<string>>();

            // Load the image filepaths for the game graphics.
            // This step needs to be complete before generating resources
            // to the render-flow, at initial game load.
            this.LoadBitmapFromFilepath(); // Use this call for testing default graphics loading.
        }

        /// <summary>
        /// Pass the number of frames for the sprite sheet.
        /// </summary>
        public int SpriteFrameCount
        {
            get
            {
                return this.spriteFrameCount;
            }
            set
            {
                this.spriteFrameCount = value;
            }
        }

        /// <summary>
        /// Pass the file path of the sprite.
        /// </summary>
        public string SpriteLocalfilePath
        {
            get
            {
                return this.spriteLocalfilePath;
            }
            set
            {
                this.spriteLocalfilePath = value;
            }
        }

        /// <summary>
        /// Pass the file path of the sprite.
        /// </summary>
        public List<List<string>> SpriteLocalfilePathList
        {
            get
            {
                return this.spriteLocalfilePathList;
            }
            set
            {
                this.spriteLocalfilePathList = value;
            }
        }

        /// <summary>
        /// A list holding a nested list of bitmap graphics.
        /// </summary>
        public List<List<CanvasBitmap>> CanvasBitmapList
        {
            get
            {
                return this.canvasBitmapList;
            }
            set
            {
                this.canvasBitmapList = value;
            }
        }

        /// <summary>
        /// The canvas bitmap object used to create a graphic.
        /// </summary>
        public CanvasBitmap CanvasBitmapObject
        {
            get
            {
                return this.canvasBitmapObject;
            }
            set
            {
                this.canvasBitmapObject = value;
            }
        }

        /// <summary>
        /// Load the bitmap assets for access in the render-flow, namely the Draw() calls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void GenerateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs e)
        {
            int index2 = 0;

            // Loop through the outer LIST<List>.
            for (int index = 0; index < this.SpriteLocalfilePathList.Count; index++)
            {
                // Create a new outer-nesting list entry.
                // This is a new sprite-sheet.
                CanvasBitmapList.Add(new List<CanvasBitmap>()); // Might need validation to avoid creating an extra entry.

                // Loop through the nested List<LIST>.
                for (index2 = 0; index2 < this.SpriteLocalfilePathList[index].Count; index2++) // Make sure I'm accessing the correct list nesting.
                {
                    // Load a bitmap graphic from an image file.
                    canvasBitmapObject = await Microsoft.Graphics.Canvas.CanvasBitmap.LoadAsync(sender, this.SpriteLocalfilePathList[index][index2]);

                    // Add the bitmap graphic to the NESTED List<LIST>.
                    CanvasBitmapList[index].Add(this.canvasBitmapObject); //CanvasBitmapObject
                }

                // Reset the check-index for frames within the inner-nesting collection.
                index2 = 0;
            }

            // At this point, the 'Controller' structure should initialise the sprite sheet indexes
            // for accessing the counts/limits of sprite aniamtions.
            Controllers.ResourceController.InitialiseSpritesheetFrameIndexes(mainPageRef);

            this.resourcesLoaded = true;
        }

        /// <summary>
        /// Add the needed file paths for the assets, to be subsequently loaded to the render-flow. 
        /// </summary>
        public void LoadBitmapFromFilepath()
        {
            int index = 0;
            this.SpriteLocalfilePathList.Add(new List<string>());

            void loadSprites()
            {
                this.SpriteLocalfilePathList[index].Add(AppDomain.CurrentDomain.BaseDirectory + "Assets\\" + "backgrounds\\" + "gg_0.png");
                this.SpriteLocalfilePathList.Add(new List<string>());
                index++;

                this.SpriteLocalfilePathList[index].Add(AppDomain.CurrentDomain.BaseDirectory + "Assets\\" + "HUD\\" + "parallax_fighter_uwp_title_screen.png");
                this.SpriteLocalfilePathList.Add(new List<string>());
                index++;

                this.SpriteLocalfilePathList[index].Add(AppDomain.CurrentDomain.BaseDirectory + "Assets\\" + "space\\" + "saturn_tilted.png");
                this.SpriteLocalfilePathList.Add(new List<string>());
                index++;
            }

            loadSprites();

            return;


            // Pipeline to load the sprite assets file-paths into memory.
            // One row on the outer-list represents a set of sprites.
            this.SpriteLocalfilePathList.Add(new List<string>());
            loadSprites();
            index++;


            void loadBlock()
            {
                // Loop to add elements into the nested List<LIST>.
                // Load the training stage background.
                for (int index8 = 0; index8 < (int)SpritesheetFrameCounts.MistTextureIndex; index8++)
                {
                    // Add the filepath into the list.
                    this.SpriteLocalfilePathList[index].Add(AppDomain.CurrentDomain.BaseDirectory + "Assets\\" + "HUD\\" + "reticule.png");
                }
            }
            void loadMainSpaceBG()
            {
                // Loop to add elements into the nested List<LIST>.
                // Load the training stage background.
                for (int spaceBGIndex = 0; spaceBGIndex < (int)SpritesheetFrameCounts.GameplayGrid; spaceBGIndex++)
                {
                    // Add the filepath into the list.
                    this.SpriteLocalfilePathList[index].Add(AppDomain.CurrentDomain.BaseDirectory + "Assets\\" + "backgrounds\\" + "starfield_1.png");
                }
            }
        }
    }
}
