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

        [Display(Name = "Username")]
        public string Username
        {
            get { return this._Username; }
            set { this._Username = value; }
        }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
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

        public static User getById(int id)
        {
            User user = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE id=" + id, con);

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

        public static List<User> getAll()
        {
            List<User> results = new List<User>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM USERS", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    User newUser = new User();
                    newUser._id = Convert.ToInt32(dar["id"]);
                    newUser._Username = dar["Username"] as string;
                    newUser._Password = dar["Password"] as string;
                    results.Add(newUser);
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
            return results;
        }

        public int save()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            int retVal = -1;
            bool existing;
            if (this._id == 0)
            {
                existing = false;
                cmd = new SqlCommand("INSERT INTO USERS (Username, Password) VALUES ('" + this._Username + "','" + this._Password + "'); SELECT SCOPE_IDENTITY()", con);
            }
            else
            {
                existing = true;
                cmd = new SqlCommand("UPDATE USERS SET Username='" + this._Username + "', Password='" + this._Password + "' WHERE id=" + this._id, con);
            }
            try
            {
                con.Open();
                if (!existing)
                {
                    retVal = cmd.ExecuteNonQuery();
                }
                else
                {
                    retVal = -1;
                    cmd.ExecuteScalar();
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
            return retVal;
        }

        public static void deleteById(int id)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE id = " + id, con);

            try
            {
                con.Open();
                cmd.ExecuteScalar();
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
