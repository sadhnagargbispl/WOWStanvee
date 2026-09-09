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

public partial class Main : System.Web.UI.Page
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
            //if (Ds.Tables[0].Rows.Count > 0)
            //{
            //    RptOffers.DataSource = Ds.Tables[0];
            //    RptOffers.DataBind();
            //}
            if (Ds.Tables[1].Rows.Count > 0)
            {
                Rptfreeproduct.DataSource = Ds.Tables[1];
                Rptfreeproduct.DataBind();
            }
            if (Ds.Tables[2].Rows.Count > 0)
            {
                RptMovies.DataSource = Ds.Tables[2];
                RptMovies.DataBind();
            }
            if (Ds.Tables[3].Rows.Count > 0)
            {
                RptScratch.DataSource = Ds.Tables[3];
                RptScratch.DataBind();
            }
            if (Ds.Tables[3].Rows.Count > 0)
            {
                Repeater2.DataSource = Ds.Tables[3];
                Repeater2.DataBind();
            }
            //if (Ds.Tables[4].Rows.Count > 0)
            //{
            //    RptHotels.DataSource = Ds.Tables[4];
            //    RptHotels.DataBind();
            //}
            //if (Ds.Tables[5].Rows.Count > 0)
            //{
            //    RptstanveeProducts.DataSource = Ds.Tables[5];
            //    RptstanveeProducts.DataBind();
            //}
        }
        catch (Exception ex)
        {
            //throw new Exception(ex.Message);
        }
    }
}