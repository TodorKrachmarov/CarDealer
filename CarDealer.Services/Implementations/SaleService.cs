namespace CarDealer.Services.Implementations
{
    using Data;
    using System.Collections.Generic;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using CarDealer.Data.Models;

    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext db;

        public SaleService(ApplicationDbContext db)
        {
            this.db = db;
        }
        
        public IEnumerable<AllSaleModel> GetAllSales()
        {
            var sales = this.db
                .Sales
                .Include(s => s.Car)
                .ThenInclude(s => s.Parts)
                .ThenInclude(s => s.Part)
                .Include(s => s.Customer)
                .Select(s => new
                {
                    Id = s.Id,
                    CarModel = s.Car.Model,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    Discount = s.Discount
                })
                .ToList();

            return sales
                .Select(s => new AllSaleModel
                {
                    Id = s.Id,
                    CarModel = s.CarModel,
                    CustomerName = s.CustomerName,
                    Price = s.Price,
                    Discount = s.Discount,
                    PriceWithDis = s.Discount != 0 ? s.Price * s.Discount : s.Price
                })
                .ToList();
        }

        public SingleSaleModel GetSaleById(int id)
        {
            var sale = this.db
                .Sales
                .Where(s => s.Id == id)
                .Include(s => s.Car)
                .ThenInclude(s => s.Parts)
                .ThenInclude(s => s.Part)
                .Include(s => s.Customer)
                .Select(s => new
                {
                    CarMake = s.Car.Make,
                    CarModel = s.Car.Model,
                    CarTravelledDistance = s.Car.TravelledDistance,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    Discount = s.Discount
                })
                .FirstOrDefault();

            return new SingleSaleModel
                {
                    CarMake = sale.CarMake,
                    CarModel = sale.CarModel,
                    CarTravelledDistance = sale.CarTravelledDistance,
                    CustomerName = sale.CustomerName,
                    Price = sale.Price,
                    Discount = sale.Discount,
                    PriceWithDis = sale.Discount != 0 ? sale.Price * sale.Discount : sale.Price
                };
        }

        public IEnumerable<AllSaleModel> GetSalesWithDiscount()
        {
            var sales = this.db
                .Sales
                .Where(s => s.Discount != 0)
                .Include(s => s.Car)
                .ThenInclude(s => s.Parts)
                .ThenInclude(s => s.Part)
                .Include(s => s.Customer)
                .Select(s => new
                {
                    Id = s.Id,
                    CarModel = s.Car.Model,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    Discount = s.Discount
                })
                .ToList();

            return sales
                .Select(s => new AllSaleModel
                {
                    Id = s.Id,
                    CarModel = s.CarModel,
                    CustomerName = s.CustomerName,
                    Price = s.Price,
                    Discount = s.Discount,
                    PriceWithDis = s.Discount != 0 ? s.Price * s.Discount : s.Price
                })
                .ToList();
        }

        public IEnumerable<AllSaleModel> GetSalesWithSpesificDiscount(double percent)
        {
            var sales = this.db
                .Sales
                .Where(s => s.Discount == percent)
                .Include(s => s.Car)
                .ThenInclude(s => s.Parts)
                .ThenInclude(s => s.Part)
                .Include(s => s.Customer)
                .Select(s => new
                {
                    Id = s.Id,
                    CarModel = s.Car.Model,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    Discount = s.Discount
                })
                .ToList();

            return sales
                .Select(s => new AllSaleModel
                {
                    Id = s.Id,
                    CarModel = s.CarModel,
                    CustomerName = s.CustomerName,
                    Price = s.Price,
                    Discount = s.Discount,
                    PriceWithDis = s.Discount != 0 ? s.Price * s.Discount : s.Price
                })
                .ToList();
        }

        public ReviewSaleModel ReviewSale(int customerId, int carId, int discount)
        {
            var car = this.db
                            .Cars
                            .Where(c => c.Id == carId)
                            .Select(c => new
                            {
                                Id = c.Id,
                                Name = $"{c.Make} {c.Model}",
                                Price = c.Parts.Sum(p => p.Part.Price)
                            })
                            .FirstOrDefault();

            var customer = this.db
                                .Customers
                                .Where(c => c.Id == customerId)
                                .Select(c => new
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    IsYoungDriver = c.IsYoungDriver
                                })
                                .FirstOrDefault();

            if (customer == null || car == null)
            {
                return null;
            }

            var sale = new ReviewSaleModel
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                IsYoungDriver = customer.IsYoungDriver,
                CarId = car.Id,
                CarName = car.Name,
                Price = double.Parse(car.Price.ToString())
            };

            sale.Discount = ((sale.IsYoungDriver ? 5 : 0) + discount);
            sale.FinalPrice = (sale.Price * (1 - ((double)(sale.Discount) / 100)));

            return sale;
        }

        public bool Create(int carId, int customerId, int discount)
        {
            var car = this.db.Cars.FirstOrDefault(c => c.Id == carId);
            var customer = this.db.Customers.FirstOrDefault(c => c.Id == customerId);

            if (car == null || customer == null)
            {
                return false;
            }

            var sale = new Sale
            {
                CarId = carId,
                Car = car,
                CustomerId = customerId,
                Customer = customer,
                Discount = (double)discount / 100.00
            };

            this.db.Sales.Add(sale);
            this.db.SaveChanges();

            return true;
        }
    }
}
