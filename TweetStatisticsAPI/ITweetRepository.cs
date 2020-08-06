using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TweetStatisticsAPI.Models;

namespace TweetStatisticsAPI
{
    public interface ITweetRepository
    {
      Task<TweetStats> GetTweetstats(int tweetcount);
    }
}
