using System.ComponentModel.DataAnnotations;

namespace BookComparerAPI.Models
{
    public class PriceDate
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public string Store { get; set; }

        public PriceDate(decimal price, string store)
        {
            this.Date = DateTime.Now;
            this.Price = price;
            this.Store = store;
        }
    }
}
