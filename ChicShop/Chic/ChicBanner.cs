using SkiaSharp;

namespace ChicShop.Chic
{
    public class ChicBanner
    {
        public static void DrawBanner(SKCanvas c, BaseIcon icon)
        {
            using (var textPaint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Color = SKColors.White,
                Typeface = ChicTypefaces.BurbankBigRegularBlack,
                TextAlign = SKTextAlign.Left,
                TextSize = 50
            })
            {
                int width = (int)textPaint.MeasureText(icon.Banner);

                using (var path = new SKPath())
                {
                    path.FillType = SKPathFillType.EvenOdd;

                    path.MoveTo(15, 15);
                    path.LineTo(width + 50, 12);
                    path.LineTo(width + 25, textPaint.TextSize + 23);
                    path.LineTo(20, textPaint.TextSize + 18);
                    path.Close();

                    using (var filter = SKImageFilter.CreateDropShadow(0, 0, 7, 7, SKColors.Black))
                        c.DrawPath(path,
                            new SKPaint
                            {
                                IsAntialias = true,
                                FilterQuality = SKFilterQuality.High,
                                Color = SKColor.Parse("#f5112c"),
                                ImageFilter = filter
                            });
                }

                c.DrawText(icon.Banner, 25, textPaint.TextSize + 9, textPaint);
            }
        }
    }
}