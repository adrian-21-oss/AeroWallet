using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace FINALS
{
    public partial class PasswordRecovery : System.Web.UI.Page
    {

        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try {

                if (!IsPostBack)
                {

                    string token = Request.QueryString["token"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        ValidateResetToken(token);
                    }
                    else
                    {

                        ClientScript.RegisterStartupScript(this.GetType(), "Alert1", "alert('Invalid password reset token.'); window.location.href = 'Login.aspx';", true);
                    }



                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Alert2", "alert('Something went wrong.');", true);
            }




        }






        private void ValidateResetToken(string token)
        {
                
            try
            {

                string connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

                int accountID = 0;



                //VALIDATION OF TOKEN
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    bool isTokenValid = false;
                    string query = "SELECT Account_ID FROM PasswordResets WHERE ResetToken = @Token AND ExpirationDate > GETDATE()";

                    using (var cmd = db.CreateCommand())
                    {

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@Token", token);

                        using (var res = cmd.ExecuteReader())
                        {

                            if (res.Read())
                            {
                                accountID = Convert.ToInt32(res["Account_ID"]);

                                isTokenValid = true;



                            }
                        }
                    }

                    if (!isTokenValid)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Alert5", "alert('Invalid or expired password reset token.'); window.location.href = 'Login.aspx';", true);
                        return;
                    }

                    //FETCHING USER INFORMATION FOR ACCOUNT INFORMATION DISPLAY IF TOKEN IS VALID

                    using (var cmd2 = db.CreateCommand())
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "SELECT Username, (First_Name + ' ' + ISNULL(Middle_Name, '') + ' ' + Last_Name) AS Full_Name FROM USER_TABLE WHERE Account_ID = @AccountID";
                        cmd2.Parameters.AddWithValue("@AccountID", accountID);
                        
                        using (var res2 = cmd2.ExecuteReader())
                        {

                            if (res2.Read())
                            {

                                accountIDDisplay.Text = "Account ID: " + accountID.ToString();
                                fullNameDisplay.Text = "Full Name: " + res2["Full_Name"].ToString();
                                usernameDisplay.Text = "Username: " + res2["Username"].ToString();

                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "Alert6", "alert('Something went wrong.')", true);
                            }

                        }
                        






                    }
                }


            }
            catch (NullReferenceException ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Alert7", "alert('Something went wrong.');", true);
            }




        }





        private int GetAccountIDFromToken(string token)
        {
            string connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                string query = "SELECT Account_ID FROM PasswordResets WHERE ResetToken = @Token AND ExpirationDate > GETDATE()";
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@Token", token);
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int accountID))
                    {
                        return accountID;
                    }
                }
            }
            return 0; // Return 0 if the token is invalid or expired
        }



        protected void RecoverChangePassword(object sender, EventArgs e)
        {

            Page.Validate("Recover");

            string token = Request.QueryString["token"];

            string conn = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

            if (Page.IsValid)
            {

                try { 
                    
                    string password = newPasswordInput.Text;
                    string confirm = confirmPass.Text;

                    if (confirm != password) {
                        return;
                    }

                    int accountID = GetAccountIDFromToken(token);

                    using (var connDB = new SqlConnection(conn))
                    {
                        connDB.Open();

                        string query = "UPDATE USER_TABLE SET userPassword = @Password WHERE Account_ID = @AccountID";

                        using (var cmd = new SqlCommand(query, connDB))
                        {
                            cmd.Parameters.AddWithValue("@Password", password);
                            cmd.Parameters.AddWithValue("@AccountID", accountID);
                            cmd.ExecuteNonQuery();
                            ClientScript.RegisterStartupScript(this.GetType(), "Alert9", "alert('Password updated successfully.'); window.location.href = 'Login.aspx';", true);
                        }

                        // Your database operations here
                    }

                } catch (Exception ex)
                {
                    string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Alert8", "alert('Something went wrong.');", true);
                }


            }


        }

    }
}