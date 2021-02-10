using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicShop.Chic.Shop.V2
{
    public class ShopV2
    {
        [JsonProperty("shopDate")]
        public DateTime ShopDate;
        [JsonProperty("expiration")]
        public DateTime Expiration;
        [JsonProperty("sections")]
        public Dictionary<string, List<ShopEntry>> Sections;
        [JsonProperty("sectionInfos")]
        public List<ShopSection> SectionInfos;

        public static ShopV2 Get() => Get(out Status s);

        public static ShopV2 Get(out Status status)
        {
            var client = new RestClient("https://ChicAPI.chiciscoolio.repl.co/api/v1");
            var request = new RestRequest("/shop/br");
            var response = JsonConvert.DeserializeObject<ChicResponse<ShopV2>>(client.Execute(request).Content);
            status = response.Status;
            return response.Data;
        }
    }

    public struct ChicResponse<T>
    {
        [JsonProperty("status")]
        public Status Status;
        [JsonProperty("data")]
        public T Data;

    }

    public enum Status
    {
        NOT_READY,
        OK
    }

    public struct ShopSection
    {
        [JsonProperty("displayName")]
        public string DisplayName;
        [JsonProperty("id")]
        public string SectionId;
        [JsonProperty("landingPriority")]
        public int LandingPriority;

        public bool HasName => !string.IsNullOrWhiteSpace(DisplayName);
    }

    public struct ShopEntry
    {
        [JsonProperty("offerId")]
        public string OfferId;
        [JsonProperty("offerType")]
        public string OfferType;
        [JsonProperty("currencyType")]
        public string CurrencyType;
        [JsonProperty("basePrice")]
        public int BasePrice;
        [JsonProperty("regularPrice")]
        public int RegularPrice;
        [JsonProperty("finalPrice")]
        public int FinalPrice;
        [JsonProperty("categories")]
        public string[] Categories;
        [JsonProperty("items")]
        public List<EntryItem> Items;
        [JsonProperty("bundle")]
        public EntryBundle Bundle;
        [JsonProperty("banner")]
        public EntryBanner Banner;
        [JsonProperty("sortPriority")]
        public int SortPriority;
        [JsonProperty("metaInfo")]
        public KeyValue[] MetaInfo;
        [JsonProperty("meta")]
        public Dictionary<string, string> Meta;

        public bool IsBundle => Bundle != null;
        public bool HasBanner => Banner != null;

        string cacheid;
        public string CacheId
        {
            get
            {
                if (!string.IsNullOrEmpty(cacheid)) return cacheid;

                cacheid = $"{FinalPrice}{IsBundle}{(HasBanner ? Banner.BackendValue : "")}";

                foreach (var item in Items)
                {
                    cacheid += item.Id;
                }

                return cacheid;
            }
        }
    }

    public struct KeyValue
    {
        [JsonProperty("key")]
        public string Key;
        [JsonProperty("value")]
        public string Value;
    }

    public struct EntryItem
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("quantity")]
        public int Quantity;
        [JsonProperty("icon")]
        public Uri Image;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("rarity")]
        public ItemValue Rarity;
        [JsonProperty("series")]
        public SeriesValue Series;
        [JsonProperty("type")]
        public ItemValue Type;
        [JsonProperty("shopHistory")]
        public DateTime[] ShopHistory;

        public bool HasSeries => Series != null;
    }
}
