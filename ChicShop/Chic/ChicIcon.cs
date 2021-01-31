using FModel.Creator.Bases;
using FModel.Creator.Icons;
using FModel.Creator.Rarities;
using FModel.Creator.Stats;
using FModel.Creator.Texts;
using FModel.PakReader.Parsers.Class;
using FModel.PakReader.Parsers.Objects;
using FModel.Utils;
using FModel.ViewModels.ImageBox;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.TextChicShopamespace FModel.Chic
{
    static class ChicIcon
    {
        public static bool GenerateIcon(IUExport export, string exportType, ref string assetName)
          => GenerateIcon(new BaseIcon(export, exportType, ref assetName), assetName);

        public static bool GenerateIcon(BaseIcon icon, string assetName)
        {
            int width = icon.Size + (icon.AdditionalSize > 0 ? (int)(icon.Size / 1.5f) : 0);
            using (var ret = new SKBitmap(width, icon.Size, SKColorType.Rgba8888, SKAlphaType.Premul))
            using (var c = new SKCanvas(ret))
            {
                icon.Margin = 0;

                FConsole.AppendText($"[CHIC MODEL DEBUG] - {width}", "#CE9BB5", true);

                //Background
                ChicRarity.DrawRarity(c, icon);

                //Draw item icon
                ChicImage.DrawPreviewImage(c, icon);

                // has more things to show
                if (width > icon.Size)
                {
                    ChicStatistics.DrawStats(c, icon);
                }

                //Draw Text Background
                ChicText.DrawBackground(c, icon);
                //Display Name
                ChicText.DrawDisplayName(c, icon);
                //Description
                ChicText.DrawDescription(c, icon);

                if (!icon.ShortDescription.Equals(icon.DisplayName) && !icon.ShortDescription.Equals(icon.Description))
                {
                    //Draw Item Type
                    ChicText.DrawToBottom(c, icon, ETextSide.Left, icon.ShortDescription);
                }

                string sourceText = icon.CosmeticSource switch
                {
                    "ItemShop" => "Item Shop",
                    "Granted.Founders" => "Founder's Pack",
                    _ => icon.CosmeticSource
                };

                if (sourceText.Contains("BattlePass.Paid"))
                {
                    string season = sourceText.Replace("Season", "").Replace(".BattlePass.Paid", "");
                    season = season == "10" ? "X" : season;

                    sourceText = $"Season {season} Battle Pass";
                }

                //Draw Source
                ChicText.DrawToBottom(c, icon, ETextSide.Right, sourceText);

                //Draw Flags
                ChicUserFacingFlags.DrawUserFacingFlags(c, icon);

                //Watermark
                //ChicWatermark.DrawWatermark(c, icon.Size, shadow: true); // watermark should only be applied on icons with width = 512
                //ChicWatermark.DrawWatermark(c, icon, 18, "@ChicIsCoolio", 2);
                ChicWatermark.DrawChicFace(c, SKColors.White, icon.Size - 120);

                //Shows the image
                ImageBoxVm.imageBoxViewModel.Set(ret, assetName);
            }

            return true;
        }
    }
}
