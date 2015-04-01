using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public class Borrower
    {
        private string _PersonId,
                       _FirstName,
                       _LastName,
                       _Address,
                       _Telno,
                       _Username,
                       _Password;
        private int _CategoryId;

        public string PersonId
        {
            get { return this._PersonId; }
        }

        public string Address
        {
            get { return this._Address; }
        }

        public string Telno
        {
            get { return this._Telno; }
        }

        public string Username
        {
            get { return this._Username; }
        }

        public int CategoryId
        {
            get { return this._CategoryId; }
        }

        [Display(Name = "First name")]
        public string FirstName
        {
            get { return this._FirstName; }
            set { this._FirstName = value; }
        }
        [Display(Name = "Last name")]
        public string LastName
        {
            get { return this._LastName; }
            set { this._LastName = value; }
        }

        public static Borrower getByLoginAndPasswd(string login, string password)
        {
            Borrower borrower = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BORROWER WHERE Username=@Username AND Password=@Password", con);
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
                    borrower = new Borrower();
                    borrower._PersonId = dar["PersonId"] as string;
                    borrower._FirstName = dar["FirstName"] as string;
                    borrower._LastName = dar["LastName"] as string;
                    borrower._Address = dar["Address"] as string;
                    borrower._CategoryId = Convert.ToInt32(dar["CategoryId"]);
                    borrower._Telno = dar["Telno"] as string;
                    borrower._Username = dar["Username"] as string;
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
            return borrower;
        }
    }
}