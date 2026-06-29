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
    public partial class Register : System.Web.UI.Page
    {

        string connDB;
        protected void Page_Load(object sender, EventArgs e)
        {

            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

        }

        private string NameCleaning(string name)
        {

            name = name.Trim();

            for (int i = 0; i < name.Length; i++) {

                if (i == 0)
                {
                    name[i].ToString().ToUpper();

                } else
                {

                    name[i].ToString().ToLower();

                }


            }
                       
            
            return name;

        }



        protected void Create_Account(object sender, EventArgs e)
        {   

            Page.Validate();

            if (Page.IsValid)
            {

                string firstName = NameCleaning(firstN.Text);
                string middleName = NameCleaning(middleN.Text);
                string lastName = NameCleaning(lastN.Text);

                string phoneNumber = phoneNum.Text;
                string email = email_Input.Text;

                string username = accUsername_Input.Text;
                string password = accPassword_Input.Text;

                using (var db = new SqlConnection(connDB))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO USER_TABLE (First_Name, Middle_Name, Last_Name, " +
                            "Email, PhoneNumber, Date_Registered, Total_Current_Balance, Username, userPassword)" +
                            "VALUES (@firstName, @middleName, @lastName, @email, @phoneNumber, " +
                            "GETDATE(), @balance, @username, @password)";

                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@middleName", middleName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@balance", decimal.Parse("0"));
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);


                        var ctr = cmd.ExecuteNonQuery();


                        if (ctr>0)
                        {

                            cmd.Parameters.Clear();


                            cmd.CommandText = "SELECT Account_ID, Date_Registered FROM USER_TABLE WHERE " +
                                "Username = @username AND userPassword = @password";
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);


                            var result = cmd.ExecuteReader();


                            if (result.Read())
                            {



                                Session["Account_ID"] = result["Account_ID"].ToString();
                                Session["Date_Registered"] = result["Date_Registered"] .ToString();


                                string script = "alert('Account created successfully'); window.location='Dashboard.aspx';";
                                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);

                            }



                        }

                        db.Close();

                        

                        


                    }

                }



            }

        }


        protected void backtoLogin(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}