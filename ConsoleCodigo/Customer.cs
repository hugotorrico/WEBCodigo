using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCodigo
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public bool IsActive { get; set; }
    }
}
