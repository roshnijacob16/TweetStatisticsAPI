using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TweetStatisticsAPI.Models
{
    public class TweetStats

    {
        
        ///Total Tweets Received
        public int TotalTweetsCount { get; set; } = 0;

        ///Average Tweets Received
        public Double AveTweetsPersec { get; set; } = 0;
        public Double AveTweetsPerMin { get; set; } = 0;

        public Double AveTweetsPerHr { get; set; } = 0;

        ///Percent of tweets that contain a URL
        public string PercentOfURL { get; set; } = "";


        ///Percent of tweets that contain a photo URL
        public string PercentOfPicURL { get; set; } = "";

        ///Top domains of URLs in tweets
        public List<string> TopDomainURLs { get; set; } = null;

        ///Total Occurence of Top domains of URLs in tweets
        public int TotalOccurenceofTopDomainURL  {    get; set; } = 0;

        ///Top domains of URLs in tweets
        public List<string> TopHashtags { get; set; } = null;

        ///Total Occurence of Top Hashtags in tweets
        public int TotalOccurenceofTopHashTag {  get; set; } = 0;
    }
}