using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AtmProject
{
    public partial class Form1 : Form
    {
        string todayDate;
        Customer newCust;
        Transaction newTransaction;
        ArrayList accounts;
        Account curAccount;
        Account fromAccount;
        Account toAccount;
        double machineCash = 100000.0;
        int withdrawCode = -1;
        int transId;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "You are in the Process of Withdrawal";
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel2.Visible = true;
            for (int i = 0; i < accounts.Count; i++)
            {
                listBox1.Items.Add(((Account)accounts[i]).getId());
            }            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = false;
            listBox1.Items.Clear();
            label1.Text = "Welcome to ZZZ Bank ATM Machine";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "2";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "1";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "4";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            tableLayoutPanel3.Visible = false;
            tableLayoutPanel2.Visible = true;
        }

        

        private void button26_Click(object sender, EventArgs e) //signing in
        {
            int id = Int32.Parse(textBox2.Text);
            string password = textBox3.Text;
            textBox2.Clear();
            textBox3.Clear();

            if (Customer.checkCredentials(id, password))
            {
                tableLayoutPanel9.Visible = false;
                tableLayoutPanel1.Visible = true;
                newCust = new Customer(id, password);
                accounts = Account.retrieveAccounts(id);
            }
            else
            {
                tableLayoutPanel9.Visible = false;
                tableLayoutPanel13.Visible = true;

            }

            
            


        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            curAccount = (Account)accounts[listBox1.SelectedIndex];
            label29.Text = "The Balance of this Account is " + curAccount.getBalance() + "$";
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel23.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "3";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "5";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "6";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "7";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "8";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "9";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "0";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0){
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            }
        }

        private void button8_Click(object sender, EventArgs e) //withdrawing money
        {
            double oldBalance;
            double newBalance;
            double amount;
            if (textBox1.Text == "")
            {
                label18.Text = "Error, you must Input an Amount";
                tableLayoutPanel3.Visible = false;
                tableLayoutPanel16.Visible = true;
            }
            else
            {
                oldBalance = curAccount.getBalance();
                amount = Double.Parse(textBox1.Text);
                withdrawCode = curAccount.withdrawMoney(amount, machineCash);
                if (withdrawCode == 1)
                {
                    // tableLayoutPanel3.Visible = false;
                    // tableLayoutPanel4.Visible = true;
                    label18.Text = "Error, this Account has already reached its daily limit of 3000$ for Withdrawals";
                    tableLayoutPanel3.Visible = false;
                    tableLayoutPanel16.Visible = true;

                }
                else if (withdrawCode == 2)
                {
                    //tableLayoutPanel3.Visible = false;
                    //tableLayoutPanel5.Visible = true;
                    label18.Text = "Error, this Amount exceeds the daily transaction limit of 3000$";
                    tableLayoutPanel3.Visible = false;
                    tableLayoutPanel16.Visible = true;
                }
                else if (withdrawCode == 3)
                {
                   // tableLayoutPanel3.Visible = false;
                   // tableLayoutPanel6.Visible = true;
                    label18.Text = "Error, this Amount Exceeds the Remaining Account Balance";
                    tableLayoutPanel3.Visible = false;
                    tableLayoutPanel16.Visible = true;
                }
                else if (withdrawCode == 4)
                {
                  //  tableLayoutPanel3.Visible = false;
                   // tableLayoutPanel7.Visible = true;
                    label18.Text = "Error, the Machine does not have Enough Cash for this Withdrawal";
                    tableLayoutPanel3.Visible = false;
                    tableLayoutPanel16.Visible = true;
                }
                else
                {
                    newBalance = curAccount.getBalance();
                    label19.Text = "You have Withdrawn " + amount + "$";
                    label21.Text = "Your Account's Old Balance was " + oldBalance + "$";
                    label22.Text = "Your Account's New Balance is " + newBalance + "$";
                    todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    transId = Transaction.getTransId();
                    newTransaction = new Transaction(todayDate, amount, curAccount.getId(), "Withdraw", transId, -1, -1);
                    newTransaction.saveTransaction();
                    tableLayoutPanel3.Visible = false;
                    tableLayoutPanel20.Visible = true;
                    textBox1.Clear();
                }                
            }

            
        }

        private void button27_Click(object sender, EventArgs e)
        {
            tableLayoutPanel10.Visible = false;
            tableLayoutPanel1.Visible = true;
            label1.Text = "Welcome o ZZZ Bank ATM Machine";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Account.checkIfMultAcc(accounts))
            {
                tableLayoutPanel1.Visible = false;
                tableLayoutPanel11.Visible = true;
                label1.Text = "You are in the Process of Transfering Money";
                for (int i = 0; i < accounts.Count; i++)
                {
                    listBox2.Items.Add(((Account)accounts[i]).getId());
                }
            }
            else
            {
                tableLayoutPanel1.Visible = false;
                tableLayoutPanel10.Visible = true;
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fromAccount = (Account)accounts[listBox2.SelectedIndex];
            for (int i = 0; i < listBox2.SelectedIndex; i++)
            {
                listBox3.Items.Add(((Account)accounts[i]).getId());
            }
            for (int i = listBox2.SelectedIndex+1; i < accounts.Count; i++)
            {
                listBox3.Items.Add(((Account)accounts[i]).getId());
            }
            tableLayoutPanel11.Visible = false;
            tableLayoutPanel12.Visible = true;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= accounts.IndexOf(fromAccount))
            {
                toAccount = (Account)accounts[listBox3.SelectedIndex+1];
            }
            else
            {
                toAccount = (Account)accounts[listBox3.SelectedIndex];
            }
            label24.Text = "The Balance of the Account you are Transferring from is " + fromAccount.getBalance() + "$";
            label23.Text = "The Balance of the Account you are Transferring to is " + toAccount.getBalance() + "$";
            tableLayoutPanel12.Visible = false;
            tableLayoutPanel21.Visible = true;

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        

        private void button28_Click(object sender, EventArgs e)
        {
            tableLayoutPanel11.Visible = false;
            tableLayoutPanel1.Visible = true;
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            label1.Text = "Welcom to ZZZ Bank ATM Machine";
        }

        private void button29_Click(object sender, EventArgs e)
        {
            tableLayoutPanel12.Visible = false;
            tableLayoutPanel1.Visible = true;
            listBox3.Items.Clear();
            listBox2.Items.Clear();
            label1.Text = "Welcom to ZZZ Bank ATM Machine";
        }

        private void button50_Click(object sender, EventArgs e)
        {
            textBox5.Text += "1";
        }

        private void button49_Click(object sender, EventArgs e)
        {
            textBox5.Text += "2";
        }

        private void button52_Click(object sender, EventArgs e)
        {
            textBox5.Text += "3";
        }

        private void button53_Click(object sender, EventArgs e)
        {
            textBox5.Text += "4";
        }

        private void button54_Click(object sender, EventArgs e)
        {
            textBox5.Text += "5";
        }

        private void button55_Click(object sender, EventArgs e)
        {
            textBox5.Text += "6";
        }

        private void button56_Click(object sender, EventArgs e)
        {
            textBox5.Text += "7";
        }

        private void button57_Click(object sender, EventArgs e)
        {
            textBox5.Text += "8";
        }

        private void button58_Click(object sender, EventArgs e)
        {
            textBox5.Text += "9";
        }

        private void button59_Click(object sender, EventArgs e)
        {
            textBox5.Text += "0";
        }

        private void button60_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
        }

        private void button61_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Length > 0)
            {
                textBox5.Text = textBox5.Text.Substring(0, textBox5.Text.Length - 1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button66_Click(object sender, EventArgs e)
        {
            tableLayoutPanel23.Visible = false;
            tableLayoutPanel3.Visible = true;
        }

        private void button63_Click(object sender, EventArgs e)
        {
            tableLayoutPanel20.Visible = false;
            tableLayoutPanel1.Visible = true;
            listBox1.Items.Clear();
            label1.Text = "Welcome to ZZZ Bank ATM Machine";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel18.Visible = true;
            for (int i = 0; i < accounts.Count; i++)
            {
                listBox6.Items.Add(((Account)accounts[i]).getId().ToString()+"");
            }
            label1.Text = "You are in the Process of Checking the Balance of an Account.";
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            curAccount = (Account)accounts[listBox6.SelectedIndex];
            label8.Text = "The Balance of this Account is " + curAccount.getBalance() + "$";
            tableLayoutPanel18.Visible = false;
            tableLayoutPanel8.Visible = true;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            listBox6.Items.Clear();
            tableLayoutPanel8.Visible = false;
            tableLayoutPanel1.Visible = true;
            label1.Text = "Welcome to ZZZ Bank ATM Machine";
        }

        private void button48_Click(object sender, EventArgs e)
        {
            listBox6.Items.Clear();
            tableLayoutPanel18.Visible = false;
            tableLayoutPanel1.Visible = true;
            label1.Text = "Welcome to ZZZ Bank ATM Machine";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel14.Visible = true;
            for (int i = 0; i < accounts.Count; i++)
            {
                listBox5.Items.Add(((Account)accounts[i]).getId());
            }
            label1.Text = "You are in the Process of Depositing Money";
        }

        private void button31_Click(object sender, EventArgs e)
        {
            tableLayoutPanel14.Visible = false;
            tableLayoutPanel1.Visible = true;
            listBox5.Items.Clear();
            label1.Text = "Welcome to ZZZ Bank ATM Machine";
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            curAccount = (Account)accounts[listBox5.SelectedIndex];
            label31.Text = "The Balance of this Account is " + curAccount.getBalance() + "$";
            tableLayoutPanel14.Visible = false;
            tableLayoutPanel24.Visible = true;
        }

        private void button67_Click(object sender, EventArgs e)
        {
            tableLayoutPanel24.Visible = false;
            tableLayoutPanel15.Visible = true;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            textBox4.Text += "1";
        }

        private void button32_Click(object sender, EventArgs e)
        {
            textBox4.Text += "2";
        }

        private void button35_Click(object sender, EventArgs e)
        {
            textBox4.Text += "3";
        }

        private void button36_Click(object sender, EventArgs e)
        {
            textBox4.Text += "4";
        }

        private void button37_Click(object sender, EventArgs e)
        {
            textBox4.Text += "5";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            textBox4.Text += "6";
        }

        private void button39_Click(object sender, EventArgs e)
        {
            textBox4.Text += "7";
        }

        private void button40_Click(object sender, EventArgs e)
        {
            textBox4.Text += "8";
        }

        private void button41_Click(object sender, EventArgs e)
        {
            textBox4.Text += "9";
        }

        private void button42_Click(object sender, EventArgs e)
        {
            textBox4.Text += "0";
        }

        private void button43_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
        }

        private void button44_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Length > 0)
            {
                textBox4.Text = textBox4.Text.Substring(0, textBox4.Text.Length - 1);
            }
        }

        private void button45_Click(object sender, EventArgs e)
        {

            tableLayoutPanel15.Visible = false;
            tableLayoutPanel14.Visible = true;
        }

        private void button34_Click(object sender, EventArgs e) //depositing money
        {
            double oldBalance;
            double newBalance;
            double amount;
            if (textBox4.Text == "")
            {
                tableLayoutPanel15.Visible = false;
                tableLayoutPanel17.Visible = true;
            }
            else
            {
                oldBalance = curAccount.getBalance();
                amount = Double.Parse(textBox4.Text);
                curAccount.depositMoney(amount);
                newBalance = curAccount.getBalance();
                label32.Text = "You have Depsoited " + amount + "$";
                label34.Text = "Your Account's Old Balance was " + oldBalance + "$";
                label33.Text = "Your Account's New Balance is " + newBalance + "$";
                todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                transId = Transaction.getTransId();
                newTransaction = new Transaction(todayDate, amount, curAccount.getId(), "Deposit", transId, -1, -1);
                newTransaction.saveTransaction();
                tableLayoutPanel15.Visible = false;
                tableLayoutPanel25.Visible = true;
                textBox4.Clear();
                }
        }

        private void button68_Click(object sender, EventArgs e) //destroying objects when signing out
        {
            newCust = Customer.destroyCust(newCust);
            accounts = Account.destroyAccs(accounts);
            curAccount = Account.destroyAcc(curAccount);
            fromAccount = Account.destroyAcc(fromAccount);
            toAccount = Account.destroyAcc(toAccount);
            newTransaction = Transaction.destroyTrans(newTransaction);
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel9.Visible = true;
        }

        private void button51_Click(object sender, EventArgs e) //transferring money
        {
            double fromOldBalance;
            double fromNewBalance;
            double toOldBalance;
            double toNewBalance;
            double amount;
            if (textBox5.Text == "")
            {
                label37.Text = "Error, you must Input an Amount";
                tableLayoutPanel19.Visible = false;
                tableLayoutPanel27.Visible = true;
            }
            else
            {
                fromOldBalance = fromAccount.getBalance();
                toOldBalance = toAccount.getBalance();
                amount = Double.Parse(textBox5.Text);
                withdrawCode = fromAccount.withdrawMoney(amount, machineCash);

                if (withdrawCode == 1)
                {
                    tableLayoutPanel19.Visible = false;
                    tableLayoutPanel27.Visible = true;
                    label37.Text = "Error, this Account has already reached its daily limit of 3000$ for Withdrawals";
                }
                else if (withdrawCode == 2)
                {
                    tableLayoutPanel19.Visible = false;
                    tableLayoutPanel27.Visible = true;
                    label37.Text = "Error, this Amount exceeds the daily transaction limit of 3000$";
                }
                else if (withdrawCode == 3)
                {
                    tableLayoutPanel19.Visible = false;
                    tableLayoutPanel27.Visible = true;
                    label37.Text = "Error, this Amount Exceeds the Remaining Account Balance";

                }
                else if (withdrawCode == 4)
                {
                    tableLayoutPanel19.Visible = false;
                    tableLayoutPanel27.Visible = true;
                    label37.Text = "Error, the Machine does not have Enough Cash for this Withdrawal";

                }
                else
                {
                    toAccount.depositMoney(amount);
                    label14.Text = "You have Transferred " + amount + "$";
                    label26.Text = "The Old Balance of the Account you are Transferring from was " + fromOldBalance + "$";
                    label25.Text = "The Old Balance of the Account you are Transferring to was " + toOldBalance + "$";
                    fromNewBalance = fromAccount.getBalance();
                    label27.Text = "The New Balance of the Account you are Transferring from is " + fromNewBalance + "$";
                    toNewBalance = toAccount.getBalance();
                    label28.Text = "The New Balance of the Account you are Transferring to is " + toNewBalance + "$";
                    textBox5.Clear();
                    transId = Transaction.getTransId();
                    todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    newTransaction = new Transaction(todayDate, amount, -1, "transfer", transId, fromAccount.getId(), toAccount.getId());
                    newTransaction.saveTransaction();
                    tableLayoutPanel19.Visible = false;
                    tableLayoutPanel22.Visible = true;
                }
            }
                
           
        }

        private void button62_Click(object sender, EventArgs e)
        {
            tableLayoutPanel19.Visible = false;
            tableLayoutPanel11.Visible = true;
        }

        private void button65_Click(object sender, EventArgs e)
        {
            tableLayoutPanel22.Visible = false;
            tableLayoutPanel1.Visible = true;
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            label1.Text = "Welcom to ZZZ Bank ATM Machine";
        }

        private void button64_Click(object sender, EventArgs e)
        {
            tableLayoutPanel21.Visible = false;
            tableLayoutPanel19.Visible = true;
        }

        private void button69_Click(object sender, EventArgs e)
        {
            tableLayoutPanel25.Visible = false;
            tableLayoutPanel1.Visible = true;
            label1.Text = "Welcome to ZZZ Bank ATM Machine";
            listBox5.Items.Clear();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            tableLayoutPanel13.Visible = false;
            tableLayoutPanel9.Visible = true;
        }

        private void button46_Click(object sender, EventArgs e)
        {
            tableLayoutPanel16.Visible = false;
            tableLayoutPanel3.Visible = true;
        }

       

        

        

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button47_Click(object sender, EventArgs e)
        {
            tableLayoutPanel17.Visible = false;
            tableLayoutPanel15.Visible = true;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void button71_Click(object sender, EventArgs e)
        {
            tableLayoutPanel27.Visible = false;
            tableLayoutPanel19.Visible = true;
        }
    }
}
