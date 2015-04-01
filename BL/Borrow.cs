using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public class Borrow
    {
        private string _PersonId,
                       _Barcode,
                       _BookTitle;
        private DateTime _BorrowDate,
                         _ToBeReturnedDate,
                         _ReturnDate;

        public string PersonId
        {
            get { return this._PersonId; }
        }

        public string Barcode
        {
            get { return this._Barcode; }
        }

        public string BookTitle
        {
            get { return this._BookTitle; }
        }

        public DateTime BorrowDate
        {
            get { return this._BorrowDate; }
        }

        public DateTime ToBeReturnedDate
        {
            get { return this._ToBeReturnedDate; }
        }

        public DateTime ReturnDate
        {
            get { return this._ReturnDate; }
        }

        public static List<Borrow> getByPersonId(string personId)
        {
            string sql = "SELECT BORROW.*, BOOK.Title FROM BORROW " +
                         "INNER JOIN COPY ON COPY.Barcode = BORROW.Barcode " +
                         "INNER JOIN BOOK ON BOOK.ISBN = COPY.ISBN " +
                         "WHERE BORROW.PersonId=@PersonId AND BORROW.ReturnDate IS NULL";

            List<Borrow> results = new List<Borrow>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlParameter paramPersonId = new SqlParameter("PersonId", SqlDbType.VarChar);
            paramPersonId.Value = personId;
            cmd.Parameters.Add(paramPersonId);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Borrow borrow = new Borrow();

                    borrow._Barcode = dar["Barcode"] as string;
                    borrow._PersonId = dar["PersonId"] as string;
                    borrow._BorrowDate = Convert.ToDateTime(dar["BorrowDate"]);
                    borrow._ToBeReturnedDate = Convert.ToDateTime(dar["ToBeReturnedDate"]);
                    borrow._BookTitle = dar["Title"] as string;

                    results.Add(borrow);
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

        public static void renewLoan(string barcode, int period)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            
            cmd = new SqlCommand("UPDATE BORROW SET BORROW.ToBeReturnedDate = DATEADD(dd, @Period, BORROW.ToBeReturnedDate) WHERE Barcode = @Barcode", con);

            SqlParameter paramPeriod = new SqlParameter("Period", SqlDbType.Int);
            paramPeriod.Value = period;
            cmd.Parameters.Add(paramPeriod);

            SqlParameter paramBarcode = new SqlParameter("Barcode", SqlDbType.VarChar);
            paramBarcode.Value = barcode;
            cmd.Parameters.Add(paramBarcode);

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