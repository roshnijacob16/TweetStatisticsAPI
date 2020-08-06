using System;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Tweetinvi;
using Tweetinvi.Models;



namespace TweetStatisticsAPI.Models
{
    public class TweetRepository :ITweetRepository
    {
       
            public string ConsumerKey;
            public string ConsumerSecret;
            public string AccessToken;
            public string AccessTokenSecret;
           int rank = 0;
        
            public TweetRepository(string ConsumerKey, string ConsumerSecret, string AccessToken, string AccessTokenSecret)
            {
                this.ConsumerKey = ConsumerKey;
                this.ConsumerSecret = ConsumerSecret;
                this.AccessToken = AccessToken;
                this.AccessTokenSecret = AccessTokenSecret;
            }

        public async Task<TweetStats> GetTweetstats(int tweetcount) //Getting TweetStatistics 
        {
            Auth.SetUserCredentials(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
            var user = User.GetAuthenticatedUser();

            long elapsedsec = 0;
            double avetweetperms = 0, piccount = 0, URLcount = 0;
            int count = 0;
            TweetStats tweetstats = new TweetStats();
            List<string> tweets = new List<string>();
            List<string> hashtagList = new List<string>();
            List<string> urlList = new List<string>();
            List<long> tlist = new List<long>();

            var stream = Stream.CreateSampleStream();
            stream.AddTweetLanguageFilter(LanguageFilter.English);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (tweetcount == 0) { tweetcount = 100; } // default tweetcount set to 100;

            stream.TweetReceived += (sender, arguments) =>
            {
                
                elapsedsec = sw.ElapsedMilliseconds- elapsedsec;
                tlist.Add(elapsedsec);
               
                count++;

                if (count == tweetcount)
                {
                    stream.StopStream();
                    sw.Stop();
                }
                   
                    //progress.Report(count);
                if (arguments.Tweet.Hashtags.Count > 0)
                {
                    foreach (var item in arguments.Tweet.Hashtags)
                    {
                        hashtagList.Add(item.Text);
                    }
                }

                if (arguments.Tweet.Urls.Count > 0)
                {
                    foreach (var item in arguments.Tweet.Urls)
                    {
                        string strUrl = item.URL;
                        Uri uri = new Uri(strUrl);
                        strUrl = uri.Host;
                        urlList.Add(strUrl);
                        URLcount++;
                    }
                }
                if (arguments.Tweet.Entities.Medias.Count>0)
                {
                    foreach (var item in arguments.Tweet.Entities.Medias)
                    {
                        if (item.MediaURL.Contains("pic.twitter.com") || (item.MediaURL.Contains("instagram")))
                        {  
                            piccount++;
                        }
                           
                    }
                }            
               
           };
            await stream.StartStreamAsync();

            if (tlist.Count() > 0)
            {
                avetweetperms = 1/(tlist.Average());//average tweets per millisec
            }
                
            tweetstats =  await Processtweet(hashtagList, urlList,  count, piccount, URLcount, avetweetperms);
            return tweetstats;

            }
            private async Task<TweetStats> Processtweet(List<string> hashtagList, List<string> urlList,  int count, double piccount, double URLcount,double avecount)
            {
                Dictionary<string, int> hashdict =
                       new Dictionary<string, int>();
                TweetStats ts = new TweetStats();

                ts.TotalTweetsCount = count;
                ts.AveTweetsPersec = avecount * 1000;
                ts.AveTweetsPerMin = avecount * 60 * 60;
                ts.AveTweetsPerHr = avecount * 60 * 60 * 60;
                ts.TopHashtags = new List<string>();
                ts.TopHashtags.AddRange(FindTopRank(hashtagList));
                ts.TotalOccurenceofTopHashTag = rank;
                ts.TopDomainURLs = new List<string>();
                ts.TopDomainURLs.AddRange(FindTopRank(urlList));
                ts.TotalOccurenceofTopDomainURL = rank;
                ts.PercentOfURL = (URLcount / count) * 100 + " %";
                ts.PercentOfPicURL = (piccount / count) * 100 + " %";
               
                return await Task.FromResult(ts);
            }

            private List<string> FindTopRank(List<string> tempList)
            {

                Dictionary<string, int> hashdict =
                       new Dictionary<string, int>();
                int hcount = 1; int maxvalue = 1;

                foreach (var item in tempList)
                {
                    if (hashdict.ContainsKey(item))
                    {
                        hcount = hashdict[item];
                        hcount++;
                        hashdict[item] = hcount;
                    }
                    else
                    {
                        hcount = 1;
                        hashdict.Add(item, hcount);
                    }
                }
                if (hcount > maxvalue)
                    maxvalue = hcount;

                tempList.Clear();
                foreach (var item in hashdict)
                {
                    if (item.Value == maxvalue)

                        tempList.Add(item.Key);
                }
                rank = maxvalue;
                return tempList;

            }


       
    }
}

