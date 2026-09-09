using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Irony.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MM_Voucher : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindServices();
        }
    }
    private void BindServices()
    {
        DataSet Ds = new DataSet();
        string Sql = "Exec Sp_GetDetailsdiss";
        Ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, Sql);

        var list = new List<object>();

        if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
        {
            RptProducts.DataSource = Ds.Tables[0];
            RptProducts.DataBind();
        }
    }
}