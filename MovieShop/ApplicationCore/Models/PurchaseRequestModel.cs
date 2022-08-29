﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class PurchaseRequestModel
    {
        public Guid PurchaseNumber => Guid.NewGuid();
        public DateTime PurchaseDateTime => DateTime.UtcNow;
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
    }
}
