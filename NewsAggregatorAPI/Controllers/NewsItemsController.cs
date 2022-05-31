using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorAPI.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAggregatorAPI.Controllers
{
    //[Route("api/[controller]/[action]/{id?}")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsItemsController : ControllerBase
    {
        private readonly NewsContext _context;

        public NewsItemsController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/NewsItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsItemDTO>>> GetNewsItems()
        {
          if (_context.NewsItems == null)
          {
              return NotFound();
          }
            return await _context.NewsItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/NewsItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsItemDTO>> GetNewsItem(long id)
        {
            var newsItem = await _context.NewsItems.FindAsync(id);

            if (newsItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(newsItem);
        }

        // GET : api/NewsItems/rss
        [HttpGet("rss")]
        public async Task<IEnumerable<NewsItemDTO>> GetRSSFeedAsync()
        {
            string url = "http://feeds.bbci.co.uk/news/rss.xml";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach (SyndicationItem item in feed.Items)
            {
                NewsItem listNewsItem = new NewsItem();
                listNewsItem.ArticleTitle = item.Title.Text;
                listNewsItem.ArticleSummary = item.Summary.Text;
                listNewsItem.ArticleDateTime = item.PublishDate.DateTime;
                _context.NewsItems.Add(listNewsItem);
                //String summary = item.Summary.Text;

                //await CreateNewsItem(listNewsItem);
            }
            _context.SaveChanges();
            return await _context.NewsItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // PUT: api/NewsItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewsItem(long id, NewsItemDTO newsItemDTO)
        {
            if (id != newsItemDTO.Id)
            {
                return BadRequest();
            }

            //_context.Entry(newsItemDTO).State = EntityState.Modified;

            var newsItem = await _context.NewsItems.FindAsync(id);
            if (newsItem == null) {
                return NotFound();
            }

            newsItem.ArticleTitle = newsItemDTO.ArticleTitle;
            newsItem.ArticleDateTime = newsItemDTO.ArticleDateTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsItemExists(id))
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

        // POST: api/NewsItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewsItemDTO>> CreateNewsItem(NewsItemDTO newsItemDTO)
        {

            var newsItem = new NewsItem
            {
                ArticleTitle = newsItemDTO.ArticleTitle,
                ArticleDateTime = newsItemDTO.ArticleDateTime
            };

          if (_context.NewsItems == null)
          {
              return Problem("Entity set 'NewsContext.NewsItems'  is null.");
          }
            _context.NewsItems.Add(newsItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetNewsItem", new { id = newsItem.Id }, newsItem);
            return CreatedAtAction(nameof(GetNewsItem), new { id = newsItem.Id }, ItemToDTO(newsItem));
        }

        // DELETE: api/NewsItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsItem(long id)
        {
            if (_context.NewsItems == null)
            {
                return NotFound();
            }
            var newsItem = await _context.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            _context.NewsItems.Remove(newsItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsItemExists(long id)
        {
            return (_context.NewsItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static NewsItemDTO ItemToDTO(NewsItem newsItem) =>
            new NewsItemDTO
            {
                Id = newsItem.Id,
                ArticleTitle = newsItem.ArticleTitle,
                ArticleSummary = newsItem.ArticleSummary,
                ArticleDateTime = newsItem.ArticleDateTime
            };
    }
}
