using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Http;
using System.IO;
using Fortnite_API;
using Fortnite_API.Objects.V2;
using SkiaSharp;

namespace ChicShop
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            var api = new FortniteApi();

            var shop = api.V2.Shop.GetBr().Data;

            var watch = new Stopwatch();
            watch.Start();
            DrawItem(shop.Featured.Entries[0].Items[0]);
            watch.Stop();
            Console.WriteLine("Done in " + watch.Elapsed);

            await Task.Delay(-1);        
        }

        public SKBitmap DrawItem(BrCosmeticV2 item)
        {
            SKBitmap icon = new SKBitmap(1024, 1024);
            
            using (SKCanvas canvas = new SKCanvas(icon))
            {
                canvas.DrawRect(0, 0, icon.Width, icon.Height,
                    new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        Shader = SKShader.CreateRadialGradient(
                            new SKPoint(icon.Width / 2, icon.Height / 2),
                            icon.Width / 5 * 4,
                            new SKColor[] {
                                new SKColor(30, 30, 30),
                                new SKColor(50, 50, 50)
                            },
                            SKShaderTileMode.Clamp)
                    });

                using (var cosmetic = GetBitmapFromUrl(item.Images.Featured))
                {
                    canvas.DrawBitmap(cosmetic, 0, 0,
                        new SKPaint
                        {
                            IsAntialias = true,
                            FilterQuality = SKFilterQuality.High,
                            ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 5, 5, SKColors.Black)
                        });
                }

                var rarityPath = new SKPath { FillType = SKPathFillType.EvenOdd }
                rarityPath.MoveTo(0, icon.Height);
                rarityPath.LineTo(0, icon.Height - 75);
                rarityPath.LineTo(icon.Width, icon.Height - 85);
                rarityPath.LienTo(icon.WIdth, icon.Height);
                rarityPath.Close();

                SKRarityColor rarityColor
                {
                    
                }
            }

            using (var stream = File.OpenWrite(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.png")))
                using (var data = SKImage.FromBitmap(icon).Encode(SKEncodedImageFormat.Png, 100))
                {
                    data.SaveTo(stream);
                }

            return null;
        }

        public SKBitmap GetBitmapFromUrl(string url) => GetBitmapFromUrl(new Uri(url));
        public SKBitmap GetBitmapFromUrl(Uri url)
        {
            using (var client = new HttpClient())
            {
                var bytes = client.GetByteArrayAsync(url).Result;

                using (var stream = new MemoryStream(bytes))
                {
                    return SKBitmap.Decode(stream);
                }
            }
        }
    }
}
