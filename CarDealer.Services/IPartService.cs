namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;

    public interface IPartService
    {
        bool Create(string name, double price, int supplierId, int quantity);

        IEnumerable<PartModel> AllParts();

        bool Delete(int id);

        EditPartModel FindToEdit(int id);

        bool Edit(int id, double price, int quantity);
    }
}
