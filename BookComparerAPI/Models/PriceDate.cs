namespace BookComparerAPI.Models
{
    public class PriceDate
    {
        public DateTime date;
        public double price;
        public string? store;

        public PriceDate(double price, string store)
        {
            this.date = DateTime.Now;
            this.price = price;
            this.store = store;
        }
    }
}
