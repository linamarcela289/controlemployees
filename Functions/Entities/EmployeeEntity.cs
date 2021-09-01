using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Functions.Entities
{
    public class EmployeeEntity : TableEntity
    {
        public int IdEmployee { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }

        public bool Consolidated { get; set; }
    }
}
