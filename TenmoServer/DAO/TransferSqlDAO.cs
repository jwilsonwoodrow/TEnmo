using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private string connectionString;

        public TransferSqlDAO(string connectionstring)
        {
            this.connectionString = connectionstring;
        }

        public List<Transfer> SendTEBucks(int senderid, int receiverid, decimal amount)
        {
            //deduct money from the users balance, and then add money to the receivers balance.
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("", conn);


                    //update the sender and receiver and write new transaction to log it. 

                }
            }
            catch
            {

            }




        }



    }
}
