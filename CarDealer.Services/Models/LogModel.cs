namespace CarDealer.Services.Models
{
    using System;

    public class LogModel
    {
        public string Username { get; set; }

        public string Action { get; set; }

        public string ModifiedTable { get; set; }

        public DateTime ActionDate { get; set; }
    }
}
