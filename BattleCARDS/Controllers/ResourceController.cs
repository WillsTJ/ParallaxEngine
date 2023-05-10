using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BattleCARDS.Controllers
{
    /// <summary>
    /// This class generally controls resources-flow and access throughout the program.
    /// Most class members should be accessible from this class.
    /// </summary>
    public class ResourceController
    {
        MainPage mainPageRef;

        public ResourceController(MainPage mainPage)
        {
            this.mainPageRef = mainPage;
        }

        /// <summary>
        /// Call an initialisation function to obtain sprite indexes to help for parsing sprite sheet animations.
        /// </summary>
        /// <param name="mainPageReference"></param>
        public static void InitialiseSpritesheetFrameIndexes(MainPage mainPageReference)
        {
            mainPageReference.draw.ObtainFrameIndexes();
        }

        public static class SerialiseAssets
        {
            public static string ConstructTextFromFile(string filepath)
            {
                string textData = string.Empty;

                return textData = (string)DeserialiseTextFile<string>(filepath);
            }

            public static void PassFrameFilePaths(List<List<string>> filepathsList)
            {
                SerialiseFilePaths<string>(filepathsList);
            }

            private static void SerialiseFilePaths<T>(List<List<string>> filepathsList)// where T : new() <-- this could be pretty useful.
            {
                TextWriter writer = null;
                try
                {
                    // Loop through each frame via nesting to serialise each frame into a discrete directory for the debug tool to read.
                    for (int outerNestingIndex = 0; outerNestingIndex < filepathsList.Count; outerNestingIndex++)
                    {
                        for (int innerNestingIndex = 0; innerNestingIndex < filepathsList[outerNestingIndex].Count; innerNestingIndex++)
                        {
                            var serialiser = new XmlSerializer(typeof(T));
                            writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Assets\\" + "\\debugAssets");
                            serialiser.Serialize(writer, filepathsList[outerNestingIndex][innerNestingIndex]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }

            private static T DeserialiseTextFile<T>(string filepath)// where T : new() <-- this could be pretty useful.
            {
                TextReader reader = null;

                try
                {
                    var serialiser = new XmlSerializer(typeof(T));
                    reader = new StreamReader(filepath);
                    return (T)serialiser.Deserialize(reader);

                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }
    }
}
