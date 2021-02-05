using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ChicShop.Chic
{
    static class ChicIcon
    {
        public static SKBitmap GenerateIcon(BaseIcon icon)
        {
            int h = (int)(ChicRatios.Get1024(100) * icon.Height);

            var bitmap = new SKBitmap(icon.Width + h, icon.Height + h);

            using (var bmp = new SKBitmap(icon.Width, icon.Height, SKColorType.Rgba8888, SKAlphaType.Premul))
            {
                using (var c = new SKCanvas(bmp))
                {
                    //#CE9BB5

                    //Background
                    ChicRarity.DrawRarity(c, icon);

                    //Draw item icon
                    ChicImage.DrawPreviewImage(c, icon);

                    //Draw Text Background
                    ChicText.DrawBackground(c, icon);
                    //Display Name
                    ChicText.DrawDisplayName(c, icon);

                    if (!icon.ShortDescription.Equals(icon.DisplayName))
                    {
                        //Draw Item Type
                        ChicText.DrawToBottom(c, icon, SKTextAlign.Left, icon.ShortDescription.ToUpper());
                    }

                    ChicText.DrawVbuck(c, icon);

                    {
                        string priceText = icon.Price > 0 ? icon.Price.ToString("N0", CultureInfo.GetCultureInfo("en-US")) : "FREE";

                        ChicText.DrawToBottom(c, icon, SKTextAlign.Right, priceText);
                    }

                    //Watermark
                    //ChicWatermark.DrawWatermark(c, icon.Size, shadow: true); // watermark should only be applied on icons with width = 512
                    //ChicWatermark.DrawWatermark(c, icon, 18, "@ChicIsCoolio", 2);
                    //ChicWatermark.DrawChicFace(c, SKColors.White, icon.Size - 120);

                    //Shows the image
                    //ImageBoxVm.imageBoxViewModel.Set(ret, assetName);
                }

                using (var c = new SKCanvas(bitmap))
                {
                    int f = (int)(ChicRatios.Get1024(50) * icon.Height);
                    int t = (int)(ChicRatios.Get1024(20) * icon.Height);

                    using (var paint = new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.Low,
                        ImageFilter = SKImageFilter.CreateDropShadow(0, 0, t, t, SKColors.Black)
                    }) c.DrawBitmap(bmp, f, f, paint);

                    if (icon.HasBanner) ChicBanner.DrawBanner(c, icon);
                }
            }



            return bitmap;
        }
    }
}
