//using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Appinsurance : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                this.btnSubmit.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnSubmit));
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    FillDetail();
                }

            }

            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            // throw new Exception(ex.Message);
        }
    }
    private void FillDetail()
    {
        try
        {
            string sql = "Exec Sp_GetInsurance '" + Session["Formno"] + "'";
            DataTable dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["FullName"].ToString();
                // txtDOB.Text = dt.Rows[0]["DateOfBirth"].ToString();
                txtMobile.Text = dt.Rows[0]["MobileNumber"].ToString();
                txtEmail.Text = dt.Rows[0]["EmailID"].ToString();
                txtNominee.Text = dt.Rows[0]["NomineeName"].ToString();
                //txtNomineeDOB.Text = dt.Rows[0]["NomineeDOB"].ToString();
                txtRelation.Text = dt.Rows[0]["RelationshipWithNominee"].ToString();
                if (dt.Rows[0]["DateOfBirth"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(dt.Rows[0]["DateOfBirth"]);
                    txtDOB.Text = dob.ToString("yyyy-MM-dd");
                }
                if (dt.Rows[0]["NomineeDOB"] != DBNull.Value)
                {
                    DateTime ndob = Convert.ToDateTime(dt.Rows[0]["NomineeDOB"]);
                    txtNomineeDOB.Text = ndob.ToString("yyyy-MM-dd");
                }
                // Disable filled fields
                txtName.Enabled = string.IsNullOrEmpty(txtName.Text);
                txtDOB.Enabled = string.IsNullOrEmpty(txtDOB.Text);
                txtMobile.Enabled = string.IsNullOrEmpty(txtMobile.Text);
                txtEmail.Enabled = string.IsNullOrEmpty(txtEmail.Text);
                txtNominee.Enabled = string.IsNullOrEmpty(txtNominee.Text);
                txtNomineeDOB.Enabled = string.IsNullOrEmpty(txtNomineeDOB.Text);
                txtRelation.Enabled = string.IsNullOrEmpty(txtRelation.Text);

                // 👉 Button hide logic
                bool allFilled =
                    !string.IsNullOrEmpty(txtName.Text) &&
                    !string.IsNullOrEmpty(txtDOB.Text) &&
                    !string.IsNullOrEmpty(txtMobile.Text) &&
                    !string.IsNullOrEmpty(txtEmail.Text) &&
                    !string.IsNullOrEmpty(txtNominee.Text) &&
                    !string.IsNullOrEmpty(txtNomineeDOB.Text) &&
                    !string.IsNullOrEmpty(txtRelation.Text);

                btnSubmit.Visible = !allFilled; // sab filled → button hide
            }
        }
        catch (Exception ex)
        {
            // optional: log error
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
    void ShowAlert(string message)
    {
        string script = $"alert('{message}');";
        ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string script_ = "";
        // Name validation
        // Full Name
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            ShowAlert("Please enter Full Name");
            return;
        }

        // DOB empty
        if (string.IsNullOrWhiteSpace(txtDOB.Text))
        {
            ShowAlert("Please enter Date of Birth");
            return;
        }

        // DOB valid
        DateTime dob;
        if (!DateTime.TryParse(txtDOB.Text, out dob))
        {
            ShowAlert("Please select valid Date of Birth");
            return;
        }

        // Age calculation
        int age = DateTime.Today.Year - dob.Year;
        if (dob > DateTime.Today.AddYears(-age)) age--;

        if (age < 18)
        {
            ShowAlert("Age must be 18 years or above");
            return;
        }

        // Mobile
        if (string.IsNullOrWhiteSpace(txtMobile.Text))
        {
            ShowAlert("Please enter Mobile Number");
            return;
        }

        // Email
        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            ShowAlert("Please enter Email ID");
            return;
        }

        // Nominee Name
        if (string.IsNullOrWhiteSpace(txtNominee.Text))
        {
            ShowAlert("Please enter Nominee Name");
            return;
        }

        // Nominee DOB
        if (string.IsNullOrWhiteSpace(txtNomineeDOB.Text))
        {
            ShowAlert("Please enter Nominee Date of Birth");
            return;
        }

        // Relationship
        if (string.IsNullOrWhiteSpace(txtRelation.Text))
        {
            ShowAlert("Please enter Relationship with Nominee");
            return;
        }
        string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
        int isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));

        if (isOk1 > 0)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Insurance
                  (FullName, DateOfBirth, MobileNumber, EmailID,
                   NomineeName, NomineeDOB, RelationshipWithNominee,FormNo)
                  VALUES
                  (@Name, @DOB, @Mobile, @Email,
                   @Nominee, @NomineeDOB, @Relation,@FormNo)", con);

                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@DOB", dob);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Nominee", txtNominee.Text);
                cmd.Parameters.AddWithValue("@NomineeDOB", txtNomineeDOB.Text);
                cmd.Parameters.AddWithValue("@Relation", txtRelation.Text);
                cmd.Parameters.AddWithValue("@FormNo", Session["FormNo"]);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            string message1 = "Insurance Form Submitted Successfully ✔";
            string script = "window.onload=function(){alert('" + message1 + "');window.location='insurance.aspx';}";
            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            ClearForm();
            return;
        }
        else
        {
            string script = "window.onload=function(){alert('Try Again After Some Time.!');window.location='insurance.aspx';}";
            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            return;
        }
    }

    void ClearForm()
    {
        txtName.Text = "";
        txtDOB.Text = "";
        txtMobile.Text = "";
        txtEmail.Text = "";
        txtNominee.Text = "";
        txtNomineeDOB.Text = "";
        txtRelation.Text = "";
    }
}