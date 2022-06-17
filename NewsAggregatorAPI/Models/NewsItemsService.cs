using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAggregatorAPI.Models
{
    public class NewsItemsService
    {
        private readonly NewsContext _newsContext;

        public NewsItemsService(NewsContext context)
        {
            _newsContext = context;
        }


        public async Task PopulateNewsFeedItems(string id)
        {
            if (!_newsContext.RSSFeeds.Any(x => x.ID.Equals(id, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception("No RSS feed with specified ID");
            }

            RSSFeed rssFeedItem = await _newsContext.RSSFeeds.SingleAsync(x => x.ID.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            string rssURL = rssFeedItem.FeedURL;
            string rssPublisher = rssFeedItem.FeedName;

            XmlReader reader = XmlReader.Create(rssURL);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach (SyndicationItem item in feed.Items)
            {
                NewsItem listNewsItem = new NewsItem();
                listNewsItem.ArticlePublisher = rssPublisher;
                listNewsItem.PublisherID = id;
                listNewsItem.ArticleTitle = item.Title?.Text;
                listNewsItem.ArticleSummary = item.Summary?.Text;
                listNewsItem.ArticleDateTime = item.PublishDate.DateTime;

                var isDuplicate = await _newsContext.NewsItems.AnyAsync(x => x.ArticleTitle == listNewsItem.ArticleTitle || x.ArticlePublisher == listNewsItem.ArticlePublisher);

                if (!isDuplicate)
                {
                    await _newsContext.NewsItems.AddAsync(listNewsItem); //Check against ArticlePublisher and ArticleTitle to check for duplicates/existing newsitems in the list
                }

            }
            await _newsContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<NewsItem>> GetFeedNewsItems(string id)
        {
            return await _newsContext.NewsItems.Where(x => x.PublisherID == id).ToListAsync();
        }
    }
}
