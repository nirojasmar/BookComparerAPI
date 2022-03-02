using System.ComponentModel.DataAnnotations;

namespace BookComparerAPI.Models
{
    public class PriceDate
    {
        [Required]
        public DateTime date { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal price { get; set; }
        [Required]
        public string store { get; set; }

        public PriceDate(decimal price, string store)
        {
            this.date = DateTime.Now;
            this.price = price;
            this.store = store;
        }
    }
}
