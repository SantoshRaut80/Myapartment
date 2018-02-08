using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;
using System.Data;
using System.Text;

public partial class Login : System.Web.UI.Page
{

    User muser;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnForgotpass_Click(object sender, EventArgs e)
    {
        String UserLogin = txtForgotText.Text;
        String NewPassword = RandomPassword(8);
        DataAccess dacess = new DataAccess();
        String UserExistQuery = "select max(UserID) from dbo.TotalUsers where UserLogin ='" + UserLogin + "' or EmailId =  '" + UserLogin + "'";

        int User = dacess.GetSingleUserValue(UserExistQuery);

        if (User != 0)
        {
            String ForgotPassQuery = "Update dbo.TotalUsers set Password = '" + NewPassword + "' where UserLogin = '" + UserLogin + "'";
            bool result = dacess.UpdateUser(ForgotPassQuery);

            if (result == true)
            {
                String UserLoginQuery = "Select EmailId from dbo.TotalUsers where UserLogin= '" + UserLogin + "'";
                String EmailId = dacess.GetStringValue(UserLoginQuery);

                User muser = new User();

                bool result1 = muser.UpdatePassword(UserLogin, NewPassword);
                if (result1 == true)
                {
                    try
                    {
                        SendMailForgot(UserLogin, NewPassword, EmailId);
                        lblres.Text = "Password  link  is sent  to  your EmailId.";
                        lblPasswordRes.ForeColor = System.Drawing.Color.Gray;
                        lblPasswordRes.Visible = true;
                        lblPasswordRes.Text = "Your  new password  is sent to your Registered EmailId";
                        lblerror.Visible = false;
                    }

                    catch
                    {
                        lblPasswordRes.Text = "Some  problem  with  our  server, try later.";
                        lblerror.Visible = false;
                    }
                }

            }
            else
            {

            }
        }
        else
        {
            lblPasswordRes.Text = "Please Enter Valid UserID";
        }
    
    }

    public void SendMailForgot(string UserLogin, string password, string EmailID)
    {

        string URI = "http://www.kevintech.in/mailserver.php";
        WebRequest request = WebRequest.Create(URI);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        string postData = "ToMail=" + EmailID + "&Password=" + password + "&Username=" + UserLogin;
        Stream dataStream = request.GetRequestStream();
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        WebResponse response = request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        reader.Close();
        dataStream.Close();
        response.Close();

    }

    public static string RandomPassword(int length)
    {


        string Charactersallowed = "";
        Charactersallowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Charactersallowed += "abcdefghijklmnopqrstuvwxyz";
        Charactersallowed += "0123456789@#$%&";
        char[] chars = new char[length];
        int aloowedchars = Charactersallowed.Length;
        Random rdm = new Random();


        for (int i = 0; i < 8; i++)
        {
            chars[i] = Charactersallowed[rdm.Next(0, aloowedchars)];

        }

        return new string(chars);
    }
    protected void lnkForgotPass_Click(object sender, EventArgs e)
    {
        String UserLogin = TxtUserID.Text;

        //Session["UserLogin"] = UserLogin;
        SessionVariables.UserLogin = UserLogin;

        if (Session != null)
        {

            txtForgotText.Text = UserLogin;
        }

    }



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static bool ValidateUser(Users user)
    {
        User muser = new User();
        if (muser.Validate(user.Username, user.Password))
        {
            muser.SetUserInfo();
            Utility.Initializehashtables();


            //added by aarshi to test session
            SessionVariables.User = muser;
            SessionVariables.UserType = muser.UserType;
            SessionVariables.ResiID = muser.ResiID;
            SessionVariables.UserLogin = muser.UserLogin;
            SessionVariables.SocietyName = muser.SocietyName;
            SessionVariables.FlatNumber = muser.FlatNumber;
            SessionVariables.LName = muser.strLastName;
            SessionVariables.FName = muser.strFirstName;

            return true;
            //HttpContext.Current.Response.Redirect("MainPage.aspx", false);
        }
        else
            return false;

    }
    public class Users
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}