using SkiaSharp;

namespace ChicShop.Chic
{
    public class ChicBanner
    {
        public static void DrawBanner(SKCanvas c, BaseIcon icon)
        {
            var f = (int)(ChicRatios.Get1024(50) * icon.Height);
            var tf = (int)(ChicRatios.Get1024(25) * icon.Height);
            var tt = (int)(ChicRatios.Get1024(23) * icon.Height);
            var t = (int)(ChicRatios.Get1024(20) * icon.Height);
            var et = (int)(ChicRatios.Get1024(18) * icon.Height);
            var ft = (int)(ChicRatios.Get1024(15) * icon.Height);
            var tw = (int)(ChicRatios.Get1024(12) * icon.Height);
            var n = (int)(ChicRatios.Get1024(9) * icon.Height);
            var s = (int)(ChicRatios.Get1024(7) * icon.Height);

            using (var textPaint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.Low,
                Color = SKColors.White,
                Typeface = ChicTypefaces.BurbankBigRegularBlack,
                TextAlign = SKTextAlign.Left,
                TextSize = f
            })
            {
                int width = (int)textPaint.MeasureText(icon.Banner);

                using (var path = new SKPath())
                {
                    path.FillType = SKPathFillType.EvenOdd;

                    path.MoveTo(ft, ft);
                    path.LineTo(width + f, tw);
                    path.LineTo(width + tf, textPaint.TextSize + tt);
                    path.LineTo(t, textPaint.TextSize + et);
                    path.Close();

                    using (var filter = SKImageFilter.CreateDropShadow(0, 0, s, s, SKColors.Black))
                        c.DrawPath(path,
                            new SKPaint
                            {
                                IsAntialias = true,
                                FilterQuality = SKFilterQuality.Low,
                                Color = SKColor.Parse("#f5112c"),
                                ImageFilter = filter
                            });
                }

                c.DrawText(icon.Banner, tf, textPaint.TextSize + n, textPaint);
            }
        }
    }
}