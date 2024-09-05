﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCodigo
{
    public class Product
    {

        public int ProductID { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
