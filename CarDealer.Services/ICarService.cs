namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;

    public interface ICarService
    {
        IEnumerable<CarModel> CarsFromMake(string make);

        SingleCarModel CarInfo(int Id);

        IEnumerable<CarModel> AllCars();

        void Create(string make, string model, long trDistance, List<int> parts);

        IEnumerable<AddSaleCarModel> SaleCar();
    }
}
