namespace NewsAggregatorAPI.Models
{
    public class RSSFeedDTO
    {
        public string? ID { get; set; }
        public string? FeedName { get; set; }
        public string FeedURL { get; set; } //To be removed as we transition to a list of RSS Feeds that the user selects

    }
}
