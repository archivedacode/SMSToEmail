//M----------------------------------------------------------------------------
//M   MODULE HEADER
//M----------------------------------------------------------------------------
//M
//M        File Name : Engine.cs   (C) Carbon Software 2009, 2010
//M
//M----------------------------------------------------------------------------
//M   MODULE CHANGE HISTORY
//M----------------------------------------------------------------------------
//M
//M        DATE         BY     DESCRIPTION
//M        ----         --     -----------
//M
//M        02-01-2010   DER    Removed IsEnabled DbManager check
//M
//M        21-10-2009   DER    Changed constructor to do a lot of the common 
//M                            load work. Improve performance.
//M
//M        27-09-2009   DER    Added DatabaseManager class
//M
//M        28-08-2009   DER    Version
//M
//M----------------------------------------------------------------------------
//M   MODULE DESCRIPTION
//M----------------------------------------------------------------------------
//M
//M        Form class for the Engine application
//M
//M----------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsMobile.PocketOutlook;
using Microsoft.WindowsMobile.PocketOutlook.MessageInterception;

namespace SmsEngine
{
    public partial class Engine : Form
    {
        private bool bSent = false;
        private static int COUNT = 0;

        private DatabaseManager dbManager = null;
        private MessageInterceptor smsInterceptor = null;
        private const string AppRegisterName = "OuchSmsToEmail";

        private OutlookSession session = null;
        private EmailAccount emailAccount = null; 
        private EmailAccountCollection collection = null;
        private DataRowCollection recipientCollection = null;

        public Engine(string[] args)
        {
            // If the interceptor is enabled
            if (MessageInterceptor.IsApplicationLauncherEnabled(AppRegisterName))
            {
                // We started this app via an SMS {1}
                if (args.Length > 0)
                {
                    // An SMS started the Engine...
                    if (args[0].Equals("1"))
                    {
                        InitializeComponent();
                        
                        dbManager = new DatabaseManager(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));

                        // we must have both accounts and recipients to proceed!
                        if ((dbManager.ACCOUNTS.Rows.Count > 0) && (dbManager.RECIPIENTS.Rows.Count > 0))
                        {
                            this.session = new OutlookSession();
                            this.collection = session.EmailAccounts;

                            // Set the recipient collection
                            this.recipientCollection = dbManager.RECIPIENTS.Rows;

                            if (collection.Count > 0)
                            {
                                // Set the email account
                                this.emailAccount = collection[Convert.ToString(dbManager.ACCOUNTS.Rows[0]["ADDRESS"])];

                                if (this.emailAccount != null)
                                {
                                    // Create the Interceptor
                                    smsInterceptor = new MessageInterceptor(AppRegisterName, true);
                                    smsInterceptor.MessageReceived += new MessageInterceptorEventHandler(smsInterceptor_MessageReceived);
                                }
                                else
                                {
                                    this.Close();
                                }
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void ForwardSms(SmsMessage sms)
        {
            // get the SMS message
            if (sms != null)
            {
                // just check the recipient collection isnt goosed
                if (this.recipientCollection != null)
                {
                    foreach (DataRow recipient in this.recipientCollection)
                    {
                        EmailMessage msg = new EmailMessage();
                        
                        msg.To.Add(new Recipient("", recipient["ADDRESS"].ToString()));
                        msg.Subject = "SMS To Email : " + sms.Received.ToLocalTime().ToString();

                        StringBuilder sb = new StringBuilder();

                        sb.Append("From : " + sms.From.Address);
                        sb.Append("\r\n");
                        sb.Append("\r\n");

                        try
                        {
                            if ((sms.To[0] != null) && (sms.To[0].Address.Length > 0))
                            {
                                sb.Append("To : " + sms.To[0].Address);
                                sb.Append("\r\n");
                                sb.Append("\r\n");
                            }
                        }
                        catch
                        { }

                        sb.Append(sms.Body);

                        msg.BodyText = sb.ToString();

                        emailAccount.Send(msg);

                        this.bSent = true;
                    }
                }
            }
        }

        private void Engine_Activated(object sender, EventArgs e)
        {
            this.Hide();
            this.Height = 0;
            this.Visible = false;
        }

        private void smsInterceptor_MessageReceived(object sender, MessageInterceptorEventArgs e)
        {
            try
            {
                // Increment the SMS count
                COUNT = COUNT + 1;

                SmsMessage sms = (SmsMessage)e.Message;

                // If Prompt is enabled
                if (dbManager.CanPrompt())
                {
                    string text = sms.Body;

                    if (sms.Body.Length > 90)
                    {
                        text = sms.Body.Substring(0, 90) + " ...";
                    }

                    if (MessageBox.Show("From : " + sms.From.Address + "\r\n" + "Text : " + text,
                                        "Forward via SMS to Email ?" + " (" + COUNT.ToString() + ")",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        // Forward the SMS message
                        this.ForwardSms(sms);
                    }
                }
                else
                {
                    // Forward the SMS message
                    this.ForwardSms(sms);
                }
            }
            catch (StackOverflowException) { }
            catch (Exception) { }
            finally
            {
                // Decrement the COUNT once an SMS has been processed
                COUNT = COUNT - 1;

                // If all pending SMS messages have been processed, dispose and close.
                if (COUNT <= 0)
                {
                    // If we sent an SMS message
                    if (this.bSent)
                    {
                        // Sync the Outbox now if we can
                        if (this.dbManager.CanSynchronize())
                        {
                            MessagingApplication.Synchronize(this.emailAccount);
                        }
                    }

                    // Remove the MessageReceived event handler
                    smsInterceptor.MessageReceived -= smsInterceptor_MessageReceived;

                    // Dispose of the MessageInterceptor
                    smsInterceptor.Dispose();

                    // Close and Exit
                    this.Close();
                }
            }
        }
    }

    internal class DatabaseManager
    {
        private string appPath = "";
        private string xmlFile = "";

        private DataSet ds = null;
        private DataTable dtConfig = null;
        private DataTable dtAccounts = null;
        private DataTable dtRecipients = null;

        private const string DATA_SET = "DATA";
        private const string FILE_NAME = "DATA.xml";

        private const string CONFIG_TABLE = "CONFIG";
        private const string ACCOUNTS_TABLE = "ACCOUNTS";
        private const string RECIPIENTS_TABLE = "RECIPIENTS";

        public DataTable CONFIG
        {
            get { return dtConfig; }
            set { dtConfig = value; }
        }
        public DataTable ACCOUNTS
        {
            get { return dtAccounts; }
            set { dtAccounts = value; }
        }
        public DataTable RECIPIENTS
        {
            get { return dtRecipients; }
            set { dtRecipients = value; }
        }

        private DataColumn dcAccountsAddress = null;
        private DataColumn dcRecipientsAddress = null;
        private DataColumn dcConfigPrompt = null;
        private DataColumn dcConfigSynchronize = null;

        public enum ConfigValue : int { T, F };
        public enum ConfigColumn : int { Prompt, Synchronize };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationPath"></param>
        public DatabaseManager(string applicationPath)
        {
            this.appPath = applicationPath;

            this.xmlFile = Path.Combine(this.appPath, FILE_NAME);

            this.DefineSchema();

            if (!File.Exists(this.xmlFile))
            {
                this.CreateDefaultXmlData();
            }
            else
            {
                this.ReadXmlData();
            }
        }

        /// <summary>
        /// Read the XML data
        /// </summary>
        private void ReadXmlData()
        {
            ds.ReadXml(this.xmlFile);
        }

        /// <summary>
        /// Write the XML data file
        /// </summary>
        public void SaveXmlData()
        {
            this.ds.AcceptChanges();
            this.ds.WriteXml(this.xmlFile);
        }

        /// <summary>
        /// Create the XML Database if not present
        /// </summary>
        private void CreateDefaultXmlData()
        {
            DataRow row = this.ds.Tables[CONFIG_TABLE].NewRow();

            row[this.dcConfigPrompt] = "T";
            row[this.dcConfigSynchronize] = "T";

            this.ds.Tables[CONFIG_TABLE].Rows.Add(row);
            this.ds.AcceptChanges();

            this.ds.WriteXml(this.xmlFile);
        }

        /// <summary>
        /// Define the XML Schema
        /// </summary>
        private void DefineSchema()
        {
            this.ds = new DataSet(DATA_SET);
            this.dtConfig = new DataTable(CONFIG_TABLE);
            this.dtAccounts = new DataTable(ACCOUNTS_TABLE);
            this.dtRecipients = new DataTable(RECIPIENTS_TABLE);

            this.dcConfigPrompt = new DataColumn("PROMPT", typeof(string));
            this.dcConfigSynchronize = new DataColumn("SYNCHRONIZE", typeof(string));

            this.dcAccountsAddress = new DataColumn("ADDRESS", typeof(string));
            this.dcRecipientsAddress = new DataColumn("ADDRESS", typeof(string));

            this.dtConfig.Columns.Add(this.dcConfigPrompt);
            this.dtConfig.Columns.Add(this.dcConfigSynchronize);

            this.dtAccounts.Columns.Add(this.dcAccountsAddress);
            this.dtRecipients.Columns.Add(this.dcRecipientsAddress);

            this.dtAccounts.PrimaryKey = new DataColumn[] { this.dcAccountsAddress };
            this.dtRecipients.PrimaryKey = new DataColumn[] { this.dcRecipientsAddress };

            this.ds.Tables.Add(this.dtConfig);
            this.ds.Tables.Add(this.dtAccounts);
            this.ds.Tables.Add(this.dtRecipients);
        }

        /// <summary>
        /// Checks whether the app should sync immediately
        /// </summary>
        /// <returns></returns>
        public bool CanSynchronize()
        {
            if (ds != null)
            {
                if (ds.Tables["CONFIG"] != null)
                {
                    if (ds.Tables["CONFIG"].Rows.Count > 0)
                    {
                        string result = Convert.ToString(ds.Tables["CONFIG"].Rows[0][dcConfigSynchronize]);

                        if (result.Equals("T"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether the app should prompt on SMS recieved
        /// </summary>
        /// <returns></returns>
        public bool CanPrompt()
        {
            if (ds != null)
            {
                if (ds.Tables["CONFIG"] != null)
                {
                    if (ds.Tables["CONFIG"].Rows.Count > 0)
                    {
                        string result = Convert.ToString(ds.Tables["CONFIG"].Rows[0][dcConfigPrompt]);

                        if (result.Equals("T"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set a Config parameter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="column"></param>
        public void SetConfig(ConfigValue value, ConfigColumn column)
        {
            try
            {
                if (ds != null)
                {
                    if (ds.Tables["CONFIG"] != null)
                    {
                        switch (column)
                        {
                            case ConfigColumn.Prompt:
                                ds.Tables["CONFIG"].Rows[0][dcConfigPrompt] = value.ToString();
                                break;

                            case ConfigColumn.Synchronize:
                                ds.Tables["CONFIG"].Rows[0][dcConfigSynchronize] = value.ToString();
                                break;
                        }
                    }
                }
            }
            catch { }
        }
    }
}