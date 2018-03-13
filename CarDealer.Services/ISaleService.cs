namespace CarDealer.Services
{
    using Models;
    using System.Collections.Generic;

    public interface ISaleService
    {
        IEnumerable<AllSaleModel> GetAllSales();

        SingleSaleModel GetSaleById(int id);

        IEnumerable<AllSaleModel> GetSalesWithDiscount();

        IEnumerable<AllSaleModel> GetSalesWithSpesificDiscount(double percent);

        ReviewSaleModel ReviewSale(int customerId, int carId, int discount);

        bool Create(int carId, int customerId, int discount);
    }
}
