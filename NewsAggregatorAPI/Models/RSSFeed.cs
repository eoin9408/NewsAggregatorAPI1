using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregatorAPI.Models
{
    public class RSSFeed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string? FeedName { get; set; }
        public string FeedURL { get; set; }

    }
}
