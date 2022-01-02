using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace AtmProject
{
    class Transaction
    {
        string date;
        double amount;
        int accountId;
        string transactionType;
        int transactionId;
        int fromAccount;
        int toAccount;
        public Transaction(string date, double amount, int accountId, string transactionType, int transactionId, int fromAccount,
            int toAccount)
        {
            this.date = date;
            this.amount = amount;
            this.accountId = accountId;
            this.transactionType = transactionType;
            this.transactionId = transactionId;
            this.fromAccount = fromAccount;
            this.toAccount = toAccount;
        }
        public void saveTransaction() //save the transaction to the database
        {
            string connStr = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO luikhamtransaction (date, amount, accountID, transactionType, transactionID, fromAccount, toAccount) " +
                    "VALUES (@date, @amount, @accID, @transType, @transID, @fromAcc, @toAcc)";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@accID", accountId);
                cmd.Parameters.AddWithValue("@transType", transactionType);
                cmd.Parameters.AddWithValue("@transID", transactionId);
                cmd.Parameters.AddWithValue("@fromAcc", fromAccount);
                cmd.Parameters.AddWithValue("@toAcc", toAccount);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

        }
        public static int getTransId() //get a random number for the id of the transaction
        {
            int transId = 0;
            Random rand = new Random();
            for (int i = 1; i <= 4; i++)
            {
                transId *= 10;
                transId += rand.Next(0, 10);
            }
            return transId;

        }
        public static Transaction destroyTrans(Transaction trans) //destroy the transaction object
        {
            trans = null;
            return trans;
        }

    }
}
