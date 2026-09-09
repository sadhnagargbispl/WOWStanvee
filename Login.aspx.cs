using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Xml;

public partial class Login : System.Web.UI.Page
{
    string uid;
    string Pwd;
    string Memberid;
    string type;
    string scrname;
    DAL obj = new DAL();
    ModuleFunction objModuleFun;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string IsoStart;
    string IsoEnd;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Application["WebStatus"] != null)
            {
                if (Application["WebStatus"].ToString() == "N")
                {
                    Session.Abandon();
                    Response.Write("<big><b>" + Application["WebMessage"] + "</b></big>");
                    Response.End();
                    return;
                }
            }
            //if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            //{
            //    Response.Redirect("Index.aspx", false);
            //    return;
            //}

            string strURL = HttpContext.Current.Request.Url.AbsoluteUri;
            string url = "";
            string Str;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {
                if (Request["lgnT"] != null)
                {
                    ModuleFunction objModuleFun = new ModuleFunction();

                    Str = Crypto.Decrypt(Request["lgnT"].Replace(" ", "+"));

                    Str = Str.Replace("uid=", "þ").Replace("&pwd=", "þ").Replace("&mobile=", "þ");
                    string[] qrystr = Str.Split(new string[] { "þ" }, StringSplitOptions.None);
                    //                if ((DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Year.ToString() + (DateTime.Now.Month - 1).ToString() == Request["ID"]) ||
                    //(DateTime.Now.Day.ToString() + (DateTime.Now.Hour - 1).ToString() + DateTime.Now.Year.ToString() + (DateTime.Now.Month - 1).ToString() == Request["ID"]))

                    //                {
                    if (Str.Contains("þ"))
                    {
                        int UIdIndx = Str.IndexOf("&pwd");
                        uid = qrystr[1].ToString();
                        Pwd = qrystr[2].ToString();
                        //Session["Adminmob"] = qrystr[3].ToString();
                    }
                    //}
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                        return;
                    }
                }
                else if (Request["uid"] != null)
                {
                    uid = Request["uid"];
                    Pwd = Request["pwd"];
                    type = Request["ref"];
                    uid = uid.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                    Pwd = Pwd.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                }
                if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))

                {

                    enterHomePgForAdmin();
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    private void enterHomePg()
    {
        SqlConnection cnn = new SqlConnection();
        try
        {
            string scrname;
            DataTable dt = new DataTable();
            string strSql = "Exec sp_Login '" + ClearInject(string.IsNullOrEmpty(uid) ? ClearInject(TxtUserID.Text) : ClearInject(uid)) + "',";
            strSql += "'" + (string.IsNullOrEmpty(Pwd) ? ClearInject(TxtPassword.Text) : ClearInject(Pwd)) + "'";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                scrname = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            }
            else
            {
                Session["Run"] = 0;
                Session["Status"] = "OK";
                Session["IDNo"] = dt.Rows[0]["IDNo"];
                Session["FormNo"] = dt.Rows[0]["Formno"];
                Session["MemName"] = dt.Rows[0]["MemFirstName"] + " " + dt.Rows[0]["MemLastName"];
                Session["MobileNo"] = dt.Rows[0]["Mobl"];
                Session["MemKit"] = dt.Rows[0]["KitID"];
                Session["Package"] = dt.Rows[0]["KitName"];
                Session["Position"] = dt.Rows[0]["fld3"];
                Session["Doj"] = string.Format("{0:dd-MMM-yyyy}", dt.Rows[0]["Doj"]);
                Session["DOA"] = string.Format("{0:dd-MMM-yyyy}", dt.Rows[0]["Upgradedate"]);
                Session["Address"] = dt.Rows[0]["Address1"];
                Session["IsFranchise"] = dt.Rows[0]["Fld5"];
                Session["ActiveStatus"] = dt.Rows[0]["ActiveStatus"];
                Session["MemPassw"] = dt.Rows[0]["Passw"];
                Session["MFormno"] = dt.Rows[0]["MFormNo"];
                Session["MemUpliner"] = dt.Rows[0]["UplnFormno"];
                Session["MID"] = dt.Rows[0]["MID"];
                Session["EMail"] = dt.Rows[0]["Email"];
                Session["profilepic"] = dt.Rows[0]["profilepic"];
                Session["Panno"] = dt.Rows[0]["Panno"];
                Session["ActivationDate"] = dt.Rows[0]["Upgradedate"];
                Session["MemEPassw"] = dt.Rows[0]["Epassw"];
               
                string formNo = dt.Rows[0]["FormNo"].ToString();
                bool isAllow = false;
                if (Session["MemKit"] != null)
                {
                    string memKit = Session["MemKit"].ToString();
                    if (memKit.Contains("18") || memKit.Contains("19"))
                    {
                        isAllow = true;
                    }
                }
                if (!isAllow)
                {
                    string sql = @"SELECT TOP 1 KitId FROM repurchincome WHERE FormNo='" + formNo + @"'  AND kitid in (18,19) ORDER BY BillDate DESC";
                    DataTable dtKit = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                    if (dtKit.Rows.Count > 0)
                    {
                        string kitId = dtKit.Rows[0]["KitId"].ToString();
                        if (kitId == "18" || kitId == "19")
                        {
                            isAllow = true;
                        }
                    }
                }
                if (isAllow)
                {
                    Response.Redirect("index.aspx", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                        "alert('Access to this portal is restricted. You are not permitted to log in.!');location.replace('logout.aspx');", true);
                    return;
                }

                //if (Session["MemKit"] != null)
                //{
                //    string memKit = Session["MemKit"].ToString();

                //    DataTable dt1 = new DataTable();
                //    string strSql1 = "Exec sp_Login '" + ClearInject(string.IsNullOrEmpty(uid) ? ClearInject(TxtUserID.Text) : ClearInject(uid)) + "',";
                //    strSql1 += "'" + (string.IsNullOrEmpty(Pwd) ? ClearInject(TxtPassword.Text) : ClearInject(Pwd)) + "'";
                //    DataSet ds1 = new DataSet();
                //    ds1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql1);
                //    dt1 = ds1.Tables[0];
                //    if (dt1.Rows.Count == 0)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Access to this portal is restricted. You are not permitted to log in.!');location.replace('logout.aspx');", true);
                //        return;
                //    }

                //    if (memKit.Contains("18") || memKit.Contains("19"))
                //    {
                //        Response.Redirect("index.aspx", false);
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Access to this portal is restricted. You are not permitted to log in.!');location.replace('logout.aspx');", true);
                //        return;
                //    }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Access to this portal is restricted. You are not permitted to log in.!');location.replace('logout.aspx');", true);
                //    return;
                //}
            }
        }
        catch (Exception ex)
        {
            if (cnn != null && cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
            Response.Write(ex.Message);
        }
    }
    public string Fun_Login(string emailid, string formno, string password)
    {
        string URL = string.Empty;
        string sResult = string.Empty;
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        string message = string.Empty;
        DataSet data = new DataSet();

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string apiKey = "Xr21X8isdf867asdf86adf26wIxwFMyq3ve";
            string email = emailid.Trim().ToLower();
            URL = "https://uatwavemoneyapp.wavemoney.app/api/shop/auth_login?token=" + apiKey + "&username=" + email + "&password=" + password;
            string sqlReq = @"INSERT INTO Tbl_ApiRequest_ResponsePayment
                      (ReqID, Request, postdata, IsWebUser)
                      VALUES (@ReqID, @Request, @PostData, @IsWebUser)";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq,
                new SqlParameter("@ReqID", sResult.Trim()),
                new SqlParameter("@Request", URL),
                new SqlParameter("@PostData", "GET"),
                new SqlParameter("@IsWebUser", "Login")
            );

            HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
            tRequest.Method = "GET";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add("key", apiKey);

            string responseText = "";

            try
            {
                using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
                using (Stream dataStream = tResponse.GetResponseStream())
                using (StreamReader tReader = new StreamReader(dataStream))
                {
                    responseText = tReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var dataStream = errorResponse.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                else
                {
                    responseText = ex.Message;
                }
            }

            // 🔹 Log API response
            string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
                new SqlParameter("@Response", responseText),
                new SqlParameter("@ReqID", sResult.Trim())
            );

            // 🔹 Parse JSON
            data = convertJsonStringToDataSet(responseText);

            string statusCode = data.Tables[0].Rows[0]["status"].ToString();
            //  message = data.Tables[0].Rows[0]["message"].ToString();

            if (statusCode.ToUpper() == "SUCCESS")
            {
                // string token = data.Tables[1].Rows[0]["access_token"].ToString();

                //string sqlRes1 = @"UPDATE m_membermaster 
                //           SET Token = @Response 
                //           WHERE idno = @idno AND formno = @formno";

                //SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes1,
                //    new SqlParameter("@Response", token),
                //    new SqlParameter("@idno", emailid.Trim().ToLower()),
                //    new SqlParameter("@formno", formno)
                //);

                //Session["Token"] = token;
            }
        }
        catch (Exception ex)
        {
            string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
                new SqlParameter("@Response", ex.ToString()),
                new SqlParameter("@ReqID", sResult.Trim())
            );
        }

        return message;
        //string URL = string.Empty;
        //string sResult = string.Empty;
        //string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //int random_number = new Random().Next(0, 999);
        //string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        //sResult = formatted_datetime;
        //string postData = string.Empty;
        //string message = string.Empty;
        //string str = string.Empty;
        //DataSet data = new DataSet();
        //try
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //    URL = "https://uatwavemoney.onewave.app/api/shop/auth_login";
        //    string apiKey = "Xr21X8iqmq8n212gj26wIxwFMyq3ve";
        //    string email = emailid.Trim().ToLower();
        //    postData = "{\"email\": \"" + email + "\"}";
        //    string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponsePayment (ReqID, Request, postdata,IsWebUser)VALUES (@ReqID, @Request, @PostData,@IsWebUser)";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq,
        //        new SqlParameter("@ReqID", sResult.Trim()),
        //        new SqlParameter("@Request", URL),
        //        new SqlParameter("@PostData", postData),
        //         new SqlParameter("@IsWebUser", "Login")
        //    );
        //    HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
        //    tRequest.Method = "POST";
        //    tRequest.ContentType = "application/json";
        //    tRequest.Headers.Add("key", apiKey);
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    tRequest.ContentLength = byteArray.Length;
        //    using (Stream dataStream = tRequest.GetRequestStream())
        //    {
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //    }
        //    string responseText = "";
        //    try
        //    {
        //        using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
        //        using (Stream dataStream = tResponse.GetResponseStream())
        //        using (StreamReader tReader = new StreamReader(dataStream))
        //        {
        //            responseText = tReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            using (var errorResponse = (HttpWebResponse)ex.Response)
        //            using (var dataStream = errorResponse.GetResponseStream())
        //            using (var reader = new StreamReader(dataStream))
        //            {
        //                // READ JSON EVEN IF STATUS = 400
        //                responseText = reader.ReadToEnd();
        //            }
        //        }
        //        else
        //        {
        //            responseText = ex.Message;
        //        }
        //    }
        //    // Log API response
        //    string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
        //        new SqlParameter("@Response", responseText),
        //        new SqlParameter("@ReqID", sResult.Trim())
        //    );
        //    // Parse JSON
        //    data = convertJsonStringToDataSet(responseText);
        //    string statusCode = data.Tables[0].Rows[0]["status"].ToString();
        //    message = data.Tables[0].Rows[0]["message"].ToString();
        //    if (message.ToString().ToUpper() == "ACCESS TOKEN FETCH SUCCESSFUL")
        //    {
        //        string sqlRes1 = "UPDATE m_membermaster SET Token = @Response WHERE idno = @idno AND formno = @formno";
        //        SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes1,
        //            new SqlParameter("@Response", data.Tables[1].Rows[0]["access_token"].ToString()),
        //            new SqlParameter("@idno", emailid.Trim().ToLower()),
        //            new SqlParameter("@formno", formno)
        //        );
        //        Session["Token"] = data.Tables[1].Rows[0]["access_token"].ToString();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    // Log error response
        //    string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
        //        new SqlParameter("@Response", ex.ToString()),
        //        new SqlParameter("@ReqID", sResult.Trim())
        //    );
        //}
        //return message;
    }
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
    private void enterHomePgForAdmin()
    {
        SqlConnection cnn = new SqlConnection();
        try
        {
            string scrname;
            DataTable dt = new DataTable();
            string strSql = "Exec sp_Login1 '" + ClearInject(string.IsNullOrEmpty(uid) ? ClearInject(TxtUserID.Text) : ClearInject(uid)) + "',";
            strSql += "'" + (string.IsNullOrEmpty(Pwd) ? ClearInject(TxtPassword.Text) : ClearInject(Pwd)) + "'";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                scrname = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            }
            else
            {
                Session["Run"] = 0;
                Session["Status"] = "OK";
                Session["IDNo"] = dt.Rows[0]["IDNo"];
                Session["FormNo"] = dt.Rows[0]["Formno"];
                Session["MemName"] = dt.Rows[0]["MemFirstName"] + " " + dt.Rows[0]["MemLastName"];
                Session["MobileNo"] = dt.Rows[0]["Mobl"];
                Session["MemKit"] = dt.Rows[0]["KitID"];
                Session["Package"] = dt.Rows[0]["KitName"];
                Session["Position"] = dt.Rows[0]["fld3"];
                Session["Doj"] = string.Format("{0:dd-MMM-yyyy}", dt.Rows[0]["Doj"]);
                Session["DOA"] = string.Format("{0:dd-MMM-yyyy}", dt.Rows[0]["Upgradedate"]);
                Session["Address"] = dt.Rows[0]["Address1"];
                Session["IsFranchise"] = dt.Rows[0]["Fld5"];
                Session["ActiveStatus"] = dt.Rows[0]["ActiveStatus"];
                Session["MemPassw"] = dt.Rows[0]["Passw"];
                Session["MFormno"] = dt.Rows[0]["MFormNo"];
                Session["MemUpliner"] = dt.Rows[0]["UplnFormno"];
                Session["MID"] = dt.Rows[0]["MID"];
                Session["EMail"] = dt.Rows[0]["Email"];
                Session["profilepic"] = dt.Rows[0]["profilepic"];
                Session["Panno"] = dt.Rows[0]["Panno"];
                Session["ActivationDate"] = dt.Rows[0]["ActivationDate"];
                Session["MemEPassw"] = dt.Rows[0]["Epassw"];
                Session["Planid"] = dt.Rows[0]["Planid"];
                Response.Redirect("index.aspx", false);
            }
        }
        catch (Exception ex)
        {
            if (cnn != null && cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
            Response.Write(ex.Message);
        }
    }
    private string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj.Trim();
    }
    protected void BtnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string uid;
            string pwd;
            string type;

            if (Request["uid"] != null)
            {
                uid = Request["uid"];
                pwd = Request["pwd"];
            }
            else
            {
                uid = TxtUserID.Text;
                pwd = TxtPassword.Text;
            }

            type = Request["ref"];
            uid = uid.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
            pwd = pwd.Trim().Replace("'", "").Replace("=", "").Replace(";", "");

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(pwd))
            {
                enterHomePg();
            }
            else
            {
                Response.Redirect("logout.aspx", false);
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
}
