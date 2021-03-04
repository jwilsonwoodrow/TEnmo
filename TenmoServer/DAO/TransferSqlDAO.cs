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

        public void SendTEBucks(int senderid, int receiverid, decimal amount)
        {
            //deduct money from the users balance, and then add money to the receivers balance.
            try
            {
                UpdateSenderBalance(senderid, amount);

                UpdateReceiverBalance(receiverid, amount);

                CreatesTransferInDatabase(senderid, receiverid, amount);

            }
            catch
            {

            }

        }

        public void CreatesTransferInDatabase(int senderid, int receiverid, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //writes the transfer to the database
                SqlCommand cmd = new SqlCommand("insert into transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) values (@transferType, @transferStatus, @accountFrom, @accountTo, @amount", conn);
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
    }
}
