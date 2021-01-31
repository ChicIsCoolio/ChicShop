using System;
using System.Threading.Tasks;
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

            await Task.Delay(-1);        }

        public SKBitmap DrawItem(BrCosmeticV2 item)
        {
            using (var cosmetic = GetBitmapFromUrl(item.Images.Featured))
            {
                using (var stream = File.OpenWrite(Path.GetDirectoryName(Assembly.GetExecitingAssembly().CodeBase)))
                SKImage.FromBitmap(cosmetic).Encode(SKEncodedImageFormat.Png, 100).SaveTo()
            }
        }

        public SKBitmap GetBitmapFromUrl(string url) => GetBitmapFromUri(new Uri(url));
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
