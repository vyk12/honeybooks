using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public class Book
    {

        static Book()
        {
        }

        private string _title,
                       _isbn,
                       _author,
                       _publicationinfo,
                       _publicationYear;
        private int _pages;

        public string Title
        {
            get { return this._title; }
        }
        public string ISBN
        {
            get { return this._isbn; }
        }
        public string Author
        {
            get { return this._author; }
        }
        public string PublicationInfo
        {
            get { return this._publicationinfo; }
        }
        public string PublicationYear
        {
            get { return this._publicationYear; }
        }
        public int Pages
        {
            get { return this._pages; }
        }
        public static List<Book> search(string query, int byAuthor = 0)
        {
            string SQL = "SELECT BOOK.*, AUTHOR.FirstName, AUTHOR.LastName FROM BOOK INNER JOIN BOOK_AUTHOR ON BOOK_AUTHOR.ISBN=BOOK.ISBN INNER JOIN AUTHOR ON AUTHOR.Aid = BOOK_AUTHOR.Aid ";

            if (byAuthor == 1)
            {
                SQL += "WHERE AUTHOR.FirstName LIKE @Query OR AUTHOR.LastName LIKE @Query";
            }
            else
            {
                SQL += "WHERE Title LIKE @Query";
            }

            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            SqlParameter paramQuery = new SqlParameter("Query", SqlDbType.VarChar);
            paramQuery.Value = "%" + query + "%";
            cmd.Parameters.Add(paramQuery);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Book book = new Book();
                    book._title = dar["Title"] as string;
                    book._isbn = dar["ISBN"] as string;
                    book._author = dar["FirstName"] + " " + dar["LastName"];
                    results.Add(book);
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
        public static List<Book> getAll(int sortByTitle = 1)
        {
            string SQL = "SELECT BOOK.*, AUTHOR.LastName, AUTHOR.FirstName FROM BOOK INNER JOIN BOOK_AUTHOR ON BOOK_AUTHOR.ISBN=BOOK.ISBN INNER JOIN AUTHOR ON AUTHOR.Aid = BOOK_AUTHOR.Aid ORDER BY " + (sortByTitle == 1 ? "BOOK.Title" : "AUTHOR.LastName");
            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Book book = new Book();
                    book._title = dar["Title"] as string;
                    book._isbn = dar["ISBN"] as string;
                    book._author = dar["FirstName"] + " " + dar["LastName"];
                    results.Add(book);
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
        public static List<Book> getByAuthor(int authorID)
        {
            string SQL = "SELECT BOOK.* FROM BOOK " +
                "INNER JOIN BOOK_AUTHOR ON BOOK_AUTHOR.ISBN=BOOK.ISBN " +
                "WHERE BOOK_AUTHOR.Aid=" + authorID;
            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Book book = new Book();
                    book._title = dar["Title"] as string;
                    book._isbn = dar["ISBN"] as string;
                    results.Add(book);
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

        public static Book getByISBN(string ISBN)
        {
            string SQL = "SELECT BOOK.*, AUTHOR.FirstName, AUTHOR.LastName FROM BOOK " +
                "INNER JOIN BOOK_AUTHOR ON BOOK_AUTHOR.ISBN=BOOK.ISBN " +
                "INNER JOIN AUTHOR ON AUTHOR.Aid = BOOK_AUTHOR.Aid " +
                "WHERE BOOK.ISBN='" + ISBN + "'";
            Book book = new Book();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                dar.Read();

                book._title = dar["Title"] as string;
                book._isbn = dar["ISBN"] as string;
                book._publicationYear = dar["PublicationYear"] as string;
                book._publicationinfo = dar["publicationinfo"] as string;
                book._pages = Convert.ToInt32(dar["pages"]);
                book._author = dar["FirstName"] + " " + dar["LastName"];
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return book;
        }

        public void delete()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);

            SqlCommand cmdDelCopy = new SqlCommand("DELETE FROM COPY WHERE ISBN = '" + this._isbn + "'", con);
            SqlCommand cmd = new SqlCommand("DELETE FROM BOOK WHERE ISBN = '" + this._isbn + "'", con);

            try
            {
                con.Open();
                cmdDelCopy.ExecuteScalar();
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

        public bool onlyBelongsTo(int aid)
        {
            bool result;

            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BOOK_AUTHOR WHERE Aid <> @Aid AND ISBN = @ISBN", con);

            SqlParameter paramAid = new SqlParameter("Aid", SqlDbType.Int);
            paramAid.Value = aid;
            cmd.Parameters.Add(paramAid);

            SqlParameter paramISBN = new SqlParameter("ISBN", SqlDbType.VarChar);
            paramISBN.Value = this._isbn;
            cmd.Parameters.Add(paramISBN);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                result = !dar.Read();
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }

            return result;
        }
    }
}