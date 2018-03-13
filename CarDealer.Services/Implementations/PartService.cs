namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class PartService : IPartService
    {
        private readonly ApplicationDbContext db;

        public PartService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool Create(string name, double price, int supplierId, int quantity)
        {
            var supplier = this.db.Suppliers.FirstOrDefault(s => s.Id == supplierId);

            if (supplier == null)
            {
                return false;
            }

            var part = new Part
            {
                Name = name,
                Price = price,
                Quantity = quantity,
                Supplier = supplier,
                SupplierId = supplierId
            };

            this.db.Parts.Add(part);
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<PartModel> AllParts()
                    => this.db
                            .Parts
                            .OrderByDescending(p => p.Id)
                            .Select(p => new PartModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Price = p.Price
                            })
                            .ToList();

        public bool Delete(int id)
        {
            var part = this.db.Parts
                .FirstOrDefault(p => p.Id == id);

            if (part == null)
            {
                return false;
            }

            this.db.Parts.Remove(part);
            this.db.SaveChanges();

            return true;
        }

        public EditPartModel FindToEdit(int id)
                        => this.db
                                .Parts
                                .Where(p => p.Id == id)
                                .Select(p => new EditPartModel
                                {
                                    Name = p.Name,
                                    Price = double.Parse(p.Price.ToString()),
                                    Quantity = p.Quantity,
                                })
                                .FirstOrDefault();

        public bool Edit(int id, double price, int quantity)
        {

            var part = this.db.Parts.FirstOrDefault(p => p.Id == id);

            if (part == null)
            {
                return false;
            }
            part.Price = price;
            part.Quantity = quantity;
            
            this.db.SaveChanges();

            return true;
        }
    }
}
