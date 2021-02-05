using SkiaSharp;
using System;
using System.Diagnostics;

namespace ChicShop.Chic
{
    public enum EFortRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    static class ChicRarity
    {
        public static void GetRarityColors(BaseIcon icon, string rarity)
        {
            switch (rarity)
            {
                case "EFortRarity::Common":
                case "EFortRarity::Handmade":
                default:
                    icon.RarityColors = new SKColor[2] { SKColor.Parse("6D6D6D"), SKColor.Parse("333333") };
                    break;
                case "EFortRarity::Uncommon":
                    icon.RarityColors = new SKColor[2] { SKColor.Parse("5EBC36"), SKColor.Parse("305C15") };
                    break;
                case "EFortRarity::Rare":
                case "EFortRarity::Sturdy":
                    icon.RarityColors = new SKColor[2] { SKColor.Parse("3669BB"), SKColor.Parse("133254") };
                    break;
                case "EFortRarity::Epic":
                case "EFortRarity::Quality":
                    icon.RarityColors = new SKColor[2] { SKColor.Parse("8138C2"), SKColor.Parse("35155C") };
                    break;
                case "EFortRarity::Legendary":
                case "EFortRarity::Fine":
                    icon.RarityColors = new SKColor[2] { SKColor.Parse("C06A38"), SKColor.Parse("5C2814") };
                    break;
            }
        }

        public static void DrawRarity(SKCanvas c, BaseIcon icon)
        {
            if (icon.RarityBackgroundImage != null)
            {
                int size = Math.Max(icon.Width, icon.Height);
                int x = (icon.Width - size) / 2;
                int y = (icon.Height - size) / 2;

                using (var paint = new SKPaint { FilterQuality = SKFilterQuality.Low, IsAntialias = true })
                    c.DrawBitmap(icon.RarityBackgroundImage, new SKRect(x, y, size, size), paint);
            }
            else
            {
                using (var shader = SKShader.CreateRadialGradient(
                            new SKPoint(icon.Width / 2, icon.Height / 2),
                            icon.Width / 5 * 4,
                            new SKColor[] {
                                new SKColor(30, 30, 30),
                                new SKColor(50, 50, 50)
                            },
                            SKShaderTileMode.Clamp))
                using (var paint = new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.Low,
                    Shader = shader
                }) c.DrawRect(new SKRect(0, 0, icon.Width, icon.Height), paint);
            }
        }
    }
}
