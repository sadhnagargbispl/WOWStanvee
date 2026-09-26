using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AppIndex : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindServices();
            //string sql = "SELECT * FROM ServiceMaster WHERE IsActive = 1";
            //DataTable dt = new DataTable();
            //dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            //rptServices.DataSource = dt;
            //rptServices.DataBind();
        }
    }
    private void BindServices()
    {
        try
        {
            DataSet Ds = new DataSet();
            string Sql = "Exec Sp_GetDetails";
            Ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, Sql);

            if (Ds.Tables[1].Rows.Count > 0)
            {
                Rptfreeproduct.DataSource = Ds.Tables[1];
                Rptfreeproduct.DataBind();
            }

            if (Ds.Tables[3].Rows.Count > 0)
            {
                Repeater2.DataSource = Ds.Tables[3];
                Repeater2.DataBind();
            }

        }
        catch (Exception ex)
        {
            //throw new Exception(ex.Message);
        }
    }
}