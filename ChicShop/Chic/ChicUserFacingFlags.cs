/*;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

naChicShope FModel.Chic
{
    static class ChicUserFacingFlags
    {
        public static void DrawUserFacingFlags(SKCanvas c, BaseIcon icon)
        {
            if (icon.UserFacingFlags != null)
            {
                int size = 25;
                int x = icon.Margin + 2;
                foreach (SKBitmap b in icon.UserFacingFlags)
                {
                    if (b == null) return;

                    c.DrawBitmap(b.Resize(size, size), new SKPoint(x, 2.5f), new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        Color = SKColors.Red,
                        ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 2, 2, SKColors.Black)
                    });
                    x += size + 1;
                }
 */          }
        }
    }
}
