using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BattleCARDS.Model
{
    public class ContentPipeline
    {

        public List<Rect> contentRectList = new List<Rect>();

        public ContentPipeline()
        {

        }

        public void PopulateContentPipelineRects(Rect rect)
        {
            contentRectList.Add(rect);
        }

    }
}
