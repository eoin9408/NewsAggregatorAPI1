using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorAPI.Models;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssFeedsController : ControllerBase
    {

        private readonly NewsContext _rssContext;


        public RssFeedsController(NewsContext context)
        {
            _rssContext = context;
        }


        // GET: api/RssFeeds/
        [HttpGet]
        //public async Task<ActionResult<IDictionary<string, string>>> GetRSSFeeds()
        public async Task<ActionResult<IEnumerable<RSSFeedDTO>>> GetRSSFeeds()
        {
            return await _rssContext.RSSFeeds.Select(x => RSSFeedToDTO(x)).ToListAsync();
        }


        // GET: api/RssFeeds/BBC
        [HttpGet("{id}")]
        public async Task<ActionResult<RSSFeedDTO>> GetRSSFeed(string id)
        {
            var rssFeed = await _rssContext.RSSFeeds.SingleOrDefaultAsync(x => x.ID.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (rssFeed == null)
            {
                return NotFound();
            }

            return Ok(RSSFeedToDTO(rssFeed));
        }


        // PUT: api/RssFeeds/CNN
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRSSFeed(string id, RSSFeedDTO rssFeedDTO)
        {
            var rssFeed = await _rssContext.RSSFeeds.FindAsync(id);
            if (rssFeed == null)
            {
                return NotFound();
            }

            rssFeed.ID = rssFeedDTO.ID;
            rssFeed.FeedName = rssFeedDTO.FeedName;
            rssFeed.FeedURL = rssFeedDTO.FeedURL;

            try
            {
                await _rssContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RSSFeedExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/RssFeeds
        [HttpPost]
        public async Task<ActionResult<RSSFeedDTO>> CreateRSSFeed(RSSFeedDTO rssFeedDTO)
        {

            var rssFeed = new RSSFeed
            {
                ID = rssFeedDTO.ID,
                FeedName = rssFeedDTO.FeedName,
                FeedURL = rssFeedDTO.FeedURL
            };

            _rssContext.RSSFeeds.Add(rssFeed);
            await _rssContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRSSFeed), new { ID = rssFeed.ID }, RSSFeedToDTO(rssFeed)); //ID is string, needs .equals check?
        }


        // DELETE: api/NewsItems/NYT
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRSSFeed(string id)
        {
            var rssFeed = await _rssContext.RSSFeeds.FindAsync(id);
            if (rssFeed == null)
            {
                return NotFound();
            }

            _rssContext.RSSFeeds.Remove(rssFeed);
            await _rssContext.SaveChangesAsync();

            return NoContent();
        }


        private bool RSSFeedExists(string id)
        {
            return (_rssContext.RSSFeeds?.Any(e => e.ID == id)).GetValueOrDefault();
        }


        private static RSSFeedDTO RSSFeedToDTO(RSSFeed rssFeed) =>
        new RSSFeedDTO
        {
            ID = rssFeed.ID,
            FeedName = rssFeed.FeedName,
            FeedURL = rssFeed.FeedURL
        };
    }
}
