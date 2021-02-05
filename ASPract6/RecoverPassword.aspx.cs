using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace ASPract6
{
    public partial class RecoverPassword : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB(AS)"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        private int checkPassword(string password)
        {
            int score = 0;

            //Score 1 Very weak
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            //Score 2 Weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            //Score 3 Medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            //Score 4 Strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            //Score 5 Excellent
            if (Regex.IsMatch(password, @"[\W]{1,}"))
            {
                score++;
            }

            return score;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tb_email.Text = HttpUtility.HtmlEncode(Request.QueryString["Email"]);
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {       
            string pwd = tb_pwd.Text.ToString().Trim();
            string cfmpwd = tb_cfmpwd.Text.ToString().Trim();
            string userid = tb_email.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
            int scores = checkPassword(pwd);
            if (scores != 5 || pwd != cfmpwd)
            {
                lbl_error.Text = "Please make sure you filled in correctly.";
            }
            else
            {
                lbl_error.Text = "";

                string oldpwdWithSalt = pwd + dbSalt;
                byte[] oldhashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(oldpwdWithSalt));
                string olduserHash = Convert.ToBase64String(oldhashWithSalt);
                if (olduserHash.Equals(dbHash))
                {
                    lbl_error.Text = "Cannot use the same password!";
                }
                else
                {
                    lbl_error.Text = "";
                    //Generate random "salt"
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);

                    string pwdWithSalt = pwd + salt;
                    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                    finalHash = Convert.ToBase64String(hashWithSalt);


                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key = cipher.Key;
                    IV = cipher.IV;

                    updatePassword(userid);
                    Response.Redirect("Login.aspx", false);
                }
            }
        }
        public void updatePassword(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET Status = @Status, PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, PasswordTimeUpdate = @PasswordTimeUpdate WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            command.Parameters.AddWithValue("@PasswordHash", finalHash);
            command.Parameters.AddWithValue("@PasswordSalt", salt);
            command.Parameters.AddWithValue("@PasswordTimeUpdate", DateTime.Now);
            command.Parameters.AddWithValue("@Status", 0);
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
    }
}