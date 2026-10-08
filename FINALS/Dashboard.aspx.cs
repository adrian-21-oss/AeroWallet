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

namespace FINALS
{
    public partial class _Default : Page
    {   

        string connDB;
        string accountId;

        protected void Page_Load(object sender, EventArgs e)
        {

            try {

                connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

                if (!IsPostBack)
                {

                    if (Session["Account_ID"] != null)
                    {
                        accountId = Session["Account_ID"].ToString();
                        LoadUserData(accountId);

                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }


            } catch (Exception ex) {

                System.Diagnostics.Trace.WriteLine("Page_Load error: " + ex.ToString());
            

            }



        }


        private void LoadUserData(string accountId)
        {
            // I will put here now

            using (var db = new SqlConnection(connDB))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                { 
                    
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT First_Name, Middle_Name, Last_Name, " +
                        "Date_Registered, Total_Current_Balance " +
                        "FROM USER_TABLE WHERE Account_ID = @AccountId";

                    cmd.Parameters.AddWithValue("@AccountId", int.Parse(accountId));

                    var ctr = cmd.ExecuteReader();


                    if (ctr.Read())
                    {
                        string firstName = ctr["First_Name"].ToString();
                        string middleName = ctr["Middle_Name"].ToString();
                        string lastName = ctr["Last_Name"].ToString();

                        Header.Text = firstName + "'s Wallet";
                        user_accountNo.Text = "" + accountId;
                        user_fullName.Text = $"{firstName} {middleName} {lastName}".Replace("  ", " ");
                        user_dateRegistered.Text = Convert.ToDateTime(ctr["Date_Registered"]).ToString("yyyy-MM-dd");
                        user_totalCurrentBalance.Text = (decimal.Parse(ctr["Total_Current_Balance"].ToString())).ToString("C");
                       

                    }

                    //NEED TO DO: ERROR HANDLING WHEN EXISTING USERNAME AND PASS, 
                    // NEXT CHANGE PASSWORD AND DISPLAY THE TABLE REPORTS, AND THEN DESIGN IMPROVING
               

                }

                db.Close();

                db.Open();

                using (var cmd2 = db.CreateCommand())
                {
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "SELECT TOP 6 (u.First_Name + ' ' + u.Last_Name) " +
                        "AS Full_Name, sr.Amount, sr.Transaction_Date, LEFT(sr.Transaction_Time, 8) AS FormattedTransaction_Time " +
                        "FROM USER_TABLE u " +
                        "INNER JOIN SR_TRANSACTION_TABLE sr ON sr.Account_ReceiveFrom = u.Account_ID " +
                        "WHERE sr.Account_SendTo = @AccountID " +
                        "ORDER BY sr.Transaction_Date DESC, sr.Transaction_Time DESC";

                    cmd2.Parameters.AddWithValue("@AccountID", int.Parse(accountId));

                    var result = cmd2.ExecuteReader();

                    RPTNotification.DataSource = result;
                    RPTNotification.DataBind();

                    if (!RPTNotification.HasControls())
                    {
                        pnlNoData.Visible = true;
                    }

                }

                db.Close();

                db.Open();

                using (var cmd3 = db.CreateCommand())
                {
                    cmd3.CommandType = CommandType.Text;
                    cmd3.CommandText = "SELECT SUM(Amount) FROM SR_TRANSACTION_TABLE WHERE Account_ReceiveFrom = @AccountID";
                        
                    cmd3.Parameters.AddWithValue("@AccountID", int.Parse(accountId));

                    var result2 = cmd3.ExecuteScalar();

                    if (result2 != null && result2 != DBNull.Value)
                    {
                        decimal totalReceived = Convert.ToDecimal(result2);
                        user_totalAmountSent.Text = totalReceived.ToString("C");
                    }
                    else
                    {
                        user_totalAmountSent.Text = " $0.00";
                    }



                }



            }





        }
    }
}