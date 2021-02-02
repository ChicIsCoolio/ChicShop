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
        public string Banner;
        public int Price;
        public int Width = 1024;
        public int Height = 1024;

        public bool HasBanner => !string.IsNullOrWhiteSpace(Banner);

        public void Dispose()
        {
            if (IconImage != null) IconImage.Dispose();
            if (RarityBackgroundImage != null) RarityBackgroundImage.Dispose();

            DisplayName = "";
            ShortDescription = "";
            Banner = "";
            Price = 0;
            Width = 0;
            Height = 0;
        }
    }
}
