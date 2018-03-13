namespace CarDealer.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface ICustomerService
    {
        IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order);

        SingleCustomerModel FindCustomer(int id);

        void Create(string name, DateTime birthDate);

        bool Edit(int id, string name, DateTime birthDate);

        EditCustomerModel FindById(int id);

        IEnumerable<AddSaleCustomerModel> SaleCustomers();
    }
}
