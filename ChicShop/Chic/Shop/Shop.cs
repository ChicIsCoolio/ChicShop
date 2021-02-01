using Newtonsoft.Json;
using RestSharp;
using System;

namespace ChicShop.Chic.Shop
{
    public class Shop
    {
        [JsonProperty("hash")]
        public string Hash;
        [JsonProperty("date")]
        public DateTime ShopDate;
        [JsonProperty("featured")]
        public ShopStorefront Featured;
        [JsonProperty("daily")]
        public ShopStorefront Daily;
        [JsonProperty("specialFeatured")]
        public ShopStorefront SpecialFeatured;
        [JsonProperty("specialDaily")]
        public ShopStorefront SpecialDaily;
        [JsonProperty("votes")]
        public ShopStorefront Votes;
        [JsonProperty("voteWinners")]
        public ShopStorefront VoteWinners;

        public bool HasFeatured => Featured != null;
        public bool HasDaily => Daily != null;
        public bool HasSpecialFeatured => SpecialFeatured != null;
        public bool HasSpecialDaily => SpecialDaily != null;
        public bool HasVotes => Votes != null;
        public bool HasVoteWinners => VoteWinners != null;

        public static FortniteApiData<Shop> Get(string apiKey = "")
        {
            var client = new RestClient("https://fortnite-api.com/v2/shop/br");
            var request = new RestRequest(Method.GET)
                .AddHeader("x-api-key", apiKey);

            return JsonConvert.DeserializeObject<FortniteApiData<Shop>>(client.Execute(request).Content);
        }
    }

    public class ShopStorefront
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("entries")]
        public StorefrontEntry[] Entries;
    }

    public class StorefrontEntry
    {
        [JsonProperty("regularPrice")]
        public int RegularPrice;
        [JsonProperty("finalPrice")]
        public int FinalPrice;
        [JsonProperty("bundle")]
        public EntryBundle Bundle;
        [JsonProperty("banner")]
        public EntryBanner Banner;
        [JsonProperty("giftable")]
        public bool Giftable;
        [JsonProperty("refundable")]
        public bool Refundable;
        [JsonProperty("sortPriority")]
        public int SortPriority;
        [JsonProperty("categories")]
        public string[] Categories;
        [JsonProperty("sectionId")]
        public string SectionId;
        [JsonProperty("devName")]
        public string DevName;
        [JsonProperty("offerId")]
        public string OfferId;
        [JsonProperty("displayAssetPath")]
        public string DisplayAssetPath;
        [JsonProperty("newDisplayAssetPath")]
        public string NewDisplayAssetPath;
        [JsonProperty("items")]
        public EntryItem[] Items;
    }

    public class EntryBundle
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("image")]
        public Uri Image;
    }

    public class EntryBanner
    {
        [JsonProperty("value")]
        public string Value;
        [JsonProperty("backendValue")]
        public string BackendValue;
    }

    public class EntryItem
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("type")]
        public ItemValue Type;
        [JsonProperty("rarity")]
        public ItemValue Rarity;
        [JsonProperty("series")]
        public SeriesValue Series;
        [JsonProperty("images")]
        public ImageValue Images;

        public bool HasSeries => Series != null;
    }
    
    public class ItemValue
    {
        [JsonProperty("value")]
        public string Value;
        [JsonProperty("displayValue")]
        public string DisplayValue;
        [JsonProperty("backendValue")]
        public string BackendValue;
    }

    public class SeriesValue
    {
        [JsonProperty("value")]
        public string Value;
        [JsonProperty("image")]
        public Uri Image;
        [JsonProperty("backendValue")]
        public string BackendValue;
    }

    public class ImageValue
    {
        [JsonProperty("smallIcon")]
        public Uri SmallIcon;
        [JsonProperty("icon")]
        public Uri Icon;
        [JsonProperty("featured")]
        public Uri Featured;
        [JsonProperty("other")]
        public Uri Other;
    }
}
