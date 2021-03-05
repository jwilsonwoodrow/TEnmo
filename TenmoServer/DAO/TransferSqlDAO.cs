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
        private const string INVALID_ID_SELECT = "Invalid recipient Id";
        private string connectionString;

        public TransferSqlDAO(string connectionstring)
        {
            this.connectionString = connectionstring;
        }


        public void CreatesTransferInDatabase(int senderid, int receiverid, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
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
                catch
                {
                    Console.WriteLine(INVALID_ID_SELECT);
                }
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

            return listOfTransfers;
        }

        public TransferDetails GetTransferDetails(int transferid)
        {
            TransferDetails details = new TransferDetails();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("select ts.transfer_status_desc, tt.transfer_type_desc, t.account_to, t.account_from, amount from transfers t join transfer_statuses ts on t.transfer_status_id = ts.transfer_status_id join transfer_types tt on t.transfer_type_id = ts.transfer_status_id where t.transfer_id = @transferId", connection);
                    command.Parameters.AddWithValue("@transferid", transferid);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        details.TransferId = transferid;
                        details.TransferType = Convert.ToString(reader["transfer_type_desc"]);
                        details.StatusDesc = Convert.ToString(reader["transfer_status_desc"]);
                        UserSqlDAO dao = new UserSqlDAO(connectionString);
                        int fromId = Convert.ToInt16(reader["account_from"]);
                        details.FromUsername = dao.GetUserByID(fromId).Username;
                        int toId = Convert.ToInt16(reader["account_to"]);
                        details.ToUsername = dao.GetUserByID(toId).Username;
                        details.Amount = Convert.ToDecimal(reader["amount"]);
                    }
                    return details;
                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
