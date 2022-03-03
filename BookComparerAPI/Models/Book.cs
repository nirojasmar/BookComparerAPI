using System.ComponentModel.DataAnnotations;

namespace BookComparerAPI.Models
{
    public class Book
    {
        [Key]
        public int ISBN { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Author { get; set; }
        public string? Editor { get; set; }
        public string? Language { get; set; }
        public string? Format { get; set; }
        public string? Url { get; set; }
        public List <PriceDate> PriceDates { get; set;}

    }
}
