using System;
using System.Windows;
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
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Immutable;
using ChicShop.Chic.Twitter;

namespace ChicShop
{
    public class Program
    {
        public static string Root = "/home/runner/ChicShop/";

        public int EntryHeight { get; private set; } = 640;
        public int EntryWidth { get; private set; } = 480;
        
        static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            if (args.Contains(arg => arg.Contains("entryWidth=")))
                EntryHeight = int.Parse(args.First(arg => arg.Contains("entryHeight=")).Split('=')[1]);
            if (args.Contains(arg => arg.Contains("entryWidth=")))
                EntryWidth = int.Parse(args.First(arg => arg.Contains("entryWidth=")).Split('=')[1]);

            Console.WriteLine("Running");

            WebServer.Start();

            Console.WriteLine("Generate shop now?");
            if (Console.ReadLine() == "y") GenerateShop();

            var shop = Shop.Get(Environment.GetEnvironmentVariable("APIKEY")).Data;

            DateTimeOffset time = shop.ShopDate.AddDays(1).AddSeconds(10);
            shop = null;

            int generated = 0;

            Scheduler.Default.Schedule(time, reschedule =>
            {
                GenerateShop();
                Console.WriteLine("\"Generated\": " + time);
                Console.WriteLine($"Number: {++generated}");

                reschedule(time.AddDays(1));
            });

            await Task.Delay(-1);        
        }

        public void GenerateShop()
        {
            Directory.CreateDirectory($"{Root}Cache");

            Console.WriteLine("Generating Shop...");

            var watch = new Stopwatch();
            watch.Start();

            var shop = Shop.Get(Environment.GetEnvironmentVariable("API-KEY")).Data;
            DateTime date = shop.ShopDate;

            Dictionary<StorefrontEntry, BitmapData> entries = new Dictionary<StorefrontEntry, BitmapData>();

            if (shop.HasFeatured) GenerateEntries(shop.Featured.Entries, ref entries);
            if (shop.HasDaily) GenerateEntries(shop.Daily.Entries, ref entries);
            if (shop.HasSpecialFeatured) GenerateEntries(shop.SpecialFeatured.Entries, ref entries);
            if (shop.HasSpecialDaily) GenerateEntries(shop.SpecialDaily.Entries, ref entries);

            Dictionary<Section, BitmapData> bitmaps = GenerateSections(entries);
            List<Section> sections = bitmaps.Keys.ToList();

            sections.Sort(SectionComparer.Comparer);

            shop = null;
            entries = null;
            GC.Collect();

            using (var full = A(sections, bitmaps))
            {
                int w = (int)(full.Width / 1.5);
                int h = (int)(full.Height / 1.5);

                SaveToCache(full, "shop.png");
                sections = null;
                bitmaps = null;
                
                GC.Collect();

                using (var bmp = new SKBitmap(w, h))
                {
                    using (var c = new SKCanvas(bmp))
                    using (var b = LoadFromCache("shop.png"))
                    {
                        c.DrawBitmap(b, new SKRect(0, 0, w, h));
                    }

                    using (var image = SKImage.FromBitmap(bmp))
                    {
                        using (var dat = image.Encode(SKEncodedImageFormat.Jpeg, 100))
                        {
                            using (var stream = File.OpenWrite($"{Root}Output/{date.ToString("dd-MM-yyyy")}.jpg"))
                            {
                                dat.SaveTo(stream);
                            }
                        }
                    }
                }

                string suffix = (date.Day % 10 == 1 && date.Day != 11) ? "st"
                    : (date.Day % 10 == 2 && date.Day != 12) ? "nd"
                    : (date.Day % 10 == 3 && date.Day != 13) ? "rd"
                    : "th";

                string status = $"Fortnite Item Shop\n{string.Format("{0:dddd}, {0: d}{1} {0:MMMM yyyy}", date, suffix)}\n\nIf you want to support me,\nconsider using my code 'Chic'\n\n#Ad";

                TwitterManager.SendMediaTweet($"{Root}Output/{date.ToString("dd-MM-yyyy")}.jpg", status);
            }

            watch.Stop();
            Console.WriteLine($"Done in {watch.Elapsed} ms");

            Directory.Delete($"{Root}Cache", true);
            watch = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public SKBitmap A(List<Section> sections, Dictionary<Section, BitmapData> bitmaps)
        {
            int width = 0;
            int height = 250;

            bitmaps.Values.ToList().ForEach(b =>
            {
                if (b.Width > width) width = b.Width;

                height += b.Height;
            });

            var merge = new SKBitmap(width, height);

            using (var c = new SKCanvas(merge))
            {
                int y = 0;

                using (var paint = new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    Color = new SKColor(40, 40, 40)
                }) c.DrawRect(0, 0, merge.Width, merge.Height, paint);

                sections.ToList().ForEach(section =>
                {
                    var bitmapData = bitmaps.First(predicate: x => x.Key.SectionId == section.SectionId).Value;

                    using (var bitmap = LoadFromCache(bitmapData))
                    {
                        c.DrawBitmap(bitmap, 0, y);
                        y += bitmap.Height;
                    }

                    GC.Collect();
;                });

                using (var textPaint = new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    TextSize = (int)(ChicRatios.Get1024(150) * EntryHeight),
                    Color = SKColors.White,
                    Typeface = ChicTypefaces.BurbankBigRegularBlack,
                    ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 10, 10, SKColors.Black)
                })
                {
                    var sacText = "Code 'Chic'\n#Ad";
                    int textWidth = (int)textPaint.MeasureText(sacText);

                    int left = (merge.Width - textWidth) / 2;
                    int top = merge.Height - (int)(ChicRatios.Get1024(150) * EntryHeight * 2);
                    int right = left + textWidth;
                    int bottom = merge.Height - (int)(ChicRatios.Get1024(150) * EntryHeight);

                    ChicText.DrawMultilineText(c, sacText, new SKRect(left, top, right, bottom), textPaint);

                    sacText = "";
                }
            }

            sections = null;
            bitmaps = null;

            return merge;
        }

        /*public SKBitmap B(List<Section> sections, Dictionary<Section, SKBitmap> bitmaps)
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

            var full = new SKBitmap(fullWidth, fullHeight);
            
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
        }*/

        public Dictionary<Section, BitmapData> GenerateSections(Dictionary<StorefrontEntry, BitmapData> entries)
        {
            int maxEntriesPerRow = 6;

            Dictionary<Section, BitmapData> sectionsBitmaps = new Dictionary<Section, BitmapData>();

            var content = FortniteContent.Get();

            List<Section> sections = new List<Section>();

            entries.Keys.ToList().ForEach(key =>
            {
                if (!sections.Contains(x => x.SectionId == key.SectionId))
                {
                    if (content.ShopSections.Contains(key.SectionId))
                    {
                        sections.Add(content.ShopSections.Get(key.SectionId));
                    }
                    else
                    {
                        sections.Add(new Section
                        {
                            SectionId = key.SectionId,
                            DisplayName = "",
                            LandingPriority = 49
                        });
                    }
                }
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

            int th = (int)(ChicRatios.Get1024(200) * EntryHeight);
            int hf = (int)(ChicRatios.Get1024(150) * EntryHeight);
            int h = (int)(ChicRatios.Get1024(100) * EntryHeight);
            int f = (int)(ChicRatios.Get1024(50) * EntryHeight);

            foreach (var section in sections)
            {
                Dictionary<StorefrontEntry, BitmapData> se = new Dictionary<StorefrontEntry, BitmapData>();

                entries.ToList().ForEach(entry =>
                {
                    if (entry.Key.SectionId == section.SectionId) se.Add(entry);
                });

                Dictionary<StorefrontEntry, BitmapData> sectionEntries = new Dictionary<StorefrontEntry, BitmapData>();
                sectionEntries.Add(se.ToList().Sort(EntryComparer.Comparer, 0));

                int extraHeight = section.HasName ? th : 0;

                SKBitmap bitmap = new SKBitmap(h/*150*/ + maxEntriesPerRow * (EntryWidth + h /*+ 50*/), extraHeight + (int)Math.Ceiling((decimal)sectionEntries.Count / maxEntriesPerRow) * (EntryHeight + h /*+ 50*/));
            
                using (SKCanvas c = new SKCanvas(bitmap))
                {
                    using (var paint = new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        Color = new SKColor(40, 40, 40)
                    }) c.DrawRect(0, 0, bitmap.Width, bitmap.Height, paint);

                    int x = f;//100;
                    int y = extraHeight;
                    int row = 0;
                    int column = 0;

                    for (int i = 0; i < sectionEntries.Count; i++)
                    {
                        var entry = sectionEntries.ToList()[i];

                        using (var b = LoadFromCache(entry.Value))
                        {
                            using (var paint = new SKPaint { IsAntialias = true, FilterQuality = SKFilterQuality.High })
                                c.DrawBitmap(b, x, y, paint);
                        }

                        GC.Collect();

                        if (++column == maxEntriesPerRow)
                        {
                            row++;
                            column = 0;
                            //x = 50;//100;
                        }

                        x = f + (EntryWidth + h) * column; /*+ 50*/
                        y = extraHeight + (EntryHeight + h /*+ 50*/) * row;
                    }

                    using (var paint = new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        Color = SKColors.White,
                        TextSize = h,
                        Typeface = ChicTypefaces.BurbankBigRegularBlack,
                        TextAlign = SKTextAlign.Left,
                        ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 10, 10, SKColors.Black)
                    }) if (section.HasName)
                        c.DrawText(section.DisplayName, h, hf, paint);
                }

                sectionEntries = null;

                sectionsBitmaps.Add(section, SaveToCache(bitmap, $"{Root}/Cache/{section.SectionId}.png"));
            }

            content = null;
            sections = null;
            entries = null;

            GC.Collect();

            return sectionsBitmaps;
        }

        public void GenerateEntries(StorefrontEntry[] entries, ref Dictionary<StorefrontEntry, BitmapData> entr)
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
                    IconImage = GetBitmapFromUrl(entry.IsBundle ? entry.Bundle.Image : item.Images.Featured ?? item.Images.Icon ?? item.Images.SmallIcon, $"icon_{item.Id}{(entry.IsBundle ? "_Bundle.png" : ".png")}"),
                    RarityBackgroundImage = item.HasSeries && item.Series.Image != null ? GetBitmapFromUrl(item.Series.Image, item.Series.BackendValue + ".png") : null,
                    Width = EntryWidth,
                    Height = EntryHeight
                })
                {
                    ChicRarity.GetRarityColors(icon, item.Rarity.BackendValue);

                    entr.Add(entry, SaveToCache(ChicIcon.GenerateIcon(icon), $"{Root}Cache/{item.Id}{(entry.IsBundle ? "_Bundle" : "")}.png"));
                }
            }
        }

        public BitmapData SaveToCache(SKBitmap bitmap, string path, bool dispose = true)
        {
            BitmapData d = new BitmapData
            {
                Path = path,
                Width = bitmap.Width,
                Height = bitmap.Height
            };

            using (var image = SKImage.FromBitmap(bitmap))
            {
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (var stream = File.OpenWrite(path))
                    {
                        data.SaveTo(stream);
                    }
                }
            }

            if (dispose) bitmap.Dispose();

            GC.Collect();

            return d;
        }

        public SKBitmap LoadFromCache(BitmapData data) => LoadFromCache(data.Path);
        public SKBitmap LoadFromCache(string path) => SKBitmap.Decode(path);

        public bool IsInCache(string path) => File.Exists(path);

        public SKBitmap GetBitmapFromUrl(string url, string name = "noname.png") => GetBitmapFromUrl(new Uri(url), name);
        public SKBitmap GetBitmapFromUrl(Uri url, string name = "noname.png")
        {
            if (IsInCache($"{Root}Cache/{name}")) return LoadFromCache($"{Root}Cache/{name}");

            Console.WriteLine($"{Root}Cache/{name}");

            using (var client = new HttpClient())
            {
                var bytes = client.GetByteArrayAsync(url).Result;

                using (var stream = new MemoryStream(bytes))
                {
                    SKBitmap bitmap = SKBitmap.Decode(stream);

                    SaveToCache(bitmap, $"{Root}Cache/{name}", false);
                    return bitmap;
                }
            }
        }
    }
}
