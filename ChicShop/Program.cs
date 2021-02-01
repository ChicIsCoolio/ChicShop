using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Http;
using System.IO;
using Fortnite_API;
using Fortnite_API.Objects.V2;
using SkiaSharp;
using ChicShop.Chic;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChicShop
{
    public class Program
    {
        public static string Root = "/home/runner/ChicShop/";

        static void Main(string[]  args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            var api = new FortniteApi();

            var shop = api.V2.Shop.GetBr().Data;

            var watch = new Stopwatch();
            watch.Start();

            Console.WriteLine(shop.Date);

            if (shop.HasFeatured) GenerateEntries(shop.Featured.Entries);
            if (shop.HasDaily) GenerateEntries(shop.Daily.Entries);
            if (shop.HasSpecialFeatured) GenerateEntries(shop.SpecialFeatured.Entries);
            if (shop.HasSpecialDaily) GenerateEntries(shop.SpecialDaily.Entries);

            watch.Stop();
            Console.WriteLine("Done in " + watch.Elapsed);

            await Task.Delay(-1);        
        }

        public Dictionary<BrShopV2StoreFrontEntry, SKBitmap> GenerateEntries(List<BrShopV2StoreFrontEntry> entries)
        {
            Dictionary<BrShopV2StoreFrontEntry, SKBitmap> dictionary = new Dictionary<BrShopV2StoreFrontEntry, SKBitmap>();

            foreach (var entry in entries)
            {
                var item = entry.Items[0];

                using (var icon = new BaseIcon
                {
                    DisplayName = item.Name,
                    ShortDescription = item.Type.DisplayValue,
                    Price = entry.FinalPrice,
                    IconImage = GetBitmapFromUrl(item.Images.Featured ?? item.Images.Icon ?? item.Images.SmallIcon),
                    RarityBackgroundImage = item.HasSeries && item.Series.Image != null ? GetBitmapFromUrl(item.Series.Image) : null,
                    Width = 768,
                    Height = 1024
                });
                {
                    ChicRarity.GetRarityColors(icon, item.Rarity.BackendValue);

                    dictionary.Add(entry, ChicIcon.Generateicon(icon));

                    /*using (var bitmap = ChicIcon.GenerateIcon(icon))
                    {
                        using (var image = SKImage.FromBitmap(bitmap))
                        {
                            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                            {
                                using (var stream = File.OpenWrite(Root + "Output/" + item.Id + ".png"))
                                {
                                    data.SaveTo(stream);
                                }
                            }
                        }
                    }*/
                }
            }
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
