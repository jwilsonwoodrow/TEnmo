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


        public void CreatesTransferInDatabase(int senderid, int receiverid, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //writes the transfer to the database
                SqlCommand cmd = new SqlCommand("insert into transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) values (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)", conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", 2);
                cmd.Parameters.AddWithValue("@transfer_status_id", 2);
                cmd.Parameters.AddWithValue("@account_from", senderid);
                cmd.Parameters.AddWithValue("@account_to", receiverid);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateReceiverBalance(int receiverid, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //update reciever balance
                SqlCommand cmd = new SqlCommand("update accounts set balance = balance + @amount where account_id = @receiverid", conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@receiverid", receiverid);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSenderBalance(int senderid, decimal amount)
        {
            Transfer transfer = new Transfer();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //update sender balance 
                SqlCommand cmd = new SqlCommand("update accounts set balance = balance - @amount where account_id = @senderid", conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@senderid", senderid);
                int rows = cmd.ExecuteNonQuery();
            }
        }

        public List<Transfer> ViewAllTransfers(User user) //we need the user id somehow
        {
            List<Transfer> listOfTransfers = new List<Transfer>();
            try
            {
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //see transactions
                    SqlCommand cmd = new SqlCommand("Select Distinct * from transfers where account_from = @userid or account_to = @userid", conn);
                    cmd.Parameters.AddWithValue("@userid", user.UserId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transfer transferLog = new Transfer();
                        transferLog.TransferId = Convert.ToInt32(reader["transfer_id"]);
                        transferLog.TransferTypeID = Convert.ToInt32(reader["transfer_type_id"]);
                        transferLog.TransferStatusID = Convert.ToInt32(reader["transfer_status_id"]);
                        transferLog.AccountFrom = Convert.ToInt32(reader["account_from"]);
                        transferLog.AccountTo = Convert.ToInt32(reader["account_to"]);
                        transferLog.Amount = Convert.ToInt32(reader["amount"]);
                        listOfTransfers.Add(transferLog);
                    }
                }            
            }
            catch
            {

            }
            return listOfTransfers;


        }
    }
}
