using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DataAccess
/// </summary>
public class DataAccess
{
	public DataAccess()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public SqlConnection ConnectUserDB()
    {
        SqlConnection dbConnection;
        try
        {
            string connString = Utility.MasterDBString;
            dbConnection = new SqlConnection(connString);
            dbConnection.Open();
            return dbConnection;
        }

        catch (Exception ex)
        {
            throw;
        }
    }
    public DataSet ReadUserData(String queryString)
    {

        try
        {
            SqlConnection SQLConnection = this.ConnectUserDB();
            SqlCommand cmd = new SqlCommand(queryString, SQLConnection);
            //IDataReader rdr = cmd.ExecuteReader();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            if (ds.Tables[0].Rows.Count < 1)
            {
                return null;
            }
            else
            {
                return ds;
            }
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public DataSet GetUserData(String queryString)
    {
        string connectionString = Utility.MasterDBString;
        DataSet ds = new DataSet();

        try
        {
            // Connect to the database and run the query.
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

            // Fill the DataSet.
            adapter.Fill(ds);

        }
        catch (Exception ex)
        {

            // The connection failed. Display an error message.
            // Message.Text = "Unable to connect to the database.";

        }

        return ds;
    }

    public SqlConnection ConnectSocietyDB()
    {
        SqlConnection dbConnection;
        try
        {
            string connString = Utility.SocietyConnectionString;

            dbConnection = new SqlConnection(connString);
            dbConnection.Open();
            return dbConnection;
        }

        catch (Exception ex)
        {
            throw;
        }

    }

    public bool UpdateUser(string strQuery)
    {
        try
        {

            SqlConnection SQLConnection = this.ConnectUserDB();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm = SQLConnection.CreateCommand();
            sqlComm.CommandText = strQuery;
            int i = sqlComm.ExecuteNonQuery();
            return true;
        }

        catch (Exception ex)
        {
            return false;
        }

    }

    public bool Update(string strQuery)
    {
        try
        {

            SqlConnection SQLConnection = this.ConnectSocietyDB();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm = SQLConnection.CreateCommand();
            sqlComm.CommandText = strQuery;
            int i = sqlComm.ExecuteNonQuery();
            return true;
        }

        catch (Exception ex)
        {
            return false;
        }

    }

    public DataSet GetData(String queryString)
    {

        try
        {
            // Retrieve the connection string stored in the Web.config file.
            string connectionString = Utility.SocietyConnectionString;
            DataSet ds = new DataSet();
            // Connect to the database and run the query.
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

            // Fill the DataSet.
            adapter.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            return null;

        }


    }


    public DataSet ReadData(String queryString)
    {
        try
        {
            SqlConnection SQLConnection = this.ConnectSocietyDB();
            SqlCommand cmd = new SqlCommand(queryString, SQLConnection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count < 1)
            {
                return null;

            }


            return ds;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    public int GetSingleValue(String Querystring)
    {
        try
        {
            SqlConnection sqlcon = this.ConnectSocietyDB();
            SqlCommand cmd = new SqlCommand(Querystring, sqlcon);
            int value = Convert.ToInt32(cmd.ExecuteScalar());
            return value;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public Object GetImage(String Querystring)
    {
        try
        {
            SqlConnection sqlcon = this.ConnectSocietyDB();
            SqlCommand cmd = new SqlCommand(Querystring, sqlcon);
            Object value = cmd.ExecuteScalar();
            return value;
        }
        catch
        {
            return null;
        }

    }

    public String GetStringValueFromSocietyDB(String Querystring)
    {

        try
        {
            SqlConnection sqlcon = this.ConnectSocietyDB();
            SqlCommand cmd = new SqlCommand(Querystring, sqlcon);
            String value = (String)cmd.ExecuteScalar();
            return value;
        }
        catch
        {
            return null;
        }
    }

    public String GetStringValue(String Querystring)
    {

        try
        {
            SqlConnection sqlcon = this.ConnectUserDB();
            SqlCommand cmd = new SqlCommand(Querystring, sqlcon);
            String value = (String)cmd.ExecuteScalar();
            return value;
        }
        catch
        {
            return null;
        }
    }
    public int GetSingleUserValue(String Querystring)
    {
        try
        {
            SqlConnection sqlcon = this.ConnectUserDB();
            SqlCommand cmd = new SqlCommand(Querystring, sqlcon);
            int value = (int)cmd.ExecuteScalar();
            return value;
        }
        catch
        {
            return 0;
        }
    }

    public bool DeleteQuery(String queryString)
    {

        SqlConnection SQLConnection = this.ConnectSocietyDB();
        SqlCommand sqlComm = new SqlCommand();
        sqlComm = SQLConnection.CreateCommand();
        sqlComm.CommandText = queryString;
        int i = sqlComm.ExecuteNonQuery();
        return true;
    }



    public bool UpdateQuery(string Querystring)
    {

        try
        {

            SqlConnection con = this.ConnectSocietyDB();
            SqlCommand cmd = new SqlCommand();
            cmd = con.CreateCommand();
            cmd.CommandText = Querystring;
            int i = cmd.ExecuteNonQuery();
            return true;

        }
        catch
        {
            return false;
        }

    }
}