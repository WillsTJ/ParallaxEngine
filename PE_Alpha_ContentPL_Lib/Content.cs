using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using Windows.Foundation;

namespace PE_Alpha_ContentPL_Lib
{
    public class Content
    {
        public void LoadRectContent(List<Rect> rectList)
        {
            for (int index = 0; index < rectList.Count; index++)
            {
                // Store the rect list via serialisation onto a meta-data file.
                this.SaveMetaData(rectList);
            }
        }

        public async void SaveMetaData(List<Rect> rectList)
        {
            // Create a new file directory to store credentials, or open the existing directory if it exists.
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile file = await storageFolder.CreateFileAsync("RectListMetaData.txt",
            Windows.Storage.CreationCollisionOption.OpenIfExists);

            for (int index = 0; index < rectList.Count; index++)
            {
                // Save the URL and password crednetials.
                await Windows.Storage.FileIO.WriteTextAsync(file, rectList[index].ToString());
            }
        }
    }
}
