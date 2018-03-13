namespace CarDealer.Data.Models
{
    using System.Collections.Generic;

    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsImporter { get; set; }

        public virtual List<Part> Parts { get; set; } = new List<Part>();
    }
}
