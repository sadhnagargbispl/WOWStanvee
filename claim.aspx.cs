using ClosedXML.Excel;
using Irony.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class claim : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string productid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                this.btnPay.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnPay));
                string encoded = Request.QueryString["productid"];
                byte[] data = Convert.FromBase64String(encoded);
                productid = Encoding.UTF8.GetString(data);
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
          //  throw new Exception(ex.Message);
        }
    }
    public string GenerateRandomStringJoining(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < length; i++)
        {
            sResult += allowChrs[rdm.Next(allowChrs.Length)];
        }

        return sResult;
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    protected void btnPay_Click(object sender, EventArgs e)
    {
        string formNo = Session["formno"].ToString();

        // Scratch card claim current WOW package purchase ke against hi hota hai
        WowCycle cycle = WowBenefit.GetCurrentCycle(formNo);

        string checkSql = @"SELECT COUNT(1)
                            FROM   ScratchClaimOrder
                            WHERE  FormNo = @FormNo
                                   AND WowCycleId = @WowCycleId
                                   AND UPPER(Status) = 'SUCCESS'";
        SqlParameter[] checkParams =
        {
            new SqlParameter("@FormNo", formNo),
            new SqlParameter("@WowCycleId", cycle.CycleId)
        };

        int alreadyClaimed = Convert.ToInt32(SqlHelper.ExecuteScalar(constr, CommandType.Text, checkSql, checkParams));
        if (alreadyClaimed > 0)
        {
            string claimedScript = "window.onload=function(){alert('You have already claimed the scratch card reward for your current WOW package.');window.location='ScratchCard.aspx';}";
            ClientScript.RegisterStartupScript(this.GetType(), "AlreadyClaimed", claimedScript, true);
            return;
        }

        string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
        int isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));

        if (isOk1 > 0)
        {
            string OrderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string sql = "INSERT INTO OnlineTransaction " +
                         "(Response, Orderid, Orderdate, Amount, status, paymentToken, name, email, mobl, address1, " +
                         "pincode, statecode, city, epinno, scratchno, kitid, Refformno,ProductId) " +
                         "VALUES " +
                         "('', '" + OrderId + "', GETDATE(), '699', '', '" + OrderId + "', '" +
                         txtName.Value + "', '" + txtEmail.Value + "', '" + txtPhone.Value + "', '" +
                         txtAddress.Value + "', '" + Convert.ToInt32(txtZip.Value) + "', '0', '" + txtCity.Value + "', '', '', '0', '" + Session["formno"] + "','" + productid + "')";

            int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);
            if (i > 0)
            {
                // Order ko current purchase cycle se jodo - payment success par isi ko SUCCESS mark karenge
                string orderSql = @"INSERT INTO ScratchClaimOrder (OrderId, FormNo, WowCycleId, WowBillNo, ProductId, Status)
                                    VALUES (@OrderId, @FormNo, @WowCycleId, @WowBillNo, @ProductId, 'INITIATED')";
                SqlHelper.ExecuteNonQuery(constr, CommandType.Text, orderSql,
                    new SqlParameter("@OrderId", OrderId),
                    new SqlParameter("@FormNo", formNo),
                    new SqlParameter("@WowCycleId", cycle.CycleId),
                    new SqlParameter("@WowBillNo", (object)cycle.BillNo ?? DBNull.Value),
                    new SqlParameter("@ProductId", (object)productid ?? DBNull.Value));

                GenerateQrCode(OrderId, "699");
            }
        }
        else
        {
            string script = "window.onload=function(){alert('Try Again After Some Time.!');window.location='claim.aspx?productid=" + Request.QueryString["productid"] + ";}";
            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            return;
        }
    }
    public string GenerateQrCode(string Orderid, string Amount)
    {
        string str = string.Empty;
        decimal value = 0;
        string Code = "";
        DataSet ds = new DataSet();
        DataSet data;
        string sResult = string.Empty;

        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            string URL = "https://allupi.com/api/login";

            WebRequest tRequest = WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            string postData = "{\"merchantID\":\"fb49222f-5e56-4531-a72b-ab0ee0db7a99\",\"securityCode\":\"316a3029-e8e7-4135-889a-73e600957627\"}";
            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID) VALUES " +
                             "('" + sResult + "', '0', '" + URL + "', '" + postData +
                             "', 'LoginClaim', '" + Orderid + "')";

            int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse tResponse = tRequest.GetResponse();
            using (Stream responseStream = tResponse.GetResponseStream())
            using (StreamReader tReader = new StreamReader(responseStream))
            {
                str = tReader.ReadToEnd();
            }

            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'LoginClaim'";

            int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

            data = convertJsonStringToDataSet(str);

            string auth = data.Tables[1].Rows[0]["token"].ToString();

            string sql = "INSERT INTO LoginTransaction " +
                         "(TId, Username, role, token, refreshToken, name, transactionid, amount) VALUES (" +
                         "'" + data.Tables[1].Rows[0]["ID"] + "'," +
                         "'" + data.Tables[1].Rows[0]["username"] + "'," +
                         "'" + data.Tables[1].Rows[0]["role"] + "'," +
                         "'" + data.Tables[1].Rows[0]["token"] + "'," +
                         "'" + data.Tables[1].Rows[0]["refreshToken"] + "'," +
                         "'" + data.Tables[1].Rows[0]["name"] + "'," +
                         "'" + Orderid + "'," +
                         "'" + Amount + "')";

            int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);

            Response.Write(data.Tables[1]);

            CheckLiveRateCoin(auth, Orderid, Amount);
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" + ex.Message +
                             "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'PaymentProcessLoginLogin'";

            int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
        }

        return str;
    }
    protected string CheckLiveRateCoin(string auth, string orderid, string amount)
    {
        try
        {
            string str = string.Empty;
            decimal value = 0;
            string code = "";
            DataSet ds = new DataSet();
            DataSet data;
            DataSet dsLogin = new DataSet();

            string completeUrl = "https://allupi.com/api/InitiateTransactionAsync";
            string responseString = string.Empty;
            string CustomerOTPmessage = "";
            string url = "";

            string sResult = string.Empty;
            string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random_number = new Random().Next(0, 999);
            string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
            sResult = formatted_datetime;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                WebRequest tRequest = (HttpWebRequest)WebRequest.Create(completeUrl);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add("X-Auth", auth);

                string postdata = "{\"requestedId\":\"" + orderid + "\",";
                postdata += "\"amount\":" + amount + ",\"upiId\":\"Q395598285\",";
                postdata += "\"serverHookURL\":\"https://wow.stanvee.com/Login.aspx\",";
                postdata += "\"webHookURL\":\"https://wow.stanvee.com/PaymentGateWayNew.aspx\"}";

                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                                 "(ReqID, Formno, Request, postdata, Req_From, OrderID) VALUES " +
                                 "('" + sResult + "', '" + Session["formno"] + "', '" + url + "', '" +
                                 postdata + "', 'InitiateTransactionAsyncClaim', '" + orderid + "')";

                int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse tResponse = tRequest.GetResponse();
                using (Stream responseStream = tResponse.GetResponseStream())
                using (StreamReader tReader = new StreamReader(responseStream))
                {
                    str = tReader.ReadToEnd();
                }

                string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'InitiateTransactionAsyncClaim'";

                int x_res = SqlHelper.ExecuteNonQuery(
                    constr,
                    CommandType.Text,
                    sql_res
                );

                data = convertJsonStringToDataSet(str);

                Response.Write(str);

                if (data.Tables[0].Rows[0]["url"].ToString() != "")
                {
                    Response.Redirect(data.Tables[0].Rows[0]["url"].ToString());
                }
            }
            catch (Exception ex)
            {
                string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
                                 ex.Message + "' WHERE ReqID = '" + sResult +
                                 "' AND Req_From = 'InitiateTransactionAsyncClaim'";

                SqlHelper.ExecuteNonQuery(
                    constr,
                    CommandType.Text,
                    sql_res
                );
            }

            return url;
        }
        catch
        {
            return string.Empty;
        }
    }
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();

        jsonString = "{ \"rootNode\": {" +
                     jsonString.Trim().TrimStart('{').TrimEnd('}') +
                     "} }";

        xd = JsonConvert.DeserializeXmlNode(jsonString);

        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));

        return ds;
    }
}