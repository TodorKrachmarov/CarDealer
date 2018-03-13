namespace CarDealer.Services.Implementations
{
    using System;
    using Data;
    using Data.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext db;

        public CustomerService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order)
        {
            var customersQuery = this.db.Customers.AsQueryable();

            switch (order)
            {
                case OrderDirection.Ascending:
                    customersQuery = customersQuery
                        .OrderBy(c => c.BirthDate)
                        .ThenBy(c => c.Name);
                    break;
                case OrderDirection.Descending:
                    customersQuery = customersQuery
                        .OrderByDescending(c => c.BirthDate)
                        .ThenBy(c => c.Name);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid order direction: {order}.");
            }

            return customersQuery
                .Select(c => new CustomerModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();
        }

        public SingleCustomerModel FindCustomer(int id)
        {
            var customer = this.db
                .Customers
                .Include(c => c.Sales)
                .ThenInclude(c => c.Car)
                .ThenInclude(c => c.Parts)
                .ThenInclude(c => c.Part)
                .Where(c => c.Id == id)
                .Select(c => new 
                {
                    Id = c.Id,
                    Name = c.Name,
                    Count = c.Sales.Count,
                    Sales = c.Sales
                })
                .FirstOrDefault();

            var cus = new SingleCustomerModel
            {
                Id = customer.Id,
                Name = customer.Name,
                SalesCount = customer.Count,
                TotalPrice = customer.Sales.Sum(s => 
                s.Discount != 0 ? 
                s.Car.Parts.Sum(p => p.Part.Price) * s.Discount :
                s.Car.Parts.Sum(p => p.Part.Price))
            };

            return cus;
        }

        public void Create(string name, DateTime birthDate)
        {
            var years = DateTime.Now.Subtract(birthDate).TotalDays / 365;

            var customer = new Customer
            {
                Name = name,
                BirthDate = birthDate,
                IsYoungDriver = years <= 22 ? true : false
            };

            this.db.Customers.Add(customer);
            this.db.SaveChanges();
        }

        public bool Edit(int id, string name, DateTime birthDate)
        {
            var years = DateTime.Now.Subtract(birthDate).TotalDays / 365;

            var customer = this.db.Customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return false;
            }

            customer.Name = name;
            customer.BirthDate = birthDate;
            customer.IsYoungDriver = years <= 22 ? true : false;

            this.db.SaveChanges();

            return true;
        }

        public EditCustomerModel FindById(int id)
        {
            return this.db
                .Customers
                .Where(c => c.Id == id)
                .Select(c => new EditCustomerModel
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate
                })
                .FirstOrDefault();
        }

        public IEnumerable<AddSaleCustomerModel> SaleCustomers()
            => this.db
                   .Customers
                   .Select(c => new AddSaleCustomerModel
                   {
                       Id = c.Id,
                       Name = c.Name
                   })
                   .ToList();
    }
}
