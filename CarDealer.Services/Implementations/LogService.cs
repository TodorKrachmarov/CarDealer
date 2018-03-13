namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class LogService : ILogService
    {
        private readonly ApplicationDbContext db;

        public LogService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string username, string action, string modifiedTable)
        {
            var log = new Log
            {
                Username = username,
                Action = action,
                ModifiedTable = modifiedTable,
                ActionDate = DateTime.UtcNow
            };

            this.db.Logs.Add(log);
            this.db.SaveChanges();
        }

        public void ClearAll()
        {
            var logs = this.db.Logs;

            foreach (var log in logs)
            {
                this.db.Logs.Remove(log);
            }

            this.db.SaveChanges();
        }

        public IEnumerable<LogModel> All()
            => this.db
                    .Logs
                    .OrderBy(l => l.ActionDate)
                    .Select(l => new LogModel
                    {
                        Username = l.Username,
                        Action = l.Action,
                        ModifiedTable = l.ModifiedTable,
                        ActionDate = l.ActionDate
                    })
                    .ToList();

        //public IEnumerable<LogModel> AllByUser(string username)
        //        => this.db
        //            .Logs
        //            .Where(l => l.Username == username)
        //            .OrderBy(l => l.ActionDate)
        //            .Select(l => new LogModel
        //            {
        //                Username = l.Username,
        //                Action = l.Action,
        //                ModifiedTable = l.ModifiedTable,
        //                ActionDate = l.ActionDate
        //            })
        //            .ToList();
    }
}
