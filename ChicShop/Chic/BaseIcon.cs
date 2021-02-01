using SkiaSharp;
using System;

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

            DisplayName = "";
            ShortDescription = "";
            Price = 0;
            Width = 0;
            Height = 0;
        }
    }
}
