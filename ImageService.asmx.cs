using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServiceForImages
{
    /// <summary>
    /// Summary description for ImageService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ImageService : System.Web.Services.WebService
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        DataSet ds;

        [WebMethod]
        public string PostImage(byte[] ImgData)
        {
            string ImageTitle = "";

            da = new SqlDataAdapter("select TOP 1 ImgID from imgtbl order by ImgID desc", con);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                ImageTitle = "Img_" + DateTime.Now.Date.Day + DateTime.Now.Date.Month + "" + DateTime.Now.Date.Year + "_0";
            }
            else
            {
                ImageTitle = "Img_" + DateTime.Now.Date.Day + DateTime.Now.Date.Month + "" + DateTime.Now.Date.Year + dt.Rows[0]["ImgID"].ToString();
            }

            cmd = new SqlCommand("insert into imgtbl values(@ImageData,@ImgTitle)", con);
            cmd.Parameters.AddWithValue("@ImageData", ImgData);
            cmd.Parameters.AddWithValue("@ImgTitle", ImageTitle);
            con.Open();
            bool Status = Convert.ToBoolean(cmd.ExecuteNonQuery());
            con.Close();
            if (Status)
            {
                return "Image Saved.!!!";
            }
            else
            {
                return "There should be some error...";
            }
        }

        [WebMethod]
        public DataSet GetAllImages()
        {
            da = new SqlDataAdapter("select * from imgtbl", con);
            ds = new DataSet();
            da.Fill(ds, "imgtbl");
            return ds;
        }

        [WebMethod]
        public DataSet GetImageByName(string ImageTitle)
        {
            da = new SqlDataAdapter("select * from imgtbl where ImgTitle=@ImgTitle", con);
            da.SelectCommand.Parameters.AddWithValue("@ImgTitle", ImageTitle);
            ds = new DataSet();
            da.Fill(ds, "imgtbl");
            return ds;
        }
    }
}
