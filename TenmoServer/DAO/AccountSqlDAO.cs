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

        public Account ViewBalance(int userid)
        {
            Account newAccount = new Account();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from accounts where user_id = @userid", conn);
                cmd.Parameters.AddWithValue("@user_id", userid);
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
