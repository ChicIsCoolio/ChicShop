using Newtonsoft.Json;

namespace ChicShop.Chic.Shop
{
    public class FortniteApiData<T>
    {
        [JsonProperty("status")]
        public int Status;
        [JsonProperty("data")]
        public T Data;
    }
}
