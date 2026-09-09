using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using Irony;
using System.Configuration;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;

//partial class CheckLogin : System.Web.UI.Page
public partial class Checklogin : System.Web.UI.Page
{
    private SqlConnection Conn;
    private SqlCommand Comm;
    private DataTable Dt;
    private SqlDataAdapter Ad;
    private string str = "";
    private string _RefFormNo = "";
    private string _UpLnFormNo = "";
    private string _Company = "";
    private clsGeneral objGen = new clsGeneral();
    private DAL objAccess;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                Conn.Open();
                string sRequestData = HttpContext.Current.Request.Url.ToString();
                string _Company = "";

                if (Request["token"] == "D2014500C8824B8996D4151253C975B5fghhfghgf" || Request["CompId"] == "1017")
                {
                    try
                    {
                        string sqlstr = "INSERT INTO ReqType(reqtype) VALUES(@RequestData)";
                        SqlCommand cmd = new SqlCommand(sqlstr, Conn);
                        cmd.Parameters.AddWithValue("@RequestData", sRequestData.Replace("//n", "\n"));

                        int i = cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            if (Request["action"] == null)
                            {
                                CheckInfo();
                            }
                            else if (Request["action"].ToUpper() == "GETCOUPONDATA")
                            {
                                GetCouponDataList();
                            }
                            else if (Request["action"].ToUpper() == "GETCOUPONTYPE")
                            {
                                GetCouponDataTypeList();
                            }
                            else if (Request["action"].ToUpper() == "USECOUPON")
                            {
                                SaveUSECOUPON();
                            }
                            else if (Request["action"].ToUpper() == "USECOUPONSTATUSCHECK")
                            {
                                USECOUPONSttausCheck();
                            }
                            else
                            {
                                Response.Write("{\"response\":\"FAILED\"}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception if necessary
                    }
                }
                else
                {
                    Response.Write("{\"response\":\"FAILED\"}");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"response\":\"FAILED\"}");
        }
    }
    private string Application(string v)
    {
        throw new NotImplementedException();
    }
    private void USECOUPONSttausCheck()
    {
        try
        {

            string username = Request["Username"];
            string password = Request["Password"]
                                .Replace("%25", "%")
                                .Replace("%23", "#")
                                .Replace("%26", "&")
                                .Replace("%22", "'")
                                .Replace("%40", "@");
            string orderNo = Request["orderno"];
            string q1 = "SELECT FormNo FROM M_MemberMaster WHERE IDNo = @uid";
            DataTable dtMember = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, q1, new SqlParameter("@uid", username)).Tables[0];
            if (dtMember.Rows.Count == 0)
            {
                Response.Write("{\"status\":\"FAIL\",\"message\":\"Invalid Username\"}");
                return;
            }
            string FormNo = dtMember.Rows[0]["FormNo"].ToString();
            string qry = "Exec CheckCouponStatus '" + orderNo + "', '" + FormNo + "'";
            DataTable dt = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, qry).Tables[0];
            Response.Clear();
            Response.ContentType = "application/json";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                CouponCode = dt.Rows[0]["CouponCode"].ToString(),
                UsedDate = dt.Rows[0]["UsedDate"].ToString(),
                Txid = dt.Rows[0]["Txid"].ToString(),
                UsedType = dt.Rows[0]["UsedType"].ToString()
            });
            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private void GetCouponDataTypeList()
    {
        try
        {
            string username = Request["Username"];
            string password = Request["Password"].Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@");
            // 2️⃣ Get Coupon List from Stored Procedure
            string q2 = "Exec GetCouponList";
            DataTable dtCoupons = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, q2).Tables[0];
            // 3️⃣ JSON Response
            Response.Clear();
            Response.ContentType = "application/json";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                status = "SUCCESS",
                coupons = dtCoupons
            });

            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private void GetCouponDataList()
    {
        try
        {
            string username = Request["Username"];
            string password = Request["Password"]
                                .Replace("%25", "%")
                                .Replace("%23", "#")
                                .Replace("%26", "&")
                                .Replace("%22", "'")
                                .Replace("%40", "@");

            // 1️⃣ Get FormNo
            string q1 = "SELECT FormNo FROM M_MemberMaster WHERE IDNo = @uid";

            DataTable dtMember = SqlHelper.ExecuteDataset(
                                    constr.ToString(),
                                    CommandType.Text,
                                    q1,
                                    new SqlParameter("@uid", username)
                                 ).Tables[0];

            if (dtMember.Rows.Count == 0)
            {
                Response.Write("{\"status\":\"FAIL\",\"message\":\"Invalid Username\"}");
                return;
            }

            string FormNo = dtMember.Rows[0]["FormNo"].ToString();

            // 2️⃣ Get Coupon List from Stored Procedure
            string q2 = "Exec GetCouponData @formno";

            DataTable dtCoupons = SqlHelper.ExecuteDataset(
                                    constr.ToString(),
                                    CommandType.Text,
                                    q2,
                                    new SqlParameter("@formno", FormNo)
                                 ).Tables[0];

            // 3️⃣ JSON Response
            Response.Clear();
            Response.ContentType = "application/json";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                status = "SUCCESS",
                coupons = dtCoupons
            });

            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private void SaveUSECOUPON()
    {
        try
        {

            string username = Request["Username"];
            string password = Request["Password"]
                                .Replace("%25", "%")
                                .Replace("%23", "#")
                                .Replace("%26", "&")
                                .Replace("%22", "'")
                                .Replace("%40", "@");
            string orderNo = Request["orderno"];
            string type = Request["useservi"];
            string couponcode = Request["couponcode"];
            // If needed
            // 1️⃣ Get FormNo
            string q1 = "SELECT FormNo FROM M_MemberMaster WHERE IDNo = @uid";
            DataTable dtMember = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, q1, new SqlParameter("@uid", username)).Tables[0];
            if (dtMember.Rows.Count == 0)
            {
                Response.Write("{\"status\":\"FAIL\",\"message\":\"Invalid Username\"}");
                return;
            }
            string FormNo = dtMember.Rows[0]["FormNo"].ToString();
            string qry = "Exec CheckCouponBeforeSave '" + orderNo + "', '" + FormNo + "', '" + type + "','" + couponcode + "'";
            DataTable dt = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, qry).Tables[0];
            Response.Clear();
            Response.ContentType = "application/json";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Result = dt.Rows[0]["Result"].ToString(),
                status = dt.Rows[0]["Status"].ToString(),
                message = dt.Rows[0]["Message"].ToString(),
                orderno = dt.Rows[0]["orderno"].ToString(),
                CouponCode = dt.Rows[0]["CouponCode"].ToString(),
                UsedDate = dt.Rows[0]["UsedDate"].ToString(),
                Txid = dt.Rows[0]["Txid"].ToString()
            });
            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private void CheckInfo()
    {
        try
        {
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                Conn.Open();
                string rwallet = "";
                string ismovie = "N";
                using (SqlCommand Comm = new SqlCommand(
                    "Exec ProcCheckInfo '" +
                    Request["Username"] + "','" +
                    Request["Password"]
                        .Replace("%25", "%")
                        .Replace("%23", "#")
                        .Replace("%26", "&")
                        .Replace("%22", "'")
                        .Replace("%40", "@") + "'", Conn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            double BalanceINR = 0;
                            rwallet = "0";
                            ismovie = Dr["ismovie"].ToString();

                            if (Dr["Status"].ToString().ToUpper() == "BLOCK")
                            {
                                Response.Write("{\"formno\": \"\",\"loginid\": \"Invalid\",\"name\": \"Invalid Credentials\",\"doj\": \"\",\"email\": \"\",\"mobileno\": \"\",\"city\": \"\",\"isactive\": \"\",\"kitid\":\"\",\"kitname\":\"\",\"kitstatus\":\"\",\"status\": \"Block\",\"awallet\": \"0\",\"ismovie\":\"\",\"activedate\":\"\",\"kitamount\":\"0\",\"isholiday\":\"\",\"shoppoint\":\"\",\"promoid\":\"\",\"coupon\":\"\",\"promovalue\":\"0\"}");
                            }
                            else
                            {
                                Response.Write("{\"formno\":\"" + Dr["FormNo"] +
"\",\"loginid\":\"" + Dr["IDNo"] +
"\",\"name\":\"" + Dr["MemName"] +
"\",\"doj\":\"" + Dr["JoinDate"] +
"\",\"email\":\"" + Dr["Email"] +
"\",\"mobileno\":\"" + Dr["Mobl"] +
"\",\"city\":\"" + Dr["City"] +
"\",\"isactive\":\"" + Dr["ActiveStatus"] +
"\",\"kitid\":\"" + Dr["KitId"] +
"\",\"kitname\":\"" + Dr["KitName"] +
"\",\"kitstatus\":\"" + Dr["KitStatus"] +
"\",\"status\":\"" + Dr["STATUS1"] +
"\",\"awallet\":\"" + rwallet +
"\",\"ismovie\":\"" + ismovie +
"\",\"activedate\":\"" + Dr["DOA"] +
"\",\"kitamount\":\"" + Dr["KitAmount"] +
"\",\"isholiday\":\"" + Dr["isholiday"] +
"\",\"shoppoint\":\"" + Dr["shoppoint"] +
"\",\"promoid\":\"" + Dr["promoid"] +
"\",\"coupon\":\"" + Dr["Coupon"] +
"\",\"promovalue\":\"0" +
"\",\"token\":\"" + Dr["Token"] +
"\"}");

                            }
                        }
                        else
                        {
                            Response.Write("{\"formno\":\"0" +
"\",\"loginid\":\"" + Request["Username"] +
"\",\"name\":\"Invalid Credentials" +
"\",\"doj\":\"" +
"\",\"email\":\"" +
"\",\"mobileno\":\"" +
"\",\"city\":\"" +
"\",\"isactive\":\"" +
"\",\"kitid\":\"0" +
"\",\"kitname\":\"" +
"\",\"kitstatus\":\"" +
"\",\"status\":\"FAIL" +
"\",\"awallet\":\"" + rwallet +
"\",\"ismovie\":\"" + ismovie +
"\",\"activedate\":\"" +
"\",\"kitamount\":\"0" +
"\",\"isholiday\":\"0" +
"\",\"shoppoint\":\"0" +
"\",\"promoid\":\"0" +
"\",\"coupon\":\"" +
"\",\"promovalue\":\"0" +
"\",\"token\":\"" +
"\"}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"formno\":\"0" +
 "\",\"loginid\":\"Invalid\",\"name\":\"Invalid Credentials" +
 "\",\"doj\":\"" +
 "\",\"email\":\"" +
 "\",\"mobileno\":\"" +
 "\",\"city\":\"" +
 "\",\"isactive\":\"" +
 "\",\"kitid\":\"0" +
 "\",\"kitname\":\"" +
 "\",\"kitstatus\":\"" +
 "\",\"status\":\"FAIL" +
 "\",\"awallet\":\"0\",\"ismovie\":\"0\",\"activedate\":\"" +
 "\",\"kitamount\":\"0" +
 "\",\"isholiday\":\"0" +
 "\",\"shoppoint\":\"0" +
 "\",\"promoid\":\"0" +
 "\",\"coupon\":\"" +
 "\",\"promovalue\":\"0" +
 "\",\"token\":\"" +
 "\"}");
            // Response.Write("{\"formno\": \"0\",\"loginid\": \"Invalid\",\"name\": \"Invalid Credentials\",\"doj\": \"\",\"email\": \"\",\"mobileno\": \"0\",\"city\": \"\",\"isactive\": \"\",\"kitid\":\"0\",\"kitname\":\"\",\"kitstatus\":\"\",\"status\": \"FAIL\",\"rwallet\":\"0\",\"ewallet\":\"0\"}");
        }
    }
}
