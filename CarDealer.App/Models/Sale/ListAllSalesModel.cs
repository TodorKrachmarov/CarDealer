namespace CarDealer.App.Models.Sale
{
    using Services.Models;
    using System.Collections.Generic;

    public class ListAllSalesModel
    {
        public IEnumerable<AllSaleModel> Sales { get; set; }
    }
}
