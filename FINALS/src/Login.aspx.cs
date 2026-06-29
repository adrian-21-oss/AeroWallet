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
    public partial class Login : System.Web.UI.Page
    {

        string connDB;
        protected void Page_Load(object sender, EventArgs e)
        {

            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

        }


        protected void Login_Page(object sender, EventArgs e)
        {


            Page.Validate();

            if (Page.IsValid)
            {

                string username = accountUsername.Text;
                string password = accountPass.Text;


                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT Account_ID, Date_Registered FROM USER_TABLE WHERE username = @Username AND userPassword = @Password";
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        var result = cmd.ExecuteReader();
                        if (result.Read())
                        {
                            string accountId = result["Account_ID"].ToString();
                            string date_registered = result["Date_Registered"].ToString();

                            Session["Account_ID"] = accountId;
                            Session["Date_Registered"] = date_registered;

                            string script = "alert('Login successful!'); window.location='Dashboard.aspx'";
                            ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);

                        }
                        else
                        {

                            message.Text = "Incorrect username or password, please try again.";
                            
                            return; // Exit the method if login fails
                        }
                    }
                }








                //Response.Redirect("Dashboard.aspx");


            }



        }

        protected void Register_Page(object sender, EventArgs e)
        {


            Response.Redirect("Register.aspx");

        }
    }
}