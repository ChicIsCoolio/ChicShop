using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Reactive.Concurrency;
using SkiaSharp;
using ChicShop.Chic;
using System.Collections.Generic;
using ChicShop.Chic.Shop;
using ChicShop.Chic.Content;
using System.Linq;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using ChicShop.Chic.WebServer;

namespace ChicShop
{
    public class Program
    {
        public static string Root = "/home/runner/ChicShop/";
        public WebServer Server;

        static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            Server = new WebServer();
            Server.Start();

            var shop = Shop.Get(Environment.GetEnvironmentVariable("API-KEY")).Data;

            DateTimeOffset time = shop.ShopDate.AddDays(1);

            Scheduler.Default.Schedule(DateTimeOffset.Now.AddSeconds(5), reschedule =>
            {
                GenerateShop();

                reschedule(time.AddSeconds(5));
            });

            await Task.Delay(-1);        
        }

        public void GenerateShop()
        {
            Console.WriteLine("Generating Shop...");
            var watch = new Stopwatch();
            watch.Start();

            var shop = Shop.Get(Environment.GetEnvironmentVariable("API-KEY")).Data;

            Dictionary<StorefrontEntry, SKBitmap> entries = new Dictionary<StorefrontEntry, SKBitmap>();

            if (shop.HasFeatured) GenerateEntries(shop.Featured.Entries, ref entries);
            if (shop.HasDaily) GenerateEntries(shop.Daily.Entries, ref entries);
            if (shop.HasSpecialFeatured) GenerateEntries(shop.SpecialFeatured.Entries, ref entries);
            if (shop.HasSpecialDaily) GenerateEntries(shop.SpecialDaily.Entries, ref entries);

            Dictionary<Section, SKBitmap> bitmaps = GenerateSections(entries);
            List<Section> sections = bitmaps.Keys.ToList();

            sections.Sort(SectionComparer.Comparer);

            using (var full = A(sections, bitmaps))
            {
                using (var data = SKImage.FromBitmap(full).Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (var stream = File.OpenWrite(Root + $"Output//{shop.ShopDate.ToString("dd-MM-yyyy")}.png"))
                    {
                        data.SaveTo(stream);
                    }
                }
            }

            watch.Stop();
            Console.WriteLine($"Done in {watch.ElapsedMilliseconds} ms");
        }

        public SKBitmap A(List<Section> sections, Dictionary<Section, SKBitmap> bitmaps)
        {
            int width = 0;
            int height = 250;

            bitmaps.Values.ToList().ForEach(b =>
            {
                if (b.Width > width) width = b.Width;

                height += b.Height;
            });

            /*using (*/
            var merge = new SKBitmap(width, height);//)
            {
                using (var c =new SKCanvas(merge))
                {
                    int y = 0;

                    c.DrawRect(0, 0, merge.Width, merge.Height,
                            new SKPaint
                            {
                                IsAntialias = true,
                                FilterQuality = SKFilterQuality.High,
                                Color = new SKColor(40, 40, 40)
                            });

                    sections.ToList().ForEach(section =>
                    {
                        var bitmap = bitmaps.First(predicate: x => x.Key.SectionId == section.SectionId).Value;

                        c.DrawBitmap(bitmap, 0, y);
                        y += bitmap.Height;
                    });

                    var textPaint = new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        TextSize = 200,
                        Color = SKColors.White,
                        Typeface = ChicTypefaces.BurbankBigRegularBlack,
                        ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 10, 10, SKColors.Black)
                    };

                    var sacText = "Code 'Chic' #Ad";

                    c.DrawText(sacText, (merge.Width - textPaint.MeasureText(sacText)) / 2,
                        merge.Height - 50, textPaint);
                }

                return merge;
            }
        }

        public SKBitmap B(List<Section> sections, Dictionary<Section, SKBitmap> bitmaps)
        {
            List<int> featured = new List<int>();
            List<int> others = new List<int>();

            int featuredCount = 0;

            foreach (var section in sections)
            {
                if (section.SectionId.Contains("Featured"))
                {
                    featured.Add(bitmaps.First(predicate: x => x.Key.SectionId == section.SectionId).Value.Width);
                    featuredCount++;
                    continue;
                }
                else //if (section.SectionId.Contains("Daily"))
                {
                    others.Add(bitmaps.First(predicate: x => x.Key.SectionId == section.SectionId).Value.Width);
                    continue;
                }
            };

            int columnCount = featured.Count > 0 ? 2 : 1;

            int width = columnCount * featured.Max();
            int featuredHeight = 0;
            int othersHeight = 0;

            bitmaps.ToList().ForEach(pair =>
            {
                if (pair.Key.SectionId.Contains("Featured")) featuredHeight += pair.Value.Height;
                else othersHeight += pair.Value.Height;
            });

            int height = Math.Max(featuredHeight, othersHeight) + 250;

            int ratio = 0;

            int fullHeight = 0;
            int fullWidth = 0;

            if (height >= width) ratio = height / 9;
            else ratio = width / 16;

            fullHeight = ratio * 9;
            fullWidth = ratio * 16;

            /*using (*/
            var full = new SKBitmap(fullWidth, fullHeight);//)
            {
                using (var merge = new SKBitmap(width, height))
                {
                    using (var c = new SKCanvas(merge))
                    {
                        int y = 0;
                        int x = 0;
                        int i = 0;

                        sections.ToList().ForEach(section =>
                        {
                            var bitmap = bitmaps.First(predicate: x => x.Key.SectionId == section.SectionId).Value;

                            c.DrawBitmap(bitmap, x, y);
                            y += bitmap.Height;
                            i++;

                            if (i == featuredCount)
                            {
                                y = 0;
                                x += width / columnCount;
                            }
                        });
                    }

                    using (var c = new SKCanvas(full))
                    {
                        c.DrawRect(0, 0, full.Width, full.Height,
                            new SKPaint
                            {
                                IsAntialias = true,
                                FilterQuality = SKFilterQuality.High,
                                Color = new SKColor(40, 40, 40)
                            });

                        c.DrawBitmap(merge, (full.Width - merge.Width) / 2, 0,
                            new SKPaint
                            {
                                IsAntialias = true,
                                FilterQuality = SKFilterQuality.High
                            });

                        var textPaint = new SKPaint
                        {
                            IsAntialias = true,
                            FilterQuality = SKFilterQuality.High,
                            TextSize = 200,
                            Color = SKColors.White,
                            Typeface = ChicTypefaces.BurbankBigRegularBlack,
                            ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 10, 10, SKColors.Black)
                        };

                        var sacText = "Code 'Chic' #Ad";

                        c.DrawText(sacText, (full.Width - textPaint.MeasureText(sacText)) / 2,
                            full.Height - 50, textPaint);
                    }

                    return full;
                }
            }
        }

        public Dictionary<Section, SKBitmap> GenerateSections(Dictionary<StorefrontEntry, SKBitmap> entries)
        {
            int maxEntriesPerRow = 6;

            Dictionary<Section, SKBitmap> sectionsBitmaps = new Dictionary<Section, SKBitmap>();

            var content = FortniteContent.Get();

            List<Section> sections = new List<Section>();

            entries.Keys.ToList().ForEach(key =>
            {
                if (!sections.Contains(x => x.SectionId == key.SectionId
                    && content.ShopSections.Contains(key.SectionId)))
                    sections.Add(content.ShopSections.Get(key.SectionId));
            });

            List<int> entriesCount = new List<int>();

            sections.ForEach(section =>
            {
                int count = 0;

                entries.ToList().ForEach(entry =>
                {
                    if (entry.Key.SectionId == section.SectionId) count++;
                });

                entriesCount.Add(count);
            });

            maxEntriesPerRow = Math.Clamp(entriesCount.Max(), 0, maxEntriesPerRow);

            foreach (var section in sections)
            {
                Dictionary<StorefrontEntry, SKBitmap> sectionEntries = new Dictionary<StorefrontEntry, SKBitmap>();

                entries.ToList().ForEach(entry =>
                {
                    if (entry.Key.SectionId == section.SectionId) sectionEntries.Add(entry);
                });

                int extraHeight = section.HasName ? 200 : 0;

                SKBitmap bitmap = new SKBitmap(100/*150*/ + maxEntriesPerRow * (768 + 100 /*+ 50*/), extraHeight + (int)Math.Ceiling((decimal)sectionEntries.Count / maxEntriesPerRow) * (1024 + 100 /*+ 50*/));
            
                using (SKCanvas c = new SKCanvas(bitmap))
                {
                    c.DrawRect(0, 0, bitmap.Width, bitmap.Height,
                        new SKPaint
                        {
                            IsAntialias = true,
                            FilterQuality = SKFilterQuality.High,
                            Color = new SKColor(40, 40, 40)
                        });

                    int x = 50;//100;
                    int y = extraHeight;
                    int row = 0;

                    for (int i = 0; i < sectionEntries.Count; i++)
                    {
                        var entry = sectionEntries.ToList()[i];

                        c.DrawBitmap(entry.Value, x, y,
                            new SKPaint
                            {
                                IsAntialias = true,
                                FilterQuality = SKFilterQuality.High,
                            });

                        x += 768 + 100 /*+ 50*/;

                        if (i + 1 >= row + 1 * maxEntriesPerRow)
                        {
                            row++;
                            x = 50;//100;
                        }

                        y = extraHeight + (1024 + 100 /*+ 50*/) * row;
                    }

                    if (section.HasName)
                        c.DrawText(section.DisplayName, 100, 150,
                        new SKPaint
                        {
                            IsAntialias = true,
                            FilterQuality = SKFilterQuality.High,
                            Color = SKColors.White,
                            TextSize = 100,
                            Typeface = ChicTypefaces.BurbankBigRegularBlack,
                            TextAlign = SKTextAlign.Left,
                            ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 10, 10, SKColors.Black)
                        });
                }

                sectionsBitmaps.Add(section, bitmap);
            }

            return sectionsBitmaps;
        }

        public void GenerateEntries(StorefrontEntry[] entries, ref Dictionary<StorefrontEntry, SKBitmap> entr)
        {
            foreach (var entry in entries)
            {
                var item = entry.Items[0];

                using (var icon = new BaseIcon
                {
                    DisplayName = entry.IsBundle ? entry.Bundle.Name.ToLowerAndUpper() : item.Name,
                    ShortDescription = (entry.IsBundle ? entry.Bundle.Info : item.Type.DisplayValue).ToUpper(),
                    Banner = entry.HasBanner ? entry.Banner.Value.ToUpper() : "",
                    Price = entry.FinalPrice,
                    IconImage = GetBitmapFromUrl(entry.IsBundle ? entry.Bundle.Image : item.Images.Featured ?? item.Images.Icon ?? item.Images.SmallIcon),
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
