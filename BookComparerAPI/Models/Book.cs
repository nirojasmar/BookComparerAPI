using System.ComponentModel.DataAnnotations;

namespace BookComparerAPI.Models
{
    public class Book
    {
        [Key]
        public int ISBN { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Editor { get; set; }
        public string? Language { get; set; }
        public string? Format { get; set; }
        public string? Url { get; set; }
        public string? UrlImage { get; set; }

        [DataType(DataType.Currency)]
        public decimal AmazonPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal BLPrice { get; set; }
    }
}
