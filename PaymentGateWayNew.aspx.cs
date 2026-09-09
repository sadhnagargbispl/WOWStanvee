using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class PaymentGateWayNew : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request["requestedId"] != null)
            {
                string sRequestData = HttpContext.Current.Request.Url.ToString();
                GenerateQrCode(Request["requestedId"]);
            }
            else
            {
                Response.Redirect("Default.aspx", false);
            }
        }
        catch (Exception)
        {
            Response.Write("{\"response\":\"FAILED\"}");
        }

    }
    public string GenerateQrCode(string Orderid)
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
            tRequest.ContentLength = 0;
            string postData = "{\"merchantID\":\"fb49222f-5e56-4531-a72b-ab0ee0db7a99\",\"securityCode\":\"316a3029-e8e7-4135-889a-73e600957627\"}";
            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID) VALUES " +
                             "('" + sResult + "', '0', '" + URL + "', '" + postData +
                             "', 'LoginClaimLogin', '" + Orderid + "')";

            int x_Req = SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_req
            );

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

            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str +
                             "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'PaymentProcessLoginLogin'";

            int x_res = SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_res
            );

            data = convertJsonStringToDataSet(str);

            string auth = data.Tables[1].Rows[0]["token"].ToString();

            CheckLiveRateCoin(auth, Orderid);
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
                             ex.Message + "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'PaymentProcessLoginLogin'";

            SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_res
            );
        }

        return str;
    }
    private string CheckLiveRateCoin(string auth, string orderid)
    {
        string url = "";
        string sResult = string.Empty;

        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        try
        {
            DataSet data;
            string UrlR = "https://allupi.com/api/Status?RequestedId=" + orderid;

            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID) VALUES " +
                             "('" + sResult + "', '0', '" + UrlR + "', '" + UrlR +
                             "', 'StatusCheckClaim', '" + orderid + "')";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlR);
            request.Headers.Add("X-Auth", auth);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = 0;

            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
            string responseBody = "";

            using (Stream receiveStream = response1.GetResponseStream())
            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
            {
                responseBody = readStream.ReadToEnd();
            }

            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" +
                             responseBody + "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'StatusCheckClaim'";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

            data = convertJsonStringToDataSet(responseBody);

            string status = "";

            if (string.IsNullOrEmpty(data.Tables[1].Rows[0]["status"].ToString()))
            {
                status = "FAILED";
            }
            else
            {
                status = data.Tables[1].Rows[0]["status"].ToString();
            }

            string str = "UPDATE LoginTransaction SET Status='" + status +
                         "', response='" + responseBody +
                         "', Responsedate=GETDATE() WHERE TransactionId='" + orderid + "'";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);

            if (status.ToUpper() == "SUCCESS")
            {
                str = "EXEC sp_ScrClaim '" + orderid + "'";
                int Nx = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);

                if (Nx > 0)
                {
                    Response.Redirect("ScratchCard.aspx?status=success", false);
                }
            }
            else if (status.ToUpper() == "PENDING")
            {
                Response.Redirect("https://wow.stanvee.com/Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
                             ex.Message + "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'StatusCheckClaim'";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
        }

        return url;
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