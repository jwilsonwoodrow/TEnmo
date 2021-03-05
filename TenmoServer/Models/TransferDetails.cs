using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class TransferDetails
    {
        public int TransferId { get; set; }
        public string ToUsername { get; set; }
        public string FromUsername { get; set; }
        public string TransferType { get; set; }
        public string StatusDesc { get; set; }
        public decimal Amount { get; set; }
    }
}
