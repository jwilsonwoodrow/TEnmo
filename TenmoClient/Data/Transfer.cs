using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public int TransferTypeID { get; set; }

        public int TransferStatusID { get; set; }

        public int AccountFrom { get; set; }

        public int AccountTo { get; set; }

        public decimal Amount { get; set; }

        public Transfer()
        {

        }
    }
}
