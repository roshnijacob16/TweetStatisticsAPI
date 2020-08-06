
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using TweetStatisticsAPI.Models;

namespace TweetStatisticsAPI.Controllers
{
    public class TweetStatsController : ApiController
    {
        // GET: api/Tweetstatus
        [HttpGet]
        [Route("api/TweetStats/{tweetcount?}")]
        

        public async Task<IHttpActionResult> Get(int tweetcount=0)
        {
            TweetRepository tr = new TweetRepository(ConfigurationManager.AppSettings["ConsumerKey"], ConfigurationManager.AppSettings["ConsumerSecret"], ConfigurationManager.AppSettings["AccessToken"], ConfigurationManager.AppSettings["AccessTokenSecret"]);

            return Ok(await tr.GetTweetstats(tweetcount));
        }
    }
}
