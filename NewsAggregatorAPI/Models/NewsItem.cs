namespace NewsAggregatorAPI.Models
{
    public class NewsItem
    {
        public long Id { get; set; }
        public string? ArticleTitle { get; set; }
        public string? ArticleSummary { get; set; }
        public DateTime ArticleDateTime { get; set; }
       public string? Secret { get; set; }
    }
}
