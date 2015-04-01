using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public class Category
    {
        private int _CategoryId,
                    _Period,
                    _Penaltyperday;
        private string _Category;

        public int CategoryId
        {
            get { return this._CategoryId; }
        }

        public string CategoryValue
        {
            get { return this._Category; }
        }

        public int Period
        {
            get { return this._Period; }
        }

        public int Penaltyperday
        {
            get { return this._Penaltyperday; }
        }

        public static Category getById(int id)
        {
            Category category = null;

            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM CATEGORY WHERE CategoryId = @CategoryId", con);
            SqlParameter paramCategoryId = new SqlParameter("CategoryId", SqlDbType.Int);
            paramCategoryId.Value = id;
            cmd.Parameters.Add(paramCategoryId);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    category = new Category();

                    category._CategoryId = Convert.ToInt32(dar["CategoryId"]);
                    category._Category = dar["Category"] as string;
                    category._Period = Convert.ToInt32(dar["Period"]);
                    category._Penaltyperday = Convert.ToInt32(dar["Penaltyperday"]);
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

            return category;
        }
    }
}