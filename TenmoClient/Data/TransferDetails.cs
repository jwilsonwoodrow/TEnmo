using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    class TransferDetails
    {
        public int TransferId { get; set; }
        public string ToUsername { get; set; }
        public string FromUsername { get; set; }
        public string TransferType { get; set; }
        public string StatusDesc { get; set; }
        public decimal Amount { get; set; }
    }
}