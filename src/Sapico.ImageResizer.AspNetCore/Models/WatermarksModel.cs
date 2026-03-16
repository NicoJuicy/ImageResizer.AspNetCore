using System;
using System.Collections.Generic;
using System.Text;

namespace Sapico.ImageResizer.Models
{
    public class WatermarksModel
    {
        public IEnumerable<WatermarkTextModel> WatermarkTextList { get; set; } = Array.Empty<WatermarkTextModel>();
        public IEnumerable<WatermarkImageModel> WatermarkImageList { get; set; } = Array.Empty<WatermarkImageModel>();
    }




}
