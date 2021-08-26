using Common.Enums;
using System;

namespace Common.Models
{
    public class Employee
    {
        public int IdEmployee { get; set; }

        public TypeEnum Type { get; set; }

        public DateTime? Date { get; set; }

        public bool? Consolidated { get; set; }
    }
}
