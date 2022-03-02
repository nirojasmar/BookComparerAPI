using System.ComponentModel.DataAnnotations;

namespace BookComparerAPI.Models
{
    public class PriceDate
    {
        [Required]
        public DateTime date { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public string store { get; set; }

        public PriceDate(double price, string store)
        {
            this.date = DateTime.Now;
            this.price = price;
            this.store = store;
        }
    }
}
