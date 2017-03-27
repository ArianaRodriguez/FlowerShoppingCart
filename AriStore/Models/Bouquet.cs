
namespace AriStore.Models
{
    public class Bouquet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Shipping { get; set; }

        public Bouquet(int id, string name, double price, double shipping)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price.ToString();
            this.Shipping = shipping.ToString();           
        }
    }
}