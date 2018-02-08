using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for ImageServer
/// </summary>
public class ImageServer
{
	public ImageServer()
	{
		//
		// TODO: Add constructor logic here
		//

       
	}

    public static bool SaveImage(byte[] imageByte,String ResID)
    {
       // String Savepath = Request.PhysicalApplicationPath + "~\\ImageTest\\";
          var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/Temp");

        try
        {
             Image result = null;
             MemoryStream stream = new MemoryStream(imageByte);
             result = new Bitmap(stream);

             ImageFormat format = ImageFormat.Png;

              using (Image imageToExport = result)
             {
              // string filePath = string.Format(@"D:\SoftwareProjects\Notes\.{0}", format.ToString());

                 string filePath = string.Format(path + "\\" + ResID + ".{0}", format.ToString());

                imageToExport.Save(filePath, format);
               

               }
      stream.Close();
            return true;       
        }

        catch (Exception ex)
        {

            return false;
        }
    
    
    
    }

    public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
    {
        try
        {
            //Get the image current width
            int sourceWidth = imgToResize.Width;
            //Get the image current height
            int sourceHeight = imgToResize.Height;
            float sourceHWRatio = (float)sourceHeight / (float)sourceWidth;
            float targetHWRation = (float)size.Height / (float)size.Width;
            int newHeight, newWidth = 0;

            if (sourceHWRatio > targetHWRation)
            {
                newHeight = size.Height;
                newWidth = (int)(((float)sourceWidth / (float)sourceHeight) * newHeight);
            }
            else {
                newWidth = size.Width;
                newHeight = (int)(((float)sourceHeight / (float)sourceWidth) * newWidth);
            }

            Bitmap b = null;

            b = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height
            g.DrawImage(imgToResize, 0, 0, newWidth, newHeight);
            g.Dispose();

            return (System.Drawing.Image)b;

        }


        catch (Exception ex)
        {
            return null;
        }

        finally
        {

        }
    }

    public static byte[] ImageToByte(Image img)
    {
        ImageConverter converter = new ImageConverter();
        return (byte[])converter.ConvertTo(img, typeof(byte[]));
    }
}