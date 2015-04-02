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

        [Display(Name = "Person ID")]
        public string PersonId
        {
            get { return this._PersonId; }
            set { this._PersonId = value; }
        }

        [Display(Name = "Address")]
        public string Address
        {
            get { return this._Address; }
            set { this._Address = value; }
        }

        [Display(Name = "Tel. no.")]
        public string Telno
        {
            get { return this._Telno; }
            set { this._Telno = value; }
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

        [Display(Name = "Category ID")]
        public int CategoryId
        {
            get { return this._CategoryId; }
            set { this._CategoryId = value; }
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

        public int create()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            int retVal;

            cmd = new SqlCommand("INSERT INTO BORROWER (PersonId, FirstName, LastName, Address, Telno, CategoryId, Username, Password) VALUES " +
                "('" + this._PersonId + "', '" + this._FirstName + "','" + this._LastName + "','" + this._Address + "', " +
                "'" + this._Telno + "', " + this._CategoryId + ", '" + this._Username + "', '" + this._Password + "')", con);
            
            try
            {
                con.Open();
                retVal = cmd.ExecuteNonQuery();
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

        public static List<Borrower> getAll()
        {
            List<Borrower> results = new List<Borrower>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BORROWER", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Borrower newBorrower = new Borrower();
                    newBorrower.PersonId = dar["PersonId"] as string;
                    newBorrower.FirstName = dar["FirstName"] as string;
                    newBorrower.LastName = dar["LastName"] as string;
                    newBorrower.Address = dar["Address"] as string;
                    newBorrower.Telno = dar["Telno"] as string;
                    newBorrower.CategoryId = Convert.ToInt32(dar["CategoryId"]);
                    newBorrower.Username = dar["Username"] as string;
                    results.Add(newBorrower);
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

        public static Borrower getByPersonId(string id)
        {
            Borrower newBorrower = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BORROWER WHERE PersonId=@PersonId", con);
            SqlParameter paramPersonId = new SqlParameter("PersonId", SqlDbType.VarChar);
            paramPersonId.Value = id;
            cmd.Parameters.Add(paramPersonId);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    newBorrower = new Borrower();
                    newBorrower.PersonId = dar["PersonId"] as string;
                    newBorrower.FirstName = dar["FirstName"] as string;
                    newBorrower.LastName = dar["LastName"] as string;
                    newBorrower.Address = dar["Address"] as string;
                    newBorrower.Telno = dar["Telno"] as string;
                    newBorrower.CategoryId = Convert.ToInt32(dar["CategoryId"]);
                    newBorrower.Username = dar["Username"] as string;
                    newBorrower.Password = dar["Password"] as string;
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
            return newBorrower;
        }

        public void edit()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            int retVal;

            cmd = new SqlCommand("UPDATE BORROWER SET FirstName = '" + this._FirstName + "', " +
                "LastName = '" + this._LastName + "', Address = '" + this._Address + "', Telno = '" + this._Telno + "', " +
                "CategoryId = " + this._CategoryId + ", Username = '" + this._Username + "', Password = '" + this._Password + "'" +
                "WHERE PersonId = '" + this._PersonId + "'", con);

            try
            {
                con.Open();
                retVal = cmd.ExecuteNonQuery();
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

        public static void delete(string personId)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);

            SqlCommand cmd = new SqlCommand("DELETE FROM BORROWER WHERE PersonId = @PersonId", con);

            SqlParameter paramPersonId = new SqlParameter("PersonId", SqlDbType.VarChar);
            paramPersonId.Value = personId;
            cmd.Parameters.Add(paramPersonId);

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