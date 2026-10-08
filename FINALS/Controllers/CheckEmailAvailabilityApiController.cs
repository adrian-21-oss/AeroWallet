using System.Web.Configuration;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;


namespace FINALS.Controllers
{
	public class CheckEmailAvailabilityApiController : ApiController
	{
		[HttpGet]
		public IHttpActionResult Get(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return BadRequest("Email query parameter is missing.");
			}

			string connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;
			bool isExist = false;
			string query = "SELECT Account_ID FROM USER_TABLE WHERE Email = @Email;";

			using (var db = new SqlConnection(connDB))
			{
				db.Open();

				using (var cmd = db.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = query;

					cmd.Parameters.AddWithValue("@Email", email);

					var res = cmd.ExecuteScalar();

					if (res != null) {
						isExist = true;
					}


				}

			}


			return Ok(new 
			{ 
				success = true,
				email = email,
				isRegistered = isExist,
				message = isExist ? "Email is available." : "Email is not available."

			});




		}
		


	}
}