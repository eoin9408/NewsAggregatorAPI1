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
        private readonly NewsContext _newsContext;
        private readonly NewsItemsService _newsService;

        public NewsItemsController(NewsContext context, NewsItemsService service)
        {
            _newsContext = context;
            _newsService = service;
        }

        // GET: api/NewsItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsItemDTO>>> GetNewsItems()
        {
            return await _newsContext.NewsItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/NewsItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsItemDTO>> GetNewsItem(long id)
        {
            var newsItem = await _newsContext.NewsItems.FindAsync(id);

            if (newsItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(newsItem);
        }

        // GET : api/NewsItems/rss/BBC
        [HttpGet("rss/{id}")]
        public async Task <ActionResult<IEnumerable<NewsItemDTO>>> GetRSSFeed(string id)
        {

            var newsItems = await _newsService.GetFeedNewsItems(id);

            return Ok(newsItems.Select(x => ItemToDTO(x)));
        }

        // PUT: api/NewsItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewsItem(long id, NewsItemDTO newsItemDTO)
        {
            if (id != newsItemDTO.Id)
            {
                return BadRequest();
            }

            //_context.Entry(newsItemDTO).State = EntityState.Modified;

            var newsItem = await _newsContext.NewsItems.FindAsync(id);
            if (newsItem == null) {
                return NotFound();
            }

            newsItem.ArticleTitle = newsItemDTO.ArticleTitle;
            newsItem.ArticleDateTime = newsItemDTO.ArticleDateTime;

            try
            {
                await _newsContext.SaveChangesAsync();
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
        [HttpPost]
        public async Task<ActionResult<NewsItemDTO>> CreateNewsItem(NewsItemDTO newsItemDTO)
        {

            var newsItem = new NewsItem
            {
                ArticleTitle = newsItemDTO.ArticleTitle,
                ArticleDateTime = newsItemDTO.ArticleDateTime
            };

            _newsContext.NewsItems.Add(newsItem);
            await _newsContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNewsItem), new { id = newsItem.Id }, ItemToDTO(newsItem));
        }

        [HttpPost("rss/{id}")]
        public async Task<ActionResult> PopulateRSSFeed(string id)
        {

            await _newsService.PopulateNewsFeedItems(id);

            return Ok();
        }

        // DELETE: api/NewsItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsItem(long id)
        {
            var newsItem = await _newsContext.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            _newsContext.NewsItems.Remove(newsItem);
            await _newsContext.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsItemExists(long id)
        {
            return (_newsContext.NewsItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static NewsItemDTO ItemToDTO(NewsItem newsItem) =>
        new NewsItemDTO
        {
            Id = newsItem.Id,
            ArticlePublisher = newsItem.ArticlePublisher,
            PublisherID = newsItem.PublisherID,
            ArticleTitle = newsItem.ArticleTitle,
            ArticleSummary = newsItem.ArticleSummary,
            ArticleDateTime = newsItem.ArticleDateTime
        };
    }
}
