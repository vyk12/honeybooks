using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                       _isbn;
        public string Title
        {
            get { return this._title; }
        }
        public string ISBN
        {
            get { return this._isbn; }
        }
        public static List<Book> search(string query, int byAuthor = 0)
        {
            string SQL = "SELECT BOOK.* FROM BOOK ";

            if (byAuthor == 1)
            {
                SQL += "INNER JOIN BOOK_AUTHOR ON BOOK_AUTHOR.ISBN=BOOK.ISBN INNER JOIN AUTHOR ON AUTHOR.Aid = BOOK_AUTHOR.Aid WHERE AUTHOR.FirstName LIKE '%" + query + "%' OR AUTHOR.LastName LIKE '%" + query + "%'";
            }
            else
            {
                SQL += "WHERE Title LIKE '%" + query + "%'";
            }

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
        public static List<Book> getAll()
        {
            string SQL = "SELECT BOOK.* FROM BOOK";
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


    }
}