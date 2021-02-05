using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.OAuth;
using System.Security.Cryptography;

namespace ChicShop.Chic.Twitter
{
    public class TwitterManager
    {
        static TwitterContext Context;

        public static void Auth()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = Environment.GetEnvironmentVariable("CONSUMERKEY"),
                    ConsumerSecret = Environment.GetEnvironmentVariable("CONSUMERSECRET"),
                    AccessToken = Environment.GetEnvironmentVariable("ACCESSTOKEN"),
                    AccessTokenSecret = Environment.GetEnvironmentVariable("ACCESSSECRET")
                }
            };

            Context = new TwitterContext(auth);
        }

        public static void SendMediaTweet(string filePath, string status)
        {
            if (Context == null) Auth();

            var media = Context.UploadMediaAsync(File.ReadAllBytes(filePath), "image/jpeg", "tweet_image").Result;
            Context.TweetAsync(status, new[] { media.MediaID });
        }
    }
}
