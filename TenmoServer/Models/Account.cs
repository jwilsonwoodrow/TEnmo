﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        public int Account_id { get; set; }

        public int User_id { get; set; }

        public decimal Balance { get; set; }

        public Account()
        {

        }

    }
}
