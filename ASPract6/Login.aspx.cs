using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace ASPract6
{
    public partial class Login : System.Web.UI.Page
    {
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB(AS)"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Lcsx0IaAAAAANi4ux_jNCei8ZGubr7gyBJE9JQa &response=" + captchaResponse);

            try
            {
                using(WebResponse wResponse = req.GetResponse())
                {
                    using(StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string pwd = tb_pwd.Text.ToString().Trim();
                string userid = tb_email.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(userid);
                string dbSalt = getDBSalt(userid);
                int passStatus = getPassStatus(userid);

                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        if (passStatus < 3)
                        {
                            if (userHash.Equals(dbHash))
                            {
                                Session["UserID"] = userid;
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                DateTime CurrentPasswordAge = Convert.ToDateTime(getPasswordAge(userid));
                                DateTime MaxPasswordAge = CurrentPasswordAge.AddMinutes(15);
                                if (DateTime.Now > MaxPasswordAge)
                                {
                                    Response.Redirect("RecoverPassword.aspx?Email=" + HttpUtility.UrlEncode(tb_email.Text), false);
                                }
                                else
                                {
                                    Response.Redirect("Success.aspx", false);
                                }
                            }
                            else
                            {
                                increasePassStatus(userid, passStatus);
                                lbl_errorXSS.Text = HttpUtility.HtmlEncode(tb_email.Text);
                                lbl_errorMsg.Text = "Email or password is not valid.Please try again.";
                            }
                        }
                        else
                        {
                            btn_Recover.Visible = true;
                            btn_ChangePassword.Visible = true;
                            lbl_errorMsg.Text = "Your account is currently locked. Recover Account to unlock or change password!";
                        }

                    }
                    else
                    {
                        lbl_errorXSS.Text = HttpUtility.HtmlEncode(tb_email.Text);
                        lbl_errorMsg.Text = "Email or password is not valid.Please try again.";
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { }

            }
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

        protected void increasePassStatus(string userid, int passStatus)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET Status = @passStatus WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            command.Parameters.AddWithValue("@passStatus", passStatus+1);
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

        protected int getPassStatus(string userid)
        {
            int pass = 0;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Status FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Status"] != null)
                        {
                            if (reader["Status"] != DBNull.Value)
                            {
                                pass = Convert.ToInt32(reader["Status"].ToString());

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
            return pass;
        }

        protected void resetPassStatus(string userid)
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

        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
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

        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
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
            return s;
        }

        //unlock account to allow user to try again
        protected void btn_Recover_Click(object sender, EventArgs e)
        {
            string userid = tb_email.Text.ToString().Trim();
            resetPassStatus(userid);
            btn_Recover.Visible = false;
            btn_ChangePassword.Visible = false;
            lbl_errorMsg.Text = "Your account has been unlocked! Please try again!";
        }

        //change account password + unlock
        protected void btn_ChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecoverPassword.aspx?Email=" + HttpUtility.UrlEncode(tb_email.Text));
        }
    }
}