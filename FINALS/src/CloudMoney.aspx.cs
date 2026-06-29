using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace FINALS.src
{
    public partial class CloudMoney : System.Web.UI.Page
    {

        string connDB;
        string accountId;
        protected void Page_Load(object sender, EventArgs e)
        {
            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

            if (!IsPostBack)
            {
                accountId = Session["Account_ID"]?.ToString();

                if (!rfv_accountNumber_Input.IsValid)
                {
                    message.Text = "";

                };



                if (accountId == null)
                {
                    Response.Redirect("Login.aspx");
                }


            }



        }


        protected void Check_ID(object sender, EventArgs e)
        {
            message.Text = "";



            if (accountNumber_Input.Text.IsNullOrWhiteSpace())
            {
                message.Text = "Please enter account number.";
                return;
            }


            string recipientAccountIdText = accountNumber_Input.Text.Trim();
            int recipientAccountId = int.Parse(recipientAccountIdText);

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Account_ID, First_Name, Middle_Name, Last_Name " +
                    "FROM USER_TABLE WHERE Account_ID = @AccountId";


                    cmd.Parameters.AddWithValue("@AccountId", recipientAccountId);

                    var result = cmd.ExecuteReader();

                    if (result.Read())
                    {

                        int accountId_2 = int.Parse(Session["Account_ID"]?.ToString());

                        if (recipientAccountId == accountId_2)
                        {

                            message.Text = "You can't send money to your own account.";

                            return;

                        }
                      

                        int accountIdInt = int.Parse(result["Account_ID"].ToString());
                        string fName = result["First_Name"].ToString();
                        string mName = result["Middle_Name"].ToString();
                        string lName = result["Last_Name"].ToString();

                        AccountIDDisplay.Text = $"Account Number: {accountIdInt}";
                        NameDisplay.Text = $"Recipient Name: " + $"{fName} {mName} {lName}".Replace("  ", " ");

                    }
                    else
                    {
                        message.Text = "Account does not exist.";

                    }


                }
            }





        }








        protected void SendMoney(object sender, EventArgs e)
        {
            string accountId = Session["Account_ID"]?.ToString();


            string sendAmount = sendMoney_Input.Text.Trim();
            string accountPassword = accountPass.Text.Trim();


            bool isAmountSufficient = CheckAmount(decimal.Parse(sendAmount), accountId);
            bool isAmountValid = IsValidAmount(sendAmount);

            bool isPasswordValid = PasswordValidation(accountPassword, accountId);

            if (isAmountSufficient && isAmountValid && isPasswordValid)
            {
                SendingMoney(sendAmount);
            } else
            {
                message2.Text = $"Invalid input. Please check the amount and password. {isAmountSufficient} {isAmountValid} {isPasswordValid}";
            }



        }


        private void SendingMoney(string sendAmount)
        {
            int accountReceiver_ID = int.Parse(accountNumber_Input.Text.Trim());
            int accountSender_ID = int.Parse(Session["Account_ID"]?.ToString());


            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE USER_TABLE SET Total_Current_Balance = Total_Current_Balance + @Amount " +
                        "WHERE Account_ID = @ReceiverAccountId;" +
                        "UPDATE USER_TABLE SET Total_Current_Balance = Total_Current_Balance - @Amount " +
                        "WHERE Account_ID = @SenderAccountId;" +
                        "INSERT INTO SR_TRANSACTION_TABLE (Transaction_Date, Transaction_Time, Amount, Account_SendTo, Account_ReceiveFrom)" +
                        "VALUES (GETDATE(), CAST(GETDATE() AS TIME), @Amount, @ReceiverAccountId, @SenderAccountId);";

                    cmd.Parameters.AddWithValue("@SenderAccountId", accountSender_ID);
                    cmd.Parameters.AddWithValue("@ReceiverAccountId", accountReceiver_ID);
                    cmd.Parameters.AddWithValue("@Amount", decimal.Parse(sendAmount));


                    var ctr = cmd.ExecuteNonQuery();

                    if (ctr > 0)
                    {

                        string script = "alert('Money sent successfully!'); window.location='Dashboard.aspx'";
                        ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);

                    }
                    else
                    {
                        message2.Text = "Failed to send money. Please try again.";
                    }
                }
            }



    

        }









        private bool PasswordValidation(string accountPassword, string accountId)
        {


            using (var db = new SqlConnection(connDB))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT userPassword FROM USER_TABLE WHERE Account_ID = @AccountId";
                    cmd.Parameters.AddWithValue("@AccountId", int.Parse(accountId));

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {

                        return result.ToString().Trim() == accountPassword;
                    }
                    else
                    {
                        return false;


                    }


                }    



            }


        }





        private bool IsValidAmount(string amountText)
        {
            if (decimal.TryParse(amountText, out decimal amount))
            {
                return amount % 100 == 0 ? true : false;
            }
            return false;
        }




        private bool CheckAmount(decimal amount, string accountId)
        {   

            using (var db = new SqlConnection(connDB))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Total_Current_Balance FROM USER_TABLE WHERE Account_ID = @AccountId";

                    cmd.Parameters.AddWithValue("@AccountId", int.Parse(accountId));

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {

                        if (decimal.TryParse(result.ToString(), out decimal currentBalance))
                        {
                            return currentBalance >= amount ? true : false;
                        }
                        return false;
                    }
                    else
                    {

                        return false;

                    }

                }
            }





        }
    }
}