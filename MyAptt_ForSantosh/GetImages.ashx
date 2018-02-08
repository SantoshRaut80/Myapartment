<%@ WebHandler Language="C#" Class="GetImages" %>

using System;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using System.IO;


public class GetImages : IHttpHandler {

    //string strcon = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    public void ProcessRequest(HttpContext context)            
    {
        context.Response.Clear();
        String imageid = context.Request.QueryString["ImID"];

        if (imageid != null && imageid != "")
        {

            try
            {
                using (SqlConnection con1 = new SqlConnection(Utility.SocietyConnectionString))
                {

                    con1.Open();
                    //  SqlCommand command = new SqlCommand("select Image from Image where ImageID=" + imageid, con1);
                    SqlCommand command = new SqlCommand("select VendorIcon from Vendors where  ID=" + imageid, con1);
                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            context.Response.BinaryWrite((Byte[])dr[0]);
                            con1.Close();
                            context.Response.Flush();
                        }
                    }

                    else
                    {

                    }

                }
            }
            catch(Exception ex)
            {
                int a = 1;
            }

        }
        
        
         String ResID = context.Request.QueryString["ResID"];
         try
         {
             if (ResID != null && ResID != "")
             {
                 using (SqlConnection con1 = new SqlConnection(Utility.SocietyConnectionString))
                 {
                     con1.Open();
                     SqlCommand command = new SqlCommand("select Profile_image from ResidentImage where  ResID=" + ResID, con1);
                     SqlDataReader dr = command.ExecuteReader();
                     dr.Read();
                     
                     context.Response.BinaryWrite((Byte[])dr[0]);

                     byte[] image = ((Byte[])dr[0]);
                     ImageServer.SaveImage(image, ResID);
                  
                     con1.Close();
                     
                     context.Response.End();
                                     
                 }
             }
         }
        
        catch(Exception ex)
         {
            
         }

         try
         {
             String firstFlat = context.Request.QueryString["firstFlat"];

             if (firstFlat != null && firstFlat != "")
             {
                 using (SqlConnection con1 = new SqlConnection(Utility.SocietyConnectionString))
                 {
                     con1.Open();
                     SqlCommand command = new SqlCommand("select FirstImage from ViewThreadSummary where  firstFlat ='" + firstFlat + "'", con1);
                     SqlDataReader dr = command.ExecuteReader();
                     dr.Read();
                     context.Response.BinaryWrite((Byte[])dr[0]);
                     con1.Close();
                     context.Response.End();
                 }
             }
         }
        
        catch(Exception ex)
         {
            
         }

         try
         {
             String lastFlat = context.Request.QueryString["lastFlat"];

             if (lastFlat != null && lastFlat != "")
             {
                 using (SqlConnection con1 = new SqlConnection(Utility.SocietyConnectionString))
                 {
                     con1.Open();
                     SqlCommand command = new SqlCommand("select LastImage from ViewThreadSummary where  lastFlat='" + lastFlat + "'", con1);
                     SqlDataReader dr = command.ExecuteReader();
                     dr.Read();
                     context.Response.BinaryWrite((Byte[])dr[0]);
                     con1.Close();
                     context.Response.End();
                 }
             }

         }
         catch(Exception ex)
         {
             
         }

         try
         {
             String NotifiImID = context.Request.QueryString["NotifiImID"];

             if (NotifiImID != null && NotifiImID != "")
             {
                 using (SqlConnection con1 = new SqlConnection(Utility.SocietyConnectionString))
                 {
                     con1.Open();
                     SqlCommand command = new SqlCommand("select ImageData,AttachType from dbo.Notifications  where  ID ='" + NotifiImID + "'", con1);
                     SqlDataReader dr = command.ExecuteReader();
                     dr.Read();
                     context.Response.BinaryWrite((Byte[])dr[0]);
                     con1.Close();
                     context.Response.End();
                 }
             }

         }
         catch (Exception ex)
         {

         }

         finally
         {
             context.Response.End();
         }
        
             
    }



    public bool IsReusable {
        get {
            return false;
        }
    }

}