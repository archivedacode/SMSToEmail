//M----------------------------------------------------------------------------
//M   MODULE HEADER
//M----------------------------------------------------------------------------
//M
//M        File Name : SingletonConnection.cs   (C) Ouch-Mobile 2009
//M
//M----------------------------------------------------------------------------
//M   MODULE CHANGE HISTORY
//M----------------------------------------------------------------------------
//M
//M        DATE         BY     DESCRIPTION
//M        ----         --     -----------
//M
//M        28-08-2009   DER    Initial Version
//M
//M----------------------------------------------------------------------------
//M   MODULE DESCRIPTION
//M----------------------------------------------------------------------------
//M
//M        The Singleton Database Manager class
//M        Provides all input \ output functionality to the Database
//M
//M----------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsMobile.PocketOutlook;

namespace SmsEngine
{
    class SingletonConnection
    {
        private SqlCeEngine sqlEngine = null;
        private SqlCeCommand sqlCommand = null;
        private SqlCeDataAdapter sqlAdapter = null;
        private SqlCeConnection sqlConnection = null;

        private static SingletonConnection instance;
        private static object padlock = new object();

        private static string connectionString = "";

        protected SingletonConnection()
        {
            connectionString = "Data Source=" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "Cache.sdf") + ";Password=manager;";

            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "Cache.sdf")))
            {
                CreateDatabase();
            }
            else
            {
                sqlConnection = new SqlCeConnection(connectionString);
                sqlCommand = sqlConnection.CreateCommand();
                sqlAdapter = new SqlCeDataAdapter(sqlCommand);

                Open();
            }
        }

        public static SingletonConnection Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SingletonConnection();
                    }
                    return instance;
                }
            }
        }

        public void Open()
        {
            if (!sqlConnection.State.Equals(ConnectionState.Open))
            {
                sqlConnection.Open();
            }
        }

        /// <summary>
        /// Close the Database Manager and Dispose all resources
        /// </summary>
        public void CloseAndDispose()
        {
            sqlConnection.Close();
            sqlAdapter.Dispose();
            sqlCommand.Dispose();
            sqlConnection.Dispose();

            instance = null;
        }

        /// <summary>
        /// Create the Database
        /// </summary>
        public void CreateDatabase()
        {
            sqlEngine = new SqlCeEngine(connectionString);
            sqlEngine.CreateDatabase();
            sqlEngine.Dispose();

            sqlConnection = new SqlCeConnection(connectionString);
            sqlCommand = sqlConnection.CreateCommand();
            sqlAdapter = new SqlCeDataAdapter(sqlCommand);

            Open();

            // Create SMS_OUTBOX Table
            sqlCommand.CommandText = "CREATE TABLE SMS_OUTBOX (" +
                                     "ID int NOT NULL IDENTITY(1,1) PRIMARY KEY," +
                                     "BODY nvarchar(1600)," +
                                     "LASTMODIFIED datetime," +
                                     "ADDRESS nvarchar(100)," +
                                     "NAME nvarchar(100))";

            sqlCommand.ExecuteNonQuery();


            // Create RECIPIENTS Table
            sqlCommand.CommandText = "CREATE TABLE RECIPIENTS (" +
                                     "ADDRESS nvarchar(320) NOT NULL PRIMARY KEY)";

            sqlCommand.ExecuteNonQuery();

            // Create ACCOUNTS Table
            sqlCommand.CommandText = "CREATE TABLE ACCOUNTS (" +
                                     "ADDRESS nvarchar(320) NOT NULL PRIMARY KEY)";

            sqlCommand.ExecuteNonQuery();

            // Create OPTIONS Table
            sqlCommand.CommandText = "CREATE TABLE OPTIONS (" +
                                     "DISABLED nvarchar(1)," +
                                     "PROMPT nvarchar(1)," +
                                     "SYNCHRONIZE nvarchar(1)," +
                                     "REGISTERED nvarchar(1))";
            sqlCommand.ExecuteNonQuery();

            // Insert Default OPTIONS value
            sqlCommand.CommandText = "INSERT INTO OPTIONS (DISABLED, PROMPT, SYNCHRONIZE, REGISTERED) VALUES ('F', 'T', 'T', 'F')";
            sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Delete an Account
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        internal bool DeleteAccount(string address)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "DELETE FROM ACCOUNTS WHERE ADDRESS=" + address;

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Delete the entire ACCOUNTS table
        /// </summary>
        /// <returns></returns>
        internal bool DeleteAllAccounts()
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "DELETE FROM ACCOUNTS";

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Insert an Email Account
        /// </summary>
        /// <param name="address"></param>
        /// <param name="selected"></param>
        internal void InsertAccount(string address)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "INSERT INTO ACCOUNTS (ADDRESS) VALUES (?)";

                sqlCommand.Parameters.Add("?", SqlDbType.NVarChar).Value = address;

                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Delete a Recipient
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        internal bool DeleteRecipient(string address)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "DELETE FROM RECIPIENTS WHERE ADDRESS='" + address + "'";

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Insert a new Recipient
        /// </summary>
        /// <param name="address">Email Address</param>
        /// <param name="id"></param>
        internal bool InsertRecipient(string address)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "INSERT INTO RECIPIENTS (ADDRESS) VALUES (?)";

                sqlCommand.Parameters.Add("?", SqlDbType.NVarChar).Value = address;

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Insert an SmsMessage into the SMS_OUTBOX
        /// </summary>
        /// <param name="sms">SmsMessage</param>
        /// <param name="id">Unique Identifier</param>
        internal void InsertOutboxSms(SmsMessage sms, ref string id)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "INSERT INTO SMS_OUTBOX (BODY, ADDRESS, NAME) VALUES (?,?,?)";

                sqlCommand.Parameters.Add("?", SqlDbType.NVarChar).Value = sms.Body;
                sqlCommand.Parameters.Add("?", SqlDbType.NVarChar).Value = sms.To[0].Address;
                sqlCommand.Parameters.Add("?", SqlDbType.NVarChar).Value = "";

                sqlCommand.ExecuteNonQuery();

                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "SELECT MAX(ID) FROM SMS_OUTBOX";

                id = Convert.ToString(sqlCommand.ExecuteScalar());
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Get a single SmsMessage from SMS_OUTBOX in a DataTable
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <returns>DataTable</returns>
        internal DataTable GetOutboxSmsMessage(string id)
        {
            DataTable dt = new DataTable();

            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "SELECT * FROM SMS_OUTBOX WHERE ID=" + id;

                sqlAdapter.Fill(dt);

                return dt;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);

                dt.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                dt.Clear();
                return dt;
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// Get a unique SmsMessage
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="text"></param>
        /// <returns></returns>
        internal SmsMessage GetOutboxSms(string id, ref string text)
        {
            DataTable dt = new DataTable();

            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "SELECT * FROM SMS_OUTBOX WHERE ID=" + id;

                sqlAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    text = Convert.ToString(dt.Rows[0]["BODY"]);
                    string address = Convert.ToString(dt.Rows[0]["ADDRESS"]);

                    SmsMessage sms = new SmsMessage();

                    sms.To.Add(new Recipient(address));
                    sms.Body = Convert.ToString(dt.Rows[0]["BODY"]);

                    return sms;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get the entire SMS_OUTBOX in a DataTable
        /// Ordered by Last Modofied date
        /// </summary>
        /// <returns>DataTable</returns>
        internal DataTable GetSmsOutbox()
        {
            DataTable dt = new DataTable();

            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "SELECT * FROM SMS_OUTBOX ORDER BY LASTMODIFIED DESC";

                sqlAdapter.Fill(dt);

                return dt;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);

                dt.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                dt.Clear();
                return dt;
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// Get the entire ACCOUNTS table in a DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        internal DataTable GetAllAccounts()
        {
            DataTable dt = new DataTable();

            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "SELECT * FROM ACCOUNTS";

                sqlAdapter.Fill(dt);

                return dt;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);

                dt.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                dt.Clear();
                return dt;
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// Delete an SmsMessage with a unique identifier
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <returns>Success or Fail</returns>
        internal bool DeleteOutboxSms(string id)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "DELETE FROM SMS_OUTBOX WHERE ID=" + id;

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get all of the RECIPIENTS table
        /// </summary>
        /// <returns>DataTable</returns>
        internal DataTable GetAllRecipients()
        {
            DataTable dt = new DataTable();

            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "SELECT * FROM RECIPIENTS";

                sqlAdapter.Fill(dt);

                return dt;
            }
            catch (SqlCeException sx)
            {
                MessageBox.Show(sx.Message);

                dt.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                dt.Clear();
                return dt;
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// Get the configuration options
        /// </summary>
        /// <param name="bPrompt"></param>
        /// <param name="bSync"></param>
        /// <param name="bDisable"></param>
        /// <returns></returns>
        internal bool GetConfigOptions(ref bool bPrompt,
                                       ref bool bSync,
                                       ref bool bDisable,
                                       ref bool bRegistered)
        {
            DataTable dt = new DataTable();

            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "SELECT * FROM OPTIONS";

                sqlAdapter.Fill(dt);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        string result = "";

                        // SET DISABLED OPTION
                        result = Convert.ToString(dt.Rows[0]["DISABLED"]);
                        if (result.Equals("T"))
                        {
                            bDisable = true;
                        }
                        else
                        {
                            bDisable = false;
                        }

                        // SET PROMPT OPTION
                        result = Convert.ToString(dt.Rows[0]["PROMPT"]);
                        if (result.Equals("T"))
                        {
                            bPrompt = true;
                        }
                        else
                        {
                            bPrompt = false;
                        }

                        // SET SYNCHRONIZE OPTION
                        result = Convert.ToString(dt.Rows[0]["SYNCHRONIZE"]);
                        if (result.Equals("T"))
                        {
                            bSync = true;
                        }
                        else
                        {
                            bSync = false;
                        }

                        // SET REGISTERED OPTION
                        result = Convert.ToString(dt.Rows[0]["REGISTERED"]);
                        if (result.Equals("T"))
                        {
                            bRegistered = true;
                        }
                        else
                        {
                            bRegistered = false;
                        }

                        return true;
                    }
                    else
                    {
                        // any problems, default these
                        bDisable = false;
                        bPrompt = false;
                        bSync = false;
                        bRegistered = false;

                        return false;
                    }
                }
                else
                {
                    // any problems, default these
                    bDisable = false;
                    bPrompt = false;
                    bSync = false;
                    bRegistered = false;

                    return false;
                }
            }
            catch (SqlCeException)
            {
                // any problems, default these
                bDisable = false;
                bPrompt = false;
                bSync = false;
                bRegistered = false;

                return false;
            }
            catch (Exception)
            {
                // any problems, default these
                bDisable = false;
                bPrompt = false;
                bSync = false;
                bRegistered = false;

                return false;
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// Update the Registered value
        /// </summary>
        /// <param name="value"></param>
        internal bool UpdateRegistered(string value)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "UPDATE OPTIONS SET REGISTERED = @Value";

                sqlCommand.Parameters.Add("@Value", SqlDbType.NVarChar).Value = value;

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update the user configured Prompt value
        /// </summary>
        /// <param name="value"></param>
        internal bool UpdatePrompt(string value)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "UPDATE OPTIONS SET PROMPT = @Value";

                sqlCommand.Parameters.Add("@Value", SqlDbType.NVarChar).Value = value;

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update the user configured Disable value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool UpdateEnabled(string value)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "UPDATE OPTIONS SET DISABLED = @Value";

                sqlCommand.Parameters.Add("@Value", SqlDbType.NVarChar).Value = value;

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update the user configured Synchronize value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool UpdateSynchronize(string value)
        {
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "UPDATE OPTIONS SET SYNCHRONIZE = @Value";

                sqlCommand.Parameters.Add("@Value", SqlDbType.NVarChar).Value = value;

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlCeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
