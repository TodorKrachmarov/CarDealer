namespace CarDealer.Data.Models
{
    using System;

    public class Log
    {
        public int Id { get; set; }
        
        public string Username { get; set; }

        public string Action { get; set; }

        public string ModifiedTable { get; set; }

        public DateTime ActionDate { get; set; }
    }
}
