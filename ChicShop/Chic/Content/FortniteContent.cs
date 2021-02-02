using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicShop.Chic.Content
{
    public class FortniteContent
    {
        [JsonProperty("shopSections")]
        public ShopSections ShopSections;

        public static FortniteContent Get()
        {
            var client = new RestClient("https://fortnitecontent-website-prod07.ol.epicgames.com/content/api/pages/fortnite-game");
            var request = new RestRequest(Method.GET);

            return JsonConvert.DeserializeObject<FortniteContent>(client.Execute(request).Content);
        }
    }

    public class ShopSections
    {
        [JsonProperty("sectionList")]
        public SectionList SectionList;

        public bool Contains(string id)
            => SectionList.Sections.Contains(predicate: x => x.SectionId == id);

        public Section Get(string id)
            => SectionList.Sections.First(predicate: x => x.SectionId == id);
    }

    public class SectionList
    {
        [JsonProperty("sections")]
        public List<Section> Sections;
    }
    
    public class Section
    {
        [JsonProperty("background")]
        public Background Background;
        [JsonProperty("landingPriority")]
        public int LandingPriority;
        [JsonProperty("sectionId")]
        public string SectionId;
        [JsonProperty("bShowTimer")]
        public bool ShowTimer;
        [JsonProperty("sectionDisplayName")]
        public string DisplayName;

        public bool HasName => !string.IsNullOrEmpty(DisplayName);
    }

    public class SectionComparer : IComparer<Section>
    {
        public static SectionComparer Comparer { get; } = new SectionComparer();

        public int Compare(Section x, Section y)
        {
            string xId = x.SectionId;
            string yId = y.SectionId;


            if (xId.Contains("Featured") || yId.Contains("Featured"))
            {
                if (xId == "Featured") return -1;
                if (yId == "Featured") return 1;

                int.TryParse(xId.Replace("Featured", ""), out int xFeatured);
                int.TryParse(yId.Replace("Featured", ""), out int yFeatured);

                if (xFeatured > yFeatured) return -1;
                if (yFeatured > xFeatured) return 1;
            }

            if (xId.Contains("Daily") || yId.Contains("Daily"))
            {
                if (xId.Contains("Featured")) return -1;
                if (yId.Contains("Featured")) return 1;

                if (xId == "Daily") return -1;
                if (yId == "Daily") return 1;

                int.TryParse(xId.Replace("Daily", ""), out int xDaily);
                int.TryParse(yId.Replace("Daily", ""), out int yDaily);

                if (xDaily > yDaily) return -1;
                if (yDaily > xDaily) return 1;
            }

            return x.LandingPriority > y.LandingPriority ? 1 : x.LandingPriority < y.LandingPriority ? 
                -1 : 0;
        }
    }

    public class Background
    {
        [JsonProperty("stage")]
        public string Stage;
        [JsonProperty("key")]
        public string Key;
    }
}
