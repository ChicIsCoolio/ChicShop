using SkiaSharp;
using System;
using System.Dynamic;

namespace ChicShop.Chic
{
    static class ChicImage
    {
        public static void DrawPreviewImage(SKCanvas c, BaseIcon icon)
        {
            int x = (icon.Width - icon.Height) / 2;
            int y = 0;

            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(icon.IconImage.Width);
            Console.WriteLine(icon.IconImage.Height);
            Console.WriteLine(icon.Width);
            Console.WriteLine(icon.Height);

            c.DrawBitmap(icon.IconImage, new SKRect(x, y, x + icon.Height, y + icon.Height),
                new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    ImageFilter = SKImageFilter.CreateDropShadow(0, 0, icon.Height * 0.009765625f, icon.Height * 0.009765625f, SKColors.Black)
                });
        }
            /*c.DrawBitmap(icon.IconImage, icon.Width - icon.IconImage.Width, icon.Height - icon.IconImage.Height
                new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 5, 5, SKColors.Black)
                });*/
    }
}
