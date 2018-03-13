namespace CarDealer.Data.Models
{
    using System.Collections.Generic;

    public class Part
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }

        public int Quantity { get; set; }

        public virtual List<CarPart> Cars { get; set; } = new List<CarPart>();

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
