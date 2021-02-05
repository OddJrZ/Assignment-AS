using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPract6
{
    public partial class Success : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB(AS)"].ConnectionString;
        byte[] Key;
        byte[] IV;
        string userID = null;
        
        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;

                //Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                //Create the streams used for decryption
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            //Read the decrpyted bytes from the decryption stream
                            //and place them in a string
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }

        protected void displayUserProfile(string userid)
        {
            string FirstName = null;
            string LastName = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM Account WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_userID.Text = reader["Email"].ToString();
                        }

                        if (reader["FirstName"] != DBNull.Value)
                        {
                            FirstName = reader["FirstName"].ToString();
                        }

                        if (reader["LastName"] != DBNull.Value)
                        {
                            LastName = reader["LastName"].ToString();
                        }

                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }

                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                    }
                    lbl_name.Text = FirstName + LastName;
                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected void LogoutMe(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (Session["UserID"] == null)
                    Response.Redirect("Login.aspx", false);
                else
                {
                    Response.ClearHeaders();
                    Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                    Response.AddHeader("Pragma", "no-cache");
                }

            }
            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    userID = (string)Session["UserID"];
                    btn_Logout.Visible = true;
                    updatePassStatus(userID);
                    displayUserProfile(userID);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
            
        }

        protected void updatePassStatus(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET Status = @passStatus WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            command.Parameters.AddWithValue("@passStatus", 0);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
        }

        protected string getPasswordAge(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordTimeUpdate FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordTimeUpdate"] != null)
                        {
                            if (reader["PasswordTimeUpdate"] != DBNull.Value)
                            {
                                h = reader["PasswordTimeUpdate"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected void btn_CheckMin_Click(object sender, EventArgs e)
        {
            DateTime CurrentPasswordAge = Convert.ToDateTime(getPasswordAge(lbl_userID.Text));
            DateTime MinPasswordAge = CurrentPasswordAge.AddMinutes(5);
            if (DateTime.Now > MinPasswordAge)
            {
                lbl_MinAge.Text = "You can change your password";
                lbl_MinAge.ForeColor = System.Drawing.Color.Blue;
                btn_changePass.Visible = true;
            }
            else
            {
                lbl_MinAge.Text = "You cannot change your password yet";
                lbl_MinAge.ForeColor = System.Drawing.Color.Red;
                btn_changePass.Visible = false;
            }

        }

        protected void btn_CheckMax_Click(object sender, EventArgs e)
        {
            DateTime CurrentPasswordAge = Convert.ToDateTime(getPasswordAge(lbl_userID.Text));
            DateTime MaxPasswordAge = CurrentPasswordAge.AddMinutes(15);
            if (DateTime.Now < MaxPasswordAge)
            {
                lbl_MaxAge.Text = "Your password has yet to expire";
                lbl_MaxAge.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lbl_MaxAge.Text = "Your password has expired";
                lbl_MaxAge.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btn_changePass_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecoverPassword.aspx?Email=" + HttpUtility.UrlEncode(lbl_userID.Text));
        }
    }
}