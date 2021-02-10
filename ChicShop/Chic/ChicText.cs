using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChicShop.Chic
{
    static class ChicText
    {
        private static float STARTER_POSITION_RATIO { get; } = ChicRatios.Get(380);
        private static float BOTTOM_TEXT_SIZE_RATIO { get; } = ChicRatios.Get(15);
        private static float NAME_TEXT_SIZE_RATIO { get; } = ChicRatios.Get(47);

        public static void DrawBackground(SKCanvas c, BaseIcon icon)
        {
            /*var pathTop = new SKPath { FillType = SKPathFillType.EvenOdd };
            pathTop.MoveTo(icon.Margin, icon.Margin);
            pathTop.LineTo(icon.Margin + icon.Width, icon.Margin);
            pathTop.LineTo(icon.Margin + icon.Width, icon.Margin + 20);
            pathTop.LineTo(icon.Margin, icon.Margin + 30);
            pathTop.Close();
            c.DrawPath(pathTop, new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Color = new SKColor(30, 30, 30), //new SKColor(206, 155, 181),
                ImageFilter = SKImageFilter.CreateDropShadow(0, 3, 5, 5, SKColors.Black, null, new SKImageFilter.CropRect(SKRect.Create(icon.Margin, icon.Margin, icon.Width - icon.Margin, icon.Height - icon.Margin)))
            });*/

            using (var rarityPath = new SKPath { FillType = SKPathFillType.EvenOdd })
            {
                rarityPath.MoveTo(0, icon.Height);
                rarityPath.LineTo(0, icon.Height - icon.Height * ChicRatios.Get(75));
                rarityPath.LineTo(icon.Width, icon.Height - icon.Height * ChicRatios.Get(85));
                rarityPath.LineTo(icon.Width, icon.Height);
                rarityPath.Close();

                SKColor rarityColor = icon.RarityColors[0];

                using (var filter = SKImageFilter.CreateDropShadow(0, -3, icon.Height * ChicRatios.Get(5), icon.Height * ChicRatios.Get(5), SKColors.Black))
                using (var paint = new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    Color = rarityColor,
                    ImageFilter = filter
                }) c.DrawPath(rarityPath, paint);
            }

            using (var pathBottom = new SKPath { FillType = SKPathFillType.EvenOdd })
            {
                pathBottom.MoveTo(0, icon.Height);
                pathBottom.LineTo(0, icon.Height - icon.Height * ChicRatios.Get(65));
                pathBottom.LineTo(icon.Width, icon.Height - icon.Height * ChicRatios.Get(75));
                pathBottom.LineTo(icon.Width, icon.Height);
                pathBottom.Close();
                c.DrawPath(pathBottom, new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    Color = new SKColor(30, 30, 30)
                });
            }

            using (var pathBottomBottom = new SKPath { FillType = SKPathFillType.EvenOdd })
            {
                pathBottomBottom.MoveTo(0, icon.Height);
                pathBottomBottom.LineTo(0, icon.Height - icon.Height * ChicRatios.Get(22));
                pathBottomBottom.LineTo(icon.Width, icon.Height - icon.Height * ChicRatios.Get(32));
                pathBottomBottom.LineTo(icon.Width, icon.Height);
                pathBottomBottom.Close();
                c.DrawPath(pathBottomBottom, new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    Color = new SKColor(20, 20, 20)
                });
            }
        }

        public static void DrawDisplayName(SKCanvas c, BaseIcon icon)
        {
            string text = icon.DisplayName;

            if (string.IsNullOrEmpty(text)) return;

            int x = (int)(icon.Height * ChicRatios.Get(5));
            int y = (int)(icon.Height * STARTER_POSITION_RATIO + icon.Height * NAME_TEXT_SIZE_RATIO);

            using (var namePaint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Typeface = ChicTypefaces.BurbankBigCondensedBold,
                TextSize = icon.Height * NAME_TEXT_SIZE_RATIO,
                Color = SKColors.White,
                //TextAlign = SKTextAlign.Left,
                ImageFilter = SKImageFilter.CreateDropShadow(0, 0, icon.Height * 0.009765625f, icon.Height * 0.009765625f, SKColors.Black)
            })
            {

                while (namePaint.MeasureText(text) > icon.Width - x * 2)
                {
                    namePaint.TextSize--;
                }

                c.DrawText(text, x, y, namePaint);
            }
        }

        public static void DrawBundleInfo(SKCanvas c, BaseIcon icon)
        {
            if (string.IsNullOrEmpty(icon.BundleInfo)) return;

            int x = (int)(icon.Height * ChicRatios.Get(5));
            int y = (int)(icon.Height * STARTER_POSITION_RATIO + icon.Height * NAME_TEXT_SIZE_RATIO);

            using (var namePaint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Typeface = ChicTypefaces.BurbankBigCondensedBold,
                TextSize = icon.Height * NAME_TEXT_SIZE_RATIO,
                Color = SKColors.White,
                //TextAlign = SKTextAlign.Left,
                ImageFilter = SKImageFilter.CreateDropShadow(0, 0, icon.Height * 0.009765625f, icon.Height * 0.009765625f, SKColors.Black)
            })
            {

                while (namePaint.MeasureText(icon.DisplayName) > icon.Width - x * 10)
                {
                    namePaint.TextSize--;
                }

                x += x * 2 + (int)namePaint.MeasureText(icon.DisplayName);

                using (var paint = new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    Typeface = ChicTypefaces.BurbankBigCondensedBold,
                    TextSize = icon.Height * ChicRatios.Get(23),
                    Color = SKColors.White,
                    ImageFilter = SKImageFilter.CreateDropShadow(0, 0, icon.Height * ChicRatios.Get(5), icon.Height * ChicRatios.Get(5), SKColors.Black)
                })
                {
                    c.DrawText(icon.BundleInfo, x, y, paint);
                }
            }
        }

        public static void DrawToBottom(SKCanvas c, BaseIcon icon, SKTextAlign align, string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Typeface = ChicTypefaces.BurbankBigRegularBlack,
                TextSize = icon.Height * (align == SKTextAlign.Left ? BOTTOM_TEXT_SIZE_RATIO : ChicRatios.Get(30)),
                Color = SKColors.White,
                TextAlign = align
            })
            {

                var five = (int)(icon.Height * ChicRatios.Get(5));

                if (align == SKTextAlign.Left) c.DrawText(text, five, icon.Height - five, paint);
                else c.DrawText(text, icon.Width - five - (int)(icon.Height * ChicRatios.Get(27.5f)), icon.Height - five, paint);
            }
        }

        public static void DrawMultilineText(SKCanvas c, string text, SKRect area, SKPaint paint)
        {
            float lineHeight = paint.TextSize * 1.2f;
            var lines = SplitLines(text, paint, area.Width);
            var height = lines.Count() * lineHeight;

            var y = area.MidY - height / 2;

            foreach (var line in lines)
            {
                y += lineHeight;
                var x = area.MidX - line.Width / 2;
                c.DrawText(line.Value, x, y, paint);
            }
        }
        private class Line
        {
            public string Value { get; set; }

            public float Width { get; set; }
        }

        private static Line[] SplitLines(string text, SKPaint paint, float maxWidth)
        {
            var spaceWidth = paint.MeasureText(" ");
            var lines = text.Split('\n');

            return lines.SelectMany((line) =>
            {
                var result = new List<Line>();

                var words = line.Split(new[] { " " }, StringSplitOptions.None);

                var lineResult = new StringBuilder();
                float width = 0;
                foreach (var word in words)
                {
                    var wordWidth = paint.MeasureText(word);
                    var wordWithSpaceWidth = wordWidth + spaceWidth;
                    var wordWithSpace = word + " ";

                    if (width + wordWidth > maxWidth)
                    {
                        result.Add(new Line() { Value = lineResult.ToString(), Width = width });
                        lineResult = new StringBuilder(wordWithSpace);
                        width = wordWithSpaceWidth;
                    }
                    else
                    {
                        lineResult.Append(wordWithSpace);
                        width += wordWithSpaceWidth;
                    }
                }

                result.Add(new Line() { Value = lineResult.ToString(), Width = width });

                return result.ToArray();
            }).ToArray();
        }

        public static void DrawVbuck(SKCanvas c, BaseIcon icon)
        {
            var two = (int)(icon.Height * ChicRatios.Get(2));
            var vbuckSize = (int)(icon.Height * ChicRatios.Get(27.5f));

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
            }) c.DrawBitmap(SKBitmap.Decode(Program.Root + "Resources/T-VBuck-128.png"),
                new SKRect(icon.Width - vbuckSize - two, icon.Height - vbuckSize - two, icon.Width - two, icon.Height - two), paint);
        }
    }
}
