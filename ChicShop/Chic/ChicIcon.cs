﻿using SkiaSharp;
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
            var bitmap = new SKBitmap(icon.Width, icon.Height, SKColorType.Rgba8888, SKAlphaType.Premul);

            using (var c = new SKCanvas(bitmap))
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

                //Draw Flags
                //ChicUserFacingFlags.DrawUserFacingFlags(c, icon);

                //Watermark
                //ChicWatermark.DrawWatermark(c, icon.Size, shadow: true); // watermark should only be applied on icons with width = 512
                //ChicWatermark.DrawWatermark(c, icon, 18, "@ChicIsCoolio", 2);
                //ChicWatermark.DrawChicFace(c, SKColors.White, icon.Size - 120);

                //Shows the image
                //ImageBoxVm.imageBoxViewModel.Set(ret, assetName);
            }

            return bitmap;
        }
    }
}
