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


namespace FINALS
{
    public partial class Deposit : Page
    {

        string connDB;
        protected void Page_Load(object sender, EventArgs e)
        {

            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

                if (Session["Account_ID"] != null)
                {
                    string accountId = Session["Account_ID"].ToString();

                } else
                {

                    Response.Redirect("Login.aspx");

                }




                if (!IsPostBack)
                {

                }


        }

        protected void deposit(object sender, EventArgs e)
        {
            message.Text = "";


            string accountId = Session["Account_ID"]?.ToString();

            string amountText = depositAmount.Text;



            if (accountId == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (accountId != null)
            {

                decimal amount = decimal.Parse(amountText);
                int accountIdInt = int.Parse(accountId);


                bool isValid = IsValidAmount(amountText);

                if (isValid)
                {

                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();


                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT Total_Current_Balance FROM USER_TABLE WHERE Account_ID = @AccountId";
                            cmd.Parameters.AddWithValue("@AccountId", accountIdInt);

                            var ctr = cmd.ExecuteScalar();

                            int money = (ctr != null && ctr != DBNull.Value) ? Convert.ToInt32(ctr) : 0;

                            if ((money + amount) > 10000)
                            {
                                message.Text = "Deposit failed. Total balance cannot exceed 10,000. Please spend or use your money first.";
                                return;
                            } else
                            {



                                using (var cmd2 = db.CreateCommand())
                                {
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.CommandText = "UPDATE USER_TABLE SET Total_Current_Balance = Total_Current_Balance + @Amount " +
                                        "WHERE Account_ID = @AccountId;" +
                                        "INSERT INTO DW_TRANSACTION_TABLE (Account_ID, TransactionType, Amount, TransactionDate, TransactionTime)" +
                                        "VALUES (@AccountId, @TransactionType, @Amount, GETDATE(), CAST(GETDATE() AS TIME))";
                                    cmd2.Parameters.AddWithValue("@Amount", amount);
                                    cmd2.Parameters.AddWithValue("@AccountId", accountIdInt);
                                    cmd2.Parameters.AddWithValue("@TransactionType", "D");

                                    var ctr2 = cmd2.ExecuteNonQuery();


                                    if (ctr2 > 0)
                                    {

                                        string script = "alert('Deposit successful!'); window.location='Dashboard.aspx'";
                                        ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);

                                    }





                                }




                            }




                        }



                    }

                }
                else
                {

                    message.Text = "Invalid amount. Please enter a valid amount that is a multiple of 100.";
                    return;
                }


            } 


        }
                
            

        


        
        private bool IsValidAmount(string amountText)
        {
            if (decimal.TryParse(amountText, out decimal amount))
            {
                return amount%100 == 0 ? true: false;
            }
            return false;
        }






    }
}