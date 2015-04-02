using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public class Author
    {

        public Author(string firstname, string lastname, int birthyear)
        {
            this._FirstName = firstname;
            this._LastName = lastname;
            this._BirthYear = birthyear;
        }
        public Author() { }
        private int _Aid,
            _BirthYear;
        private string _FirstName,
            _LastName;

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
        [Display(Name = "Author ID")]
        public int Aid
        {
            get { return (int)this._Aid; }
            set { this._Aid = value; }
        }
        [Display(Name = "Birth year")]
        public int BirthYear
        {
            get { return this._BirthYear; }
            set { this._BirthYear = value; }
        }
        private string _About;
        [Display(Name = "About")]
        public string About
        {
            get
            {
                return this._About;
            }
            set { this._About = value; }
        }

        public void delete()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);

            SqlCommand cmdDelAuthor = new SqlCommand("DELETE FROM AUTHOR WHERE Aid = " + this._Aid, con);

            try
            {
                con.Open();
                cmdDelAuthor.ExecuteScalar();
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
        public List<Book> getBooks()
        {
            return Book.getByAuthor(this._Aid);
        }
        public static Author getByAid(int aid)
        {
            Author newAuthor = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM AUTHOR WHERE Aid=@Aid", con);
            SqlParameter paramAid = new SqlParameter("Aid", SqlDbType.Int);
            paramAid.Value = aid;
            cmd.Parameters.Add(paramAid);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    newAuthor = new Author();
                    newAuthor.Aid = (int)dar["Aid"];
                    newAuthor.BirthYear = (dar["BirthYear"] == DBNull.Value) ? 0 : Convert.ToInt32(dar["BirthYear"].ToString());
                    newAuthor.FirstName = dar["FirstName"] as string;
                    newAuthor.LastName = dar["LastName"] as string;
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
            return newAuthor;
        }
        public static List<Author> getAll()
        {
            List<Author> results = new List<Author>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM AUTHOR", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Author newAuthor = new Author();
                    newAuthor.Aid = (int)dar["Aid"];
                    newAuthor.BirthYear = (dar["BirthYear"] == DBNull.Value) ? 0 : Convert.ToInt32(dar["BirthYear"].ToString());
                    newAuthor.FirstName = dar["FirstName"] as string;
                    newAuthor.LastName = dar["LastName"] as string;
                    results.Add(newAuthor);
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
            if (this._Aid == 0)
            {
                existing = false;
                cmd = new SqlCommand("INSERT INTO AUTHOR (FirstName, LastName, BirthYear) VALUES ('" + this._FirstName + "','" + this._LastName + "'," + this._BirthYear + "); SELECT SCOPE_IDENTITY()", con);
            }
            else
            {
                existing = true;
                cmd = new SqlCommand("UPDATE AUTHOR set FirstName='" + this.FirstName + "', LastName='" + this.LastName + "', BirthYear=" + this.BirthYear + " WHERE Aid=" + this.Aid, con);
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
    }
}