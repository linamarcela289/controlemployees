using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Functions.Entities
{
    public class ConsolidatedEntity : TableEntity
    {
        public int IdEmployee { get; set; }

        public DateTime Date { get; set; }

        public int WorkTime { get; set; }
    }
}
