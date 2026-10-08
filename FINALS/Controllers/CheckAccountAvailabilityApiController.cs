using System.Web.Configuration;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;


namespace FINALS.Controllers
{

    public class AccountAvailabilityResult
    {
        public bool success { get; set; }
        public string accountId { get; set; }
        public string accountFullName { get; set; }
        public bool isRegistered { get; set; }
        public string message { get; set; }
    }

    public class CheckAccountAvailabilityApiController : ApiController
    {

        [HttpGet]
        public IHttpActionResult Get(string accountId)
        {

            if (string.IsNullOrWhiteSpace(accountId))
            {
                return BadRequest("Account ID is required");
            }


            if (!int.TryParse(accountId, out int int_accountId))
            {
                return Ok(new
                {
                    success = false,
                    accountId = "---",
                    accountFullName = "---",
                    isRegistered = false,
                    message = "Invalid Account ID format."
                });
            }




            string fullName = "";
            string str_accountId = "";

            bool isExist = false;


            string connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

            using (var db = new SqlConnection(connDB))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Account_ID, (First_Name + ' ' + ISNULL(Middle_Name, '') + ' ' + Last_Name) AS Full_Name FROM USER_TABLE WHERE Account_ID = @Account_ID;";

                    cmd.Parameters.AddWithValue("@Account_ID", int_accountId);


                    var res = cmd.ExecuteReader();

                    if (res.Read())
                    {
                        isExist = true;
                        fullName = res["Full_Name"].ToString();
                        str_accountId = res["Account_ID"].ToString();
                    }

                }

            }

            return Ok(new AccountAvailabilityResult{
                success = true,
                accountId = isExist ? str_accountId : "---",
                accountFullName = isExist ? fullName : "---",
                isRegistered = isExist,
                message = isExist ? "Account ID is available." : "Account ID is not available."

            });

        }











    }

}