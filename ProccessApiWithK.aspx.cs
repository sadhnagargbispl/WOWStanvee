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
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Xml;
using System.Net;
using System.Text;
using DocumentFormat.OpenXml.Presentation;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Cryptography;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System.Net.NetworkInformation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;

public partial class ProccessApiWithK : System.Web.UI.Page
{
    public string _ReqType;
    public GetMsg2 ErrObj = new GetMsg2();

    // Database-related
    SqlConnection Conn;
    SqlConnection selectConn;
    SqlCommand Comm;
    SqlDataAdapter Adp;
    DataSet Ds;
    SqlDataReader Dr;
    private static readonly TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
    private static readonly MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
    private static readonly string key = "sg75b79-nj48dh02";
    // Miscellaneous
    string _NewID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    Random Rnd = new Random();
    bool Bool = true;
    string HostIp = HttpContext.Current.Request.UserHostAddress.ToString();
    string _Company = "";
    string strQry = "";
    string _MailID = "";
    string _MailPass = "";
    string _MailHost = "";
    string _SMSSender = "APPSMS";
    string _SMSUser = "";
    string _SMSPass = "";
    string _RefFormNo = "";
    string _UpLnFormNo = "";
    string _Token = "GW739IESP1956rerir";
    string membername = "";
    // Connection strings
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    clsGeneral objGen = new clsGeneral();
    DAL ObjDAL = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string IsoStart;
    string IsoEnd;
    string sResult = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            getData();
            _Company = Session["CompName"].ToString();
            _MailID = Session["CompMail"].ToString();
            _MailPass = Session["MailPass"].ToString();
            _MailHost = Session["MailHost"].ToString();
            _SMSSender = Session["ClientId"].ToString();
            _SMSUser = Session["SmsId"].ToString();
            _SMSPass = Session["SmsPass"].ToString();

            Session["InvDB"] = "ZenexInv";
            Session["WR"] = "ZE223344";
            Session["Website"] = "http://srecwholesale.com/";
            string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random_number = new Random().Next(0, 999);
            string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
            sResult = formatted_datetime;
            HttpContext.Current.Session["CompID"] = "1001";
            string URL = "https://" + HttpContext.Current.Request.Url.Host + "/ProccessApiWithK.aspx";

            string sql =
                "CREATE TABLE [dbo].[Tbl_ApiRequest_ResponseQrCode](" +
                "[Id] [int] IDENTITY(1,1) NOT NULL, " +
                "[ReqID] [numeric](24, 0) NULL, " +
                "[Request] [nvarchar](max) NULL, " +
                "[postdata] [nvarchar](max) NULL, " +
                "[Response] [nvarchar](max) NULL, " +
                "[RecTimeStamp] [datetime] DEFAULT (getdate()) NOT NULL, " +
                "CompID Numeric(18,0),ForType varchar(255)" +
                ")";
            int i = 0;
            try
            {
                i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);
            }
            catch (Exception)
            {
            }
            selectConn = new SqlConnection(constr1);
            selectConn.Open();

            Conn = new SqlConnection(constr);
            Conn.Open();

            if (Request.Form.HasKeys() && Request.Form["MyJson"] != null)
            {
                string sRequestData = Request.Form["MyJson"];
                sRequestData = ClearInject(sRequestData);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, string> dict = jss.Deserialize<Dictionary<string, string>>(sRequestData);

                try
                {
                    Comm = new SqlCommand("insert into ReqType(reqtype) values('" + sRequestData.Replace("//n", "\\n") + "')", Conn);
                    Comm.ExecuteNonQuery();
                    string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Request, postdata, CompID) ";
                    sql_req += "VALUES ('" + sResult.Trim() + "','" + URL.Trim() + "','" + sRequestData.Replace("//n", "\n") + "','" + HttpContext.Current.Session["CompID"] + "')";
                    int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
                }
                catch (Exception)
                {
                }

                if (dict.ContainsKey("reqtype"))
                {
                    _ReqType = dict["reqtype"];
                    if (!string.IsNullOrEmpty(_ReqType))
                    {
                        ProcessFile(_ReqType, dict);
                    }
                    else
                    {
                        string Result_Json = "";
                        Result_Json = "{\"response\":\"Failed\",\"msg\":\"File Api Request Not Found.!\" }";
                        string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                        int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                        Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                        Response.Clear();
                        Response.ContentType = "application/json";
                        Response.Write(Result_Json);
                    }
                }
            }
            else if (Request.Form["File"] != null)
            {
                Response.Write(Base64ToImage(Request.Form["File"]));
            }
            else
            {
                Request.InputStream.Position = 0; // you need this else the magic doesn't happen
                StreamReader inputStream = new StreamReader(Request.InputStream);
                string json = inputStream.ReadToEnd();

                try
                {
                    Comm = new SqlCommand("insert into ReqType(reqtype) values('" + json.Replace("//n", "\\n") + "')", Conn);
                    Comm.ExecuteNonQuery();
                    string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Request, postdata, CompID) ";
                    sql_req += "VALUES ('" + sResult.Trim() + "','" + URL.Trim() + "','" + json.Replace("//n", "\n") + "','" + HttpContext.Current.Session["CompID"] + "')";
                    int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
                }
                catch (Exception)
                {
                }

                if (!string.IsNullOrEmpty(json))
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Dictionary<string, string> dict = jss.Deserialize<Dictionary<string, string>>(json);

                    if (dict.ContainsKey("reqtype"))
                    {
                        _ReqType = dict["reqtype"];
                        if (!string.IsNullOrEmpty(_ReqType))
                        {
                            Process(_ReqType, dict);
                        }
                        else
                        {
                            string Result_Json = "";
                            Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Api Request Type Not Found.!\" }";
                            string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                            Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                            Response.Clear();
                            Response.ContentType = "application/json";
                            Response.Write(Result_Json);

                        }
                    }
                }
                else
                {
                    string Result_Json = "";
                    Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Api Json Request Not Found.!\" }";
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                    Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                    Response.Clear();
                    Response.ContentType = "application/json";
                    Response.Write(Result_Json);
                }
            }
        }
        catch (Exception)
        {
            string Result_Json = "";
            Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Exception Case MyJson Not Valid.!\" }";
            string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
            Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(Result_Json);
        }
        Response.End();
    }
    private string ShowUpline(string RefFormno)
    {
        string RtrVal = "False";
        DataTable tmpTable = new DataTable();
        string str = ObjDAL.Isostart + "select Count(1) As Cnt From " + ObjDAL.dBName + "..M_MemberMaster Where RefFormno = '" + RefFormno + "'" + ObjDAL.IsoEnd;
        DataSet dsCh = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str);
        tmpTable = dsCh.Tables[0];
        if (Convert.ToInt32(tmpTable.Rows[0]["Cnt"]) >= 2)
        {
            RtrVal = "True";
        }
        else
        {
            RtrVal = "False";
        }

        return RtrVal;
    }
    private string CheckSponsor(string userid, string passwd, string sponsorid)
    {
        bool isValid = false;

        if (userid == "" && passwd == "")
        {
            isValid = true;
        }
        else
        {
            isValid = UserExists(userid, passwd);
        }

        string _output = "";

        if (isValid == true)
        {
            string sponsorname = "";
            string formno = GetFormNo(sponsorid).ToString();
            string UplnFormno = "";
            string Sqlstr = ObjDAL.Isostart + "Select idno,(MemfirstName+' '+memlastName) as MemberName,formno from " + ObjDAL.dBName + "..M_MemberMaster where Idno = '" + sponsorid.Trim() + "' " + ObjDAL.IsoEnd;
            DataTable Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sqlstr).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                sponsorname = Dt.Rows[0]["MemberName"].ToString();
                UplnFormno = Dt.Rows[0]["formno"].ToString();
                if (ShowUpline(UplnFormno.ToString()) == "True")
                {
                    _output = "{\"response\":\"OK\",\"sponsorname\":\"" + sponsorname + "\",\"showupline\":\"true\",\"msg\":\"success\"}";
                }
                else
                {
                    _output = "{\"response\":\"OK\",\"sponsorname\":\"" + sponsorname + "\",\"showupline\":\"false\",\"msg\":\"success\"}";
                }
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"sponsorname\":\"\",\"showupline\":\"false\",\"msg\":\"This ID Not Valid For Sponsor.!\"}";
            }
        }
        else
        {
            _output = "{\"response\":\"FAILED\",\"sponsorname\":\"\",\"showupline\":\"false\",\"msg\":\"Invalid Login Details.\"}";
        }

        return _output;
    }
    public void Process(string _Reqtype, Dictionary<string, string> dict)
    {
        try
        {
            if (_Reqtype == "reqlogin")
            {
                string _ReqUser = ClearInject(dict["userid"]);
                string _ReqPassw = ClearInject(dict["passwd"]);
                string Result_Json = checklogin(_ReqUser, _ReqPassw);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "',ForType = 'reqlogin' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "checksponsor")
            {
                string _ReqMobileNo = "";
                try
                {
                    _ReqMobileNo = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqOtpCode = "";
                try
                {
                    _ReqOtpCode = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string _ReqSponsor = ClearInject(dict["sponsorid"]);

                string Result_Json = CheckSponsor(_ReqMobileNo, _ReqOtpCode, _ReqSponsor);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "',ForType = 'checksponsor' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "joining")
            {
                string Result_Json = Register(dict);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "',ForType = 'joining' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "countrylist")
            {
                string Result_Json = Sp_GetCountry();
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "',ForType = 'epinpackagelist' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else
            {
                string Result_Json = "";
                Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Api Process Request Not Found.!\" }";
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
        }
        catch (Exception)
        {
            string Result_Json = "";
            Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Exception Case Process Not Valid.!\" }";
            string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
            Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(Result_Json);
        }
    }
    protected void getData()
    {
        cls_DataAccess dbConnect = new cls_DataAccess(constr1);
        DAL objdal = new DAL();
        try
        {
            SqlDataReader dRead;
            SqlCommand cmd;
            DataTable dtCompany = new DataTable();
            if (Application["dtCompany"] == null)
            {
                if (dbConnect.cnnObject == null)
                {
                    dbConnect.OpenConnection();
                }
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter();
                string strQ = objdal.Isostart + " select * from " + objdal.dBName + " ..M_CompanyMaster" + objdal.IsoEnd;
                adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
                adp.Fill(ds);
                dtCompany = ds.Tables[0];
                Application["dtCompany"] = dtCompany;
            }
            else
            {
                if (dbConnect.cnnObject == null)
                {
                    dbConnect.OpenConnection();
                }
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter();
                string strQ = objdal.Isostart + " select * from " + objdal.dBName + " ..M_CompanyMaster" + objdal.IsoEnd;
                adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
                adp.Fill(ds);
                dtCompany = ds.Tables[0];
                Application["dtCompany"] = dtCompany;
            }

            if (dtCompany.Rows.Count > 0)
            {
                Session["CompName"] = dtCompany.Rows[0]["CompName"];
                Session["CompAdd"] = dtCompany.Rows[0]["CompAdd"];
                Session["CompWeb"] = string.IsNullOrEmpty(dtCompany.Rows[0]["WebSite"].ToString()) ? "index.asp" : dtCompany.Rows[0]["WebSite"];
                Session["Title"] = dtCompany.Rows[0]["CompTitle"];
                Session["CompMail"] = dtCompany.Rows[0]["CompMail"];
                Session["CompMobile"] = dtCompany.Rows[0]["MobileNo"];
                Session["ClientId"] = dtCompany.Rows[0]["smsSenderId"];
                Session["SmsId"] = dtCompany.Rows[0]["smsUserNm"];
                Session["SmsPass"] = dtCompany.Rows[0]["smPass"];
                Session["MailPass"] = dtCompany.Rows[0]["mailPass"];
                Session["MailHost"] = dtCompany.Rows[0]["mailHost"];
                Session["AdminWeb"] = dtCompany.Rows[0]["AdminWeb"];
                Session["CompCST"] = dtCompany.Rows[0]["CompCSTNo"];
                Session["CompState"] = dtCompany.Rows[0]["CompState"];
                Session["CompDate"] = Convert.ToDateTime(dtCompany.Rows[0]["RecTimeStamp"]).ToString("dd-MMM-yyyy");
                Session["Spons"] = "KL223344";
                Session["CompWeb1"] = dtCompany.Rows[0]["WebSite"];
                Session["CompMovieWeb"] = "";
                Session["SmsAPI"] = "";
                Session["CompShortUrl"] = dtCompany.Rows[0]["UrlShort"];
                Session["LogoUrl"] = dtCompany.Rows[0]["LogoUrl"];
            }
            else
            {
                Session["CompName"] = "";
                Session["CompAdd"] = "";
                Session["CompWeb"] = "";
                Session["Title"] = "Welcome";
            }

            DataTable dtConfig = new DataTable();
            if (Application["dtConfig"] == null)
            {
                if (dbConnect.cnnObject == null)
                {
                    dbConnect.OpenConnection();
                }
                string strQ = objdal.Isostart + " select * from " + objdal.dBName + "..M_ConfigMaster " + objdal.IsoEnd;
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
                adp.Fill(ds);
                dtConfig = ds.Tables[0];
                Application["dtConfig"] = dtConfig;
            }
            else
            {
                dtConfig = (DataTable)Application["dtConfig"];
            }

            if (dtConfig.Rows.Count > 0)
            {
                Session["IsGetExtreme"] = dtConfig.Rows[0]["IsGetExtreme"];
                Session["IsTopUp"] = dtConfig.Rows[0]["IsTopUp"];
                Session["IsSendSMS"] = dtConfig.Rows[0]["IsSendSMS"];
                Session["IdNoPrefix"] = dtConfig.Rows[0]["IdNoPrefix"];
                Session["IsFreeJoin"] = dtConfig.Rows[0]["IsFreeJoin"];
                Session["IsStartJoin"] = dtConfig.Rows[0]["IsStartJoin"];
                Session["JoinStartFrm"] = dtConfig.Rows[0]["JoinStartFrm"];
                Session["IsSubPlan"] = dtConfig.Rows[0]["IsSubPlan"];
                Session["Logout"] = dtConfig.Rows[0]["LogoutPg"];
            }
            else
            {
                Session["IsGetExtreme"] = "N";
                Session["IsTopUp"] = "N";
                Session["IsSendSMS"] = "N";
                Session["IdNoPrefix"] = "";
                Session["IsFreeJoin"] = "N";
                Session["IsStartJoin"] = "N";
                Session["JoinStartFrm"] = "01-Sep-2011";
                Session["IsSubPlan"] = "N";
                Session["Logout"] = "https://djiomart.com/";
            }
        }
        catch (Exception ex)
        {
            // handle exception
        }
        DataTable dtMsession = new DataTable();
        if (Application["dtMsession"] == null)
        {
            if (dbConnect.cnnObject == null)
            {
                dbConnect.OpenConnection();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter();
            string strQ = objdal.Isostart + " select Max(SEssid) as SessID from " + objdal.dBName + "..D_Monthlypaydetail  " + objdal.IsoEnd;
            adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
            adp.Fill(ds);
            dtMsession = ds.Tables[0];
            Application["dtMsession"] = dtMsession;
        }
        else
        {
            dtMsession = (DataTable)Application["dtMsession"];
        }

        if (dtMsession.Rows.Count > 0)
        {
            Session["MaxSessn"] = dtMsession.Rows[0]["SessID"];
        }
        else
        {
            Session["MaxSessn"] = "";
        }

        DataTable dtsession = new DataTable();
        if (Application["dtsession"] == null)
        {
            if (dbConnect.cnnObject == null)
            {
                dbConnect.OpenConnection();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter();
            string strQ = objdal.Isostart + " select Max(SEssid) as SessID from " + objdal.dBName + "..m_SessnMaster  " + objdal.IsoEnd;
            adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
            adp.Fill(ds);

            dtsession = ds.Tables[0];
            Application["dtsession"] = dtsession;
        }
        else
        {
            dtsession = (DataTable)Application["dtsession"];
        }

        if (dtsession.Rows.Count > 0)
        {
            Session["CurrentSessn"] = dtsession.Rows[0]["SessID"];
        }
        else
        {
            Session["CurrentSessn"] = "";
        }
        if (dbConnect.cnnObject != null)
        {
            if (dbConnect.cnnObject.State == ConnectionState.Open)
            {
                dbConnect.cnnObject.Close();
            }
        }

    }
    public void writeJson(object _object)
    {
        try
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsondata = javaScriptSerializer.Serialize(_object);
            writeRaw(jsondata);
        }
        catch (Exception)
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }
    }
    public void writeRaw(string text)
    {
        try
        {
            Response.Write(text);
        }
        catch (Exception)
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }
    }
    public string Base64ToImage(string base64string)
    {
        string json = "";
        try
        {
            System.Drawing.Image img;
            System.IO.MemoryStream MS;

            string b64 = base64string.Replace(" ", "+");
            byte[] b = Convert.FromBase64String(b64);
            MS = new System.IO.MemoryStream(b);

            // Create image from memory stream
            img = System.Drawing.Image.FromStream(MS);

            // Resize the image (optional, as in your VB code)
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(img, 500, 500))
            {
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    string FlNm = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    string FilePath = HttpContext.Current.Server.MapPath("demoepay/images/UploadImage/" + FlNm + ".png");

                    // Save the image to the server path
                    bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Png);

                    // Build public URL for the image
                    string FileName = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + FlNm + ".png";

                    json = "{\"response\":\"OK\",\"type\":\"pancard\",\"image\":\"" + FileName + "\" }";
                }
            }
        }
        catch (Exception ex)
        {
            json = "{\"response\":\"Failed\",\"type\":\"pancard\",\"image\":\"" + ex.Message.ToString() + "\" }";
        }

        return json;
    }
    public void UploadFile()
    {
        try
        {
            string FileName = "";
            string DbFileName = "/images/UploadImage/";
            string FileExt = "";
            UploadClass Upload = new UploadClass();
            string _NewID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int i = 0;

            Upload.ReadUpload();

            if (Upload.FileCount > 0)
            {
                for (i = 0; i < Upload.FileCount; i++)
                {
                    Upload.OverwriteMode = 0;

                    // Get file extension (e.g. .jpg, .png, etc.)
                    int dotidx = Upload.FileName(i).LastIndexOf(".");
                    FileExt = Upload.FileName(i).Substring(dotidx, Upload.FileName(i).Length - dotidx);

                    // Generate local path and database path
                    FileName = HttpContext.Current.Server.MapPath("./images/UploadImage/") + _NewID + FileExt;
                    string FullDbPath = DbFileName + _NewID + FileExt;

                    // Save uploaded file to server
                    Upload.SaveFile(i, FileName);

                    // Save file reference in DB
                    using (SqlCommand Comm = new SqlCommand("UPDATE MstDist SET MemPic=@MemPic WHERE FormNo='12345'", Conn))
                    {
                        Comm.Parameters.AddWithValue("@MemPic", FullDbPath);
                        Comm.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception)
        {
            // handle exceptions silently (same as VB version)
        }
    }
    public void ProcessFile(string _Reqtype, Dictionary<string, string> dict)
    {
        string ImgPrefix = "", ImgFld = "", ImgDateFld = "", sql = "";
        string UserName = ClearInject(dict["userid"]);
        string Password = ClearInject(dict["passwd"]);
        string Remark = "";
        string formno = "0";
        bool Bool = UserExists(UserName, Password);
        formno = GetFormNo(UserName);

        if (Bool == true || _Reqtype == "paymentrequest")
        {
            if (_Reqtype == "profilepic")
            {
                ImgPrefix = "P";
                ImgFld = "ProfilePic";
                ImgDateFld = "";
            }
            else if (_Reqtype == "idproof")
            {
                ImgPrefix = "I";
                ImgFld = "IDProof";
                ImgDateFld = ",IDProofDate=Getdate()";
            }
            else if (_Reqtype == "pinrequest" || _Reqtype == "paymentrequest" ||
                     _Reqtype == "addressproof" || _Reqtype == "bankproof" || _Reqtype == "pancard")
            {
                // handled later
            }
            else
            {
                ErrObj.Response = "FAILED";
                writeJson(ErrObj);
                return;
            }

            try
            {
                string FileName = "";
                string DbFileName1 = "";
                string DbFileName = "";
                string FileExt = "";
                UploadClass Upload = new UploadClass();
                string _Output = "";

                int i = 0;
                long FileLen = 0;
                long AuditNo = 0;
                int j = 0;
                int count = 0;
                string FrontImage = "";
                string BackImage = "";

                try
                {
                    FrontImage = ClearInject(dict.ContainsKey("frontimg") ? dict["frontimg"] : "");
                }
                catch { }

                try
                {
                    BackImage = ClearInject(dict.ContainsKey("backimg") ? dict["backimg"] : "");
                }
                catch { }

                Upload.ReadUpload();
                if (Upload.FileCount > 0)
                {
                    count = Upload.FileCount;
                    for (i = 0; i < Upload.FileCount; i++)
                    {
                        Upload.OverwriteMode = 0;
                        string _NewID = ImgPrefix + DateTime.Now.ToString("yyyyMMddHHmmssfff") + i;
                        string filenamed = Upload.FileName(i);

                        int dotidx = Upload.FileName(i).LastIndexOf(".");
                        FileExt = Upload.FileName(i).Substring(dotidx, Upload.FileName(i).Length - dotidx);

                        FileName = HttpContext.Current.Server.MapPath("./upload/profilepic/") + _NewID + FileExt;

                        if (count == 2)
                        {
                            if (i == 0 && FrontImage == filenamed)
                            {
                                DbFileName = "https://" + HttpContext.Current.Request.Url.Host + "/upload/profilepic/" + _NewID + FileExt;
                            }
                            else if (i == 1 && BackImage == filenamed)
                            {
                                DbFileName1 = "https://" + HttpContext.Current.Request.Url.Host + "/upload/profilepic/" + _NewID + FileExt;
                            }
                        }
                        else
                        {
                            if (FrontImage == filenamed)
                            {
                                DbFileName = "https://" + HttpContext.Current.Request.Url.Host + "/upload/profilepic/" + _NewID + FileExt;
                            }
                            else if (BackImage == filenamed)
                            {
                                DbFileName1 = "https://" + HttpContext.Current.Request.Url.Host + "/upload/profilepic/" + _NewID + FileExt;
                            }
                            else
                            {
                                DbFileName = "https://" + HttpContext.Current.Request.Url.Host + "/upload/profilepic/" + _NewID + FileExt;
                            }
                        }

                        Upload.SaveFile(i, FileName);
                    }

                    // ROUTING BASED ON REQUEST TYPE
                    if (_Reqtype == "pinrequest")
                    {
                        // CallPinReq(_Reqtype, dict, DbFileName);
                        return;
                    }
                    //else if (_Reqtype == "paymentrequest")
                    //{
                    //    WalletReq(DbFileName, _Reqtype, dict);
                    //    return;
                    //}
                    else if (_Reqtype == "addressproof")
                    {
                        AddressProof(DbFileName, DbFileName1, _Reqtype, dict);
                        return;
                    }
                    else if (_Reqtype == "bankproof")
                    {
                        BankProof(DbFileName, _Reqtype, dict);
                        return;
                    }
                    else if (_Reqtype == "pancard")
                    {
                        PanCardNo(DbFileName, _Reqtype, dict);
                        return;
                    }
                    else
                    {
                        FileLen = Upload.FileSize(i);
                        SqlCommand Comm = new SqlCommand(
                            "Update M_MemberMaster Set " + ImgFld + "='" + DbFileName + "'" + ImgDateFld +
                            " Where IDNo='" + UserName + "'" +
                            " Insert Into TempMemberMaster Select *,'Update " + _Reqtype + " - " + HttpContext.Current.Request.UserHostAddress.ToString() + "',GetDate(),'U' From M_MemberMaster Where IDNo='" + UserName + "'" +
                            " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                            "(0,'" + HttpContext.Current.Session["MemName"] + "','" + _Reqtype + " Detail','" + _Reqtype + " Detail Update','" + Remark + "',Getdate(),'" + formno + "')",
                            Conn);

                        j = Comm.ExecuteNonQuery();
                        if (j != 0)
                        {
                            _Output = "{\"response\":\"OK\"}";
                        }
                        else
                        {
                            _Output = "{\"response\":\"FAILED\"}";
                        }
                    }
                }

                ErrObj.Response = "FAILED";
                writeJson(ErrObj);
            }
            catch (Exception)
            {
                ErrObj.Response = "FAILED";
                writeJson(ErrObj);
            }
        }
        else
        {
            ErrObj.Response = "FAILED";
            writeJson(ErrObj);
        }
    }
    private void AddressProof(string Imgpath, string Imgpath1, string _Reqtype, Dictionary<string, string> dict)
    {
        string ImgPrefix, ImgFld = "", ImgDateFld, sql;
        string UserName = ClearInject(dict["userid"]);
        string Password = ClearInject(dict["passwd"]);
        string _Output = "";
        string Remark = "";
        string formno = "0";
        int j = 0;
        string Name = "";
        bool Bool;

        try
        {
            Bool = UserExists(UserName, Password);
            formno = GetFormNo(UserName);

            if (Bool)
            {
                DataTable Dt1 = new DataTable();

                string query = IsoStart +
                    "Select a.Idno, (a.MemFirstName + ' ' + a.MemlastName) As MemberName, a.Address1, a.City, a.District, " +
                    "a.Statecode, a.pinCode, a.Areacode, b.* " +
                    "From " + ObjDAL.dBName + "..M_MemberMaster as a, " + ObjDAL.dBName + "..KYCVerify as b " +
                    "Where a.Formno = b.Formno and a.Idno = '" + UserName + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(query, selectConn))
                {
                    Adp.Fill(Dt1);
                }

                if (Dt1.Rows.Count > 0)
                {
                    Name = Dt1.Rows[0]["MemberName"].ToString();

                    if (ClearInject(Dt1.Rows[0]["Address1"].ToString().ToUpper()) != ClearInject(dict["address1"].ToUpper()))
                        Remark += "Address ,";

                    if (ClearInject(Dt1.Rows[0]["City"].ToString().ToUpper()) != ClearInject(dict["city"].ToUpper()))
                        Remark += " City ,";

                    if (ClearInject(Dt1.Rows[0]["District"].ToString().ToUpper()) != ClearInject(dict["district"].ToUpper()))
                        Remark += " District,";

                    if (Convert.ToInt32(Dt1.Rows[0]["StateCode"]) != Convert.ToInt32(ClearInject(dict["statecode"])))
                        Remark += " State ,";

                    if (ClearInject(Dt1.Rows[0]["PinCode"].ToString()) != ClearInject(dict["pincode"]))
                        Remark += " PinCode,";

                    if (ClearInject(Dt1.Rows[0]["idtype"].ToString()) != ClearInject(dict["idproofid"]))
                        Remark += " AddressProofType,";

                    if (ClearInject(Dt1.Rows[0]["IdProofNo"].ToString()) != ClearInject(dict["idproofno"]))
                        Remark += "AddressProofNo,";

                    if (ClearInject(Dt1.Rows[0]["Areacode"].ToString()) != ClearInject(dict["areacode"]))
                        Remark += "Area,";
                }

                string ImgStr = "";

                if (!string.IsNullOrEmpty(Imgpath))
                {
                    ImgPrefix = "A";
                    ImgFld = ", AddrProof ='" + Imgpath + "', AddrProofDate = Getdate()";
                }

                if (!string.IsNullOrEmpty(Imgpath1))
                {
                    ImgFld += ", BackAddressProof='" + Imgpath1 + "', BackAddressDate = Getdate()";
                }

                ImgStr = "Update KycVerify Set IsaddrssVerified='P', " +
                         "Idtype='" + Convert.ToInt32(ClearInject(dict["idproofid"])) + "', " +
                         "IdProofNo='" + ClearInject(dict["idproofno"]).Trim().ToUpper() + "'" +
                         ImgFld + " where Formno= '" + formno + "'";

                ImgPrefix = "A";

                ImgFld = "Address1='" + ClearInject(dict["address1"].ToUpper()) + "', " +
                         "Tehsil='" + ClearInject(dict["city"].ToUpper()) + "', " +
                         "City='" + ClearInject(dict["city"].ToUpper()) + "', " +
                         "District='" + ClearInject(dict["district"].ToUpper()) + "', " +
                         "StateCode='" + ClearInject(dict["statecode"]) + "', " +
                         "Pincode='" + ClearInject(dict["pincode"]) + "', " +
                         "areacode='" + Convert.ToInt32(ClearInject(dict["areacode"])) + "', " +
                         "citycode='" + Convert.ToInt32(ClearInject(dict["areacode"])) + "', " +
                         "districtcode='" + Convert.ToInt32(ClearInject(dict["districtcode"])) + "'";

                sql = "Update M_MemberMaster Set " + ImgFld + " Where IDNo='" + UserName + "' " +
                      ImgStr + " " +
                      " Insert Into TempKycVerify Select *, GetDate(), '" + Convert.ToInt32(formno) + "' From KycVerify Where FormNo='" + Convert.ToInt32(formno) + "' " +
                      " Insert Into TempMemberMaster Select *,'Update " + _Reqtype + " - " + Context.Request.UserHostAddress.ToString() + "', GetDate(), 'U' From M_MemberMaster Where IDNo='" + UserName + "' " +
                      " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) " +
                      " Values (0,'" + Name.Trim() + "','" + _Reqtype + " Detail','" + _Reqtype + " Detail Update','" + Remark + "', Getdate(), '" + formno + "')";

                using (SqlCommand Comm = new SqlCommand(sql, Conn))
                {
                    j = Comm.ExecuteNonQuery();
                }

                if (j != 0)
                {
                    _Output = "{\"response\":\"OK\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        Response.Write(_Output);
    }
    private void BankProof(string Imgpath, string _Reqtype, Dictionary<string, string> dict)
    {
        string ImgPrefix, ImgFld = "", ImgDateFld, sql;
        string UserName = ClearInject(dict["userid"]);
        string Password = ClearInject(dict["passwd"]);
        string _Output = "";
        string Remark = "";
        string formno = "0";
        string Name = "";
        int j = 0;
        bool Bool;

        try
        {
            Bool = UserExists(UserName, Password);
            formno = GetFormNo(UserName);

            if (Bool)
            {
                DataTable Dt1 = new DataTable();

                string query = IsoStart +
                    " Select a.Formno, a.Bankid, a.BranchName, a.Acno, a.Ifscode, a.fax, b.BankProof, " +
                    " b.BankProofDate, (a.MemFirstName + '' + a.MemLastName) as MemberName " +
                    " From " + ObjDAL.dBName + "..M_MemberMaster as a, " + ObjDAL.dBName + "..KycVerify as b " +
                    " Where a.Formno = b.formno and a.Idno = '" + UserName + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(query, selectConn))
                {
                    Adp.Fill(Dt1);
                }

                if (Dt1.Rows.Count > 0)
                {
                    Name = Dt1.Rows[0]["MemberName"].ToString();

                    if (Convert.ToInt32(Dt1.Rows[0]["BankId"]) != Convert.ToInt32(ClearInject(dict["bankcode"])))
                    {
                        Remark += " Bank,";
                    }

                    if (ClearInject(Dt1.Rows[0]["BranchName"].ToString()) != ClearInject(dict["branchname"].ToUpper()))
                    {
                        Remark += " BranchName,";
                    }

                    if (ClearInject(Dt1.Rows[0]["AcNo"].ToString()) != ClearInject(dict["accountno"]))
                    {
                        Remark += " AccountNo,";
                    }

                    if (ClearInject(Dt1.Rows[0]["IFSCode"].ToString()) != ClearInject(dict["ifsccode"].ToUpper()))
                    {
                        Remark += " IFSCCode,";
                    }

                    if (Dt1.Rows[0]["Fax"].ToString() != ClearInject(dict["accounttype"].ToUpper()))
                    {
                        Remark += " Account Type,";
                    }
                }

                if (!string.IsNullOrEmpty(Imgpath))
                {
                    ImgFld = " Update KycVerify Set BankProof='" + Imgpath + "', BankProofDate=Getdate(), IsBankVerified='P' where Formno= '" + Convert.ToInt32(formno) + "'";
                }

                ImgPrefix = "B";

                sql = " Update M_MemberMaster Set Acno='" + ClearInject(dict["accountno"]) + "', " +
                      " Bankid='" + ClearInject(dict["bankcode"]) + "', " +
                      " IFscode='" + ClearInject(dict["ifsccode"].ToUpper()) + "', " +
                      " Branchname='" + ClearInject(dict["branchname"].ToUpper()) + "', " +
                      " Fax='" + ClearInject(dict["accounttype"].ToUpper()) + "' " +
                      " Where IDNo='" + UserName + "' " +
                      ImgFld + " " +
                      " Insert Into TempMemberMaster Select *,'Update " + _Reqtype + " - " + Context.Request.UserHostAddress.ToString() + "', GetDate(), 'U' From M_MemberMaster Where IDNo='" + UserName + "' " +
                      " Insert Into TempKycVerify Select *, GetDate(), '" + Convert.ToInt32(formno) + "' From KycVerify Where FormNo='" + Convert.ToInt32(formno) + "' " +
                      " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) " +
                      " Values (0, '" + Name.Trim() + "', '" + _Reqtype + " Detail', '" + _Reqtype + " Detail Update', '" + Remark + "', Getdate(), '" + formno + "')";

                using (SqlCommand Comm = new SqlCommand(sql, Conn))
                {
                    j = Comm.ExecuteNonQuery();
                }

                if (j != 0)
                {
                    _Output = "{\"response\":\"OK\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        Response.Write(_Output);
    }
    private void PanCardNo(string Imgpath, string _Reqtype, Dictionary<string, string> dict)
    {
        string ImgPrefix, ImgFld = "", sql;
        string UserName = ClearInject(dict["userid"]);
        string Password = ClearInject(dict["passwd"]);
        string _Output = "";
        string Remark = "";
        string formno = "0";
        string Name = "";
        int j = 0;
        bool Bool;

        try
        {
            Bool = UserExists(UserName, Password);
            formno = GetFormNo(UserName);

            if (Bool)
            {
                DataTable Dt1 = new DataTable();
                string query = IsoStart +
                    "Select a.IDNo, (a.MemFirstName + ' ' + a.MemLastName) As MemName, a.Panno, b.PanImg, " +
                    "Replace(Convert(Varchar, b.PANImgDate, 106), ' ', '-') as PanProofdate, " +
                    "b.IsPanVerified, Case when b.IsPanVerified <> 'N' then Replace(Convert(varchar, b.PanVerifyDate, 106), ' ', '-') " +
                    "Else '' End as PanVerifyDate, " +
                    "CASE WHEN b.IsPanVerified = 'Y' THEN 'Verified' " +
                    "WHEN b.IsPanVerified = 'P' THEN 'Pending' " +
                    "WHEN b.IsPanVerified = 'R' THEN 'Rejected' " +
                    "ELSE 'Not Verified' END AS PanVerf, " +
                    "case when b.IsPanVerified = 'R' then b.PanRemarks else '' end as RejectRemark " +
                    "From " + ObjDAL.dBName + "..M_MemberMaster as a " +
                    "Inner Join " + ObjDAL.dBName + "..KycVerify as b On a.Formno = b.Formno " +
                    "where a.Idno = '" + UserName + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(query, selectConn))
                {
                    Adp.Fill(Dt1);
                }

                if (Dt1.Rows.Count > 0)
                {
                    Name = Dt1.Rows[0]["MemName"].ToString();

                    if (ClearInject(Dt1.Rows[0]["Panno"].ToString()) != ClearInject(dict["panno"]))
                    {
                        Remark += " PANNo,";
                    }

                    if (ClearInject(Dt1.Rows[0]["PanImg"].ToString()) != ClearInject(Imgpath))
                    {
                        Remark += " PanCardImage,";
                    }
                }

                if (!string.IsNullOrEmpty(Imgpath))
                {
                    ImgFld = " Update KycVerify Set PanImg='" + Imgpath + "', PANImgDate=Getdate(), IsPanVerified='P' where Formno= '" + Convert.ToInt32(formno) + "'";
                }

                ImgPrefix = "B";

                sql = " Update M_MemberMaster Set panno='" + ClearInject(dict["panno"]) + "' Where IDNo='" + UserName + "' " +
                      ImgFld + " " +
                      " Insert Into TempMemberMaster Select *,'Update " + _Reqtype + " - " + Context.Request.UserHostAddress.ToString() + "',GetDate(),'U' From M_MemberMaster Where IDNo='" + UserName + "' " +
                      " Insert Into TempKycVerify Select *,GetDate(),'" + Convert.ToInt32(formno) + "' From KycVerify Where FormNo='" + Convert.ToInt32(formno) + "' " +
                      " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values " +
                      "(0,'" + Name.Trim() + "','" + _Reqtype + " Detail','" + _Reqtype + " Detail Update','" + Remark + "',Getdate(),'" + formno + "')";

                using (SqlCommand Comm = new SqlCommand(sql, Conn))
                {
                    j = Comm.ExecuteNonQuery();
                }

                if (j != 0)
                {
                    _Output = "{\"response\":\"OK\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        Response.Write(_Output);
    }
    private string Directs(string userid, string passwd, string mlevel, string legno, int FromNo, int ToNo, int pageIndex, string Searchby)
    {
        string _Output = "";
        try
        {
            bool Bool = UserExists(userid, passwd);
            string FormNo = GetFormNo(ClearInject(userid));

            string qry = IsoStart + "Select * from " + ObjDAL.dBName + "..V#ReferalDownlineinfo where Formno=" + FormNo + " " + IsoEnd;
            DataTable DtCountry = new DataTable();
            using (SqlDataAdapter Adp = new SqlDataAdapter(qry, selectConn))
            {
                Adp.Fill(DtCountry);
            }

            if (DtCountry.Rows.Count > 0)
            {
                _Output = "{\"totaldirectleft\":\"" + DtCountry.Rows[0]["RegisterLeft"] + "\"," +
                          "\"totaldirectright\":\"" + DtCountry.Rows[0]["RegisterRight"] + "\"," +
                          "\"activedirectleft\":\"" + DtCountry.Rows[0]["ConfirmLeft"] + "\"," +
                          "\"activedirectright\":\"" + DtCountry.Rows[0]["ConfirmRight"] + "\"," +
                          "\"directbvleft\":\"" + DtCountry.Rows[0]["LeftBv"] + "\"," +
                          "\"directbvright\":\"" + DtCountry.Rows[0]["RightBv"] + "\",";
            }

            if (Bool && FormNo != "0")
            {
                DataSet ds = new DataSet();
                string s = "";

                if (Searchby == "A")
                    Searchby = "";

                // Stored procedure call
                s = IsoStart + "exec sp_GetLevelDetail1 '" + mlevel + "','" + legno + "','" + Searchby + "','" + FormNo + "',1,'" + FromNo + "','" + ToNo + "','1'" + IsoEnd;

                using (SqlCommand Comm = new SqlCommand(s, selectConn))
                {
                    Comm.CommandTimeout = 100000000;
                    using (SqlDataAdapter adp1 = new SqlDataAdapter(Comm))
                    {
                        adp1.Fill(ds);
                    }
                }

                int recordCount = ds.Tables[1].Rows.Count > 0 ? Convert.ToInt32(ds.Tables[1].Rows[0]["RecordCount"]) : 0;

                _Output += "\"directs\": [";

                if (recordCount > 0)
                {
                    foreach (DataRow Dr in ds.Tables[0].Rows)
                    {
                        _Output += "{";
                        _Output += "\"level\":\"" + Dr["Mlevel"] + "\",";
                        _Output += "\"idno\":\"" + Dr["idno"] + "\",";
                        _Output += "\"memname\":\"" + Dr["MemName"] + "\",";
                        _Output += "\"pkg\":\"" + Dr["PackageName"] + "\",";
                        _Output += "\"sponsorid\":\"" + Dr["sponsorid"] + "\",";
                        _Output += "\"sponsorname\":\"" + Dr["MemberName"] + "\",";
                        _Output += "\"activestatus\":\"" + Dr["Status"] + "\",";
                        _Output += "\"activationdate\":\"" + Dr["UpgradeDate"] + "\",";
                        _Output += "\"bv\":\"" + Dr["bv"] + "\"},";
                    }
                    // Remove last comma
                    _Output = _Output.TrimEnd(',');
                }

                _Output += "],";
                _Output += "\"recordcount\":\"" + recordCount + "\",";
                _Output += "\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string Validate_(string Referral, string Sponsor, string Side, string Type)
    {
        SqlDataReader Dread;

        // Checking Referral ID
        Comm = new SqlCommand(
                IsoStart +
                        "Select FormNo, MemFirstName + ' ' + MemLastName as MemName, ActiveStatus " +
                        "from " + ObjDAL.dBName + "..M_MemberMaster where Idno='" + Referral + "'" + IsoEnd,
                 selectConn);
        Dread = Comm.ExecuteReader();

        if (!Dread.Read())
        {
            Dread.Close();
            return "Invalid Referral ID.";
        }
        _RefFormNo = Dread["FormNo"].ToString();
        Dread.Close();
        Comm.Cancel();

        // If TYPE is empty, do full validation
        if (Type == "")
        {
            string _IsGetExtreme = "N";

            // Fetch config
            Comm = new SqlCommand(ObjDAL.Isostart + "select * from " + ObjDAL.dBName + "..M_ConfigMaster" + ObjDAL.IsoEnd, selectConn);
            Dread = Comm.ExecuteReader();

            if (Dread.Read())
            {
                _IsGetExtreme = Dread["IsGetExtreme"].ToString();
            }

            Dread.Close();
            Comm.Cancel();

            if (_IsGetExtreme == "N")
            {
                // Validate Sponsor ID
                Comm = new SqlCommand(
                    ObjDAL.Isostart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName from " + ObjDAL.dBName + "..M_MemberMaster where Idno='" + Sponsor + "'" + ObjDAL.IsoEnd,
                    selectConn);

                Dread = Comm.ExecuteReader();

                if (!Dread.Read())
                {
                    Dread.Close();
                    return "Invalid Sponsor ID.";
                }

                _UpLnFormNo = Dread["FormNo"].ToString();
                Dread.Close();
                Comm.Cancel();

                // Validate Side availability
                //Comm = new SqlCommand(
                //    ObjDAL.Isostart + "SELECT COUNT(*) AS CNT From " + ObjDAL.dBName + "..M_MemberMaster WHERE UpLnFormNo in " +
                //    "(Select FormNo From " + ObjDAL.dBName + "..M_MemberMaster Where IDNo='" + Sponsor + "') And Legno = " + Side + ObjDAL.IsoEnd,
                //    selectConn);

                //Dread = Comm.ExecuteReader();

                //if (!Dread.Read())
                //{
                //    Dread.Close();
                //    return "Selected Side Not Available.";
                //}
                //else
                //{
                //    if (Convert.ToInt32(Dread["CNT"]) >= 1)
                //    {
                //        Dread.Close();
                //        return "Selected Side Not Available.";
                //    }
                //}

                //Dread.Close();
                //Comm.Cancel();

                // Check if Sponsor is in Referral Downline
                //if (_RefFormNo != _UpLnFormNo)
                //{
                //    Comm = new SqlCommand(
                //        ObjDAL.Isostart  + "Select * from " + ObjDAL.dBName + "..R_MemTreeRelation where FormNo=" + _RefFormNo +
                //        " And FormNoDwn=" + _UpLnFormNo + ObjDAL.IsoEnd,
                //        selectConn);

                //    Dread = Comm.ExecuteReader();

                //    if (!Dread.Read())
                //    {
                //        Dread.Close();
                //        return "Sponsor does not exist in referral downline.";
                //    }

                //    Dread.Close();
                //    Comm.Cancel();
                //}
            }
        }

        return "OK";
    }
    //public string Validate_(string Referral, string Sponsor, string Side, string Type)
    //{
    //    SqlDataReader Dread;

    //    //// Checking If Entered Referral Id Exists Or Not
    //    //Comm = new SqlCommand(IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName from "
    //    //                      + ObjDAL.dBName + "..M_MemberMaster " +
    //    //                      "where Idno='" + Referral + "' AND (Formno in (Select distinct FormnoDwn FROM "
    //    //                      + ObjDAL.dBName + "..M_MemTreeRelation) Or Formno in (select distinct formno from "
    //    //                      + ObjDAL.dBName + "..M_MemTreeRelation)) " + IsoEnd, selectConn);

    //    //Dread = Comm.ExecuteReader();
    //    //if (!Dread.Read())
    //    //{
    //    //    Dread.Close();
    //    //    return "Invalid Referral ID.";
    //    //}
    //    //_RefFormNo = Dread["FormNo"].ToString();
    //    //Dread.Close();
    //    //Comm.Cancel();

    //    //if (string.IsNullOrEmpty(Type))
    //    //{
    //    //    string _IsGetExtreme = "N";

    //    //    // Get Config Value
    //    //    Comm = new SqlCommand(IsoStart + "select * from " + ObjDAL.dBName + "..M_ConfigMaster " + IsoEnd, selectConn);
    //    //    Dread = Comm.ExecuteReader();
    //    //    if (Dread.Read())
    //    //    {
    //    //        _IsGetExtreme = Dread["IsGetExtreme"].ToString();
    //    //    }
    //    //    Dread.Close();
    //    //    Comm.Cancel();
    //    // Checking If Entered Sponsor Id Exists Or Not
    //    Comm = new SqlCommand(IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName from "
    //                          + ObjDAL.dBName + "..M_MemberMaster where Idno='" + Sponsor + "'" + IsoEnd, selectConn);
    //    Dread = Comm.ExecuteReader();
    //    if (!Dread.Read())
    //    {
    //        Dread.Close();
    //        return "Invalid Sponsor ID.";
    //    }
    //    _UpLnFormNo = Dread["FormNo"].ToString();
    //    Dread.Close();
    //    Comm.Cancel();

    //    // Checking If Entered Side Valid Or Not
    //    Comm = new SqlCommand(IsoStart + "SELECT COUNT(*) AS CNT From " + ObjDAL.dBName + "..M_MemberMaster " +
    //                          "WHERE UpLnFormNo in (Select FormNo From " + ObjDAL.dBName + "..M_MemberMaster " +
    //                          "Where IDNo='" + Sponsor + "') And Legno = " + Side + IsoEnd, selectConn);
    //    Dread = Comm.ExecuteReader();
    //    if (!Dread.Read())
    //    {
    //        Dread.Close();
    //        return "Selected Side Not Available.";
    //    }
    //    else
    //    {
    //        if (Convert.ToInt32(Dread["CNT"]) >= 1)
    //        {
    //            Dread.Close();
    //            return "Selected Side Not Available.";
    //        }
    //    }
    //    Dread.Close();
    //    Comm.Cancel();

    //    // Checking If Entered Sponsor ID Exists In Referral Downline Or Not
    //    if (_RefFormNo != _UpLnFormNo)
    //    {
    //        Comm = new SqlCommand(IsoStart + "Select * from " + ObjDAL.dBName + "..M_MemTreeRelation " +
    //                              "where FormNo=" + _RefFormNo + " And FormNoDwn=" + _UpLnFormNo + IsoEnd, selectConn);
    //        Dread = Comm.ExecuteReader();
    //        if (!Dread.Read())
    //        {
    //            Dread.Close();
    //            return "Sponsor does not exist in referral downline.";
    //        }
    //        Dread.Close();
    //        Comm.Cancel();
    //    }
    //    //if (_IsGetExtreme == "N")
    //    //{

    //    //}
    //    //}

    //    return "OK";
    //}
    private string CreateRandomNumericString(int size)
    {
        char[] allowedChars = "123456789".ToCharArray();
        byte[] bytes = new byte[size];

        using (var crypto = new RNGCryptoServiceProvider())
        {
            crypto.GetNonZeroBytes(bytes);
        }

        StringBuilder retVal = new StringBuilder(size);
        foreach (byte b in bytes)
        {
            retVal.Append(allowedChars[b % allowedChars.Length]);
        }

        return retVal.ToString();
    }
    private string checkMobileNo(string MobileNo)
    {
        string sql = "";
        string _Output = "";
        string errType = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(MobileNo))
            {
                sql = ObjDAL.Isostart + "SELECT COUNT(*) Cnt FROM " + ObjDAL.dBName + "..M_Membermaster WHERE Mobl='" + MobileNo + "'" + ObjDAL.IsoEnd;

                DataTable Dt = new DataTable();
                Dt = SqlHelper.ExecuteDataset(selectConn, CommandType.Text, sql).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(Dt.Rows[0]["Cnt"]) >= 1)
                    {
                        errType = "Mobile Number";
                        _Output = "Faild";
                    }
                    else
                    {
                        _Output = "Ok";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // log exception if needed
        }

        return _Output;
    }
    private string checkEmailID(string EmailID)
    {
        string sql = "";
        string _Output = "";
        string errType = "";

        try
        {
            if (EmailID == null)
            {
                _Output = "Ok";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(EmailID))
                {
                    sql = ObjDAL.Isostart + "SELECT COUNT(*) Cnt FROM " + ObjDAL.dBName + "..M_Membermaster WHERE Email='" + EmailID + "'" + ObjDAL.IsoEnd; ;
                    Comm = new SqlCommand(sql, selectConn);
                    Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        if (Convert.ToInt32(Dr["Cnt"]) >= 1)
                        {
                            errType = "Email ID";
                            _Output = "Faild";
                        }
                        else
                        {
                            _Output = "Ok";
                        }
                    }

                    Dr.Close();
                }
            }
        }
        catch (Exception ex)
        {
            // log error if required
        }

        return _Output;
    }
    public string Register(Dictionary<string, string> dict)
    {
        string _Output = "";
        try
        {
            string strQry = "";
            string dblDistrict = "", dblTehsil = "", IfSC = "", dblPlan = "", JoinStatus = "", Category = "", Formno = "0";
            int SessID = 0, dblState = 0, dblBank = 0, InVoiceNo = 0, KitID = 0;
            double Bv = 0, Rp = 0, Kitamount = 0;
            char cGender = 'M', cMarried = 'N';
            string HostIp = HttpContext.Current.Request.UserHostAddress.ToString();
            string _Response = "";
            SqlDataReader DRead;

            try
            {
                if (string.IsNullOrWhiteSpace(dict["referralid"]))
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Sponsor ID.\"}";
                    return _Output;
                }
                if (string.IsNullOrWhiteSpace(dict["name"]))
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Full Name.\"}";
                    return _Output;
                }
                if (dict["mobl"] == "0")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Mobile No.\"}";
                    return _Output;
                }
                if (checkMobileNo(dict["mobl"]) != "Ok")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Mobile No. already registered on another Ids.\"}";
                    return _Output;
                }
                if (dict["email"] == "")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Email ID.\"}";
                    return _Output;
                }
                if (checkEmailID(dict["email"]) != "Ok")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Email ID already registered on another Ids.\"}";
                    return _Output;
                }
                if (dict["countrycode"] == "0")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please select Country.\"}";
                    return _Output;
                }

                if (!string.IsNullOrWhiteSpace(dict["referralid"]))
                {
                    _Response = Validate_(dict["referralid"], dict["referralid"], "1", "");
                }
                else
                {
                    _Response = "OK";
                }
                if (_Response == "OK")
                {

                    Bv = 0; Rp = 0; Category = "Registration"; KitID = 1; JoinStatus = "N";
                    if (_Response == "OK")
                    {
                        dblDistrict = "0";
                        dblTehsil = "0";
                        dblState = 0;
                        dblBank = 0;
                        IfSC = "";
                        dblPlan = "0";
                        InVoiceNo = 0;
                        if (selectConn.State == ConnectionState.Closed) selectConn.Open();
                        Comm = new SqlCommand(IsoStart + "Select top 1 SessId as SessId from " + ObjDAL.dBName + "..M_SessnMaster order by SessID desc" + IsoEnd, selectConn);
                        DRead = Comm.ExecuteReader();
                        SessID = DRead.Read() ? Convert.ToInt32(DRead["SessID"]) : 0;
                        DRead.Close();
                        Comm.Cancel();
                        DateTime Dtp, Dtp1, MariedDate;
                        try { Dtp = Convert.ToDateTime(dict["dob"]); } catch { Dtp = DateTime.Parse("1940-01-01"); }
                        try { Dtp1 = Convert.ToDateTime(dict["transdate"]); } catch { Dtp1 = DateTime.Now; }
                        try { MariedDate = Convert.ToDateTime(dict["marriagedate"]); } catch { MariedDate = DateTime.Now; }
                        Random rand = new Random();
                        string RandomNumber = rand.Next(10000, 99999).ToString();
                        DataTable dt = new DataTable();
                        string OrderNo;
                        DAL obj = new DAL();
                        string ddlMobileNAme = "1";
                        try
                        {

                            string strQuery = IsoStart + "SELECT StdCode FROM " + ObjDAL.dBName + "..M_CountryMaster WHERE ACTIVESTATUS='Y' AND Cid = '" + dict["countrycode"] + "' " + IsoEnd;
                            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                ddlMobileNAme = dt.Rows[0]["StdCode"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception
                        }




                        strQry = "INSERT INTO m_memberMaster (" +
 "SessId,IdNo,CardNo,FormNo,KitId," +
 "UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo," +
 "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation," +
 "NomineeName,Address1,Address2,Post," +
 "Tehsil,City,CityCode,District,DistrictCode,StateCode,CountryId,AreaCode," +
 "PinCode,PhN1,Fax,Mobl,MarrgDate," +
 "Passw,Doj,Relation,PanNo," +
 "BankID,MICRCode,BranchName,EMail,BV," +
 "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp," +
 "PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,AadharNo,AadharNo2,AAdharNo3,fld6, usercode) " +
 "VALUES (" +
 SessID + ",0,0,0," + KitID + "," + _UpLnFormNo + ",0,1,0," + _RefFormNo + "," +
 "'" + ClearInject(dict["name"].ToUpper()) + "',''," +
 "'S/O','','" + Dtp.ToString("dd-MMM-yyyy") + "','" + cGender + "',''," +
 "'','','',''," +
 dblTehsil + "," + dblTehsil + ",0," +
 dblDistrict + ",0," + dblState + ",'" + dict["countrycode"] + "',0," +
 "'','0','CHOOSE ACCOUNT TYPE','" + dict["mobl"] + "','" + MariedDate.ToString("dd-MMM-yyyy") + "'," +
 "'" + RandomNumber + "',GETDATE(),'',''," +
 dblBank + ",'','','" + dict["email"] + "'," +
 Bv + ",0,'" + RandomNumber + "','" + RandomNumber + "'," +
 "'" + JoinStatus + "'," +
 "'" + InVoiceNo + "','" + Rp + "','" + HostIp + "',0,'',''," +
 "'0','','" + Dtp1.ToString("dd-MMM-yyyy") + "','','N','','0','0','APP', '" + ddlMobileNAme + "')";
                        string Ks = " BEGIN TRY BEGIN TRANSACTION " + strQry + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH ";
                        int i = 0;
                        if (Conn.State == ConnectionState.Closed) Conn.Open();
                        Comm = new SqlCommand(Ks, Conn);
                        i = Comm.ExecuteNonQuery();
                        Comm.Cancel();
                        string membername = "", Mobl = "", LastInsertid = "", Password = "", lastformno = "", sponsorName = "", sponsorMobl = "";
                        if (i != 0)
                        {
                            Comm = new SqlCommand(IsoStart + "SELECT TOP 1 a.Mid,a.DSessid, a.epassw,a.IDNO,a.formno,b.IsBill,a.Passw,a.MemFirstname,a.MemlastName,a.Email,a.Mobl," +
                                                  " (c.MemFirstName+''+c.MemLastName) as SponsorName,c.Mobl as SponsorMobl " +
                                                  " FROM " + ObjDAL.dBName + "..m_MemberMaster as a," + ObjDAL.dBName + "..m_KitMaster as b ," + ObjDAL.dBName + "..M_MemberMaster as c where a.RefFormno=c.Formno  and" +
                                                  " a.kitid=b.kitid  ORDER BY a.mid DESC" + IsoEnd, selectConn);
                            DRead = Comm.ExecuteReader();

                            if (DRead.Read())
                            {
                                //_Output = "{\"response\":\"OK\",\"msg\":\"Registered Successfully!!\",\"idno\":\"" + DRead["IDNo"] + "\",\"password\":\"" + DRead["passw"] + "\",\"trapassword\":\"" + DRead["epassw"] + "\"," +
                                //          "\"url\":\"" + HttpContext.Current.Session["CompWeb"].ToString().Trim() + "/Welcome.aspx?id=" + DRead["IDNo"] + "\"," +
                                //          "\"formno\":\"" + DRead["Formno"] + "\"}";
                                _Output = "{\"response\":\"OK\",\"msg\":\"Thank You For Registration Your Login Details Is..\",\"idno\":\"" + DRead["IDNo"] + "\",\"password\":\"" + DRead["Passw"] + "\",\"trapassword\":\"" + DRead["epassw"] + "\"," +
                                       "\"formno\":\"" + DRead["Formno"] + "\"}";

                                membername = DRead["MemfirstName"] + " " + DRead["MemLastName"];
                                LastInsertid = DRead["idno"].ToString();
                                lastformno = DRead["formno"].ToString();
                                Password = DRead["Passw"].ToString();
                                Mobl = DRead["Mobl"].ToString();
                                HttpContext.Current.Session["Kit"] = DRead["IsBill"];
                                SendToMemberMail(LastInsertid, DRead["Email"].ToString(), membername, Password, DRead["epassw"].ToString());
                            }
                            DRead.Close();


                        }

                    }
                    else
                    {
                        _Output = "{\"response\":\"FAILED\",\"msg\":\"" + _Response + "\"}";
                    }
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"" + _Response + "\"}";
                }
            }
            catch (Exception e)
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"" + e.Message + "\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"FAILED\"}";
        }
        return _Output;
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password, string TransactionPassword)
    {
        try
        {
            System.Net.Mail.MailAddress sendFrom =
                new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress sendTo =
                new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage myMessage =
                new System.Net.Mail.MailMessage(sendFrom, sendTo);

            string strMsg = @"
<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width,initial-scale=1.0'>
<title>Welcome to ePay Digital India</title>
<style>
  body{margin:0;padding:0;background:#f4f6fb;font-family:Arial,Helvetica,sans-serif}
  table{border-collapse:collapse}
  .outer{width:100%;background:#f4f6fb;padding:32px 16px}
  .card{width:560px;margin:0 auto;background:#ffffff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0}
  .header{background:#0f2b5b;padding:32px 36px 28px}
  .header-brand{margin:0 0 8px;font-size:12px;color:#93c5fd;letter-spacing:0.5px;font-family:Arial,sans-serif}
  .header-title{margin:0;font-size:20px;font-weight:bold;color:#ffffff;line-height:1.35;font-family:Arial,sans-serif}
  .body{padding:28px 36px}
  .p{margin:0 0 16px;font-size:14px;color:#1e293b;line-height:1.75;font-family:Arial,sans-serif}
  .name{color:#1a4db3;font-weight:bold}
  .cred-table{width:100%;background:#f8faff;border-radius:8px;margin:0 0 20px;border:1px solid #dbeafe}
  .cred-row-top{padding:12px 20px 8px;border-bottom:1px solid #e2e8f0}
  .cred-row-mid{padding:8px 20px;border-bottom:1px solid #e2e8f0}
  .cred-row-bot{padding:8px 20px 12px}
  .cred-label{font-size:13px;color:#64748b;font-family:Arial,sans-serif}
  .cred-val{font-size:13px;font-weight:bold;color:#1e293b;font-family:'Courier New',monospace;text-align:right}
  .btn{display:inline-block;background:#1a4db3;color:#ffffff;text-decoration:none;padding:11px 26px;border-radius:8px;font-size:14px;font-weight:bold;font-family:Arial,sans-serif;margin:0 0 20px}
  .warn{font-size:13px;color:#92400e;background:#fff7ed;border:1px solid #fed7aa;border-radius:8px;padding:12px 16px;margin:0 0 20px;line-height:1.65;font-family:Arial,sans-serif}
  .footer{border-top:1px solid #e2e8f0;padding:20px 36px}
  .footer-p{margin:0;font-size:13px;color:#64748b;line-height:1.65;font-family:Arial,sans-serif}
  .footer-name{color:#1e293b;font-weight:bold}
</style>
</head>
<body>
<div class='outer'>
  <div class='card'>

    <div class='header'>
      <p class='header-brand'>ePay Digital India Pvt. Ltd.</p>
      <p class='header-title'>Welcome to ePay Digital India &ndash;<br>Your Account is Ready</p>
    </div>

    <div class='body'>
      <p class='p'>Dear <span class='name'>" + MemberName + @"</span>,</p>
      <p class='p'>Welcome to <strong>ePay Digital India Pvt. Ltd.</strong> &mdash; your gateway to smart digital services and earning opportunities.</p>
      <p class='p'>Your account has been successfully created. Please find your login credentials below:</p>

      <table class='cred-table'>
        <tr>
          <td class='cred-row-top'>
            <table width='100%'><tr>
              <td class='cred-label'>User ID</td>
              <td class='cred-val'>" + IdNo + @"</td>
            </tr></table>
          </td>
        </tr>
        <tr>
          <td class='cred-row-mid'>
            <table width='100%'><tr>
              <td class='cred-label'>Login Password</td>
              <td class='cred-val'>" + Password + @"</td>
            </tr></table>
          </td>
        </tr>
        <tr>
          <td class='cred-row-bot'>
            <table width='100%'><tr>
              <td class='cred-label'>Transaction Password</td>
              <td class='cred-val'>" + TransactionPassword + @"</td>
            </tr></table>
          </td>
        </tr>
      </table>

      <a href='https://epayindia.in/' class='btn' style='color: white;'>Login Now &rarr;</a>

      <p class='warn'>For your security, we strongly recommend changing your password after your first login.</p>

      <p class='p' style='margin:0'>If you need any assistance, our support team is always here to help.</p>
    </div>

    <div class='footer'>
      <p class='footer-p'>Warm regards,<br><span class='footer-name'>Team ePay Digital India Pvt. Ltd.</span></p>
    </div>

  </div>
</div>
</body>
</html>";

            myMessage.Subject = "Welcome to ePay Digital India – Your Account is Ready";
            myMessage.Body = strMsg;
            myMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp =
                new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials =
                new System.Net.NetworkCredential(
                    Session["CompMail"].ToString(),
                    Session["MailPass"].ToString()
                );

            smtp.Send(myMessage);
            return true;
        }
        catch (Exception)
        {
            Response.Write("Mail could not be sent. Please try again later.");
            return false;
        }
    }
    private bool SendSMSJoining(string SMS, string MobileNo)
    {
        using (WebClient client = new WebClient())
        {
            string baseurl = string.Empty;
            try
            {
                baseurl = "http://78.46.58.54/vb/apikey.php?apikey=baeVEum0EOQkbng7&senderid=OVRNET&templateid=1707160354001194457&number="
                          + MobileNo + "&message=" + SMS;
                // Open the URL and read the response
                using (Stream data = client.OpenRead(baseurl))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        string response = reader.ReadToEnd();
                        // Optionally, you can log the response if needed
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Optionally, log ex.Message
                return false;
            }
        }
    }
    public string ViewProductOrderDetail(string userid, string Passw, string Orderno)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, Passw))
            {
                DataTable DtMembers = new DataTable();
                _Output = "{\"detail\": [";

                string strquery = IsoStart + "select *, (Rate*Qty) as TotalAmount from ZenexInv..trnbillDetails " +
                                  "where Convert(varchar,BillNo)='" + Orderno + "' " +
                                  "and Formno=(Select formno from " + ObjDAL.dBName + "..M_membermaster where idno='" + userid.Trim() + "') " +
                                  "and Qty<>0 and NetAmount>0" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(strquery, selectConn))
                {
                    Adp.Fill(DtMembers);
                }

                if (DtMembers.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtMembers.Rows)
                    {
                        _Output += "{\"orderno\":\"" + Dr["billno"] + "\",\"productname\":\"" + Dr["productname"] + "\",\"rate\":\"" + Dr["rate"] + "\"," +
                                   "\"qty\":\"" + Dr["qty"] + "\",\"amount\":\"" + Dr["totalamount"] + "\"},";
                    }
                    _Output = _Output.TrimEnd(',');
                }

                _Output += "],\"response\":\"OK\"}";
                Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string ProductRequestDetail(string userid, string Passw)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, Passw))
            {
                DataTable DtMembers = new DataTable();
                _Output = "{\"products\": [";

                string strquery = IsoStart + " select Cast(Orderno as varchar) Orderno, replace(convert(varchar,orderdate,106),' ','-') as orderdate, OrderAmt, " +
                                  "OrderQty as TotalQty, WalletAmt as Pinwallet, OtherAmt as OtherAmt, " +
                                  "Case when ActiveStatus='Y' and DispatchStatus='C' then 'Dispatched' when ActiveStatus='D' then 'Rejected' else 'Pending' end as status, " +
                                  "Case when Ordertype='T' then 'Activation' else 'Repurchase' end as OrderType, " +
                                  "bv, '#' as Website, '' as CourierName, '' as DocketNo, '' as DocketDate " +
                                  "from " + ObjDAL.dBName + "..TrnOrder where Formno='" + userid.Trim() + "' and DispatchStatus <> 'C' " +
                                  "Union All " +
                                  "Select Cast(billno as varchar) as Orderno, replace(convert(varchar,BillDate,106),' ','-') as orderdate, repurchincome as OrderAmount, " +
                                  "repurchincome as OrderQty, 0 as Pinwallet, 0 as OtherAmt, " +
                                  "'Dispatched' as status, Case when Billtype='B' then 'Activation' else 'Repurchase' end as OrderType, " +
                                  "repurchincome as bv, '' as Website, '' as CourierName, '' as DocketNo, '' as DocketDate " +
                                  "from " + ObjDAL.dBName + "..repurchincome as a Where Formno='" + userid.Trim() + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(strquery, selectConn))
                {
                    Adp.Fill(DtMembers);
                }

                if (DtMembers.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtMembers.Rows)
                    {
                        _Output += "{\"orderno\":\"" + Dr["orderno"] + "\",\"orderdate\":\"" + Dr["orderdate"] + "\",\"amount\":\"" + Dr["orderamt"] + "\"," +
                                   "\"qty\":\"" + Dr["totalqty"] + "\",\"type\":\"" + Dr["ordertype"] + "\",\"status\":\"" + Dr["status"] + "\"},";
                    }
                    _Output = _Output.TrimEnd(',');
                }

                _Output += "],\"response\":\"OK\"}";
                Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string AllAchiever(string type, int fromno, int tono)
    {
        string _Output = "";
        try
        {
            DAL obj = new DAL();
            int RecordCount = 0;
            DataTable DtCountry = new DataTable();
            string sql = string.Empty;

            _Output = "{\"achiever\": [";

            if (type == "0")
            {
                string strquery = IsoStart + "select Count(*) as cnt from " + ObjDAL.dBName + "..M_memberMaster as a," +
                    "(select Max(RankId) as Rankid, Formno from " + ObjDAL.dBName + "..MstRankachievers Group by formno) as b," +
                    ObjDAL.dBName + "..MstRanks as c where a.formno=b.Formno and c.Rankid=b.Rankid " + IsoEnd;

                using (SqlCommand Comm = new SqlCommand(strquery, selectConn))
                {
                    RecordCount = Convert.ToInt32(Comm.ExecuteScalar());
                }

                sql = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,a.Idno,a.MemfirstName+' '+a.memlastname as MemberName," +
                      "c.Rank,CASE WHEN ProfilePic='' THEN 'https://overnettrading.com/images/no_photo.jpg' " +
                      "WHEN ProfilePic like 'http%' THEN ProfilePic else 'https://overnettrading.com/images/UploadImage/'+ProfilePic END AS ProfilePic " +
                      "from " + ObjDAL.dBName + "..M_memberMaster as a," +
                      "(select Max(RankId) as Rankid,Formno from " + ObjDAL.dBName + "..MstRankachievers Group by formno) as b," +
                      ObjDAL.dBName + "..MstRanks as c where a.formno=b.Formno and c.Rankid=b.Rankid";
            }
            else
            {
                string strquery = IsoStart + "select Count(*) as cnt from " + ObjDAL.dBName + "..M_memberMaster as a," +
                    "(select Max(RankId) as Rankid,Formno,Msessid from " + ObjDAL.dBName + "..MstRankachievers Group by formno,Msessid) as b," +
                    ObjDAL.dBName + "..MstRanks as c," +
                    "(select Max(Sessid) as Sessid from " + ObjDAL.dBName + "..M_MonthSessnMaster) as d " +
                    "where a.formno=b.Formno and c.Rankid=b.Rankid and b.msessid=d.sessid " + IsoEnd;

                using (SqlCommand Comm = new SqlCommand(strquery, selectConn))
                {
                    RecordCount = Convert.ToInt32(Comm.ExecuteScalar());
                }

                sql = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,a.Idno,a.MemfirstName+' '+a.memlastname as MemberName," +
                      "c.Rank,CASE WHEN ProfilePic='' THEN 'https://overnettrading.com/images/no_photo.jpg' " +
                      "WHEN ProfilePic like 'http%' THEN ProfilePic else 'https://overnettrading.com/images/UploadImage/'+ProfilePic END AS ProfilePic " +
                      "from " + ObjDAL.dBName + "..M_memberMaster as a," +
                      "(select Max(RankId) as Rankid,Formno,Msessid from " + ObjDAL.dBName + "..MstRankachievers Group by formno,Msessid) as b," +
                      ObjDAL.dBName + "..MstRanks as c," +
                      "(select Max(Sessid) as Sessid from " + ObjDAL.dBName + "..M_MonthSessnMaster) as d " +
                      "where a.formno=b.Formno and c.Rankid=b.Rankid and b.msessid=d.sessid";
            }

            using (SqlDataAdapter Adp = new SqlDataAdapter(IsoStart + "Select * FROM (" + sql + ") as b WHERE SNo>=" + fromno + " And SNo<=" + tono + " " + IsoEnd, selectConn))
            {
                Adp.Fill(DtCountry);
            }

            if (DtCountry.Rows.Count > 0)
            {
                foreach (DataRow Dr in DtCountry.Rows)
                {
                    _Output += "{\"sno\":\"" + Dr["sno"] + "\",\"idno\":\"" + Dr["Idno"] + "\",\"pic\":\"" + Dr["ProfilePic"] + "\",\"membername\":\"" + Dr["MemberName"] + "\"},";
                }
                _Output = _Output.TrimEnd(',');
            }

            _Output += "],\"recordcount\":\"" + RecordCount + "\",\"response\":\"OK\"}";
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string AmountTransfertootherid(string userid, string passwd, Decimal Amount, string FromActype, string Touserid, string transpassword, string userRemarks)
    {
        string _output = "";
        string voucherNo = "";
        string VouherNo2 = "";
        string membername = "";
        string toformno = "";
        DAL Objdal = new DAL();

        bool Bool = MemberExist(userid, passwd);
        string Formno = GetFormNo(userid);

        if (Amount > 0)
        {
            if (Bool)
            {
                using (SqlCommand Comm = new SqlCommand(IsoStart + "select * from " + Objdal.dBName + "..M_MemberMaster where Epassw='" + transpassword + "' and Formno=" + Formno + IsoEnd, selectConn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        Bool = Dr.Read();
                        if (Bool)
                        {
                            membername = Dr["memfirstname"].ToString();
                        }
                    }
                }

                using (SqlCommand Comm = new SqlCommand(IsoStart + "select * from " + Objdal.dBName + "..M_MemberMaster where Idno='" + Touserid + "'" + IsoEnd, selectConn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        Bool = Dr.Read();
                        if (Bool)
                        {
                            toformno = Dr["formno"].ToString();
                        }
                    }
                }

                if (Bool)
                {
                    if (CheckAmount(Formno, FromActype, Amount))
                    {
                        string sql = IsoStart + "select IsNull (Max(VoucherNo+1),100001) as VoucherNo from " + Objdal.dBName + "..TrnVoucher" + IsoEnd;
                        DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            voucherNo = dt.Rows[0]["VoucherNo"].ToString();
                            VouherNo2 = (Convert.ToInt32(voucherNo) + 1).ToString();
                        }

                        string Remarks = " Main Wallet Amount Transfer To " + Touserid + " from " + userid;
                        string Remark2 = " Main Wallet Amount Transfer To " + Touserid + "(" + userRemarks + ")";
                        string Remark1 = " Main Wallet Amount Received From " + userid + " " + membername + "(" + userRemarks + ")";

                        string query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID) values " +
                                       "('" + voucherNo + "',Getdate(),'" + Formno + "',0, '" + Amount + "', '" + Remark2 + "',  '" + voucherNo + "/" + Formno + "','" + FromActype + "',GetDate(),'D',Convert(Varchar,GetDate(),112), " + Session["CurrentSessn"] + ");";

                        query += "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID) values " +
                                 "('" + VouherNo2 + "',Getdate(),0,'" + toformno + "', '" + Amount + "',  '" + Remark1 + "', '" + VouherNo2 + "/" + Formno + "','" + FromActype + "',GetDate(),'C',Convert(Varchar,GetDate(),112), " + Session["CurrentSessn"] + ");";

                        query += "insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) values " +
                                 "(0,'" + membername + "','Main Wallet Transfer ','Main Wallet Transfer To Other Id','" + Remarks + "',Getdate(),'" + Formno + "')";

                        if (CheckAmount(Formno, FromActype, Amount))
                        {
                            int i = Objdal.SaveData("BEGIN TRY BEGIN TRANSACTION " + query + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH");
                            if (i > 0)
                                _output = "{\"response\":\"OK\",\"msg\":\"Amount Transfer Successfully.\"}";
                            else
                                _output = "{\"response\":\"FAILED\",\"msg\":\"Try Again\"}";
                        }
                        else
                        {
                            _output = "{\"response\":\"FAILED\",\"msg\":\"Insufficient Balance.\"}";
                        }
                    }
                    else
                    {
                        _output = "{\"response\":\"FAILED\",\"msg\":\"Insufficient Balance.\"}";
                    }
                }
                else
                {
                    _output = "{\"response\":\"FAILED\",\"msg\":\"Invalid To userid.\"}";
                }
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"msg\":\"Invalid transaction password.\"}";
            }
        }
        else
        {
            _output = "{\"response\":\"FAILED\",\"msg\":\"Amount cannot be negative\"}";
        }

        return _output;
    }
    public string GetBalance(string userid, string passwd, double MBalance, string Actype = "M")
    {
        string _Output = "";
        try
        {
            bool Bool = UserExists(userid, passwd);
            string FormNo = GetFormNo(ClearInject(userid));

            if (Bool && FormNo != "0")
            {
                string strQry = IsoStart + "Select * From " + ObjDAL.dBName + "..ufnGetBalance('" + FormNo + "','" + Actype + "') " + IsoEnd;
                using (SqlCommand Comm = new SqlCommand(strQry, selectConn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            _Output = "{\"credit\":\"" + Dr["Credit"] + "\",\"debit\":\"" + Dr["Debit"] + "\",\"balance\":\"" + Dr["Balance"] + "\",\"response\":\"OK\"}";
                            MBalance = Convert.ToDouble(Dr["Balance"]);
                        }
                    }
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string MonthlyIncentive(string userid, string passwd, int SNoFrom, int SNoTo)
    {
        string _Output = "";
        try
        {
            bool Bool = false;

            using (SqlCommand Comm = new SqlCommand(IsoStart + "Select * from " + ObjDAL.dBName + "..M_AppUser where UserId='" + userid + "' And Otp='" + passwd + "' and ActiveStatus='Y'" + IsoEnd, selectConn))
            {
                using (SqlDataReader Dr = Comm.ExecuteReader())
                {
                    Bool = Dr.Read();
                }
            }

            if (Bool)
            {
                DataTable DtCountry = new DataTable();
                _Output = "{\"monthlyincentive\":[";

                string strQry = IsoStart + "Select Count(*) from " + ObjDAL.dBName + "..V#MonthlyPayoutDetail as a," + ObjDAL.dBName + "..M_MonthSessnmaster as b where a.sessid=b.sessid and idno='" + userid + "' and (NetIncome>0 Or PrevBal>0 Or ClsBal>0)and OnWebSite = 'Y' " + IsoEnd;
                int RecordCount;
                using (SqlCommand CommCount = new SqlCommand(strQry, selectConn))
                {
                    RecordCount = Convert.ToInt32(CommCount.ExecuteScalar());
                }

                string query = IsoStart + "Select * FROM (Select ROW_NUMBER() OVER( ORDER BY a.SessID DESC ) as RwNo,A.* from " + ObjDAL.dBName + "..V#MonthlyPayoutDetail as a," + ObjDAL.dBName + "..M_MonthSessnmaster as b where a.sessid=b.sessid and idno='" + userid + "' and (NetIncome>0 Or PrevBal>0 Or ClsBal>0)and OnWebSite = 'Y'  ) as a WHERE RwNo>='" + SNoFrom + "' AND RwNo<='" + SNoTo + "' " + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(query, selectConn))
                {
                    Adp.Fill(DtCountry);
                }

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{\"payoutno\":\"" + Dr["SessID"] + "\",\"fromdate\":\"" + Dr["FromDate"] + "\",\"todate\":\"" + Dr["ToDate"] + "\",\"selfrepurchincome\":\"" + Dr["SelfRepurchaseIncome"] + "\",\"teamrepurchincome\":\"" + Dr["TeamRepurchaseIncome"] + "\",\"teamturnover\":\"" + Dr["TeamTurnOver"] + "\",\"sponsordevbonus\":\"" + Dr["SponsorDevelopment"] + "\",\"diamonddirect\":\"" + Dr["DiamondDirect"] + "\",\"grossamount\":\"" + Dr["NetIncome"] + "\",\"tdsamt\":\"" + Dr["TdsAmount"] + "\",\"admincharge\":\"" + Dr["AdminCharge"] + "\",\"previous\":\"" + Dr["PrevBal"] + "\",\"chqamt\":\"" + Dr["ChqAmt"] + "\",\"closing\":\"" + Dr["ClsBal"] + "\"},";
                    }
                    _Output = _Output.TrimEnd(',');
                }

                _Output += "],\"recordcount\":\"" + RecordCount + "\",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string Getnews()
    {
        string _Output = "";
        try
        {
            DataTable DtMembers = new DataTable();
            _Output = "{\"news\":[";

            Adp = new SqlDataAdapter(IsoStart + "Select * From " + ObjDAL.dBName + "..V#News Order by Nid Desc" + IsoEnd, selectConn);
            Adp.Fill(DtMembers);

            if (DtMembers.Rows.Count > 0)
            {
                foreach (DataRow Dr in DtMembers.Rows)
                {
                    string newsHead = JsonEncode(Dr["NewsHdr"].ToString().Replace("\"", ""));
                    string newsDetail = JsonEncode(Dr["NewsDtl"].ToString().Replace("\"", ""));
                    _Output += "{\"newsid\":\"" + Dr["NewsId"] + "\",\"newshead\":\"" + newsHead + "\",\"newsdetail\":\"" + newsDetail + "\"},";
                }
                _Output = _Output.TrimEnd(',');
            }

            _Output += "],\"response\":\"OK\"}";
            Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string DailyEverestIncome(string userid, string Passw, string SNoFrom, string ToNo)
    {
        string _Output = "";
        try
        {
            if (Bool) // Presuming Bool is a class-level variable for authentication
            {
                string FormNo = GetFormNo(ClearInject(userid));
                string sql = IsoStart + "exec sp_DailyEverestIncome '" + FormNo + "','" + SNoFrom + "','" + ToNo + "','1'" + IsoEnd;
                DataSet Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql);
                DataTable dt = Ds.Tables[0];
                int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);

                _Output = "{\"dailyeverestincome\":["; // <-- fixed semicolon here
                foreach (DataRow Dr in dt.Rows)
                {
                    string col = "{";
                    foreach (DataColumn column in dt.Columns)
                    {
                        string value = Dr[column] == DBNull.Value ? "0" : Dr[column].ToString();
                        col += "\"" + column.ColumnName + "\":\"" + value + "\",";
                    }
                    col = col.TrimEnd(',') + "},";
                    _Output += col;
                }
                if (dt.Rows.Count > 0)
                    _Output = _Output.TrimEnd(',');
                _Output += "],\"recordcount\":\"" + recordCount + "\",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string RefIncome(string userid, string Passw, string sessid, string SNoFrom, string ToNo)
    {
        string _Output = "";
        try
        {
            if (Bool) // Assuming Bool is authentication flag
            {
                DateTime currentDateTime = DateTime.Parse(sessid);
                string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                string FormNo = GetFormNo(ClearInject(userid));
                DataSet Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text,
                    IsoStart + "exec sp_refincome '" + currentDateTime.ToString("yyyyMMdd") + "','" + FormNo + "','" + SNoFrom + "','" + ToNo + "','1'" + IsoEnd);
                DataTable dt = Ds.Tables[0];
                int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);

                _Output = "{\"refincome\":[";
                foreach (DataRow Dr in dt.Rows)
                {
                    string col = "{";
                    foreach (DataColumn column in dt.Columns)
                    {
                        string value = Dr[column] == DBNull.Value ? "0" : Dr[column].ToString();
                        col += "\"" + column.ColumnName + "\":\"" + value + "\",";
                    }
                    col = col.TrimEnd(',') + "},";
                    _Output += col;
                }
                if (dt.Rows.Count > 0)
                    _Output = _Output.TrimEnd(',');
                _Output += "],\"recordcount\":\"" + recordCount + "\",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string Business(string userid, string Passw)
    {
        string _Output = "";
        try
        {
            if (Bool) // Presuming Bool is a class-level variable for authentication
            {
                string FormNo = GetFormNo(ClearInject(userid));
                DataSet Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text,
                    IsoStart + "exec Sp_LevelWiseBusinessReport_update '" + FormNo + "'" + IsoEnd);
                DataTable dt = Ds.Tables[0];

                _Output = "{\"business\":[";
                foreach (DataRow Dr in dt.Rows)
                {
                    string col = "{";
                    foreach (DataColumn column in dt.Columns)
                    {
                        string value = Dr[column] == DBNull.Value ? "0" : Dr[column].ToString();
                        col += "\"" + column.ColumnName + "\":\"" + value + "\",";
                    }
                    col = col.TrimEnd(',') + "},";
                    _Output += col;
                }
                if (dt.Rows.Count > 0)
                    _Output = _Output.TrimEnd(',');
                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;

    }
    public string EverestTree(string userid, string Passw)
    {
        string _Output = "";
        try
        {
            if (Bool) // Assuming Bool indicates valid login/authentication
            {
                string FormNo = GetFormNo(ClearInject(userid));
                DataSet Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text,
                    IsoStart + "exec Sp_GetEverestDataDeatil_New '" + FormNo + "'" + IsoEnd);
                DataTable dt = Ds.Tables[0];

                _Output = "{\"everestTree\":[";
                foreach (DataRow Dr in dt.Rows)
                {
                    string col = "{";
                    foreach (DataColumn column in dt.Columns)
                    {
                        string value = Dr[column] == DBNull.Value ? "0" : Dr[column].ToString();
                        col += "\"" + column.ColumnName + "\":\"" + value + "\",";
                    }
                    col = col.TrimEnd(',') + "},";
                    _Output += col;
                }
                if (dt.Rows.Count > 0)
                    _Output = _Output.TrimEnd(',');
                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;

    }
    public string Products(string userMobile, string OtpCode)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userMobile, OtpCode))
            {
                DataTable DtMembers = new DataTable();
                _Output = "{\"products\": [";

                string query = IsoStart +
                    "SELECT ProductCode AS ProdId, ProductName, MRP, DP, BV " +
                    "FROM " + Session["InvDB"] + "..M_ProductMaster " +
                    "WHERE ActiveStatus='Y' AND Imported='J' " +
                    "ORDER BY ProductName" + IsoEnd;

                Adp = new SqlDataAdapter(query, selectConn);
                Adp.Fill(DtMembers);

                if (DtMembers.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtMembers.Rows)
                    {
                        _Output += "{\"prodid\":\"" + Dr["ProdID"] + "\"," +
                                   "\"product\":\"" + Dr["ProductName"] + "\"," +
                                   "\"dp\":\"" + Dr["DP"] + "\"," +
                                   "\"mrp\":\"" + Dr["MRP"] + "\"," +
                                   "\"bv\":\"" + Dr["BV"] + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"response\":\"OK\"}";
                Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }
        return _Output;
    }
    public string DeliveryCenter(string userMobile, string OtpCode)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userMobile, OtpCode))
            {
                DataTable DtMembers = new DataTable();
                _Output = "{\"deliverycenter\": [";

                string query = IsoStart +
                    "SELECT * FROM (" +
                    "SELECT '0' AS PartyCode, '-- Choose Delivery Center--' AS PartyName " +
                    "UNION " +
                    "SELECT PartyCode, PartyName + ' ( ' + Address1 + ' )( ' + CityName + ' ) ' AS PartyName " +
                    "FROM " + Session["InvDB"] + "..M_Ledgermaster " +
                    "WHERE GroupId NOT IN (5,21) AND OnWebSite='Y') AS Temp " +
                    "ORDER BY PartyCode " + IsoEnd;

                Adp = new SqlDataAdapter(query, selectConn);
                Adp.Fill(DtMembers);

                if (DtMembers.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtMembers.Rows)
                    {
                        _Output += "{\"partycode\":\"" + Dr["PartyCode"] + "\"," +
                                   "\"partyname\":\"" + Dr["PartyName"] + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"response\":\"OK\"}";
                Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }
        return _Output;
    }
    public string PinTransferPackageList(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtState = new DataTable();
                _Output = "{\"packages\": [";

                string query = IsoStart +
                    "SELECT a.KitId, CAST(KitName AS varchar) AS KitName, Qty " +
                    "FROM " + ObjDAL.dBName + "..M_KitMaster AS a, " +
                    "(SELECT DISTINCT(ProdId) AS KitId, COUNT(FormNo) AS Qty " +
                    " FROM " + ObjDAL.dBName + "..M_FormGeneration " +
                    " WHERE FCode='" + userid + "' AND IsIssued='N' AND IsCancel='N' AND ActiveStatus='Y' " +
                    " GROUP BY ProdId) AS b " +
                    "WHERE a.KitId = b.KitId AND RowStatus='Y' " + IsoEnd;

                Adp = new SqlDataAdapter(query, selectConn);
                Adp.Fill(DtState);

                if (DtState.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtState.Rows)
                    {
                        _Output += "{\"pkgid\":\"" + Dr["KitID"] + "\"," +
                                   "\"pkgname\":\"" + Dr["KitName"] + "\"," +
                                   "\"qty\":\"" + Dr["Qty"] + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string DownlineSummary(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtDownline = new DataTable();
                string query = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..V#DownlineInfo " +
                               "WHERE FormNo IN (SELECT FormNo FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE IDNo='" + userid + "')" + IsoEnd;

                Adp = new SqlDataAdapter(query, selectConn);
                Adp.Fill(DtDownline);

                if (DtDownline.Rows.Count > 0)
                {
                    DataRow row = DtDownline.Rows[0];
                    _Output = "{\"registerleft\":\"" + row["RegisterLeft"] + "\"," +
                              "\"registerright\":\"" + row["RegisterRight"] + "\"," +
                              "\"confirmleft\":\"" + row["ConfirmLeft"] + "\"," +
                              "\"confirmright\":\"" + row["ConfirmRight"] + "\"," +
                              "\"leftbv\":\"" + row["leftbv"] + "\"," +
                              "\"rightbv\":\"" + row["rightbv"] + "\"," +
                              "\"response\":\"OK\"}";
                }
                else
                {
                    _Output = "{\"registerleft\":\"0\",\"registerright\":\"0\",\"confirmleft\":\"0\",\"confirmright\":\"0\"," +
                              "\"leftbv\":\"0\",\"rightbv\":\"0\",\"response\":\"OK\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string Downline(string userid, string passwd, int side, string FromNo, string ToNo)
    {
        string _Output = "";
        try
        {
            bool Bool = UserExists(userid, passwd);
            if (Bool)
            {
                string FormNo = GetFormNo(userid.Trim());
                DataTable DtCountry = new DataTable();
                DataTable Dt = new DataTable();
                string Condition = "";

                // Get total count
                Adp = new SqlDataAdapter(IsoStart + "SELECT COUNT(Idno) as TotalCount FROM " + ObjDAL.dBName + "..V#Downline WHERE FormNO='" + FormNo + "' AND LegNo=" + side + " " + Condition + " " + IsoEnd, selectConn);
                Adp.Fill(Dt);

                _Output = "{\"downline\": [";

                // Get paginated downline data
                Adp = new SqlDataAdapter(IsoStart +
                    "SELECT * FROM (" +
                    "SELECT *, ROW_NUMBER() OVER (ORDER BY JoinDate DESC) as SNO FROM (" +
                    "SELECT IdNo, MemName, Sponsor, Refformno, kitname, Doj, JoinDate, TopUpDate, KitAmount, BV " +
                    "FROM " + ObjDAL.dBName + "..V#Downline " +
                    "WHERE FormNO='" + FormNo + "' AND LegNo=" + side + " " + Condition +
                    ") as a) as b " +
                    "WHERE SNo >= " + FromNo + " AND SNo <= " + ToNo + " ORDER BY JoinDate DESC " + IsoEnd, selectConn);
                Adp.Fill(DtCountry);

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{\"sno\":\"" + Dr["SNo"] + "\"," +
                                   "\"idno\":\"" + Dr["IDNo"] + "\"," +
                                   "\"member\":\"" + Dr["MemName"] + "\"," +
                                   "\"uplineid\":\"" + Dr["Sponsor"] + "\"," +
                                   "\"refid\":\"" + Dr["Refformno"] + "\"," +
                                   "\"kitname\":\"" + Dr["KitName"] + "\"," +
                                   "\"doj\":\"" + Dr["Doj"] + "\"," +
                                   "\"upgradedate\":\"" + Dr["TopUpDate"] + "\"," +
                                   "\"kitamount\":\"" + Dr["KitAmount"] + "\"," +
                                   "\"bv\":\"" + Dr["BV"] + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1); // Remove trailing comma
                }

                _Output += "],\"recordcount\":\"" + Dt.Rows[0]["TotalCount"] + "\",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }
        return _Output;
    }
    public string IdTypelist()
    {
        string _Output = "";
        try
        {
            DataTable DtState = new DataTable();
            _Output = "{\"idtype\":[";

            Adp = new SqlDataAdapter(IsoStart + "SELECT Id, IdType FROM " + ObjDAL.dBName + "..M_IdTypeMaster WHERE ACTIVESTATUS='Y' ORDER BY id" + IsoEnd, selectConn);
            Adp.Fill(DtState);

            if (DtState.Rows.Count > 0)
            {
                foreach (DataRow Dr in DtState.Rows)
                {
                    _Output += "{\"id\":\"" + Dr["id"].ToString() + "\"," +
                               "\"Idtype\":\"" + Dr["IdType"].ToString() + "\"},";
                }
                _Output = _Output.Remove(_Output.Length - 1, 1); // Remove trailing comma
            }

            _Output += "],\"response\":\"OK\"}";
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }
        return _Output;
    }
    public string Achieverlist()
    {
        string _Output = "";
        try
        {
            DataTable DtState = new DataTable();
            _Output = "{\"achievertype\":[";

            Adp = new SqlDataAdapter(IsoStart + "SELECT Id, IdType FROM " + ObjDAL.dBName + "..M_AchievetypeMaster WHERE ACTIVESTATUS='Y' ORDER BY id" + IsoEnd, selectConn);
            Adp.Fill(DtState);

            if (DtState.Rows.Count > 0)
            {
                foreach (DataRow Dr in DtState.Rows)
                {
                    _Output += "{\"id\":\"" + Dr["id"].ToString() + "\"," +
                               "\"Idtype\":\"" + Dr["IdType"].ToString() + "\"},";
                }
                _Output = _Output.Remove(_Output.Length - 1, 1); // Remove trailing comma
            }

            _Output += "],\"response\":\"OK\"}";
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }
        return _Output;
    }
    public string AccountType()
    {
        string _Output = "";
        try
        {
            DataTable DtState = new DataTable();
            _Output = "{\"accounttype\":[";

            Adp = new SqlDataAdapter(IsoStart + "Select * From " + ObjDAL.dBName + "..AccountType " +
                                     "Where ActiveStatus='Y' Order by accid" + IsoEnd, selectConn);
            Adp.Fill(DtState);

            if (DtState.Rows.Count > 0)
            {
                foreach (DataRow Dr in DtState.Rows)
                {
                    _Output += "{\"accid\":\"" + Dr["accid"].ToString() + "\"," +
                               "\"accountype\":\"" + Dr["AccountType"].ToString() + "\"},";
                }
                _Output = _Output.Remove(_Output.Length - 1, 1); // Remove trailing comma
            }

            _Output += "],\"response\":\"OK\"}";
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string NewMonthlyIncentive(string userid, string passwd, string SNoFrom, string SNoTo)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtCountry = new DataTable();
                _Output = "{\"monthlyincentive\": [";

                string strQry = IsoStart + "Select Count(*) from " + ObjDAL.dBName + "..V#Monthlyincome " +
                                 "where idno='" + userid + "' and (NetIncome>0 Or PrevBal>0 Or ClsBal>0) and OnWebSite='Y' " + IsoEnd;
                Comm = new SqlCommand(strQry, selectConn);
                int RecordCount = Convert.ToInt32(Comm.ExecuteScalar());

                strQry = IsoStart + "Select * FROM (Select ROW_NUMBER() OVER(ORDER BY SessID DESC) as RwNo,* " +
                         "from " + ObjDAL.dBName + "..V#Monthlyincome " +
                         "where idno='" + userid + "' and (NetIncome>0 Or PrevBal>0 Or ClsBal>0) and OnWebSite='Y') as a " +
                         "WHERE RwNo >= " + SNoFrom + " AND RwNo <= " + SNoTo + " " + IsoEnd;

                Adp = new SqlDataAdapter(strQry, selectConn);
                Adp.Fill(DtCountry);

                if (DtCountry.Rows.Count > 0)
                {
                    // Optional header row
                    _Output += "{\"sno\":\"Sno\",\"payoutdate\":\"Payout Date\",\"everestincome\":\"Everest Income\"," +
                               "\"directsponsorincome\":\"Direct Sponsor Income\",\"matchingincome\":\"Matching Income\"," +
                               "\"diamondclub\":\"Diamond Club\",\"netincomeact\":\"Net Income Act\",\"wdeduct\":\"Retopup Deduction\"," +
                               "\"grossamount\":\"Gross Income\",\"tdsamt\":\"Tds Amount\",\"admincharge\":\"Admin charge\"," +
                               "\"deduction\":\"Total deduction\",\"previous\":\"Previous Income\",\"chqamt\":\"Net Income\"," +
                               "\"closing\":\"Closing Balance\",\"statement\":\"Statement\"},";

                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{\"sno\":\"" + Dr["RwNo"].ToString() +
                                   "\",\"payoutdate\":\"" + Dr["PayoutDate"].ToString() +
                                   "\",\"everestincome\":\"" + Dr["EverestIncome"].ToString() +
                                   "\",\"directsponsorincome\":\"" + Dr["SLIIncome"].ToString() +
                                   "\",\"matchingincome\":\"" + Dr["BinaryIncome"].ToString() +
                                   "\",\"diamondclub\":\"" + Dr["ClubIncome"].ToString() +
                                   "\",\"netincomeact\":\"" + Dr["NetIncomeAct"].ToString() +
                                   "\",\"wdeduct\":\"" + Dr["Wdeduct"].ToString() +
                                   "\",\"grossamount\":\"" + Dr["NetIncome"].ToString() +
                                   "\",\"tdsamt\":\"" + Dr["TdsAmount"].ToString() +
                                   "\",\"admincharge\":\"" + Dr["AdminCharge"].ToString() +
                                   "\",\"deduction\":\"" + Dr["deduction"].ToString() +
                                   "\",\"previous\":\"" + Dr["PrevBal"].ToString() +
                                   "\",\"chqamt\":\"" + Dr["ChqAmt"].ToString() +
                                   "\",\"closing\":\"" + Dr["ClsBal"].ToString() +
                                   "\",\"statement\":\"" + Dr["SessID"].ToString() + "\"},";
                    }

                    _Output = _Output.Remove(_Output.Length - 1, 1); // remove trailing comma
                }

                _Output += "],\"recordcount\":\"" + RecordCount + "\",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string NewDailyIncentive(string userid, string passwd, string SNoFrom, string SNoTo)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtCountry = new DataTable();
                _Output = "{\"dailyincentive\": [";

                string strQry = IsoStart + "Select Count(*) from " + ObjDAL.dBName + "..V#DailyPayoutDetailNew " +
                                "where idno='" + userid + "' and OnWebSite='Y' " + IsoEnd;
                Comm = new SqlCommand(strQry, selectConn);
                int RecordCount = Convert.ToInt32(Comm.ExecuteScalar());

                strQry = IsoStart + "Select * FROM (Select Row_Number() Over(Order by Sessid Desc) as RwNo,* " +
                         "from " + ObjDAL.dBName + "..V#DailyPayoutDetailNew " +
                         "where idno='" + userid + "' and OnWebSite='Y') as a " +
                         "WHERE RwNo >= " + SNoFrom + " AND RwNo <= " + SNoTo + " " + IsoEnd;

                Adp = new SqlDataAdapter(strQry, selectConn);
                Adp.Fill(DtCountry);

                if (DtCountry.Rows.Count > 0)
                {
                    // Optional header row
                    _Output += "{\"sno\":\"SNo\",\"payoutdate\":\"Payout Date\",\"everestincome\":\"Everest Income\"," +
                               "\"sponsoreverestincome\":\"Sponsor Everest Income\",\"universalincome\":\"Sponsor Income\"," +
                               "\"diamondclub\":\"Diamond Club\",\"repurchincome\":\"Repurchase Income\",\"sponsorreward\":\"Sponsor Reward\"," +
                               "\"grossamount\":\"Gross Income\",\"tdsamt\":\"Tds Amount\",\"admincharge\":\"Admin charge\"," +
                               "\"retopupdeduction\":\"Retopup Deduction\",\"deduction\":\"Total deduction\",\"previous\":\"Previous Balance\"," +
                               "\"chqamt\":\"Net Income\",\"closing\":\"Carry Forward Balance\"},";

                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{\"sno\":\"" + Dr["RwNo"].ToString() +
                                   "\",\"payoutdate\":\"" + Dr["PayoutDate"].ToString() +
                                   "\",\"everestincome\":\"" + Dr["EverestIncome"].ToString() +
                                   "\",\"sponsoreverestincome\":\"" + Dr["MagicBinary"].ToString() +
                                   "\",\"universalincome\":\"" + Dr["SLIIncome"].ToString() +
                                   "\",\"diamondclub\":\"" + Dr["clubincome"].ToString() +
                                   "\",\"repurchincome\":\"" + Dr["RepairIncome"].ToString() +
                                   "\",\"sponsorreward\":\"" + Dr["spreward"].ToString() +
                                   "\",\"grossamount\":\"" + Dr["NetIncome"].ToString() +
                                   "\",\"tdsamt\":\"" + Dr["TdsAmount"].ToString() +
                                   "\",\"admincharge\":\"" + Dr["AdminCharge"].ToString() +
                                   "\",\"retopupdeduction\":\"" + Dr["Wdeduct"].ToString() +
                                   "\",\"deduction\":\"" + Dr["deduction"].ToString() +
                                   "\",\"previous\":\"" + Dr["PrevBal"].ToString() +
                                   "\",\"chqamt\":\"" + Dr["ChqAmt"].ToString() +
                                   "\",\"closing\":\"" + Dr["ClsBal"].ToString() + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1); // remove last comma
                }

                _Output += "],\"recordcount\":\"" + RecordCount + "\",\"response\":\"OK\"}";

            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string DailyIncentive(string userid, string passwd, string SNoFrom, string SNoTo)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtCountry = new DataTable();
                _Output = "{\"dailyincentive\": [";

                string strQry = IsoStart + "Select Count(*) from " + ObjDAL.dBName + "..V#DailyPayoutDetail " +
                                "where idno='" + userid + "' and (NetIncome>0 Or PrevBal>0 Or ClsBal>0) and OnWebSite='Y' " + IsoEnd;
                Comm = new SqlCommand(strQry, selectConn);
                int RecordCount = Convert.ToInt32(Comm.ExecuteScalar());

                strQry = IsoStart + "Select * FROM (Select ROW_NUMBER() OVER(ORDER BY SessID DESC) as RwNo,* " +
                         "from " + ObjDAL.dBName + "..V#DailyPayoutDetail " +
                         "where idno='" + userid + "' and (NetIncome>0 Or PrevBal>0 Or ClsBal>0) and OnWebSite='Y') as a " +
                         "WHERE RwNo >= " + SNoFrom + " AND RwNo <= " + SNoTo + " " + IsoEnd;

                Adp = new SqlDataAdapter(strQry, selectConn);
                Adp.Fill(DtCountry);

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{\"payoutno\":\"" + Dr["SessID"].ToString() +
                                   "\",\"fromdate\":\"" + Dr["FromDate"].ToString() +
                                   "\",\"todate\":\"" + Dr["ToDate"].ToString() +
                                   "\",\"everestincome\":\"" + Dr["binaryIncome"].ToString() +
                                   "\",\"sponsorincome\":\"" + Dr["Spillincome"].ToString() +
                                   "\",\"sponsoreverestincome\":\"" + Dr["MagicBinary"].ToString() +
                                   "\",\"universalincome\":\"" + Dr["SLIIncome"].ToString() +
                                   "\",\"diamondclub\":\"" + Dr["clubincome"].ToString() +
                                   "\",\"grossamount\":\"" + Dr["NetIncome"].ToString() +
                                   "\",\"tdsamt\":\"" + Dr["TdsAmount"].ToString() +
                                   "\",\"admincharge\":\"" + Dr["AdminCharge"].ToString() +
                                   "\",\"previous\":\"" + Dr["PrevBal"].ToString() +
                                   "\",\"chqamt\":\"" + Dr["ChqAmt"].ToString() +
                                   "\",\"closing\":\"" + Dr["ClsBal"].ToString() +
                                   "\",\"deduction\":\"" + Dr["deduction"].ToString() + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1); // Remove last comma
                }

                _Output += "],\"recordcount\":\"" + RecordCount + "\",\"response\":\"OK\"}";

            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }
        return _Output;
    }
    public string LevelList(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            bool Bool = UserExists(userid, passwd);
            if (Bool)
            {
                string FormNo = GetFormNo(ClearInject(userid));
                _Output = "{\"levels\": [";

                string sql = ObjDAL.Isostart + "Exec sp_GetLevel '" + FormNo + "','N'" + ObjDAL.IsoEnd;
                DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        _Output += "{\"levelid\":\"" + dr["MLevel"].ToString() + "\",\"levelname\":\"" + dr["LevelName"].ToString() + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1); // Remove last comma
                }

                _Output += "],\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public bool CheckAmount(string Formno, string Actype, Decimal amount)
    {
        try
        {
            DataTable Dt = new DataTable();
            string str = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..ufnGetBalance('" + Convert.ToInt32(Formno) + "','" + Actype + "')" + IsoEnd;

            using (SqlDataAdapter sda = new SqlDataAdapter(str, selectConn))
            {
                sda.Fill(Dt);
            }

            if (Dt.Rows.Count > 0)
            {
                Decimal balance = Convert.ToDecimal(Dt.Rows[0]["Balance"]);
                if (balance < amount)
                    return false;
                else
                    return true;
            }

            // If no rows returned, treat as insufficient balance
            return false;
        }
        catch
        {
            return false;
        }
    }
    public string GeneratePin(string userid, string Passwd, string actype, string kitid, string qty, string TransPassw)
    {
        try
        {
            string sql;
            DataTable dt = new DataTable();
            string _output = "";
            bool Bool = false;
            DAL obj = new DAL();

            if (!UserExists(userid, Passwd))
                return "{\"response\":\"Failed\",\"msg\":\"invalid login detail\"}";

            sql = IsoStart + "SELECT KitAmount, KitId, KitName FROM " + ObjDAL.dBName + "..M_KitMaster WHERE KitId='" + kitid + "'" + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            if (dt.Rows.Count == 0)
                return "{\"response\":\"Failed\",\"msg\":\"invalid kit\"}";

            string KitName = dt.Rows[0]["KitName"].ToString();
            decimal Kitamount = Convert.ToDecimal(dt.Rows[0]["KitAmount"]);

            if (Convert.ToInt32(qty) <= 0)
                return "{\"response\":\"Failed\",\"msg\":\"pin quantity invalid\"}";

            string FormNo = GetFormNo(ClearInject(userid));
            if (FormNo == "0")
                return "{\"response\":\"Failed\",\"msg\":\"invalid login detail\"}";

            using (SqlCommand Comm = new SqlCommand(IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE Epassw='" + TransPassw + "' AND Formno=" + FormNo + IsoEnd, selectConn))
            {
                using (SqlDataReader Dr = Comm.ExecuteReader())
                {
                    Bool = Dr.Read();
                }
            }

            if (!Bool)
                return "{\"response\":\"Failed\",\"msg\":\"invalid transaction password\"}";
            decimal kitAmountDecimal = Convert.ToDecimal(Kitamount);
            decimal qtyDecimal = Convert.ToDecimal(qty);
            if (!CheckAmount(FormNo, actype, kitAmountDecimal * qtyDecimal))
                return "{\"response\":\"Failed\",\"msg\":\"insufficient balance\"}";

            using (SqlCommand Comm = new SqlCommand("Generate_EPins_Web " + userid + "," + kitid + "," + Convert.ToInt32(qty) + ";", Conn))
            {
                int i = Comm.ExecuteNonQuery();
                if (i > 0)
                    _output = "{\"response\":\"OK\",\"msg\":\"pin successfully generated\"}";
                else
                    _output = "{\"response\":\"Failed\",\"msg\":\"try again\"}";
            }

            return _output;
        }
        catch (Exception ex)
        {
            return "{\"response\":\"Failed\",\"msg\":\"" + ex.Message + "\"}";
        }
    }
    public string GetPinKit(string UserId, string Passwd)
    {
        string _Output = "";
        string formno;
        DataTable Dtstate = new DataTable();

        try
        {
            bool Bool = UserExists(UserId, Passwd);
            if (!Bool)
                return "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";

            formno = GetFormNo(UserId);
            if (formno == "0")
                return "{\"response\":\"FAILED\",\"msg\":\"Invalid FormNo.\"}";

            // Get Kit Detail
            if (selectConn.State == ConnectionState.Closed)
                selectConn.Open();

            _Output = "{\"kit\": [";

            string strQuery = IsoStart + "SELECT kitId, KitName, KitAmount " +
                              "FROM " + ObjDAL.dBName + "..M_KitMaster " +
                              "WHERE ActiveStatus = 'Y' AND Rowstatus = 'Y' " +
                              "AND kitamount <= (SELECT Balance FROM " + ObjDAL.dBName + "..ufnGetBalance(" + formno + ",'M')) " +
                              "AND kitamount <> 0 " +
                              "ORDER BY kitId" + IsoEnd;

            using (SqlDataAdapter Adp = new SqlDataAdapter(strQuery, selectConn))
            {
                Adp.Fill(Dtstate);
            }

            if (Dtstate.Rows.Count > 0)
            {
                foreach (DataRow Dr in Dtstate.Rows)
                {
                    _Output += "{" +
                               "\"kitamount\":\"" + Convert.ToInt32(Dr["KitAmount"]) + "\"," +
                               "\"kitid\":\"" + Convert.ToInt32(Dr["kitid"]) + "\"," +
                               "\"kitname\":\"" + Dr["kitname"] + "\"" +
                               "},";
                }
                _Output = _Output.TrimEnd(','); // Remove trailing comma
            }

            _Output += "],\"response\":\"OK\"}";
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\",\"kitamount\":\"0\",\"msg\":\"Invalid.\"}";
        }

        return _Output;
    }
    public string PinReceiveDetails(string userid, string passwd, int pkgId, string FromNo, string ToNo)
    {
        string _Output = "";
        string Condition = "";

        try
        {
            bool Bool = UserExists(userid, passwd);
            if (!Bool)
                return "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";

            string FormNo = GetFormNo(ClearInject(userid));
            DataTable DtCountry = new DataTable();

            if (pkgId > 0)
                Condition = " AND d.ProdID='" + pkgId + "' ";

            _Output = "{\"receivedetail\": [";

            // Get total record count
            string strquery = IsoStart + "SELECT COUNT(*) AS cnt FROM " + ObjDAL.dBName + "..TrnTransferPinDetail AS a, " +
                              ObjDAL.dBName + "..M_Membermaster AS b, " +
                              ObjDAL.dBName + "..M_MemberMaster AS c, " +
                              ObjDAL.dBName + "..M_Formgeneration AS d, " +
                              ObjDAL.dBName + "..M_kitMaster AS e " +
                              "WHERE a.FromIdno = b.Idno AND a.ToIdno = c.Idno AND a.PinNo = d.Formno AND d.prodid = e.kitid " +
                              "AND ToIdno = '" + userid + "'" + Condition + IsoEnd;

            using (SqlCommand Comm = new SqlCommand(strquery, selectConn))
            {
                int RcordCount = Convert.ToInt32(Comm.ExecuteScalar());

                // Get received pin details
                strquery = "SELECT ROW_NUMBER() OVER(ORDER BY a.RecTimeStamp DESC) AS SNo, a.toidno, a.pinno, a.ScratchNo, a.fromidno, " +
                           "b.MemFirstName + '' + b.MemLastName AS FromMemName, " +
                           "c.MemFirstName + '' + c.MemLastName AS ToMemName, " +
                           "REPLACE(CONVERT(VARCHAR, a.RecTimeStamp , 106), ' ', '-') + ' ' + " +
                           "CONVERT(VARCHAR(15), CAST(a.RecTimeStamp AS TIME), 100) AS ToDate, " +
                           "CASE WHEN d.Isissued = 'N' THEN 'UnUsed' ELSE 'Used' END AS PinStatus, " +
                           "e.kitName, a.remark " +
                           "FROM " + ObjDAL.dBName + "..TrnTransferPinDetail AS a, " +
                           ObjDAL.dBName + "..M_Membermaster AS b, " +
                           ObjDAL.dBName + "..M_MemberMaster AS c, " +
                           ObjDAL.dBName + "..M_Formgeneration AS d, " +
                           ObjDAL.dBName + "..M_kitMaster AS e " +
                           "WHERE a.FromIdno = b.Idno AND a.ToIdno = c.Idno AND a.PinNo = d.Formno AND d.prodid = e.kitid " +
                           "AND ToIdno = '" + userid + "'" + Condition;

                using (SqlDataAdapter Adp = new SqlDataAdapter(IsoStart + "SELECT * FROM (" + strquery + ") AS b " +
                                                               "WHERE SNo >= " + FromNo + " AND SNo <= " + ToNo + " " + IsoEnd, selectConn))
                {
                    Adp.Fill(DtCountry);
                }

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{" +
                                   "\"sno\":\"" + Dr["SNo"] + "\"," +
                                   "\"fromidno\":\"" + Dr["FromIDNo"] + "\"," +
                                   "\"frommember\":\"" + Dr["FromMemName"] + "\"," +
                                   "\"kitname\":\"" + Dr["kitname"] + "\"," +
                                   "\"pinno\":\"" + Dr["PinNo"] + "\"," +
                                   "\"scratchno\":\"" + Dr["ScratchNo"] + "\"," +
                                   "\"pindate\":\"" + Dr["ToDate"] + "\"," +
                                   "\"PinStatus\":\"" + Dr["PinStatus"] + "\"," +
                                   "\"remark\":\"" + Dr["remark"] + "\"" +
                                   "},";
                    }
                    _Output = _Output.TrimEnd(','); // Remove trailing comma
                }

                _Output += "],\"recordcount\":\"" + RcordCount + "\",\"response\":\"OK\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string PinTransferDetails(string userid, string passwd, int pkgId, string FromNo, string ToNo)
    {
        string _Output = "";
        string Condition = "";

        try
        {
            bool Bool = UserExists(userid, passwd);
            if (!Bool)
                return "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";

            string FormNo = GetFormNo(ClearInject(userid));
            DataTable DtCountry = new DataTable();

            if (pkgId > 0)
                Condition = " AND d.ProdID='" + pkgId + "'";

            _Output = "{\"transferdetail\": [";

            // Get total record count
            string strquery = IsoStart + "SELECT COUNT(*) cnt FROM " + ObjDAL.dBName + "..TrnTransferPinDetail AS a, " +
                              ObjDAL.dBName + "..M_Membermaster AS b, " +
                              ObjDAL.dBName + "..M_MemberMaster AS c, " +
                              ObjDAL.dBName + "..M_Formgeneration AS d, " +
                              ObjDAL.dBName + "..M_KitMaster AS e " +
                              "WHERE a.FromIdno = b.IDNO AND a.ToIdno = c.IDNo AND a.PinNo = d.Formno AND d.prodid = e.kitid " +
                              "AND FromIdno = '" + userid + "'" + Condition + IsoEnd;

            using (SqlCommand Comm = new SqlCommand(strquery, selectConn))
            {
                int RcordCount = Convert.ToInt32(Comm.ExecuteScalar());

                // Get transfer details
                strquery = "SELECT ROW_NUMBER() OVER(ORDER BY a.FromIdno, a.RecTimeStamp DESC) AS SNo, a.*, " +
                           "REPLACE(CONVERT(VARCHAR(11), a.RecTimeStamp , 106), ' ', '-') + ' ' + " +
                           "STUFF(RIGHT(CONVERT(VARCHAR,a.RecTimeStamp ,100),7), 6, 0, ' ') AS PinDate, " +
                           "b.MemFirstName + '' + b.MemLastName AS FromMemName, " +
                           "c.MemFirstName + '' + c.MemLastName AS ToMemName, " +
                           "CASE WHEN d.Isissued = 'N' THEN 'UnUsed' ELSE 'Used' END AS PinStatus, " +
                           "e.kitname " +
                           "FROM " + ObjDAL.dBName + "..TrnTransferPinDetail AS a, " +
                           ObjDAL.dBName + "..M_Membermaster AS b, " +
                           ObjDAL.dBName + "..M_MemberMaster AS c, " +
                           ObjDAL.dBName + "..M_Formgeneration AS d, " +
                           ObjDAL.dBName + "..M_KitMaster AS e " +
                           "WHERE a.FromIdno = b.IDNO AND a.ToIdno = c.IDNo AND a.PinNo = d.Formno AND d.prodid = e.kitid " +
                           "AND FromIdno = '" + userid + "'" + Condition;

                DtCountry = new DataTable();
                using (SqlDataAdapter Adp = new SqlDataAdapter(IsoStart + "SELECT * FROM (" + strquery + ") AS b " +
                                                               "WHERE SNo >= " + FromNo + " AND SNo <= " + ToNo + IsoEnd, Conn))
                {
                    Adp.Fill(DtCountry);
                }

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{" +
                                   "\"sno\":\"" + Dr["SNo"] + "\"," +
                                   "\"toidno\":\"" + Dr["ToIdno"] + "\"," +
                                   "\"tomember\":\"" + Dr["ToMemName"] + "\"," +
                                   "\"kitname\":\"" + Dr["kitname"] + "\"," +
                                   "\"pinno\":\"" + Dr["PinNo"] + "\"," +
                                   "\"scratchno\":\"" + Dr["ScratchNo"] + "\"," +
                                   "\"pindate\":\"" + Dr["PinDate"] + "\"," +
                                   "\"PinStatus\":\"" + Dr["PinStatus"] + "\"," +
                                   "\"remark\":\"" + Dr["remark"] + "\"" +
                                   "},";
                    }
                    _Output = _Output.TrimEnd(','); // Remove last comma
                }

                _Output += "],\"recordcount\":\"" + RcordCount + "\",\"response\":\"OK\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string TransferPin(string userid, string passwd, string OtP, string toidno, int pkgid, int qty, string remark)
    {
        string scrname = "";
        int isAPISuccess = 0;

        try
        {
            // Sanitize input
            toidno = toidno.Replace("'", "").Replace(";", "").Replace("=", "");
            OtP = OtP.Replace("'", "").Replace(";", "").Replace("=", "");

            bool Bool = UserExists(userid, passwd);
            string FormNo = GetFormNo(ClearInject(userid));

            if (!Bool)
                return "{\"response\":\"FAILED\",\"msg\":\"Invalid login details.\"}";

            if (qty <= 0)
                return "{\"response\":\"FAILED\",\"msg\":\"Please Enter Qty.\"}";

            if (userid == toidno)
                return "{\"response\":\"FAILED\",\"msg\":\"You Can't Transfer Pin to Your Account.\"}";

            string Fromformno = "", Frmmoblno = "", FrmMname = "", epassw = "";
            string Toformno = "", tomoblno = "", tomname = "";

            // Get From ID Detail
            if (selectConn.State == ConnectionState.Closed) selectConn.Open();
            using (SqlCommand cmd = new SqlCommand(IsoStart +
                "SELECT Formno, Mobl, MemFirstName, MemLastName, Epassw FROM " + ObjDAL.dBName + "..M_MemberMaster " +
                "WHERE idno='" + userid + "' AND Passw='" + passwd + "'" + IsoEnd, selectConn))
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    Fromformno = dr["Formno"].ToString();
                    Frmmoblno = dr["Mobl"].ToString();
                    FrmMname = (dr["MemFirstName"].ToString() + " " + dr["MemLastName"].ToString()).Trim();
                    epassw = dr["Epassw"].ToString();
                    if (epassw == OtP)
                    {
                        return "{\"response\":\"FAILED\",\"msg\":\"Invalid Transaction Password.\"}";
                    }
                }
                else
                {
                    return "{\"response\":\"FAILED\",\"msg\":\"Invalid IdNo.\"}";
                }
            }

            // Get To ID Detail
            if (selectConn.State == ConnectionState.Closed) selectConn.Open();
            using (SqlCommand cmd = new SqlCommand(IsoStart +
                "SELECT Formno, Mobl, MemFirstName, MemLastName FROM " + ObjDAL.dBName + "..M_MemberMaster " +
                "WHERE idno='" + toidno + "'" + IsoEnd, selectConn))
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (!dr.Read())
                    return "{\"response\":\"FAILED\",\"msg\":\"ID Not exist.\"}";
                Toformno = dr["Formno"].ToString();
                tomoblno = dr["Mobl"].ToString();
                tomname = (dr["MemFirstName"].ToString() + " " + dr["MemLastName"].ToString()).Trim();
            }

            // Get Kit Amount
            string KitName = "";
            int amount = 0, Kitvalue = 0;
            if (selectConn.State == ConnectionState.Closed) selectConn.Open();
            using (SqlCommand cmd = new SqlCommand(IsoStart +
                "SELECT KitAmount, KitName FROM " + ObjDAL.dBName + "..M_KitMaster WHERE KitId='" + pkgid + "'" + IsoEnd, selectConn))
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    amount = Convert.ToInt32(dr["KitAmount"]);
                    KitName = dr["KitName"].ToString();
                    Kitvalue = amount * qty;
                }
                else
                {
                    return "{\"response\":\"FAILED\",\"msg\":\"Package not found.\"}";
                }
            }

            // Check Pin Qty in user account
            int totalPin = 0;
            if (selectConn.State == ConnectionState.Closed) selectConn.Open();
            using (SqlCommand cmd = new SqlCommand(IsoStart +
                "SELECT COUNT(formno) AS TotalPin FROM " + ObjDAL.dBName + "..M_Formgeneration " +
                "WHERE FCode='" + userid + "' AND Prodid=" + pkgid + " AND ActiveStatus='Y' AND IsIssued='N'" + IsoEnd, selectConn))
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                    totalPin = Convert.ToInt32(dr["TotalPin"]);
            }

            if (totalPin < qty)
            {
                return "{\"response\":\"FAILED\",\"msg\":\"Adjust Quantity. Your Pin stock of " + KitName + " is " + totalPin + ".\"}";
            }

            // Transfer Pins
            if (Conn.State == ConnectionState.Closed) Conn.Open();
            string query = "EXEC PinTransfer '" + userid + "','" + toidno + "'," + qty + ",'" + pkgid + "','" + remark + "';";
            using (SqlCommand cmd = new SqlCommand(query, Conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Save in History
            string Remarks = "Pin Transfer To " + toidno + " of " + KitName;
            string histQuery = "INSERT INTO UserHistory(UserId, UserName, PageName, Activity, ModifiedFlds, RecTimeStamp) " +
                               "VALUES('" + Fromformno + "','" + FrmMname + "','App: EPin Transfer','Pin Transfer','" + Remarks + "', GETDATE())";
            using (SqlCommand cmd = new SqlCommand(histQuery, Conn))
            {
                cmd.ExecuteNonQuery();
            }

            scrname = "Pin successfully transferred to ID " + toidno + ".";
            isAPISuccess = 1;

            if (isAPISuccess == 1)
                return "{\"response\":\"OK\",\"msg\":\"" + scrname + "\"}";
            else
                return "{\"response\":\"FAILED\",\"msg\":\"" + scrname + "\"}";
        }
        catch (Exception ex)
        {
            return "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }
    }
    public string ChangeTransactionPassword(string UserID, string Passwd, string OldPasswd, string NewPasswd)
    {
        string _Output = "";
        try
        {
            if (UserExists(UserID, Passwd))
            {
                if (Conn.State == ConnectionState.Closed) Conn.Open();

                int IsMemExist = 0;
                string checkQuery = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_MemberMaster " +
                                    "WHERE IDNo='" + UserID + "' AND EPassw='" + OldPasswd + "'" + IsoEnd;

                using (SqlCommand cmd = new SqlCommand(checkQuery, selectConn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        IsMemExist = 1;
                    }
                    else
                    {
                        _Output = "{\"response\":\"Incorrect Transaction Password\"}";
                    }
                }

                if (IsMemExist == 1)
                {
                    string strQry = "INSERT INTO TempMemberMaster " +
                                    "SELECT *, 'Transaction Password updated through App', GETDATE(), 'C' " +
                                    "FROM M_MemberMaster WHERE IDNo='" + UserID + "'; " +
                                    "UPDATE M_MemberMaster SET EPassw='" + NewPasswd + "' WHERE IDNo='" + UserID + "';";

                    if (Conn.State == ConnectionState.Closed) Conn.Open();
                    using (SqlCommand cmdUpdate = new SqlCommand(strQry, Conn))
                    {
                        if (cmdUpdate.ExecuteNonQuery() != 0)
                            _Output = "{\"response\":\"OK\"}";
                        else
                            _Output = "{\"response\":\"FAILED\"}";
                    }
                }

                if (Comm != null) Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
                Conn.Close();

            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string Relation()
    {
        string _Output = "";
        try
        {
            // If you need authentication, you can uncomment and implement it
            // bool Bool = UserExists(userid, passwd);
            // if (Bool)
            // {

            DataTable dtState = new DataTable();
            _Output = "{\"memrels\": [";

            string strQry = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_Relation WHERE ActiveStatus='Y' ORDER BY ID" + IsoEnd;
            SqlDataAdapter adp = new SqlDataAdapter(strQry, selectConn);
            adp.Fill(dtState);

            if (dtState.Rows.Count > 0)
            {
                foreach (DataRow dr in dtState.Rows)
                {
                    _Output += "{\"memrel\":\"" + dr["Relation"] + "\"},";
                }
                // Remove trailing comma
                _Output = _Output.Remove(_Output.Length - 1, 1);
            }

            _Output += "],\"response\":\"OK\"}";
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string EpinDetail(string userid, string passwd, int PkgID, string PType, string FromNo, string ToNo)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                string col = "";
                string WhereCondition = "";

                if (PType == "U")
                    WhereCondition = " AND [Status]='Used'";
                else if (PType == "N")
                    WhereCondition = " AND [Status]='UnUsed'";

                if (PkgID > 0)
                    WhereCondition += " AND KitID=" + PkgID;

                _Output = "{\"epindetail\":[";

                string strQry = "SELECT ROW_NUMBER() OVER(ORDER BY Cardno DESC) AS SNo," +
                                "CardNo as [Pin No], scratchno as [Scratch No]," +
                                "productname as [Product Name], IssuedDate as [Issue Date]," +
                                "Status as [Epin Status], UsedBy as [Used By]," +
                                "MemName as Name, UsedDate as [Used Date] " +
                                "FROM " + ObjDAL.dBName + "..V#EpinStatus " +
                                "WHERE ReqFormNo='" + userid + "'" + WhereCondition;

                strQry = IsoStart + "SELECT * FROM (" + strQry + ") AS b " +
                         "WHERE SNo >= " + FromNo + " AND SNo <= " + ToNo + IsoEnd;

                DataSet ds1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQry);
                DataTable dt = new DataTable();

                if (ds1.Tables.Count > 0)
                    dt = ds1.Tables[0];

                foreach (DataRow Dr in dt.Rows)
                {
                    col = "{";

                    foreach (DataColumn column in dt.Columns)
                    {
                        string value = Dr[column] == DBNull.Value ? "" : Dr[column].ToString();
                        col += "\"" + column.ColumnName + "\":\"" + value + "\",";
                    }

                    col = col.Remove(col.Length - 1, 1); // remove inner comma
                    col += "},";

                    _Output += col;
                }

                // 🔴 FIX: remove comma ONLY if records exist
                if (dt.Rows.Count > 0)
                {
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message.Replace("\"", "") + "\"}";
        }

        return _Output;
    }


    public string Packagelist(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            bool Bool = UserExists(userid, passwd);
            if (Bool)
            {
                DataTable dtState = new DataTable();
                _Output = "{\"packages\": [";

                string query = IsoStart + @"
                SELECT KitID, KitName, kitamount 
                FROM (
                    SELECT 0 AS KitID, '-- ALL --' AS KitName, 0 AS kitamount
                    UNION
                    SELECT KitID, KitName + ' (' + CAST(KitAmount AS VARCHAR) + ')' AS KitName, kitamount
                    FROM " + ObjDAL.dBName + "..M_KitMaster " +
                        "WHERE ActiveStatus='Y' AND RowStatus='Y' AND AllowTopup='Y') AS temp " +
                    "ORDER BY KitID " + IsoEnd;

                SqlDataAdapter adp = new SqlDataAdapter(query, selectConn);
                adp.Fill(dtState);

                if (dtState.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtState.Rows)
                    {
                        _Output += "{\"pkgid\":\"" + dr["KitID"] + "\",\"pkgname\":\"" + dr["KitName"] + "\",\"pkgamount\":\"" + dr["Kitamount"] + "\"},";
                    }
                    // Remove trailing comma
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"response\":\"OK\"}";
                if (Comm != null) Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null) Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string KYCBankDetail(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtProfile = new DataTable();
                string strQry = IsoStart + @"
                SELECT a.IDNo,
                       a.MemFirstName AS MemName,
                       a.Panno,
                       a.Acno,
                       a.BAnkid,
                       a.IFscode,
                       a.Fax,
                       a.Branchname,
                       b.BankProof,
                       CASE WHEN b.ISbankverified<>'N' THEN REPLACE(CONVERT(VARCHAR,b.BankVerifyDate,106),' ','-') ELSE '' END AS BankProofDate,
                       b.isBankverified,
                       CASE 
                           WHEN b.IsBankVerified='Y' THEN 'Verified'
                           WHEN b.IsBankVerified='P' THEN 'Pending'
                           WHEN b.IsBankVerified='R' THEN 'Rejected'
                           ELSE 'Verification Due' 
                       END AS BankVerf,
                       CASE WHEN b.IsBankVerified='R' THEN b.BankProofRemark ELSE '' END AS RejectRemark,
                       ISNULL(f.Reason,' ') AS RejectReason
                FROM " + ObjDAL.dBName + @"..M_MemberMaster AS a
                INNER JOIN " + ObjDAL.dBName + @"..KycVerify AS b ON a.Formno = b.Formno
                LEFT JOIN " + ObjDAL.dBName + @"..M_KycReject AS f ON b.BankRejectId = f.Kid
                WHERE a.IdNo = '" + userid + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(strQry, selectConn))
                {
                    Adp.Fill(DtProfile);
                }

                if (DtProfile.Rows.Count > 0)
                {
                    DataRow dr = DtProfile.Rows[0];
                    _Output = "{" +
                        "\"idno\":\"" + dr["IdNo"] + "\"," +
                        "\"bankid\":\"" + dr["Bankid"] + "\"," +
                        "\"acno\":\"" + dr["acno"] + "\"," +
                        "\"ifscode\":\"" + dr["IFscode"] + "\"," +
                        "\"accounttype\":\"" + dr["Fax"] + "\"," +
                        "\"branchname\":\"" + dr["Branchname"] + "\"," +
                        "\"bankproof\":\"" + dr["BankProof"] + "\"," +
                        "\"bankproofdate\":\"" + dr["BankProofDate"] + "\"," +
                        "\"bankverf\":\"" + dr["BankVerf"] + "\"," +
                        "\"isbankverified\":\"" + dr["isBankverified"] + "\"," +
                        "\"rejectremark\":\"" + dr["RejectRemark"] + "\"," +
                        "\"rejectreason\":\"" + dr["RejectReason"] + "\"," +
                        "\"response\":\"OK\"" +
                        "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string KYCPancardDetail(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtProfile = new DataTable();
                string strQry = IsoStart + @"
                SELECT a.IDNo,
                       a.MemFirstName AS MemName,
                       a.Panno,
                       b.PanImg,
                       REPLACE(CONVERT(VARCHAR,b.PANImgDate,106),' ','-') AS PanProofdate,
                       b.IsPanVerified,
                       CASE WHEN b.IsPanVerified<>'N' THEN REPLACE(CONVERT(VARCHAR,b.PanVerifyDate,106),' ','-') ELSE '' END AS PanVerifyDate,
                       CASE 
                           WHEN b.IsPanVerified='Y' THEN 'Verified'
                           WHEN b.IsPanVerified='P' THEN 'Pending'
                           WHEN b.IsPanVerified='R' THEN 'Rejected'
                           ELSE 'Verification Due'
                       END AS PanVerf,
                       CASE WHEN b.IsPanVerified='R' THEN b.PanRemarks ELSE '' END AS RejectRemark,
                       ISNULL(f.Reason,'') AS RejectReason
                FROM " + ObjDAL.dBName + @"..M_MemberMaster AS a
                INNER JOIN " + ObjDAL.dBName + @"..KycVerify AS b ON a.Formno = b.Formno
                LEFT JOIN " + ObjDAL.dBName + @"..M_KycReject AS f ON b.PanRejectId = f.Kid
                WHERE a.IdNo = '" + userid + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(strQry, selectConn))
                {
                    Adp.Fill(DtProfile);
                }

                if (DtProfile.Rows.Count > 0)
                {
                    DataRow dr = DtProfile.Rows[0];
                    _Output = "{" +
                        "\"idno\":\"" + dr["IdNo"] + "\"," +
                        "\"panno\":\"" + dr["Panno"] + "\"," +
                        "\"panimage\":\"" + dr["PanImg"] + "\"," +
                        "\"panproofdate\":\"" + dr["PanProofdate"] + "\"," +
                        "\"ispanverified\":\"" + dr["IsPanVerified"] + "\"," +
                        "\"panverifydate\":\"" + dr["PanVerifyDate"] + "\"," +
                        "\"panverf\":\"" + dr["PanVerf"] + "\"," +
                        "\"rejectremark\":\"" + dr["RejectRemark"] + "\"," +
                        "\"rejectreason\":\"" + dr["RejectReason"] + "\"," +
                        "\"response\":\"OK\"" +
                        "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string KYCDetail(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            if (UserExists(userid, passwd))
            {
                DataTable DtProfile = new DataTable();
                string strQry = IsoStart + @"
                SELECT a.IDNo,
                       a.MemFirstName AS MemName,
                       a.Address1,
                       a.City,
                       a.Tehsil,
                       a.District,
                       AreaCode,
                       CityCode,
                       DistrictCode,
                       a.Statecode,
                       a.Pincode,
                       b.IdproofNo,
                       b.IdProof,
                       b.AddrProof,
                       CASE WHEN b.Isidverified<>'N' THEN REPLACE(CONVERT(VARCHAR,b.idVerifyDate,106),' ','-') ELSE '' END AS IdProofDate,
                       CASE WHEN b.IsAddrssverified<>'N' THEN REPLACE(CONVERT(VARCHAR,b.AddrssVerifyDate,106),' ','-') ELSE '' END AS AddrProofDate,
                       b.IsAddrssverified,
                       b.Isidverified,
                       CASE 
                           WHEN b.IsAddrssverified='Y' THEN 'Verified' 
                           WHEN b.IsAddrssverified='P' THEN 'Pending'
                           WHEN b.IsAddrssverified='R' THEN 'Rejected'
                           ELSE 'Verification Due' 
                       END AS idVerf,
                       CASE WHEN b.IsAddrssverified='R' THEN b.AddrssRemark ELSE '' END AS RejectRemark,
                       b.BackAddressProof,
                       b.BackAddressDate,
                       c.IdType,
                       c.id,
                       ISNULL(f.Reason,'') AS RejectReason,
                       D.Statename
                FROM " + ObjDAL.dBName + @"..M_MemberMaster AS a
                INNER JOIN " + ObjDAL.dBName + @"..KycVerify AS b ON a.Formno = b.formno
                LEFT JOIN " + ObjDAL.dBName + @"..M_KycReject AS f ON f.Kid = b.AddressRejectId
                INNER JOIN " + ObjDAL.dBName + @"..M_IdTypeMaster AS c ON b.IdType = c.Id AND c.ActiveStatus='Y'
                INNER JOIN " + ObjDAL.dBName + @"..M_StateDivMaster AS d ON a.Statecode = d.Statecode AND d.RowStatus='Y'
                WHERE a.IdNo = '" + userid + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(strQry, selectConn))
                {
                    Adp.Fill(DtProfile);
                }

                if (DtProfile.Rows.Count > 0)
                {
                    DataRow dr = DtProfile.Rows[0];
                    _Output = "{" +
                        "\"idno\":\"" + dr["IDNo"] + "\"," +
                        "\"idproof\":\"" + dr["IdProof"] + "\"," +
                        "\"address\":\"" + dr["Address1"] + "\"," +
                        "\"pincode\":\"" + dr["Pincode"] + "\"," +
                        "\"city\":\"" + dr["City"] + "\"," +
                        "\"district\":\"" + dr["District"] + "\"," +
                        "\"statecode\":\"" + dr["Statecode"] + "\"," +
                        "\"idproofdate\":\"" + dr["IdProofDate"] + "\"," +
                        "\"idverf\":\"" + dr["Isidverified"] + "\"," +
                        "\"status\":\"" + dr["idVerf"] + "\"," +
                        "\"addrproof\":\"" + dr["AddrProof"] + "\"," +
                        "\"addrproofdate\":\"" + dr["AddrProofDate"] + "\"," +
                        "\"IdproofNo\":\"" + dr["IdproofNo"] + "\"," +
                        "\"addrsverf\":\"" + dr["IsAddrssverified"] + "\"," +
                        "\"backaddressproof\":\"" + dr["BackAddressProof"] + "\"," +
                        "\"backaddressdate\":\"" + dr["BackAddressDate"] + "\"," +
                        "\"idtype\":\"" + dr["IdType"] + "\"," +
                        "\"rejectremark\":\"" + dr["RejectRemark"] + "\"," +
                        "\"rejectreason\":\"" + dr["RejectReason"] + "\"," +
                        "\"areacode\":\"" + dr["AreaCode"] + "\"," +
                        "\"statename\":\"" + dr["Statename"] + "\"," +
                        "\"citycode\":\"" + dr["CityCode"] + "\"," +
                        "\"districtcode\":\"" + dr["DistrictCode"] + "\"," +
                        "\"response\":\"OK\"" +
                        "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string GetProfile(string userid, string passwd, string memberid)
    {
        string _Output = "";
        try
        {
            bool Bool = MemberExist(userid, passwd);
            if (Bool)
            {
                DataTable DtProfile = new DataTable();

                string strQry = IsoStart +
                    "SELECT FormNo, IdNo, MemFirstName + ' ' + MemLastName AS MemName, MemFName, " +
                    "REPLACE(CONVERT(VARCHAR, MemDob, 106), ' ', '-') AS MemDob, " +
                    "REPLACE(CONVERT(VARCHAR, Doj, 106), '', '-') AS Doj, " +
                    "REPLACE(CONVERT(VARCHAR, Upgradedate, 106), ' ', '-') AS Upgradedate, " +
                    "Mobl, Email, NomineeName, Relation, " +
                    "CASE WHEN Legno=1 THEN 'Left' ELSE 'Right' END AS position, " +
                    "MemRelation, PhN1 " +
                    "FROM " + ObjDAL.dBName + "..M_MemberMaster " +
                    "WHERE IDNo='" + memberid + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(strQry, selectConn))
                {
                    Adp.Fill(DtProfile);
                }

                if (DtProfile.Rows.Count > 0)
                {
                    DataRow dr = DtProfile.Rows[0];
                    _Output = "{"
                        + "\"idno\":\"" + dr["IdNo"] + "\","
                        + "\"name\":\"" + dr["MemName"] + "\","
                        + "\"position\":\"" + dr["position"] + "\","
                        + "\"relation\":\"" + dr["MemRelation"] + "\","
                        + "\"fname\":\"" + dr["MemFName"] + "\","
                        + "\"dob\":\"" + dr["MemDob"] + "\","
                        + "\"mobile\":\"" + dr["Mobl"] + "\","
                        + "\"phoneno\":\"" + dr["PhN1"] + "\","
                        + "\"email\":\"" + dr["Email"] + "\","
                        + "\"nominee\":\"" + dr["NomineeName"] + "\","
                        + "\"nomineerelation\":\"" + dr["Relation"] + "\","
                        + "\"dateofjoining\":\"" + dr["Doj"] + "\","
                        + "\"dateofactivation\":\"" + dr["Upgradedate"] + "\","
                        + "\"response\":\"OK\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string SetProfile(string userid, string passwd, string memberid, string Gaurdian, string Memrelation, string Dob, string Email, string Nominee, string Relation, string Phoneno, string Mobile = "0")
    {
        string _Output = "";
        try
        {
            bool Bool = MemberExist(userid, passwd);
            if (Bool)
            {
                DAL obj = new DAL();
                DataTable dt1 = new DataTable();
                string remark = "";
                string Str = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE Idno='" + userid.Trim() + "'" + IsoEnd;
                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    DataRow dr = dt1.Rows[0];

                    if (dr["MemFName"].ToString() != ClearInject(Gaurdian)) remark += "FatherName, ";
                    if (dr["MemDob"].ToString() != Dob) remark += "Dob, ";
                    if (ClearInject(dr["PhN1"].ToString()) != ClearInject(Phoneno)) remark += "PhoneNo, ";
                    if (ClearInject(dr["Mobl"].ToString()) != ClearInject(Mobile)) remark += "MobileNo, ";
                    if (ClearInject(dr["Email"].ToString()) != ClearInject(Email)) remark += "Email, ";
                    if (ClearInject(dr["NomineeName"].ToString()) != ClearInject(Nominee)) remark += "NomineeName, ";
                    if (ClearInject(dr["Relation"].ToString()) != ClearInject(Relation)) remark += "Relation, ";

                    remark += "Changed";
                }

                string mobileStr = Mobile != "0" ? ",Mobl='" + Mobile + "'" : "";

                string updateQry = "UPDATE " + ObjDAL.dBName + "..M_MemberMaster SET " +
                                   "MemFName='" + Gaurdian + "', " +
                                   "Memrelation='" + Memrelation + "', " +
                                   "MemDob='" + Dob + "', " +
                                   "Email='" + Email + "', " +
                                   "NomineeName='" + Nominee + "', " +
                                   "PhN1='" + Phoneno + "', " +
                                   "Relation='" + Relation + "'" + mobileStr + " " +
                                   "WHERE IDNo='" + memberid + "'";

                string historyQry = "INSERT INTO TempMemberMaster " +
                                    "SELECT *, 'Update Profile - " + HttpContext.Current.Request.UserHostAddress + "', GETDATE(), 'U' " +
                                    "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE Idno='" + memberid + "'; " +
                                    "INSERT INTO UserHistory(UserId, UserName, PageName, Activity, ModifiedFlds, RecTimeStamp, MemberId) " +
                                    "SELECT 0, MemFirstName, 'Profile', 'Profile Update', '" + remark + "', GETDATE(), FormNo " +
                                    "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE Idno='" + memberid + "'; " +
                                    updateQry;

                using (SqlCommand Comm = new SqlCommand(historyQry, Conn))
                {
                    int i = Comm.ExecuteNonQuery();
                    _Output = i != 0 ? "{\"response\":\"OK\"}" : "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string Validate_NewJoin(string PinNo, string ScratchNo, bool checkbyPinOnly = false)
    {
        PinNo = PinNo.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
        ScratchNo = ScratchNo.Trim().Replace("'", "").Replace("=", "").Replace(";", "");

        string scrname = "";

        if (string.IsNullOrWhiteSpace(PinNo))
            return "Please enter Pin No.";

        try
        {
            using (SqlCommand cmd = new SqlCommand(
                IsoStart + "Select * from " + ObjDAL.dBName + "..M_FormGeneration where FormNo=" + PinNo + " " + IsoEnd,
                selectConn))
            {
                using (SqlDataReader dRead = cmd.ExecuteReader())
                {
                    if (dRead.Read())
                    {
                        string isIssued = dRead["IsIssued"].ToString().ToUpper();
                        string activeStatus = dRead["ActiveStatus"].ToString().ToUpper();

                        if (isIssued == "Y")
                            scrname = "Pin No. Already Used. Please Enter Another Pin No.";
                        else if (activeStatus == "Y")
                            scrname = "OK";
                        else
                            scrname = "Pin could not be used.";
                    }
                    else
                    {
                        scrname = "Pin No. not found.";
                    }
                }
            }
        }
        catch (Exception)
        {
            return "Please check Pin No.";
        }

        if (checkbyPinOnly || scrname != "OK")
            return scrname;

        if (!string.IsNullOrWhiteSpace(ScratchNo))
        {
            try
            {
                string strQry = IsoStart +
                    "Select KitM.kitid, KitM.PV, KitM.RP, KitM.KitName, JoinStatus, KitM.kitamount " +
                    "From " + ObjDAL.dBName + "..M_FormGeneration as Scrtch " +
                    "Inner Join " + ObjDAL.dBName + "..M_KitMaster as KitM On Scrtch.ProdId=KitM.KitId " +
                    "Where Scrtch.IsIssued='N' and Scrtch.ActiveStatus='Y' " +
                    "and Scrtch.FormNo='" + PinNo + "' and Scrtch.ScratchNo='" + ScratchNo + "' " + IsoEnd;

                using (SqlCommand cmd = new SqlCommand(strQry, selectConn))
                using (SqlDataReader dRead = cmd.ExecuteReader())
                {
                    if (!dRead.Read())
                        return "Invalid PIN No. or Scratch No.";

                    Session["Bv"] = dRead["PV"];
                    Session["RP"] = dRead["RP"];
                    Session["Category"] = dRead["KitName"];
                    Session["Kitid"] = dRead["Kitid"];
                    Session["JoinStatus"] = dRead["JoinStatus"];
                    Session["Kitamount"] = dRead["kitamount"];
                }
            }
            catch (Exception)
            {
                return "Please check Pin or Scratch No.";
            }

            return "OK";
        }
        else
        {
            return "Check Scratch No.";
        }
    }
    public string FillSponsor(string userid, string passwd, string Sponsor)
    {
        string _Output = "";
        try
        {
            if (MemberExist(userid, passwd))
            {
                DataTable DtSponsor = new DataTable();

                string strQry = IsoStart +
                    "Select FormNo, MemFirstName + ' ' + MemLastName as MemName " +
                    "from " + ObjDAL.dBName + "..M_MemberMaster " +
                    "where IDNo='" + Sponsor + "'" + IsoEnd;

                using (SqlDataAdapter Adp = new SqlDataAdapter(strQry, selectConn))
                {
                    Adp.Fill(DtSponsor);
                }

                if (DtSponsor.Rows.Count > 0)
                {
                    _Output = "{\"sponsorno\":\"" + DtSponsor.Rows[0]["FormNo"] + "\"," +
                              "\"sponsornm\":\"" + DtSponsor.Rows[0]["MemName"] + "\"," +
                              "\"response\":\"OK\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string Sp_GetCountry()
    {
        string _Output = "";
        try
        {
            bool Bool = true;
            if (Bool)
            {
                _Output = "{\"country\": [";
                string sql = IsoStart + "Exec Sp_GetCountry " + IsoEnd;
                DataSet Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow Dr in Ds.Tables[0].Rows)
                    {
                        _Output += "{\"countrycode\":\"" + Dr["Cid"] + "\",\"countryname\":\"" + Dr["CountryName"] + "\"},";
                    }
                    _Output = _Output.Remove(_Output.Length - 1, 1); // Remove trailing comma
                }

                _Output += "],\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }
        return _Output;
    }
    public string MWalletHistory(string userid, string passwd, string FromNo, string ToNo)
    {
        string _Output = "";
        try
        {
            bool Bool = UserExists(userid, passwd);
            int Recordcount = 0;
            string FormNo = GetFormNo(ClearInject(userid));

            if (Bool && FormNo != "0")
            {
                DataTable Dt = new DataTable();

                // Get total record count
                string strQry = IsoStart + "Select Count(*) FROM " + ObjDAL.dBName + "..[V#RptMainFund] Where FormNo='" + FormNo + "'" + IsoEnd;
                using (SqlCommand Comm = new SqlCommand(strQry, selectConn))
                {
                    Recordcount = Convert.ToInt32(Comm.ExecuteScalar());
                }

                // Get balance summary
                strQry = IsoStart + "Select * From " + ObjDAL.dBName + "..ufnGetBalance('" + FormNo + "','M') " + IsoEnd;
                using (SqlDataAdapter Adp = new SqlDataAdapter(strQry, selectConn))
                {
                    Adp.Fill(Dt);
                }

                if (Dt.Rows.Count > 0)
                {
                    DataRow Dr = Dt.Rows[0];
                    _Output = "{\"credit\":\"" + Dr["Credit"] + "\",\"debit\":\"" + Dr["Debit"] + "\",\"balance\":\"" + Dr["Balance"] + "\",\"achistory\":[";

                    // Get transaction history
                    strQry = IsoStart + "Select * FROM (Select Row_Number() Over(Order by voucherid Desc) As Rn,*,Narration as Nartn From " +
                             ObjDAL.dBName + "..[V#RptMainFund] Where FormNo='" + FormNo + "') as b WHERE Rn>=" + FromNo + " And Rn<=" + ToNo + " " + IsoEnd;

                    DataTable DtHistory = new DataTable();
                    using (SqlDataAdapter AdpHistory = new SqlDataAdapter(strQry, selectConn))
                    {
                        AdpHistory.Fill(DtHistory);
                    }

                    if (DtHistory.Rows.Count > 0)
                    {
                        foreach (DataRow Drw in DtHistory.Rows)
                        {
                            _Output += "{\"sno\":\"" + Drw["Rn"] + "\"," +
                                       "\"date\":\"" + Drw["VoucherDate"] + "\"," +
                                       "\"deposit\":\"" + Drw["Deposit"] + "\"," +
                                       "\"used\":\"" + Drw["Used"] + "\"," +
                                       "\"remarks\":\"" + Drw["Nartn"] + "\"," +
                                       "\"balance\":\"" + Drw["balance"] + "\"},";
                        }
                        _Output = _Output.Remove(_Output.Length - 1, 1); // remove last comma
                    }

                    _Output += "]";
                }

                _Output += ",\"recordcount\":\"" + Recordcount + "\",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string SaveComplaint(string userId, string passwd, string idNo, string name, string mobileNo, string email, string complaintId, string subject, string description)
    {
        string output = "";

        if (string.IsNullOrWhiteSpace(name))
            return "{\"response\":\"FAILED\",\"msg\":\"Please enter name.\"}";
        if (string.IsNullOrWhiteSpace(mobileNo))
            return "{\"response\":\"FAILED\",\"msg\":\"Please enter mobileno.\"}";
        if (string.IsNullOrWhiteSpace(email))
            return "{\"response\":\"FAILED\",\"msg\":\"Please enter email id.\"}";
        if (complaintId == "0")
            return "{\"response\":\"FAILED\",\"msg\":\"Please select nature of grievance.\"}";
        if (string.IsNullOrWhiteSpace(subject))
            return "{\"response\":\"FAILED\",\"msg\":\"Please enter subject.\"}";
        if (string.IsNullOrWhiteSpace(description))
            return "{\"response\":\"FAILED\",\"msg\":\"Please enter description.\"}";

        string sql = "INSERT INTO M_ComplaintMaster(IDNO,CTypeID,CType,Complaint,Subject,MemberName,Mobileno,Email) " +
                     "SELECT @IdNo, @CTypeID, CType, @Complaint, @Subject, @MemberName, @Mobileno, @Email " +
                     "FROM M_ComplaintTypeMaster " +
                     "WHERE RowStatus='Y' AND ActiveStatus='Y' AND CTypeId=@CTypeID";

        string complaintNo = "";
        string complaintName = "";

        using (SqlCommand cmd = new SqlCommand(sql, Conn))
        {
            cmd.Parameters.AddWithValue("@Complaint", description.Trim());
            cmd.Parameters.AddWithValue("@Subject", subject.Trim());
            cmd.Parameters.AddWithValue("@MemberName", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@Mobileno", mobileNo.Trim());
            cmd.Parameters.AddWithValue("@IdNo", idNo);
            cmd.Parameters.AddWithValue("@CTypeID", complaintId);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                output = "{\"response\":\"FAILED\",\"msg\":\"Complaint not sent.\"}";
            }
            else
            {
                SendAdminMail(complaintId, name, subject, description, complaintNo, complaintName);
                // Optional SMS/email notifications can be implemented here if required

                output = "{\"response\":\"OK\",\"msg\":\"Your complaint has been successfully submitted on " +
                         DateTime.Now.ToString(" dd MMMM yyyy,hh:mm dddd") +
                         ". Your Complaint No. is " + complaintNo +
                         ". Our customer service representative will get in touch with you shortly. Keep patience.\"}";
            }
        }

        return output;
    }
    public bool SendAdminMail(string complaintId, string memberName, string subject, string description, string complaintNo, string complaintName)
    {
        try
        {
            string sql = IsoStart + "SELECT TOP 1 * FROM " + ObjDAL.dBName + "..M_ComplaintTypeMaster AS a, " +
                         ObjDAL.dBName + "..M_ComplaintMaster AS b, " +
                         ObjDAL.dBName + "..M_UserMaster AS c " +
                         "WHERE a.CtypeId = b.CtypeId AND c.UserId = a.UserId AND c.ActiveStatus='Y' AND c.RowStatus='Y' " +
                         "AND a.RowStatus='Y' AND a.ActiveStatus='Y' AND a.CtypeId = @ComplaintId " +
                         "ORDER BY CId DESC" + IsoEnd;

            using (SqlCommand cmd = new SqlCommand(sql, selectConn))
            {
                cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        complaintNo = reader["CId"].ToString();
                        complaintName = reader["Ctype"].ToString();

                        string userEmail = reader["ToUserEmail"].ToString();

                        // If you want to send email, uncomment below
                        /*
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            string strMsg = "<table style='margin:0; padding:10px; font-size:14px; font-family:verdana,arial,helvetica; line-height:23px; text-align:justify;width:100%'>" +
                                            "<tr><td>" +
                                            "<span style='font-weight:bold;'>Dear Sir/Madam,</span><br/>" +
                                            "<strong>Name: </strong>" + memberName.Trim() + "<br/><br/>" +
                                            "I am sending a complaint, please consider it.<br/><br/>" +
                                            "<span style='font-weight:bold;'>My Complaint</span><br/>" +
                                            "<strong>Complaint Type: </strong>" + complaintName.Trim() + "<br/>" +
                                            "<strong>Subject: </strong>" + subject.Trim() + "<br/>" +
                                            "<strong>Description: </strong>" + description.Trim() + "<br/><br/>" +
                                            "You may check it at Admin Panel: <a href='" + Session["AdminWeb"] + "' target='_blank' style='color:#0000FF;text-decoration:underline;'>" +
                                            Session["AdminWeb"] + "</a><br/><br/>" +
                                            "<span style='color:#0099FF;font-weight:bold;'>Sincerely,</span><br/>" +
                                            "<a href='" + Session["CompWeb"] + "' target='_blank' style='color:#0000FF;text-decoration:underline;'>" +
                                            Session["CompName"] + "</a><br/><br/></td></tr></table>";

                            MailMessage mail = new MailMessage(Session["CompMail"].ToString(), userEmail);
                            mail.Subject = "Member Complaint";
                            mail.Body = strMsg;
                            mail.IsBodyHtml = true;

                            SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString());
                            smtp.Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
                            smtp.Send(mail);
                        }
                        */
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            // For debugging
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public string ComplaintReplyDetail(string userId, string passwd, string complaintId)
    {

        string output = "";
        try
        {
            bool isMember = MemberExist(userId, passwd); // Assuming you have a C# version of memberExist
            if (isMember)
            {
                DataTable dtCountry = new DataTable();

                string strQry = IsoStart +
                    "SELECT M.IDNo, M.MemName, ISNULL(REPLACE(CONVERT(varchar, M.RecTimeStamp, 106), ' ', '-'), '') as CDate, " +
                    "M.CType, M.Complaint, ISNULL(S.Solution, '') as Solution, ISNULL(REPLACE(CONVERT(varchar, S.RecTimeStamp, 106), ' ', '-'), '') as SDate " +
                    "FROM (SELECT b.MemFirstName + ' ' + b.MemLastName as MemName, a.* " +
                    "FROM " + ObjDAL.dBName + "..M_ComplaintMaster as a, " + ObjDAL.dBName + "..M_MemberMaster as b " +
                    "WHERE a.IDNo = b.IDNo AND a.CID = '" + complaintId + "') as M " +
                    "LEFT JOIN " + ObjDAL.dBName + "..M_SolutionMaster as S ON M.CID = S.CID " + IsoEnd;

                SqlDataAdapter adp = new SqlDataAdapter(strQry, selectConn);
                adp.Fill(dtCountry);

                // Start JSON
                var sb = new StringBuilder();
                sb.Append("{\"complaintreplydetail\":[");

                // Add replies if available
                for (int i = 0; i < dtCountry.Rows.Count; i++)
                {
                    DataRow dr = dtCountry.Rows[i];
                    sb.Append("{");
                    sb.Append("\"replydate\":\"").Append(dr["SDate"]).Append("\",");
                    sb.Append("\"reply\":\"").Append(dr["Solution"].ToString().Replace("\"", "\\\"")).Append("\"");
                    sb.Append("}");
                    if (i < dtCountry.Rows.Count - 1)
                        sb.Append(",");
                }

                sb.Append("]"); // close complaintreplydetail array

                if (dtCountry.Rows.Count > 0)
                {
                    DataRow dr = dtCountry.Rows[0];
                    sb.Append(",\"complainttype\":\"").Append(dr["CType"]).Append("\"");
                    sb.Append(",\"complaint\":\"").Append(dr["Complaint"].ToString().Replace("\"", "\\\"")).Append("\"");
                }

                sb.Append(",\"response\":\"OK\"}");
                output = sb.ToString();
            }
            else
            {
                output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            output = "{\"response\":\"FAILED\",\"error\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}";
        }

        return output;
    }
    public string ComplaintList(string userId, string passwd)
    {
        string output = "";
        try
        {
            // Optional: Validate user login here if needed
            DataTable dtState = new DataTable();
            output = "{\"complainttype\": [";

            SqlDataAdapter adp = new SqlDataAdapter(
                IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_ComplaintTypeMaster " +
                "WHERE RowStatus='Y' AND ActiveStatus='Y'" + IsoEnd, selectConn);

            adp.Fill(dtState);

            if (dtState.Rows.Count > 0)
            {
                foreach (DataRow dr in dtState.Rows)
                {
                    output += "{\"complaintid\":\"" + dr["ctypeid"] + "\",\"complaintname\":\"" + dr["ctype"] + "\"},";
                }
                output = output.Remove(output.Length - 1, 1); // Remove last comma
            }

            output += "],\"response\":\"OK\"}";
        }
        catch (Exception)
        {
            output = "{\"response\":\"FAILED\"}";
        }

        return output;
    }
    public string ValidWalletOtp(string userMobile, string otpCode)
    {
        try
        {
            int retry = 0;
            SqlCommand comm = new SqlCommand(
                IsoStart + "SELECT TOP 1 * FROM " + ObjDAL.dBName + "..Adminlogin " +
                "WHERE Mobileno='" + userMobile + "' AND OTP='" + otpCode + "' AND ForType = 'APP' " +
                "AND CAST(logintime AS date) = CAST(GETDATE() AS date) " +
                "ORDER BY AID DESC " + IsoEnd, selectConn);

            SqlDataReader dr = comm.ExecuteReader();
            bool isValid = dr.Read();
            dr.Close();
            comm.Cancel();

            if (!isValid)
            {
                // Optional: You can implement retry logic here if needed
                return "{\"response\":\"FAILED\"}";
            }
            else
            {
                return "{\"response\":\"OK\"}";
            }
        }
        catch (Exception)
        {
            return "{\"response\":\"FAILED\"}";
        }
    }
    public string GeneratePinOTP(string idno, string Passw, string otptype)
    {
        try
        {
            int _Retry = 0;
            string mobl = "";

            string Formno = GetFormNo(idno);
            string Str = IsoStart + "Select FormNo,ActiveStatus,Mobl,Address1 From " + ObjDAL.dBName + "..M_MemberMaster WHERE FormNo = '" + Formno + "'" + IsoEnd;
            if (selectConn.State == ConnectionState.Closed)
                selectConn.Open();
            using (SqlCommand Comd = new SqlCommand(Str, selectConn))
            using (SqlDataReader Dr = Comd.ExecuteReader())
            {
                if (Dr.Read())
                {
                    mobl = Dr["Mobl"].ToString();
                }
            }
            Random rnd = new Random();
            int otpCode = rnd.Next(11111, 99999);
            DateTime time = DateTime.Now;
            string format = "dd-MMM-yyyy HH:mm:ss";
            string sms = "";

            if (otptype == "reqpintransferotp")
            {
                sms = "Dear " + idno + ", OTP For Pin Transfer Is " + otpCode + " at " + time.ToString(format) + " .";
            }
            else if (otptype == "walletotp")
            {
                sms = otpCode + " is your OTP for use wallet and is valid for 10 minutes. Do not share the OTP with anyone. Regard : Overnet Team";
            }

            SqlCommand Comm = new SqlCommand(
                "INSERT AdminLogin (UserID, Username, Passw, MobileNo, Otp,ForType) VALUES ('"
                + Formno + "', '', '" + Passw + "', '" + mobl + "', '" + otpCode + "','APP')", Conn);

            int i = Comm.ExecuteNonQuery();
            if (i != 0)
            {
                if (SendSMSwallet(sms, mobl))
                {
                    return "{\"response\":\"OK\"}";
                }
                else
                {
                    return "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                return "{\"response\":\"FAILED\"}";
            }
        }
        catch (Exception ex)
        {
            return "{\"response\":\"FAILED\"}";
        }
    }
    private bool SendSMSwallet(string SMS, string MobileNo)
    {
        using (WebClient client = new WebClient())
        {
            try
            {
                string baseurl = "";
                baseurl = "http://78.46.58.54/vb/apikey.php?apikey=baeVEum0EOQkbng7&senderid=OVRNET&templateid=1707169640731570892&number="
                          + MobileNo + "&message=" + SMS;
                using (Stream data = client.OpenRead(baseurl))
                using (StreamReader reader = new StreamReader(data))
                {
                    string s = reader.ReadToEnd();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    public string GetAddressByPincode(string userid, string passwd, string pincode)
    {
        string output = "";
        try
        {
            DataTable dtState = new DataTable();
            output = "{\"pincodedetail\": [";

            // Fill DataTable with query results
            SqlDataAdapter adp = new SqlDataAdapter(
                IsoStart +
                "SELECT a.StateName, b.DistrictName, c.CityName, d.VillageName, d.Pincode, " +
                "a.StateCode, b.DistrictCode, c.CityCode, d.VillageCode " +
                "FROM " + ObjDAL.dBName + "..M_STateDivMaster AS a WITH(NOLOCK) " +
                "INNER JOIN " + ObjDAL.dBName + "..M_DistrictMaster AS b WITH(NOLOCK) ON a.StateCode = b.StateCode AND a.ActiveStatus='Y' AND b.ActiveStatus='Y' " +
                "INNER JOIN " + ObjDAL.dBName + "..M_CityStateMaster AS c WITH(NOLOCK) ON b.DistrictCode = c.DistrictCode AND c.ActiveStatus='Y' " +
                "INNER JOIN " + ObjDAL.dBName + "..M_VillageMaster AS d WITH(NOLOCK) ON c.CityCode = d.CityCode AND d.ActiveStatus='Y' " +
                "WHERE d.Pincode='" + Convert.ToInt32(pincode) + "'" + IsoEnd, selectConn);
            adp.Fill(dtState);

            string str = "";
            if (dtState.Rows.Count > 0)
            {
                // State, district, city info from first row
                str = "\"statecode\":\"" + dtState.Rows[0]["StateCode"] + "\"," +
                      "\"statename\":\"" + dtState.Rows[0]["StateName"] + "\"," +
                      "\"districtname\":\"" + dtState.Rows[0]["DistrictName"] + "\"," +
                      "\"districtcode\":\"" + dtState.Rows[0]["DistrictCode"] + "\"," +
                      "\"cityname\":\"" + dtState.Rows[0]["CityName"] + "\"," +
                      "\"citycode\":\"" + dtState.Rows[0]["CityCode"] + "\"";

                // Village/area info
                foreach (DataRow dr in dtState.Rows)
                {
                    output += "{\"areacode\":\"" + dr["VillageCode"] + "\",\"areaname\":\"" + dr["VillageName"] + "\"},";
                }
                // Remove trailing comma
                output = output.Substring(0, output.Length - 1);
            }

            output += "]," + str + ",\"response\":\"OK\"}";
        }
        catch (Exception ex)
        {
            output = "{\"response\":\"FAILED\"}";
        }

        return output;
    }
    public string checklogin(string userName, string Password)
    {
        try
        {
            string MemName = "";
            int KitId = 0;
            string Formno = "", Idno = "";
            string Email = "", Package = "", DOj = "", DOA = "";
            string IsFranchise = "", ActiveStatus = "", Address = "N", profilePic = "", Mobl = "";
            bool Bool = false;

            //SqlParameter[] prms = new SqlParameter[2];
            //prms[0] = new SqlParameter("@UserID", userName);
            //prms[1] = new SqlParameter("@Password", Password);
            //Dr = SqlHelper.ExecuteReader(constr1, "sp_Login", prms);
            Comm = new SqlCommand(IsoStart + "Exec sp_LoginApp '" + userName + "','" + Password + "'" + IsoEnd, selectConn);
            Dr = Comm.ExecuteReader();
            if (Dr.Read())
            {
                MemName = (Dr["MemFirstName"].ToString().Trim() + " " + Dr["MemLastName"].ToString().Trim());
                ActiveStatus = Dr["ActiveStatus"].ToString().Trim();
                IsFranchise = Dr["Fld5"].ToString().Trim();
                Formno = Dr["Formno"].ToString().Trim();
                KitId = Convert.ToInt32(Dr["KitID"]);
                profilePic = Dr["profilePic"].ToString().Trim();
                Mobl = Dr["Mobl"].ToString().Trim();
                Email = Dr["email"].ToString().Trim();
                Idno = Dr["IDNo"].ToString();
                Package = Dr["KitName"].ToString();
                DOj = Convert.ToDateTime(Dr["Doj"]).ToString("dd-MMM-yyyy");
                DOA = Convert.ToDateTime(Dr["Upgradedate"]).ToString("dd-MMM-yyyy");
                Address = Dr["Address1"].ToString();
                Bool = true;
            }
            Dr.Close();

            if (Bool)
            {
                Comm = new SqlCommand(IsoStart + "Select * from " + ObjDAL.dBName + "..M_AppUser where UserID='" + userName + "'" + IsoEnd, selectConn);
                Dr = Comm.ExecuteReader();
                Bool = Dr.Read();
                Dr.Close();

                if (!Bool)
                {
                    Comm = new SqlCommand("insert into M_AppUser(UserID,OTP,ActiveStatus,RecTimeStamp) values('" + userName + "','" + Password + "','Y',GetDate())", Conn);
                }
                else
                {
                    Comm = new SqlCommand("Update M_AppUser Set OTP='" + Password + "' Where UserID='" + userName + "'", Conn);
                }

                int i = Comm.ExecuteNonQuery();

                if (i != 0)
                {
                    string LgnID = Encrypt("uid=" + userName + "&pwd=" + Password);
                    DateTime currentDate = DateTime.Now;
                    string result = System.DateTime.Now.Day.ToString() + (System.DateTime.Now.Hour - 1).ToString() + System.DateTime.Now.Year.ToString() + (System.DateTime.Now.Month - 1).ToString();
                    string urlmyac = "https://wow.stanvee.com/login.aspx?lgnT=" + LgnID + "&ID=" + result;

                    string url = "https://epayindia.in/AppLogin.aspx?lgnT=" + LgnID;

                    return "{ \"response\":\"OK\", \"mname\":\"" + MemName + "\", \"isactive\":\"" + ActiveStatus + "\", \"isfranchise\":\"" + IsFranchise + "\"," +
                           " \"kitid\":\"" + KitId + "\", \"profilepic\":\"" + profilePic + "\", \"mobileno\":\"" + Mobl + "\"," +
                           " \"emailid\":\"" + Email + "\", \"idno\":\"" + Idno + "\", \"package\":\"" + Package + "\", \"doj\":\"" + DOj + "\", \"doa\":\"" + DOA + "\",\"address\":\"" + Address + "\",\"url\":\"" + url + "\",\"urlmyac\":\"" + urlmyac + "\" }";
                }
                else
                {
                    return "{ \"response\":\"FAILED\" }";
                }
            }
            else
            {
                return "{ \"response\":\"FAILED\", \"msg\":\"Invalid Login Details.\" }";
            }
        }
        catch (Exception ex)
        {
            return "{ \"response\":\"FAILED\" }";
        }
    }
    private string DashBoard(string userid, string passwd)
    {
        string _Output = "";
        try
        {
            bool Bool;
            string Formno = GetFormNo(userid);
            Bool = MemberExist(userid, passwd);

            if (Bool)
            {
                // Set referral URLs
                Session["CompShortUrl"] = (HttpContext.Current.Request.Url.Host.ToUpper().StartsWith("HTTPS") ? "https://" : "https://")
                    + HttpContext.Current.Request.Url.Host + "/" + Session["JoinPage"];

                DataSet Ds = new DataSet();

                //SqlParameter[] prms = new SqlParameter[1];
                //prms[0] = new SqlParameter("@FormNo", Formno);
                string sql = IsoStart + "exec sp_LoadTeam '" + Formno + "'" + IsoEnd;
                Ds = SqlHelper.ExecuteDataset(selectConn, CommandType.Text, sql);
                _Output = "{\"leftreferral\":\"" + Session["CompShortUrl"] + "NewJoining.aspx?ref=" + Crypto.Encrypt(userid + "/1") + "&side=Left" +
                          "\",\"rightreferral\":\"" + Session["CompShortUrl"] + "NewJoining.aspx?ref=" + Crypto.Encrypt(userid + "/2") + "&side=Right" +
                          "\",\"myteam\":";

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    var row = Ds.Tables[0].Rows[0];
                    _Output += "{ \"todayleftactive\":\"" + row["LeftTodayActive"] + "\"," +
                               " \"todayrightactive\":\"" + row["RightTodayActive"] + "\"," +
                               " \"todaytotalactive\":\"" + (Convert.ToInt32(row["LeftTodayActive"]) + Convert.ToInt32(row["RightTodayActive"])) + "\"," +
                               " \"totalleftactive\":\"" + row["Leftactive"] + "\"," +
                               " \"totalrightactive\":\"" + row["RightActive"] + "\"," +
                               " \"totalactive\":\"" + (Convert.ToInt32(row["Leftactive"]) + Convert.ToInt32(row["RightActive"])) + "\"," +
                               " \"leftdirectactive\":\"" + row["Directactive"] + "\"," +
                               " \"rightdirectactive\":\"" + row["InDirectactive"] + "\"," +
                               " \"totaldirectactive\":\"" + (Convert.ToInt32(row["Directactive"]) + Convert.ToInt32(row["InDirectactive"])) + "\"," +
                               " \"leftcurrentmonthregister\":\"" + row["Crntmonthleftjoin"] + "\"," +
                               " \"rightcurrentmonthregister\":\"" + row["Crntmonthrightjoin"] + "\"," +
                               " \"totalcurrentmonthregister\":\"" + (Convert.ToInt32(row["Crntmonthleftjoin"]) + Convert.ToInt32(row["Crntmonthrightjoin"])) + "\"," +
                               " \"leftcurrentmonthactive\":\"" + row["Crntmonthleftactive"] + "\"," +
                               " \"rightcurrentmonthactive\":\"" + row["Crntmonthrightactive"] + "\"," +
                               " \"totalcurrentmonthactive\":\"" + (Convert.ToInt32(row["Crntmonthleftactive"]) + Convert.ToInt32(row["Crntmonthrightactive"])) + "\"," +
                               " \"leftretopup\":\"" + row["LeftTopup"] + "\"," +
                               " \"rightretopup\":\"" + row["RightTopup"] + "\"," +
                               " \"totalretopup\":\"" + (Convert.ToInt32(row["LeftTopup"]) + Convert.ToInt32(row["RightTopup"])) + "\"," +
                               " \"leftcurrentmonthretopup\":\"" + row["CurrentLeftReTopup"] + "\"," +
                               " \"rightcurrentmonthretopup\":\"" + row["CurrentRightReTopup"] + "\"," +
                               " \"totalcurrentmonthretopup\":\"" + (Convert.ToInt32(row["CurrentLeftReTopup"]) + Convert.ToInt32(row["CurrentRightReTopup"])) + "\"," +
                               " \"leftrepurchasepv\":\"" + row["PvL"] + "\"," +
                               " \"rightrepurchasepv\":\"" + row["PVR"] + "\"," +
                               " \"totalrepurchasepv\":\"" + (Convert.ToInt32(row["PvL"]) + Convert.ToInt32(row["PVR"])) + "\"," +
                               " \"leftcurrentrepurchasepv\":\"" + row["PvSL"] + "\"," +
                               " \"rightcurrentrepurchasepv\":\"" + row["PvSR"] + "\"," +
                               " \"totalcurrentrepurchasepv\":\"" + (Convert.ToInt32(row["PvSL"]) + Convert.ToInt32(row["PvSR"])) + "\"," +
                               " \"leftRepurchjoinpv\":\"" + (Convert.ToInt32(row["PvSL"]) + Convert.ToInt32(row["Crntmonthleftactive"])) + "\"," +
                               " \"rightRepurchjoinpv\":\"" + (Convert.ToInt32(row["PvSR"]) + Convert.ToInt32(row["Crntmonthrightactive"])) + "\"," +
                               " \"totalRepurchjoinpv\":\"" + (Convert.ToInt32(row["PvSL"]) + Convert.ToInt32(row["Crntmonthleftactive"]) + Convert.ToInt32(row["PvSR"]) + Convert.ToInt32(row["Crntmonthrightactive"])) + "\"," +
                               " \"leftjoinpv\":\"" + (Convert.ToInt32(row["PvL"]) + Convert.ToInt32(row["Leftactive"])) + "\"," +
                               " \"rightjoinpv\":\"" + (Convert.ToInt32(row["PVR"]) + Convert.ToInt32(row["RightActive"])) + "\"," +
                               " \"totaljoinpv\":\"" + (Convert.ToInt32(row["PvL"]) + Convert.ToInt32(row["Leftactive"]) + Convert.ToInt32(row["PVR"]) + Convert.ToInt32(row["RightActive"])) + "\"," +
                               " \"leftrepurchasebv\":\"" + row["BVL"] + "\"," +
                               " \"rightrepurchasebv\":\"" + row["BVR"] + "\"," +
                               " \"totalmonthrepurchasebv\":\"" + (Convert.ToInt32(row["BVL"]) + Convert.ToInt32(row["BVR"])) + "\"," +
                               " \"totalleftrepurchasebv\":\"" + row["BVTL"] + "\"," +
                               " \"totalrightrepurchasebv\":\"" + row["BVTR"] + "\"," +
                               " \"totalrepurchbv\":\"" + (Convert.ToInt32(row["BVTL"]) + Convert.ToInt32(row["BVTR"])) + "\"}";
                }
                else
                {
                    _Output += "{}";
                }

                // Profile
                if (Ds.Tables[1].Rows.Count > 0)
                {
                    _Output += ",\"profile\":{\"profilepic\":\"" + Ds.Tables[1].Rows[0]["ProfilePic"] + "\"," +
                               "\"idno\":\"" + Ds.Tables[1].Rows[0]["idno"] + "\"," +
                               "\"name\":\"" + Ds.Tables[1].Rows[0]["name"] + "\"}";
                }
                else
                {
                    _Output += ",\"profile\":{}";
                }

                // Main Wallet
                if (Ds.Tables[2].Rows.Count > 0)
                {
                    _Output += ",\"mainwallet\":{\"credit\":\"" + Ds.Tables[2].Rows[0]["credit"] + "\"," +
                               "\"debit\":\"" + Ds.Tables[2].Rows[0]["debit"] + "\"," +
                               "\"balance\":\"" + Ds.Tables[2].Rows[0]["Balance"] + "\"}";
                }
                else
                {
                    _Output += ",\"mainwallet\":{}";
                }

                // Income
                if (Ds.Tables[4].Rows.Count > 0)
                {
                    var inc = Ds.Tables[4].Rows[0];
                    _Output += ",\"income\":{\"everestincome\":\"" + inc["binaryincome"] + "\"," +
                               "\"sponsoreverestincome\":\"" + inc["magicbinary"] + "\"," +
                               "\"universalincome\":\"" + inc["sliincome"] + "\"," +
                               "\"matchingincome\":\"" + inc["matchingincome"] + "\"," +
                               "\"diamondincome\":\"" + inc["clubincome"] + "\"," +
                               "\"repairincome\":\"" + inc["Repairincome"] + "\"," +
                               "\"spreward\":\"" + inc["SpReward"] + "\"," +
                               "\"totalincome\":\"" + (Convert.ToInt32(inc["binaryincome"]) + Convert.ToInt32(inc["matchingincome"]) + Convert.ToInt32(inc["sliincome"]) + Convert.ToInt32(inc["clubincome"])) + "\"}";
                }
                else
                {
                    _Output += ",\"income\":{}";
                }

                _Output += ",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public static byte[] MD5Hash(string value)
    {
        return MD5.ComputeHash(Encoding.ASCII.GetBytes(value));
    }

    public static string Encrypt(string stringToEncrypt)
    {
        DES.Key = MD5Hash(key);
        DES.Mode = CipherMode.ECB;
        byte[] buffer = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
        return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
    }

    public static string Decrypt(string encryptedString)
    {
        try
        {
            DES.Key = MD5Hash(key);
            DES.Mode = CipherMode.ECB;
            byte[] buffer = Convert.FromBase64String(encryptedString);
            return ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }
        catch (Exception ex)
        {
            return DBNull.Value.ToString();
        }
    }
    private string GetFormNo(string IDNO)
    {
        string AStatus;
        string Mobl;
        string Address;
        AStatus = "N";
        Mobl = "0";
        Address = "";

        try
        {
            string FrmNo = "0";
            string Str = IsoStart + "Select FormNo,ActiveStatus,Mobl,Address1 From " + ObjDAL.dBName + "..M_MemberMaster WHERE IDNO='" + IDNO + "'" + IsoEnd;

            if (selectConn.State == ConnectionState.Closed)
                selectConn.Open();

            using (SqlCommand Comd = new SqlCommand(Str, selectConn))
            using (SqlDataReader Dr = Comd.ExecuteReader())
            {
                if (Dr.Read())
                {
                    FrmNo = Dr["FormNo"].ToString();
                    AStatus = Dr["ActiveStatus"].ToString();
                    Mobl = Dr["Mobl"].ToString();
                    Address = Dr["Address1"].ToString();
                }
            }
            return FrmNo;
        }
        catch (Exception ex)
        {
            // Optional: log the exception
            return "0";
        }
    }
    private bool MemberExist(string userid, string passwd)
    {
        bool exists = false;
        try
        {
            string query = IsoStart + "Select * from " + ObjDAL.dBName + "..M_Membermaster where Idno='" + userid + "' And Passw='" + passwd + "' " + IsoEnd;

            using (SqlCommand comm = new SqlCommand(query, selectConn))
            using (SqlDataReader dr = comm.ExecuteReader())
            {
                exists = dr.Read();
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        return exists;
    }
    public string ChangePassword(string userID, string oldPasswd, string newPasswd)
    {
        string output = "";
        try
        {
            if (UserExists(userID, oldPasswd))
            {
                if (selectConn.State == ConnectionState.Closed) selectConn.Open();
                bool isMemExist = false;

                string query = IsoStart + "Select * from " + ObjDAL.dBName + "..M_MemberMaster Where IDNo='" + userID + "' and Passw ='" + oldPasswd + "'" + IsoEnd;
                using (SqlCommand comm = new SqlCommand(query, selectConn))
                using (SqlDataReader dr = comm.ExecuteReader())
                {
                    isMemExist = dr.Read();
                }

                if (!isMemExist)
                {
                    return @"{""response"":""Incorrect Password""}";
                }

                string strQry = "Insert Into TempMemberMaster Select *,'Password updated through App',GetDate(),'C' From M_MemberMaster Where IDNo='" + userID + "'" +
                                ";Update M_MemberMaster Set Passw='" + newPasswd + "' Where IDNO='" + userID + "'" +
                                ";Update M_AppUser Set OTP='" + newPasswd + "' Where UserID='" + userID + "'";

                if (Conn.State == ConnectionState.Closed) Conn.Open();
                using (SqlCommand comm_ = new SqlCommand(strQry, Conn))
                {
                    int rowsAffected = comm_.ExecuteNonQuery();
                    output = rowsAffected != 0 ? @"{""response"":""OK""}" : @"{""response"":""FAILED""}";
                }
            }
            else
            {
                output = @"{""response"":""FAILED"",""msg"":""Invalid Login Details.""}";
            }
        }
        catch (Exception ex)
        {
            if (Conn != null && Conn.State == ConnectionState.Open) Conn.Close();
            output = @"{""response"":""FAILED""}";
        }

        return output;
    }
    public bool UserExists(string userId, string passwd)
    {
        try
        {
            string query = IsoStart + "Select * from " + ObjDAL.dBName + "..m_membermaster " +
                           "where idno = '" + userId + "' And passw = '" + passwd + "' " + IsoEnd;

            using (SqlCommand comm = new SqlCommand(query, selectConn))
            using (SqlDataReader dr = comm.ExecuteReader())
            {
                return dr.Read(); // Returns true if user exists, false otherwise
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
    private string GetLink(string userId, string passwd, string lnkType, int sessid = 0, string type = "S")
    {
        string output = "";
        try
        {
            HttpContext.Current.Session["CompWeb"] = "https://" + HttpContext.Current.Request.Url.Host + "";
            string formNo = GetFormNo(ClearInject(userId));
            string webUrl = "";

            if (formNo != "0")
            {
                string encryptedParam = Crypto.Encrypt(formNo + "/dttree" + DateTime.Now.ToString("yyyyMMdd") + _Token);

                if (lnkType.ToLower() == "reftree")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/ApprefTree.aspx?s=" + encryptedParam + "rm";
                }
                else if (lnkType.ToLower() == "memtree")
                {
                    if (HttpContext.Current.Session["Compid"] != null && HttpContext.Current.Session["Compid"].ToString() == "1022")
                        webUrl = HttpContext.Current.Session["Compappweb"] + "AppTree.aspx?s=" + encryptedParam + "tm";
                    else
                        webUrl = HttpContext.Current.Session["CompWeb"] + "/AppTree.aspx?s=" + encryptedParam + "tm";
                }
                else if (lnkType.ToLower() == "pooltree" && HttpContext.Current.Session["Compid"] != null && HttpContext.Current.Session["Compid"].ToString() == "1022")
                {
                    webUrl = HttpContext.Current.Session["Compappweb"] + "AppUniTree.aspx?s=" +
                             Crypto.Encrypt(formNo + "/" + type + "/dttree" + DateTime.Now.ToString("yyyyMMdd") + _Token + "tm");
                }
                else if (lnkType.ToLower() == "statement")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppStatement.aspx?s=" +
                             Crypto.Encrypt(formNo + "/dtstatement/" + DateTime.Now.ToString("yyyyMMdd") + _Token + "d/" + sessid);
                }
                else if (lnkType.ToLower() == "dashboardview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppDashboard.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "welcomedocs")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/Welcomeletter.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "levelwisedirectreportview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppMydirects.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "walletreportview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppAllWalletReport.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "businessreportview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppBusinessReport.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "changepasswordview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppChangePass.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "changetransactionpasswordview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppChangeTransPass.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "complainview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppComplain.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "complainsolutionview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppComplainSolution.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "dailyincomereportview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/Appdailyeverestincome.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "downlineview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppDownline.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "myeverestdetailview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppEverestTree.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "addressproofview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/Appiddetail.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "bankdetailview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppKycbankDetail.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "pancardview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppPancard.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "monthlypayoutdetailview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppMonthlyincome.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "receivedpindetailview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppPinReceivedDetails.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "pintransferview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppPinTransfer.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "sendpindetailview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppPinTransferDetails.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "shoppingview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppProductrequestDetail.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "profileview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/Appprofile.aspx?id=" + userId;
                }
                else if (lnkType.ToLower() == "wallettransferview")
                {
                    webUrl = HttpContext.Current.Session["CompWeb"] + "/AppServiceWalletTransfer.aspx?id=" + userId;
                }
                output = "{\"url\":\"" + webUrl + "\",\"response\":\"OK\"}";
            }
            else
            {
                output = "{\"response\":\"FAILED\"}";
            }
        }
        catch (Exception)
        {
            output = "{\"response\":\"FAILED\"}";
        }

        return output;
    }
    public string JsonEncode(string str)
    {
        str = str.Replace("\\", "\\\\");
        str = str.Replace("\"", "\\\"");
        str = str.Replace("//n", " \\n ");
        str = str.Replace("\\n", "\n");
        str = str.Replace("\n", " \\n ");
        if (!string.IsNullOrEmpty(str))
        {
            str = str.Replace(Environment.NewLine, " \\n ");
        }
        str = str.Replace("\r\n", " \\n ");
        str = str.Replace("\t", "\\t");
        return str;
    }
    private string ClearInject(string str)
    {
        string strReturn = str.Replace("'", "''").Replace("\t", " ");
        strReturn = strReturn.Replace("\\\\", "\\");
        if (!string.IsNullOrEmpty(strReturn))
        {
            strReturn = strReturn.Replace(Environment.NewLine, " \\n ");
        }
        return strReturn;
    }
    public void WriteJson(object _object)
    {
        try
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonData = javaScriptSerializer.Serialize(_object);
            WriteRaw(jsonData);
        }
        catch (Exception)
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
    public void WriteRaw(string text)
    {
        try
        {
            Response.Write(text);
        }
        catch (Exception)
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
}
public class GetMsg2
{
    private string _Error;

    public string Response
    {
        get { return _Error; }
        set { _Error = value; }
    }
}
