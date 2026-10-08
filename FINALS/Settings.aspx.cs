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
    public partial class Settings : System.Web.UI.Page
    {

        string connDB;
        protected void Page_Load(object sender, EventArgs e)
        {
            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

            if (Session["Account_ID"] != null)
            {
                string accountId = Session["Account_ID"].ToString();

            }
            else
            {

                Response.Redirect("Login.aspx");

            }



        }



        protected void Change_Password(object sender, EventArgs e)
        {
            string accountId = Session["Account_ID"]?.ToString();

            string currentPassword = currentPassword_Input.Text.Trim();
            string newPassword = newPassword_Input.Text.Trim();

            using (var db = new SqlConnection(connDB))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT userPassword FROM USER_TABLE WHERE Account_ID = @AccountId";
                    cmd.Parameters.AddWithValue("@AccountId", int.Parse(accountId));


                    var storedPassword = cmd.ExecuteScalar();

                    if (storedPassword.ToString() == currentPassword)
                    {
                        using (var updateCmd = db.CreateCommand())
                        {
                            updateCmd.CommandType = CommandType.Text;
                            updateCmd.CommandText = "UPDATE USER_TABLE SET userPassword = @NewPassword WHERE Account_ID = @AccountId";
                            updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                            updateCmd.Parameters.AddWithValue("@AccountId", int.Parse(accountId));

                            int rowsAffected = updateCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                string script = "alert('Password changed successfully. Please log in again with your new password.');" +
                                                "window.location='Login.aspx';";
                                ClientScript.RegisterStartupScript(this.GetType(), "PasswordChangeSuccess", script, true);
                            }
                            else
                            {
                                message.Text = "An error occurred while changing the password. Please try again.";
                                return;
                            }
                        }
                    }
                    else
                    {
                        message.Text = "Incorrect Current Password.";
                        return;
                    }
                }
            }





        }



        protected void Log_Out(object sender, EventArgs e)
        {

            Response.Redirect("Login.aspx");
            Session.Abandon();

        }
    }
}