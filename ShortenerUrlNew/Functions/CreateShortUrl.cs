using Microsoft.AspNetCore.Http;
using System;

namespace ShortenerUrlNew.Functions
{
    public class CreateShortUrl
    {
        private static int sizeUrl = 5;
        public static string Create(string hostName, string control_action_path = "")
        {
            string shortUrl = CreateRandomString.RandomString(sizeUrl);

            return "https://" + hostName + control_action_path + shortUrl;
        }
    }
}
