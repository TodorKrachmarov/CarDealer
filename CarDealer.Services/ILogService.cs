namespace CarDealer.Services
{
    using Models;
    using System.Collections.Generic;

    public interface ILogService
    {
        void Create(string username, string action, string modifiedTable);

        void ClearAll();

        IEnumerable<LogModel> All();
    }
}
