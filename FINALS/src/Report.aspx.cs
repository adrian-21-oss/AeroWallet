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
using Microsoft.Ajax.Utilities;


namespace FINALS
{
    public partial class Report : Page
    {

        string connDB;
        string accountId;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            connDB = WebConfigurationManager.ConnectionStrings["EWallet_dbConnect"].ConnectionString;

            if (!IsPostBack) {

                grdVwAllTransactions.DataSource = new List<object>();
                grdVwAllTransactions.DataBind();


                grdVwSRTransactions.DataSource = new List<object>();
                grdVwSRTransactions.DataBind();


                grdVwDWTransactions.DataSource = new List<object>();
                grdVwDWTransactions.DataBind();

                if (Session["Account_ID"] != null)
                {
                    accountId = Session["Account_ID"].ToString();

            
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }


            }

        }


        protected void BackToDashboard(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
            return;
        }




        
        private bool checkDate(DateTime selectedDate, string p)
        {

            DateTime currentDate = DateTime.Now;

            string accountID = Session["Account_ID"].ToString();
            string userRegistered_Date_Str = "";
            DateTime userRegistered_Date;

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Date_Registered FROM USER_TABLE WHERE Account_ID = @AccountID";
                    cmd.Parameters.AddWithValue("@AccountID", accountID);

                    var result = cmd.ExecuteReader();

                    if (result.Read())
                    {
                        userRegistered_Date_Str = result["Date_Registered"].ToString();

                    }
                }
            }

            userRegistered_Date = DateTime.Parse(userRegistered_Date_Str);


            if (p == "T")
            {
                if (selectedDate < currentDate) {

                    return true;
                
                } else
                {
                    return false;

                }

            } else if (p == "F")
            {

                if (selectedDate >= userRegistered_Date) {

                    return true;

                } else
                {

                    return false;
                }

            }


            return false;

        }





        bool dateRange = false;
        bool oneDay = false;
        bool allEmpty = false;
        private string createQuery(string str_dateFrom, string str_dateTo)
        {
            string query = "";



            if (str_dateFrom.IsNullOrWhiteSpace() && str_dateTo.IsNullOrWhiteSpace()) //If BOTH IS NULL = ALL
            {
                allEmpty = true;

                query = "WITH CombinedTransactions AS " + //All Stored here

                    "(" +

                    "SELECT TransactionDate AS [DATE], TransactionType AS [TYPE], " +
                    "CASE WHEN TransactionType = 'W' THEN Amount ELSE 0 END AS [DEBIT], " +
                    "CASE WHEN TransactionType = 'D' THEN Amount ELSE 0 END AS [CREDIT], " +
                    "NULL AS [SENDTO], NULL AS [RECEIVEFROM] " +
                    "FROM DW_TRANSACTION_TABLE " +
                    "WHERE Account_ID = @AccountID AND TransactionDate BETWEEN @TransactionDateFrom AND @TransactionDateTo      " +


                    "UNION ALL    " +


                    "SELECT Transaction_Date AS [DATE], 'T' AS [TYPE], " +
                    "Amount AS [DEBIT], 0 AS [CREDIT], " +
                    "CAST(Account_SendTo AS VARCHAR) AS [SENDTO], " +
                    "CAST(Account_ReceiveFrom AS VARCHAR) AS [RECEIVEFROM] " +
                    "FROM SR_TRANSACTION_TABLE " +
                    "WHERE Account_ReceiveFrom = @AccountID AND Transaction_Date BETWEEN @TransactionDateFrom AND @TransactionDateTo      " +


                    "UNION ALL    " +


                    "SELECT Transaction_Date AS [DATE], " + "'T' AS [TYPE], " +
                    "0 AS [DEBIT], Amount AS [CREDIT], " +
                    "CAST(Account_SendTo AS VARCHAR) AS [SENDTO], " +
                    "CAST(Account_ReceiveFrom AS VARCHAR) AS [RECEIVEFROM] " +
                    "FROM SR_TRANSACTION_TABLE " +
                    "WHERE Account_SendTo = @AccountID AND Transaction_Date BETWEEN @TransactionDateFrom AND @TransactionDateTo)    " +


                    "SELECT ROW_NUMBER() OVER (ORDER BY [DATE] ASC) AS [SEQNO], " +
                    "[TYPE], [DATE], " +
                    "NULLIF([DEBIT], 0) AS [DEBIT], " +
                    "NULLIF([CREDIT], 0) AS [CREDIT], " +
                    "SUM([CREDIT] - [DEBIT]) OVER (ORDER BY [DATE] ASC ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS [BALANCE], " +
                    "[SENDTO], " +
                    "[RECEIVEFROM] " +
                    "FROM CombinedTransactions " +
                    "ORDER BY [SEQNO] ASC;";

                return query;

            } else if (!(str_dateFrom.IsNullOrWhiteSpace()) && (str_dateTo.IsNullOrWhiteSpace())) //IF DATETO IS NULL = FROM TO PRESENT
            {

                message2.Text = "Date From and Date To should not left empty. 1";
                return query;



            } else if ((str_dateFrom.IsNullOrWhiteSpace()) && !(str_dateTo.IsNullOrWhiteSpace())) //IF DATEFROM IS NULL = START TO DATETO
            {

                message2.Text = "Date From and Date To should not left empty. 2";
                return query;


            }
            else  // SELECT THE RANGES
            {

                DateTime dateF = DateTime.Parse(str_dateFrom);
                DateTime dateT = DateTime.Parse(str_dateTo);




                bool isValid = checkDate(dateT, "T");
                bool isValid2 = checkDate(dateF, "F");

                if (isValid && isValid2) {
                    if (dateF > dateT)
                    {
                        message2.Text = "Invalid date range. Please ensure that the 'From' date is earlier than the 'To' date.";
                        return message2.Text;
                    }
                    else if (dateF == dateT)
                    {
                        dateRange = false;
                        oneDay = true;
                        query = "WITH CombinedTransactions AS " + //All Stored here

                        "(" +

                        "SELECT TransactionDate AS [DATE], TransactionType AS [TYPE], " +
                        "CASE WHEN TransactionType = 'W' THEN Amount ELSE 0 END AS [DEBIT], " +
                        "CASE WHEN TransactionType = 'D' THEN Amount ELSE 0 END AS [CREDIT], " +
                        "NULL AS [SENDTO], NULL AS [RECEIVEFROM] " +
                        "FROM DW_TRANSACTION_TABLE " +
                        "WHERE Account_ID = @AccountID AND CAST(TransactionDate AS DATE) = CAST(@TransactionDate AS DATE)   " +


                        "UNION ALL    " +


                        "SELECT Transaction_Date AS [DATE], 'T' AS [TYPE], " +
                        "Amount AS [DEBIT], 0 AS [CREDIT], " +
                        "CAST(Account_SendTo AS VARCHAR) AS [SENDTO], " +
                        "CAST(Account_ReceiveFrom AS VARCHAR) AS [RECEIVEFROM] " +
                        "FROM SR_TRANSACTION_TABLE " +
                        "WHERE Account_ReceiveFrom = @AccountID AND CAST(Transaction_Date AS DATE) = CAST(@TransactionDate AS DATE)   " +


                        "UNION ALL    " +


                        "SELECT Transaction_Date AS [DATE], " + "'T' AS [TYPE], " +
                        "0 AS [DEBIT], Amount AS [CREDIT], " +
                        "CAST(Account_SendTo AS VARCHAR) AS [SENDTO], " +
                        "CAST(Account_ReceiveFrom AS VARCHAR) AS [RECEIVEFROM] " +
                        "FROM SR_TRANSACTION_TABLE " +
                        "WHERE Account_SendTo = @AccountID AND CAST(Transaction_Date AS DATE) = CAST(@TransactionDate AS DATE))   " +


                        "SELECT ROW_NUMBER() OVER (ORDER BY [DATE] ASC) AS [SEQNO], " +
                        "[TYPE], [DATE], " +
                        "NULLIF([DEBIT], 0) AS [DEBIT], " +
                        "NULLIF([CREDIT], 0) AS [CREDIT], " +
                        "SUM([CREDIT] - [DEBIT]) OVER (ORDER BY [DATE] ASC ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS [BALANCE], " +
                        "[SENDTO], " +
                        "[RECEIVEFROM] " +
                        "FROM CombinedTransactions " +
                        "ORDER BY [SEQNO] ASC;";



                        return query;



                    }
                    else if (dateF <= dateT)
                    {
                        dateRange = true;
                        oneDay = false;

                        query = "WITH CombinedTransactions AS " + //All Stored here

                        "(" +

                        "SELECT TransactionDate AS [DATE], TransactionType AS [TYPE], " +
                        "CASE WHEN TransactionType = 'W' THEN Amount ELSE 0 END AS [DEBIT], " +
                        "CASE WHEN TransactionType = 'D' THEN Amount ELSE 0 END AS [CREDIT], " +
                        "NULL AS [SENDTO], NULL AS [RECEIVEFROM] " +
                        "FROM DW_TRANSACTION_TABLE " +
                        "WHERE Account_ID = @AccountID AND TransactionDate BETWEEN @TransactionDateFrom AND @TransactionDateTo  " +


                        "UNION ALL    " +


                        "SELECT Transaction_Date AS [DATE], 'T' AS [TYPE], " +
                        "Amount AS [DEBIT], 0 AS [CREDIT], " +
                        "CAST(Account_SendTo AS VARCHAR) AS [SENDTO], " +
                        "CAST(Account_ReceiveFrom AS VARCHAR) AS [RECEIVEFROM] " +
                        "FROM SR_TRANSACTION_TABLE " +
                        "WHERE Account_ReceiveFrom = @AccountID AND Transaction_Date BETWEEN @TransactionDateFrom AND @TransactionDateTo    " +


                        "UNION ALL    " +


                        "SELECT Transaction_Date AS [DATE], " + "'T' AS [TYPE], " +
                        "0 AS [DEBIT], Amount AS [CREDIT], " +
                        "CAST(Account_SendTo AS VARCHAR) AS [SENDTO], " +
                        "CAST(Account_ReceiveFrom AS VARCHAR) AS [RECEIVEFROM] " +
                        "FROM SR_TRANSACTION_TABLE " +
                        "WHERE Account_SendTo = @AccountID AND Transaction_Date BETWEEN @TransactionDateFrom AND @TransactionDateTo)  " +


                        "SELECT ROW_NUMBER() OVER (ORDER BY [DATE] ASC) AS [SEQNO], " +
                        "[TYPE], [DATE], " +
                        "NULLIF([DEBIT], 0) AS [DEBIT], " +
                        "NULLIF([CREDIT], 0) AS [CREDIT], " +
                        "SUM([CREDIT] - [DEBIT]) OVER (ORDER BY [DATE] ASC ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS [BALANCE], " +
                        "[SENDTO], " +
                        "[RECEIVEFROM] " +
                        "FROM CombinedTransactions " +
                        "ORDER BY [SEQNO] ASC;";


                        return query;






                    }



                } else
                {

                    message2.Text = "Please double check the dates.";

                }
               


            }








                return query;
        }





        bool dateRange2 = false;
        bool oneDay2 = false;
        bool allEmpty2 = false;
        string query_2 = "";
        private string createQuery_DW(string str_dateFrom, string str_dateTo, string queryType)
        {
            string query = "";



            if (str_dateFrom.IsNullOrWhiteSpace() && str_dateTo.IsNullOrWhiteSpace()) //If BOTH IS NULL = ALL
            {
                allEmpty2 = true;

                if (queryType == "D")
                {
                    query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                    " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionType = 'D'";

                } else if (queryType == "W")
                {
                    query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                    " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionType = 'W'";


                } else if (queryType == "All")
                {
                    query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                   " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID";
                }




                    return query;

            }
            else if (!(str_dateFrom.IsNullOrWhiteSpace()) && (str_dateTo.IsNullOrWhiteSpace())) //IF DATETO IS NULL = FROM TO PRESENT
            {

                message3.Text = "Date From and Date To should not left empty. 1";
                return query;



            }
            else if ((str_dateFrom.IsNullOrWhiteSpace()) && !(str_dateTo.IsNullOrWhiteSpace())) //IF DATEFROM IS NULL = START TO DATETO
            {

                message3.Text = "Date From and Date To should not left empty. 2";
                return query;


            }
            else  // SELECT THE RANGES
            {

                DateTime dateF = DateTime.Parse(str_dateFrom);
                DateTime dateT = DateTime.Parse(str_dateTo);




                bool isValid = checkDate(dateT, "T");
                bool isValid2 = checkDate(dateF, "F");

                if (isValid && isValid2)
                {
                    if (dateF > dateT)
                    {
                        message3.Text = "Invalid date range. Please ensure that the 'From' date is earlier than the 'To' date.";
                        return message3.Text;
                    }
                    else if (dateF == dateT)
                    {
                        dateRange2 = false;
                        oneDay2 = true;



                        if (queryType == "W")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                                " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionType = 'W' AND TransactionDate >= @TransactionDate AND TransactionDate < @NextDay";


                        }
                        else if (queryType == "D")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                                " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionType = 'D' AND TransactionDate >= @TransactionDate AND TransactionDate < @NextDay";



                        } else if (queryType == "All")
                        {


                            query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC, DW_Transaction_ID ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                                " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionDate >= @TransactionDate AND TransactionDate < @NextDay";



                        }



                        return query;



                    }
                    else if (dateF <= dateT)
                    {
                        dateRange2 = true;
                        oneDay2 = false;




                        if (queryType == "W")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                                " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionType = 'W' AND TransactionDate BETWEEN @TransactionDateFrom AND @TransactionDateTo";



                        }
                        else if (queryType == "D")
                        {


                            query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                                " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionType = 'D' AND TransactionDate BETWEEN @TransactionDateFrom AND @TransactionDateTo";


                        }
                        else if (queryType == "All")
                        {


                            query = "SELECT ROW_NUMBER() OVER (ORDER BY TransactionDate ASC) AS [SEQNO], TransactionType AS [TYPE], TransactionDate AS [DATE], Amount AS [AMOUNT]" +
                                " FROM DW_TRANSACTION_TABLE WHERE Account_ID = @AccountID AND TransactionDate BETWEEN @TransactionDateFrom AND @TransactionDateTo";



                        }




                        return query;






                    }



                }
                else
                {

                    message3.Text = "Please double check the dates.";

                }



            }








            return query;
        }





        bool dateRange3 = false;
        bool oneDay3 = false;
        bool allEmpty3 = false;
        private string createQuery_SR(string str_dateFrom, string str_dateTo, string queryType)
        {
            string query = "";



            if (str_dateFrom.IsNullOrWhiteSpace() && str_dateTo.IsNullOrWhiteSpace()) //If BOTH IS NULL = ALL
            {
                allEmpty3 = true;
                oneDay3 = false;
                dateRange3 = false;

                if (queryType == "S")
                {
                    query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                    " FROM SR_TRANSACTION_TABLE WHERE Account_ReceiveFrom = @AccountID";

                }
                else if (queryType == "R")
                {
                    query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                    " FROM SR_TRANSACTION_TABLE WHERE Account_SendTo = @AccountID";


                }
                else if (queryType == "All")
                {
                    query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                   " FROM SR_TRANSACTION_TABLE WHERE Account_SendTo = @AccountID OR Account_ReceiveFrom = @AccountID";
                }




                return query;

            }
            else if (!(str_dateFrom.IsNullOrWhiteSpace()) && (str_dateTo.IsNullOrWhiteSpace())) //IF DATETO IS NULL = FROM TO PRESENT
            {

                message4.Text = "Date From and Date To should not left empty. 1";
                return query;



            }
            else if ((str_dateFrom.IsNullOrWhiteSpace()) && !(str_dateTo.IsNullOrWhiteSpace())) //IF DATEFROM IS NULL = START TO DATETO
            {

                message4.Text = "Date From and Date To should not left empty. 2";
                return query;


            }
            else  // SELECT THE RANGES
            {

                DateTime dateF = DateTime.Parse(str_dateFrom);
                DateTime dateT = DateTime.Parse(str_dateTo);




                bool isValid = checkDate(dateT, "T");
                bool isValid2 = checkDate(dateF, "F");

                if (isValid && isValid2)
                {
                    if (dateF > dateT)
                    {
                        message4.Text = "Invalid date range. Please ensure that the 'From' date is earlier than the 'To' date.";
                        return message4.Text;
                    }
                    else if (dateF == dateT)
                    {
                        allEmpty3 = false;
                        oneDay3 = true;
                        dateRange3 = false;

                        if (queryType == "S")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                                " FROM SR_TRANSACTION_TABLE WHERE Account_ReceiveFrom = @AccountID AND Transaction_Date >= @TransactionDate AND Transaction_Date < @NextDay";



                        }
                        else if (queryType == "R")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                                " FROM SR_TRANSACTION_TABLE WHERE Account_SendTo = @AccountID AND Transaction_Date >= @TransactionDate AND Transaction_Date < @NextDay";
                        }
                        else if (queryType == "All")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                                " FROM SR_TRANSACTION_TABLE WHERE Transaction_Date >= @TransactionDate AND Transaction_Date < @NextDay";
                        }

                        return query;



                    }
                    else if (dateF <= dateT)
                    {
                        dateRange3 = true;
                        oneDay3 = false;
                        allEmpty3 = false;
                      

                        if (queryType == "S")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                                " FROM SR_TRANSACTION_TABLE WHERE Account_ReceiveFrom = @AccountID AND Transaction_Date BETWEEN @TransactionDateFrom AND @TransactionDateTo";



                        }
                        else if (queryType == "R")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                                " FROM SR_TRANSACTION_TABLE WHERE Account_SendTo = @AccountID AND Transaction_Date BETWEEN @TransactionDateFrom AND @TransactionDateTo";
                        }
                        else if (queryType == "All")
                        {

                            query = "SELECT ROW_NUMBER() OVER (ORDER BY Transaction_Date ASC) AS [SEQNO], Transaction_Date AS [DATESENT], Amount AS [AMOUNT], Account_SendTo AS [SENDTO], Account_ReceiveFrom AS [RECEIVEFROM]" +
                                " FROM SR_TRANSACTION_TABLE WHERE (Account_SendTo = @AccountID OR Account_ReceiveFrom = @AccountID) AND Transaction_Date BETWEEN @TransactionDateFrom AND @TransactionDateTo";
                        }

                        return query;






                    }



                }
                else
                {

                    message4.Text = "Please double check the dates.";

                }



            }




            return query;
        }







        protected void DisplayTable(object sender, EventArgs e)
        {

            string str_dateFrom = dateFrom.Text.Trim();
            string str_dateTo = dateTo.Text.Trim();


            DateTime date_reg = DateTime.Parse(Session["Date_Registered"].ToString());
            DateTime current_date = DateTime.Now;




            string query = createQuery(str_dateFrom, str_dateTo);

            if (query.IsNullOrWhiteSpace()) {
                return;
            }


            string accountID = Session["Account_ID"].ToString();


            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@AccountID", accountID);
                    
                    if (oneDay)
                    {
                        cmd.Parameters.AddWithValue("@TransactionDate", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date);

                    } else if (dateRange)
                    {

                        cmd.Parameters.AddWithValue("@TransactionDateFrom", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date);
                        cmd.Parameters.AddWithValue("@TransactionDateTo", (DateTime.Parse(str_dateTo.TrimStart('0'))).Date.AddDays(1));


                    }
                    else if (allEmpty)
                    {

                        cmd.Parameters.AddWithValue("@TransactionDateFrom", date_reg);
                        cmd.Parameters.AddWithValue("@TransactionDateTo", current_date.AddDays(1));


                    }

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    grdVwAllTransactions.DataSource = dt;
                    grdVwAllTransactions.DataBind();

                    int ctr = grdVwAllTransactions.Rows.Count;
                    if (ctr == 0)
                    {
                        Response.Write("<script>alert('No records found')</script>");
                    }

                }
            }








        }







        protected void DisplayTable2(object sender, EventArgs e)
        {

            string str_dateFrom = dateFrom2.Text.Trim();
            string str_dateTo = dateTo2.Text.Trim();
            string qType = DW_DDL.SelectedValue;

            DateTime date_reg = DateTime.Parse(Session["Date_Registered"].ToString());
            DateTime current_date = DateTime.Now;




            string query = createQuery_DW(str_dateFrom, str_dateTo, qType);



            if (query.IsNullOrWhiteSpace())
            {
                return;
            }


            string accountID = Session["Account_ID"].ToString();


            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@AccountID", accountID);

                    if (oneDay2)
                    {
                        cmd.Parameters.AddWithValue("@TransactionDate", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date);
                        cmd.Parameters.AddWithValue("@NextDay", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date.AddDays(1));

                    }
                    else if (dateRange2)
                    {

                        cmd.Parameters.AddWithValue("@TransactionDateFrom", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date);
                        cmd.Parameters.AddWithValue("@TransactionDateTo", (DateTime.Parse(str_dateTo.TrimStart('0'))).Date.AddDays(1));


                    }
                    else if (allEmpty2)
                    {
                        
                        cmd.Parameters.AddWithValue("@TransactionDateFrom", date_reg);
                        cmd.Parameters.AddWithValue("@TransactionDateTo", current_date.AddDays(1));


                    }

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt); 
                    grdVwDWTransactions.DataSource = dt;
                    grdVwDWTransactions.DataBind();

                    int ctr = grdVwDWTransactions.Rows.Count;
                    if (ctr == 0)
                    {
                        Response.Write("<script>alert('No records found')</script>");
                    }

                }
            }








        }





        protected void DisplayTable3(object sender, EventArgs e)
        {

            string str_dateFrom = dateFrom3.Text.Trim();
            string str_dateTo = dateTo3.Text.Trim();
            string qType = SR_DDL.SelectedValue;

            DateTime date_reg = DateTime.Parse(Session["Date_Registered"].ToString());
            DateTime current_date = DateTime.Now;




            string query = createQuery_SR(str_dateFrom, str_dateTo, qType);



            if (query.IsNullOrWhiteSpace())
            {
                return;
            }


            string accountID = Session["Account_ID"].ToString();


            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@AccountID", accountID);

                    if (oneDay3)
                    {
                        cmd.Parameters.AddWithValue("@TransactionDate", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date);
                        cmd.Parameters.AddWithValue("@NextDay", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date.AddDays(1));

                    }
                    else if (dateRange3)
                    {

                        cmd.Parameters.AddWithValue("@TransactionDateFrom", (DateTime.Parse(str_dateFrom.TrimStart('0'))).Date);
                        cmd.Parameters.AddWithValue("@TransactionDateTo", (DateTime.Parse(str_dateTo.TrimStart('0'))).Date.AddDays(1));


                    }
                    else if (allEmpty3)
                    {

                        cmd.Parameters.AddWithValue("@TransactionDateFrom", date_reg);
                        cmd.Parameters.AddWithValue("@TransactionDateTo", current_date.AddDays(1));


                    }

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt); 
                    grdVwSRTransactions.DataSource = dt;
                    grdVwSRTransactions.DataBind();

                    int ctr = grdVwSRTransactions.Rows.Count;
                    if (ctr == 0)
                    {
                        Response.Write("<script>alert('No records found')</script>");
                    }

                }
            }








        }

    }
}