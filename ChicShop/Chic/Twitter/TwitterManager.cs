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
        static TwitterService Service;

        public static void Auth(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            Service = new TwitterService(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }

        public static void SendMediaTweet(string filePath, string status)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                Service.SendTweetWithMedia(new SendTweetWithMediaOptions
                {
                    Status = status,
                    Images = new Dictionary<string, Stream> { { filePath, stream } }
                });
            }
        }
    }
}
