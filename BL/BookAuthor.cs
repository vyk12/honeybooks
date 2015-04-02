using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public class BookAuthor
    {
        public static int create(string ISBN, int Aid)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            int retVal;

            cmd = new SqlCommand("INSERT INTO BOOK_AUTHOR (ISBN, Aid) VALUES (@ISBN, @Aid)", con);

            SqlParameter paramISBN = new SqlParameter("ISBN", SqlDbType.VarChar);
            paramISBN.Value = ISBN;
            cmd.Parameters.Add(paramISBN);

            SqlParameter paramAid = new SqlParameter("Aid", SqlDbType.Int);
            paramAid.Value = Aid;
            cmd.Parameters.Add(paramAid);

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

        public static void delete(string isbn)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);

            SqlCommand cmd = new SqlCommand("DELETE FROM BOOK_AUTHOR WHERE ISBN = @ISBN", con);

            SqlParameter paramISBN = new SqlParameter("ISBN", SqlDbType.VarChar);
            paramISBN.Value = isbn;
            cmd.Parameters.Add(paramISBN);

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

        public static void delete(int id)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);

            SqlCommand cmd = new SqlCommand("DELETE FROM BOOK_AUTHOR WHERE Aid = @Aid", con);

            SqlParameter paramISBN = new SqlParameter("Aid", SqlDbType.Int);
            paramISBN.Value = id;
            cmd.Parameters.Add(paramISBN);

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

        public static void edit(string ISBN, int Aid)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE BOOK_AUTHOR SET Aid=@Aid WHERE ISBN=@ISBN", con);

            SqlParameter paramISBN = new SqlParameter("ISBN", SqlDbType.VarChar);
            paramISBN.Value = ISBN;
            cmd.Parameters.Add(paramISBN);

            SqlParameter paramAid = new SqlParameter("Aid", SqlDbType.Int);
            paramAid.Value = Aid;
            cmd.Parameters.Add(paramAid);

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

        public static int getAuthorId(string isbn)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT BOOK_AUTHOR.Aid FROM BOOK_AUTHOR WHERE ISBN=@ISBN", con);

            SqlParameter paramISBN = new SqlParameter("ISBN", SqlDbType.VarChar);
            paramISBN.Value = isbn;
            cmd.Parameters.Add(paramISBN);

            int Aid;

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                dar.Read();

                Aid = Convert.ToInt32(dar["Aid"]);
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return Aid;
        }
    }
}