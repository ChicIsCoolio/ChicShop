using SkiaSharp;
using System;

namespace ChicShop.Chic
{
    static class ChicImage
    {
        public static void DrawPreviewImage(SKCanvas c, BaseIcon icon)
        {
            int x = (icon.Width - icon.Height) / 2;
            int y = 0;

            /*Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(icon.IconImage.Width);
            Console.WriteLine(icon.IconImage.Height);
            Console.WriteLine(icon.Width);
            Console.WriteLine(icon.Height);*/

            using (var filter = SKImageFilter.CreateDropShadow(0, 0, icon.Height * ChicRatios.Get(5), icon.Height * ChicRatios.Get(5), SKColors.Black))
            using (var paint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.Low,
                ImageFilter = filter
            }) c.DrawBitmap(icon.IconImage, new SKRect(x, y, x + icon.Height, y + icon.Height), paint);
        }
    }
}
