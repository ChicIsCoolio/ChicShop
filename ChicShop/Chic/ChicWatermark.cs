/*
using SkiaSharp;
using System;
using System.IO;
using System.Collections.Generic;
using SysmpleConverteChicShopnamespace FModel.Chic
{
    static class ChicWatermark
    {
        public static void DrawWatermark(SKCanvas c, IBase icon, int size, string text, float offset, int opacity = 255, bool shadow = false, int sigma = 2)
        {
            SKPaint paint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High,
                IsAntialias = true,
                Typeface = Text.TypeFaces.BottomDefaultTypeface,
                Color = SKColors.Transparent.WithAlpha((byte)opacity),
                TextSize = size,
                ImageFilter = shadow ? SKImageFilter.CreateDropShadow(0, 0, sigma, sigma, SKColors.Black) : null
            };

            float width = paint.MeasureText(text);
            float x = icon.Width - width - offset;
            float y = offset + size * 0.8f;

            c.DrawText(text, x, y, paint);
        }

        public static void DrawWatermark(SKCanvas c, int width, int sizePercent = 4, bool shadow = false)
        {
            if (Settings.Default.UseIconWatermark && !string.IsNullOrEmpty(Settings.Default.IconWatermarkPath))
            {
                using SKBitmap watermarkBase = SKBitmap.Decode(new FileInfo(Settings.Default.IconWatermarkPath).Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                int sizeX = (int)(watermarkBase.Width * GetMultiplier(width, watermarkBase, sizePercent));
                int sizeY = (int)(watermarkBase.Height * GetMultiplier(width, watermarkBase, sizePercent));
                SKBitmap watermark = watermarkBase.Resize(sizeX, sizeY);

                float left = width - watermark.Width - 2;
                float top = 2;
                float right = left + watermark.Width;
                float bottom = top + watermark.Height;
                c.DrawBitmap(watermark, new SKRect(left, top, right, bottom),
                    new SKPaint
                    {
                        FilterQuality = SKFilterQuality.High,
                        IsAntialias = true,
                        Color = SKColors.Transparent.WithAlpha((byte)Settings.Default.IconWatermarkOpacity),
                        ImageFilter = shadow ? SKImageFilter.CreateDropShadow(0, 0, 2, 2, SKColors.Black) : null
                    });
            }

            static float GetMultiplier(int width, SKBitmap watermark, int size)
            {
                return (float)width / size / watermark.Width;
            }
        }

        public static void DrawChicFace(SKCanvas c, SKColor color, int x, int size = 128)
        {
            SKPath path = SKPath.ParseSvgPathData(ChicData.ChicHeadPath);

            using (SKBitmap b = new SKBitmap(320, 320))
            {
                using (SKCanvas ca = new SKCanvas(b))
                {
                    ca.DrawPath(path,
                        new SKPaint
                        {
                            IsAntialias = true,
                            FilterQuality = SKFilterQuality.High,
                            Color = color
                        });
                }

                c.DrawBitmap(size == 320 ? b : b.Resize(size, size), x, 0,
                    new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        ImageFilter = SKImageFilter.CreateDrophadow(0, 0, 5, 5, SKColors.Black)
                    });
            }
        }
    }
}
*/