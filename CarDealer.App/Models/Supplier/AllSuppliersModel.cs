namespace CarDealer.App.Models.Supplier
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;

    public class AllSuppliersModel
    {
        public IEnumerable<SupplierModel> Suppliers { get; set; }

        public bool IsImporter { get; set; } 
    }
}
