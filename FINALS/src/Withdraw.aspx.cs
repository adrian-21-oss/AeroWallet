using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FINALS.src
{
    public partial class Withdraw : System.Web.UI.Page
    {
        string connDB;
        string accountId;


        protected void Page_Load(object sender, EventArgs e)
        {
            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

            if (!IsPostBack)
            {
                accountId = Session["Account_ID"]?.ToString();

                if (accountId != null)
                {
                    DisplayCurrentBalance(accountId);
                } else
                {
                    Response.Redirect("Login.aspx");
                } 


            }
            

        }

        protected void withdraw(object sender, EventArgs e)
        {


            string accountId = Session["Account_ID"]?.ToString();

            string amountText = withdrawalAmount.Text;




            if (accountId == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (accountId != null)
            {
                decimal currentBalance = 0;
                decimal amount = decimal.Parse(amountText);
                int accountIdInt = int.Parse(accountId);


                using (var db= new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT Total_Current_Balance FROM USER_TABLE WHERE Account_ID = @AccountId";
                        cmd.Parameters.AddWithValue("@AccountId", accountIdInt);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            currentBalance = Convert.ToDecimal(result);
                        }
                    }


                }


                    bool isValid = IsValidAmount(amountText, currentBalance);
                    bool isValid_2 = IsValidAmount_Divisible(amountText);


                if (isValid && isValid_2)
                {

                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();

                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE USER_TABLE SET Total_Current_Balance = Total_Current_Balance - @Amount " +
                                "WHERE Account_ID = @AccountId;" +
                                "INSERT INTO DW_TRANSACTION_TABLE (Account_ID, TransactionType, Amount, TransactionDate, TransactionTime)" +
                                "VALUES (@AccountId, @TransactionType, @Amount, GETDATE(), CAST(GETDATE() AS TIME))";
                            cmd.Parameters.AddWithValue("@Amount", amount);
                            cmd.Parameters.AddWithValue("@AccountId", accountIdInt);
                            cmd.Parameters.AddWithValue("@TransactionType", "W");

                            var ctr = cmd.ExecuteNonQuery();


                            if (ctr > 0)
                            {

                                string script = "alert('Withdrawal successful!'); window.location='Dashboard.aspx'";
                                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);

                            }





                        }

                    }

                }
                else
                {

                    message.Text = "Invalid amount. Please enter a valid amount.";
                    return;
                }


            }



        }


        private bool IsValidAmount(string amountText, decimal currentBalance)
        {
            if (decimal.TryParse(amountText, out decimal amount))
            {
                return currentBalance >= amount ? true: false;
            }
            return false;
        }


        private bool IsValidAmount_Divisible(string amountText)
        {
            if (decimal.TryParse(amountText, out decimal amount))
            {
                return amount % 100 == 0 ? true : false;
            }
            return false;
        }



        private void DisplayCurrentBalance(string accountID) {

            int accountIDInt = int.Parse(accountID);

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Total_Current_Balance FROM USER_TABLE WHERE Account_ID = @AccountID";
                    cmd.Parameters.AddWithValue("@AccountID", accountIDInt);

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        decimal balance = Convert.ToDecimal(result);
                        yourBalance.Text = $"Current Balance: {balance:C}";
                    }
                    else
                    {
                        yourBalance.Text = "Current Balance: N/A";
                    }
                }
        
            }
        
        }
    }
}