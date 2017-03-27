using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AriStore.Models
{
    public class BouquetOrder
    {
        public string Quatity { get; set; }
        public string Tax { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public Bouquet Bouquet { get; set; }

        public BouquetOrder(int quatity, double tax, Bouquet bouquet)        {
            this.Bouquet = bouquet;
            double price = double.Parse(bouquet.Price);
            double ship = double.Parse(bouquet.Shipping);
            this.Quatity = quatity.ToString();
            this.Tax = tax.ToString();
            this.SubTotal = (price* quatity).ToString();
            this.Total = ((price * quatity) + ship + tax).ToString();
        }
    }
}