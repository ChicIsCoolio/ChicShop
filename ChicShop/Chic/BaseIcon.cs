using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicShop.Chic
{
    public class BaseIcon : IDisposable
    {
        public SKBitmap IconImage;
        public SKBitmap RarityBackgroundImage;
        public SKColor[] RarityColors;
        public string DisplayName;
        public string ShortDescription;
        public int Price;
        public int Width = 1024;
        public int Height = 1024;

        public void Dispose()
        {
            IconImage.Dispose();
            RarityBackgroundImage.Dispose();
        }
    }
}
