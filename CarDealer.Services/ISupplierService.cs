namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;

    public interface ISupplierService
    {
        IEnumerable<SupplierModel> FilterSuppliers(bool isImporter);

        IEnumerable<SupplierModel> AllSuppliers();

        IEnumerable<ListSupplierModel> ListSuppliers();

        void Create(string name, bool isImporter);

        EditSupplierModel FindToEdit(int id);

        bool Edit(int id, string name, bool isImporter);

        bool Exists(int id);

        bool Delete(int id);
    }
}
