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
    class Account
    {
        string dailyTransactionDate;
        double dailyTransactionTotal;
        double dailyTransactionLimit = 3000.0;
        double balance;
        int accountID;
        int customerID;
        public static ArrayList retrieveAccounts(int id) //this function creates objects for the accounts in the database and stores
        {                                                //them in an arraylist
            ArrayList accountList = new ArrayList();
            //ArrayList eventList = new ArrayList();  //a list to save the events
            //prepare an SQL query to retrieve all the events on the same, specified date
            DataTable myTable = new DataTable();
            string connStr = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM luikhamaccount WHERE customerID=@ID ORDER BY accountID ASC";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", id);

                MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd);
                myAdapter.Fill(myTable);
                Console.WriteLine("Table is ready.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            //convert the retrieved data to events and save them to the list
            foreach (DataRow row in myTable.Rows)
            {
                Account newAccount = new Account();

                newAccount.accountID = Int32.Parse(row["accountID"].ToString());
                newAccount.customerID = Int32.Parse(row["customerID"].ToString());
                newAccount.balance = Double.Parse(row["balance"].ToString());
                newAccount.dailyTransactionTotal = Double.Parse(row["dailyTransactionTotal"].ToString());
                newAccount.dailyTransactionDate = row["dailyTransactionDate"].ToString();
                /*
                newEvent.eventDate = row["date"].ToString();
                newEvent.eventStartTime = Int32.Parse(row["startTime"].ToString());
                newEvent.eventEndTime = Int32.Parse(row["endTime"].ToString());
                newEvent.eventContent = row["content"].ToString();
                */
                accountList.Add(newAccount);
            }
            Console.WriteLine("*********" + accountList.Count);
            return accountList;  //return the event list
        }
        public void depositMoney(double amount)
        {
           updateBalanceDeposit(amount);
           updateAccountData();
        }

        public int withdrawMoney(double amount, double machineCash) //this function calls the various functions involved in withdrawing money
        {
            updateTodayDate();
            if (checkDailyTransactions() == false)
                return 1;
            if (verifyDailyTransactions(amount) == false)
                return 2;
            if (verifyAccountBalance(amount) == false)
                return 3;
            if (checkMachineCash(amount, machineCash) == false)
                return 4;
            updateBalanceWithdraw(amount);
            updateDailyTransactionTotal(amount);

            updateAccountData();
            
            return 0;
        }
        





        private void updateTodayDate() //updates the date and amount withdrawn today on record for the account
        {
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            Console.WriteLine("old date: " + dailyTransactionDate);
            Console.WriteLine("new date: " + todayDate);
            if (!dailyTransactionDate.Equals(todayDate))
            {
                dailyTransactionDate = todayDate;
                dailyTransactionTotal = 0.0;
                Console.WriteLine("Date being changed.");
            }
        }
        private Boolean checkDailyTransactions() //check that the total withdrawals today does not exceed the transaction limit
        {
            if (dailyTransactionTotal >= dailyTransactionLimit)
                return false;
            else
                return true;
        }

        private Boolean verifyDailyTransactions(double amount) //check that the current amount being withdrawn will not cause the withdrawal
        {                                                      //total to exceed the limit
            if ((dailyTransactionTotal + amount) > dailyTransactionLimit)
                return false;
            else
                return true;
        }
        private Boolean verifyAccountBalance(double amount) //check that the amount being withdrawn does not exceed the money available
        {
            if (amount > balance)
                return false;
            else
                return true;
        }
        private Boolean checkMachineCash(double amount, double machineCash) //check that the machine has enough cash for the withdrawal to occur
        {
            if (amount > machineCash)
                return false;
            else
                return true;
        }
        private void updateBalanceWithdraw(double amount) //subtract the amount withdrawn from the balance
        {
            balance -= amount;
        }
        private void updateBalanceDeposit(double amount) //add the amount deposited to the balance
        {
            balance += amount;
        }
        private void updateDailyTransactionTotal(double amount) //update the total withdrawn amount for today
        {
            dailyTransactionTotal += amount;
        }
        private void updateAccountData() //update the account balance in the database
        {
            string connStr = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                //string sql = "INSERT INTO changstudent (id, name) VALUES (@uid, @uname)";
                string sql = "UPDATE luikhamaccount SET balance=@newBalance, dailyTransactionDate=@date, dailyTransactionTotal=@total WHERE accountID=@accNum;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", dailyTransactionDate);
                cmd.Parameters.AddWithValue("@total", dailyTransactionTotal);
                cmd.Parameters.AddWithValue("@newBalance", balance);
                cmd.Parameters.AddWithValue("@accNum", accountID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

        }
        public int getId()
        {
            return accountID;
        }
        public double getBalance()
        {
            return balance;
        }
        public static Account destroyAcc(Account acc) //destroy the account object
        {
            acc = null;
            return acc;
        }

        public static ArrayList destroyAccs(ArrayList list) //destroy all accounts
        {
            for (int i = 0; i < list.Count; i++)
            {
                Account delAccount = (Account)list[0];
                delAccount = null;

                list.RemoveAt(0);
            }
            return list;
        }
        public static Boolean checkIfMultAcc(ArrayList accounts) //check if there are currently multiple accounts for this customer
        {
            if (accounts.Count > 1)
            {
                return true;
            }
            else
                return false;
        }
    }
}
