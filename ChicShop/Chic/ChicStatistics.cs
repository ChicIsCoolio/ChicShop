using FModel.Creator;
using FModel.Creator.Bases;
using FModel.Creator.Stats;
using FModel.Creator.Texts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

ChicShopace FModel.Chic
{
    public class ChicStatistics
    {
        public static void DrawStats(SKCanvas c, BaseIcon icon)
        {
            int size = 70;
            int iconSize = 60;
            int textSize = 60;
            int y = 0;
            int margin = 2;

            var statPaint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Typeface = Text.TypeFaces.DisplayNameTypeface,
                TextSize = textSize,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Left,
                ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 5, 5, SKColors.Black)
            };

            c.DrawRect(icon.Size, 0, icon.Size / 1.5f, icon.Size,
                new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    Color = new SKColor(30, 30, 30),
                    ImageFilter = SKImageFilter.CreateDropShadow(-3, 0, 5, 5, SKColors.Black)
                });

            foreach (Statistic stat in icon.Stats)
            {
                if (stat.Icon != null) c.DrawBitmap(stat.Icon.Resize(iconSize, iconSize), icon.Size + margin * 2, y + 4,
                    new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 5, 5, SKColors.Black)
                    });

                while (statPaint.MeasureText(stat.Description) > (icon.Size / 1.5 - margin * 4 - iconSize - 4))
                {
                    statPaint.TextSize--;
                }

                c.DrawText(stat.Description, icon.Size + margin * 4 + iconSize, y + 4 + statPaint.TextSize / 2 + statPaint.TextSize / 3, statPaint);
                statPaint.TextSize = textSize;

                y += size;
            }
        }
    }
}
