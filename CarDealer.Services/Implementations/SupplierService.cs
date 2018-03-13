namespace CarDealer.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using CarDealer.Data.Models;

    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext db;

        public SupplierService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SupplierModel> FilterSuppliers(bool isImporter)
        {
            return this.db
                .Suppliers
                .Include(s => s.Parts)
                .Where(s => s.IsImporter == isImporter)
                .Select(s => new SupplierModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();
        }

        public IEnumerable<SupplierModel> AllSuppliers()
        {
            return this.db
                .Suppliers
                .Select(s => new SupplierModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();
        }

        public IEnumerable<ListSupplierModel> ListSuppliers()
            => this.db
                    .Suppliers
                    .Select(s => new ListSupplierModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsImporter = s.IsImporter
                    })
                    .ToList();

        public void Create(string name, bool isImporter)
        {
            var supplier = new Supplier
            {
                Name = name,
                IsImporter = isImporter
            };

            this.db.Suppliers.Add(supplier);
            this.db.SaveChanges();
        }

        public EditSupplierModel FindToEdit(int id)
            => this.db
                    .Suppliers
                    .Where(s => s.Id == id)
                    .Select(s => new EditSupplierModel
                    {
                        Name = s.Name,
                        IsImporter = s.IsImporter
                    })
                    .FirstOrDefault();

        public bool Edit(int id, string name, bool isImporter)
        {
            var supplier = this.db.Suppliers.FirstOrDefault(s => s.Id == id);

            if (supplier == null)
            {
                return false;
            }

            supplier.Name = name;
            supplier.IsImporter = isImporter;

            this.db.SaveChanges();

            return true;
        }

        public bool Exists(int id)
            => this.db.Suppliers.Any(s => s.Id == id);

        public bool Delete(int id)
        {
            var supplier = this.db.Suppliers.Include(s => s.Parts).FirstOrDefault(s => s.Id == id);

            if (supplier == null)
            {
                return false;
            }

            foreach (var part in supplier.Parts)
            {
                this.db.Parts.Remove(part);
            }

            this.db.Suppliers.Remove(supplier);
            this.db.SaveChanges();

            return true;
        }
    }
}
