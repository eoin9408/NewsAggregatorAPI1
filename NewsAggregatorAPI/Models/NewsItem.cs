using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregatorAPI.Models
{
    public class NewsItem
    {
        [Key]
        public long Id { get; set; }
        public string? ArticlePublisher { get; set; }
        public string PublisherID { get; set; }
        public string? ArticleTitle { get; set; }
        public string? ArticleSummary { get; set; }
        public DateTime ArticleDateTime { get; set; }
    }
}
