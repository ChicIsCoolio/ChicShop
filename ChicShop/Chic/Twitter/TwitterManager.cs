using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace ChicShop.Chic.Twitter
{
    public class TwitterManager
    {
        static string ConsumerKey = Environment.GetEnvironmentVariable("CONSUMER-KEY");
        static string ConsumerSecret = Environment.GetEnvironmentVariable("CONSUMER-SECRET");
        static string AccessToken = Environment.GetEnvironmentVariable("ACCESS-TOKEN");
        static string AccessSecret = Environment.GetEnvironmentVariable("ACCESS-SECRET");

        public static void SendMediaTweet(string filePath, string status)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                new TwitterService(ConsumerKey, ConsumerSecret, AccessToken, AccessSecret)
                    .SendTweetWithMedia(new SendTweetWithMediaOptions
                {
                    Status = status,
                    Images = new Dictionary<string, Stream> { { filePath, stream } }
                });
            }
        }
    }
}
