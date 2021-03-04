using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private string connectionString;

        public AccountSqlDAO(string connectionstring)
        {
            this.connectionString = connectionstring;
        }

        /*Methods on our DAO
        ViewBalance:
        **/

        public Account ViewBalance(string username)
        {
            //int userID = 
            Account newAccount = new Account();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from accounts a join users u on a.user_id = u.user_id where u.username = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    newAccount.Account_id = Convert.ToInt32(reader["account_id"]);
                    newAccount.User_id = Convert.ToInt32(reader["user_id"]);
                    newAccount.Balance = Convert.ToDecimal(reader["balance"]);
                }
            }
            return newAccount;

        }


    }
}
