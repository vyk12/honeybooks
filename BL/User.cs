using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace BL
{
    public class User
    {
        private string _Username,
                       _Password;
        private int _id;

        public int id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Username
        {
            get { return this._Username; }
            set { this._Username = value; }
        }

        public string Password
        {
            get { return this._Password; }
            set { this._Password = value; }
        }

        public static User getByLoginAndPasswd(string login, string password)
        {
            User user = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE Username=@Username AND Password=@Password", con);
            SqlParameter paramUsername = new SqlParameter("Username", SqlDbType.VarChar);
            SqlParameter paramPassword = new SqlParameter("Password", SqlDbType.VarChar);
            paramUsername.Value = login;
            paramPassword.Value = password;
            cmd.Parameters.Add(paramUsername);
            cmd.Parameters.Add(paramPassword);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    user = new User();

                    user._id = Convert.ToInt32(dar["id"]);
                    user._Username = dar["Username"] as string;
                    user._Password = dar["Password"] as string;
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return user;
        }
    }
}
