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
    class Customer
    {
        int id;
        string password;

        public Customer(int id, string password)
        {
            this.id = id;
            this.password = password;
        }
        public static Boolean checkCredentials(int id, string password) //check that the username and password entered match a customer in the database
        {
            bool verified = false;
            string connStr = "server=157.89.28.130;user=ChangK;database=csc340;port=3306;password=Wallace#409;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            try
            {

                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM luikhamcustomer WHERE ID=@id AND password=@password";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@password", password);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    Console.WriteLine("true");
                    verified = true;

                }
                else
                {
                    Console.WriteLine("false");
                    verified = false;

                }

                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return verified;

        }
       
        public int getId()
        {
            return id;
        }
        public static Customer destroyCust(Customer cust) //destroy customer object
        {
            cust = null;
            return cust;

        }
    }


    
}
