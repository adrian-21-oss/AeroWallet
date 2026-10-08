using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

using System.Net;
using System.Net.Mail;



using System.Web.Services;

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


            Page.Validate("Login");

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
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = password;
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

                            ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", "alert('Incorrect username or password, please try again.');", true);
                     
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






        protected void btnSendReset_Click(object sender, EventArgs e)
        {

            Page.Validate("ResetGroup");

            if (Page.IsValid)
            {
                string userEmail = txtResetEmail.Text.Trim();

                // 1. Validate if user exists in database
                bool userExists = CheckIfUserExists(userEmail); // Checked

                if (userExists)
                {
                    // 2. Generate password reset token & store in DB
                    string token = Guid.NewGuid().ToString();
                    SaveResetTokenToDatabase(userEmail, token);   // Checked
                        
                    // 3. Send email with reset URL (e.g., ...)
                    SendPasswordResetEmail(userEmail, token);

                    // 4. Update UI feedback inside modal
                    lblResetStatus.Text = "A password reset link has been sent to your email.";
                    lblResetStatus.CssClass = "alert alert-success d-block";
                    lblResetStatus.Visible = true;

                    // Hide input field after successful submit
                    divEmailInput.Visible = false;
                    btnSendReset.Visible = false;
                }
                else
                {
                    lblResetStatus.Text = "Email address not found.";
                    lblResetStatus.CssClass = "alert alert-danger d-block";
                    lblResetStatus.Visible = true;
                }
            }
        }

        private bool CheckIfUserExists(string email)
        {
        

            string query = "SELECT Account_ID FROM USER_TABLE WHERE Email = @Email;";

            using (var db = new SqlConnection(connDB))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@Email", email);
                    
                    var res = cmd.ExecuteReader();

                    if (res.Read()) {



                        return !string.IsNullOrEmpty(email); // Only return true if email is found in the database


                    }
                }

                db.Close();

            }

            return false;


        }

        private void SaveResetTokenToDatabase(string email, string token)
        {
        
            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

            int AccountId = 0;
            string clientIP = Request.UserHostAddress; // Get the client's IP address

            using (var db = new SqlConnection(connDB))
            {
                db.Open();

                using (var cmd1 = db.CreateCommand())
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "SELECT Account_ID FROM USER_TABLE WHERE Email = @Email";
                    cmd1.Parameters.AddWithValue("@Email", email);

                    var res = cmd1.ExecuteReader();

                    if (res.Read())
                    {
                        AccountId = Convert.ToInt32(res["Account_ID"]);
                    }



                }
                
                db.Close();
                db.Open();

                using (var cmd2 = db.CreateCommand())
                {
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "INSERT INTO PasswordResets (Account_ID, ResetToken, ExpirationDate, IPAddress) VALUES (@Account_ID, @ResetToken, @ExpirationDate, @IPAddress)";
                    cmd2.Parameters.AddWithValue("@Account_ID", AccountId);
                    cmd2.Parameters.AddWithValue("@ResetToken", token);
                    cmd2.Parameters.AddWithValue("@ExpirationDate", DateTime.Now.AddMinutes(10)); // Token valid for 1 hour
                    cmd2.Parameters.AddWithValue("@IPAddress", clientIP);
                    cmd2.ExecuteNonQuery();
                }

                db.Close();
            }
            // Save token and expiration timestamp to database

        }



        private void SendPasswordResetEmail(string recipientEmail, string token)
        {

            string myEmail = WebConfigurationManager.AppSettings["SmtpEmail"];
            string myPassword = WebConfigurationManager.AppSettings["SmtpPass"];

            string resetLink = $"https://aerowallet.runasp.net/PasswordRecovery.aspx?token={token}";


            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // Ensure TLS 1.2 is used for secure email sending


                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(myEmail, myPassword);// LOGGING YOUR ACCOUNT TO BE USED
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;


                    using (MailMessage mail = new MailMessage()) {

                        mail.From = new MailAddress(myEmail, "AeroWallet Website Admin");
                        mail.To.Add(recipientEmail);
                        mail.Subject = "Password Reset Request";

                        mail.IsBodyHtml = true;
                        mail.Body = $@"
                        <h3>Password Reset Request</h3>
                        <p>We received a request to reset your password. Click the link below to set a new password: </p>
                        <p><a href='{resetLink}'>Reset My Password</a></p>
                        <br/>
                        <p><small>This link will expire in 10 minutes. If you did not request this, please ignore this email.</small></p>";

                        System.Diagnostics.Debug.WriteLine($"Email: {myEmail}");
                        System.Diagnostics.Debug.WriteLine($"Pass Length: {myPassword?.Length}");

                        smtp.Send(mail);


                    } // CREATING A MESSAGE TO BE SENT


                } // Code using System.Net.Mail or an email provider API (SendGrid, Mailgun, etc.)

            }
            catch (SmtpException ex)
            {
                string realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                System.Diagnostics.Debug.WriteLine($"SMTP Error: {realError}");
                throw;
            }


        }










    }
}