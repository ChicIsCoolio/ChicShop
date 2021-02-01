using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using SkiaSharp;
using ChicShop.Chic;
using System.Collections.Generic;
using ChicShop.Chic.Shop;

namespace ChicShop
{
    public class Program
    {
        public static string Root = "/home/runner/ChicShop/";

        static void Main(string[]  args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            var shop = Shop.Get(Environment.GetEnvironmentVariable("API-KEY")).Data;

            var watch = new Stopwatch();
            watch.Start();

            Console.WriteLine(shop.ShopDate);

            Dictionary<StorefrontEntry, SKBitmap> entries = new Dictionary<StorefrontEntry, SKBitmap>();

            if (shop.HasFeatured) GenerateEntries(shop.Featured.Entries, ref entries);
            if (shop.HasDaily) GenerateEntries(shop.Daily.Entries, ref entries);
            if (shop.HasSpecialFeatured) GenerateEntries(shop.SpecialFeatured.Entries, ref entries);
            if (shop.HasSpecialDaily) GenerateEntries(shop.SpecialDaily.Entries, ref entries);

            List<string> sections = new List<string>();

            foreach (var entry in entries)
            {
                if (!sections.Contains(entry.Key.SectionId)) sections.Add(entry.Key.SectionId);

                /*using (entry.Value)
                {
                    using (var image = SKImage.FromBitmap(entry.Value))
                    {
                        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                        {
                            using (var stream = File.OpenWrite(Root + "Output/" + entry.Key.Items[0].Id + ".png"))
                            {
                                data.SaveTo(stream);
                            }
                        }
                    }
                }*/
            }

            foreach (var section in sections)
            {
                Console.WriteLine(section);
            }

            watch.Stop();
            Console.WriteLine("Done in " + watch.Elapsed);

            await Task.Delay(-1);        
        }

        public void GenerateEntries(StorefrontEntry[] entries, ref Dictionary<StorefrontEntry, SKBitmap> entr)
        {
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
                })
                {
                    ChicRarity.GetRarityColors(icon, item.Rarity.BackendValue);

                    entr.Add(entry, ChicIcon.GenerateIcon(icon));
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
