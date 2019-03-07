//M----------------------------------------------------------------------------
//M   MODULE HEADER
//M----------------------------------------------------------------------------
//M
//M        File Name : Interace.cs   (C) Carbon Software 2009, 2010
//M
//M----------------------------------------------------------------------------
//M   MODULE CHANGE HISTORY
//M----------------------------------------------------------------------------
//M
//M        DATE         BY     DESCRIPTION
//M        ----         --     -----------
//M
//M        31-01-2010   DER    Added licence feature
//M
//M        02-01-2010   DER    Removed IsEnabled DbManager check
//M
//M        16-10-2009   DER    Added IsEmail() Email validation method
//M
//M        27-09-2009   DER    Added internal DatabaseManager class
//M
//M        07-09-2009   DER    Added call to make SmsEngine invisible via 
//M                            changing FileAttributes property to 'Hidden'
//M
//M        28-08-2009   DER    Version
//M
//M----------------------------------------------------------------------------
//M   MODULE DESCRIPTION
//M----------------------------------------------------------------------------
//M
//M        Form class for the Coniguration application
//M
//M----------------------------------------------------------------------------


using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.WindowsMobile.PocketOutlook;
using Microsoft.WindowsMobile.PocketOutlook.MessageInterception;
using Microsoft.Win32;

namespace SmsToEmail
{
    public partial class Interface : Form
    {
        private bool bRecipientSelected = false;
        private DatabaseManager dbManager = null;
        private MessageInterceptor smsInterceptor = null;
        private const string AppRegisterName = "OuchSmsToEmail";

        private ArrayList serials = null;

        public Interface()
        {
            InitializeComponent();

            this.HideEngine();

            dbManager = new DatabaseManager(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
        }

        private void HideEngine()
        {
            try
            {
                // Get the current working directory
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

                // Get the SmsEngine.exe full path and file name
                string smsEngineFile = Path.Combine(dir, "SmsEngine.exe");

                // Make sure SmsEngine exists...
                if (File.Exists(smsEngineFile))
                {
                    FileInfo fi = new FileInfo(smsEngineFile);

                    fi.Attributes |= FileAttributes.Hidden;
                }
            }
            catch { }
        }

        private void SaveSettings()
        {
            dbManager.SaveXmlData();
        }

        private void ApplyConfig()
        {
            if (dbManager.CanPrompt())
            {
                this.ChkBoxPrompt.Checked = true;
            }
            else
            {
                this.ChkBoxPrompt.Checked = false;
            }

            if (MessageInterceptor.IsApplicationLauncherEnabled(AppRegisterName))
            {
                this.ChkBoxEnable.Checked = true;
            }
            else
            {
                this.ChkBoxEnable.Checked = false;
            }

            if (dbManager.CanSynchronize())
            {
                this.ChkBoxSync.Checked = true;
            }
            else
            {
                this.ChkBoxSync.Checked = false;
            }

            RegistryKey key = Registry.LocalMachine.OpenSubKey("Carbon Software Ltd");

            if (key != null)
            {
                string result = Convert.ToString(key.GetValue("serial"));

                if ( (result.Trim().Length > 0) && (this.serials.Contains(result.Trim())) )
                {
                    this.tpAccounts.Show();
                    this.tpRecipients.Show();
                    this.tpOptions.Show();

                    try
                    {
                        this.tabControl.TabPages.RemoveAt(3);
                    }
                    catch
                    {
                        this.tpRegister.Hide();
                    }
                    finally
                    {
                        this.tabControl.SelectedIndex = 0;
                    }
                }
                else
                {
                    this.tpAccounts.Hide();
                    this.tpRecipients.Hide();
                    this.tpOptions.Hide();

                    this.tabControl.SelectedIndex = 3;
                }
            }
            else
            {
                this.tpAccounts.Hide();
                this.tpRecipients.Hide();
                this.tpOptions.Hide();
            }
        }

        private void LoadAccounts()
        {
            OutlookSession session = new OutlookSession();

            EmailAccountCollection collection = session.EmailAccounts;

            if (collection.Count > 0)
            {
                int g = 0;

                for (int i = 0; i < collection.Count; i++)
                {
                    EmailAccount account = collection[i];

                    if (!account.Name.ToLower().Equals("activesync"))
                    {
                        RadioButton rb = new RadioButton();

                        if (dbManager.ACCOUNTS.Rows.Count > 0)
                        {
                            rb.Checked = dbManager.ACCOUNTS.Rows.Contains(account.Name);
                        }

                        rb.Text = account.Name;
                        rb.Top = 5 + (g * 20);
                        rb.Left = 4;
                        rb.Width = PnlAccounts.Width - 20;

                        rb.Click += new EventHandler(rb_Click);

                        g++;

                        this.PnlAccounts.Controls.Add(rb);
                    }
                }
                this.PnlAccounts.Invalidate();
            }
        }

        private void LoadRecipients()
        {
            if (this.dbManager.RECIPIENTS.Rows.Count > 0)
            {
                foreach (DataRow row in this.dbManager.RECIPIENTS.Rows)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = Convert.ToString(row["ADDRESS"]);
                    this.LvRecipients.Items.Add(item);
                }

                this.LvRecipients.Invalidate();
            }
        }
        
        private void MenuExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you ready to Save the settings and Exit the application?",
                                "Save and Exit",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                this.SaveSettings();

                this.Close();
            }
            else
            {
                this.Menu = this.mainMenu1;
            }
        }

        private void Interface_Load(object sender, EventArgs e)
        {
            this.LoadSerials();

            this.LoadAccounts();

            this.LoadRecipients();

            this.ApplyConfig();
        }

        private void LoadSerials()
        {
            this.serials = new ArrayList();

            this.serials.Add("45F7-9678-B428-4662");
            this.serials.Add("018C-6D16-42A2-45F8");
            this.serials.Add("22D2-F30D-12C6-4FB4");
            this.serials.Add("E951-8E85-14AB-464D");
            this.serials.Add("02FC-F36A-0F9F-43B0");
            this.serials.Add("E3E2-7773-8DC7-4BAB");
            this.serials.Add("76CC-7F1D-E07D-4179");
            this.serials.Add("39F1-8C05-4BB1-45BE");
            this.serials.Add("026F-E180-E0D6-4D50");
            this.serials.Add("12E8-FC58-DBB1-4258");
            this.serials.Add("F98E-BC3A-E8D5-4268");
            this.serials.Add("3373-60DB-8696-4EB7");
            this.serials.Add("0331-118D-705E-444F");
            this.serials.Add("9E0E-A2F2-1B07-4636");
            this.serials.Add("0A2E-BBB4-CFA8-4D9D");
            this.serials.Add("397E-B546-6D51-4F48");
            this.serials.Add("FAAE-EA25-6AA1-46D0");
            this.serials.Add("E29F-29C6-1A08-4BC1");
            this.serials.Add("BCFD-A9F7-92DF-42DF");
            this.serials.Add("8A72-2C38-87AA-435B");
        }

        private void rb_Click(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null)
            {
                dbManager.SetAccountName(rb.Text);
            }
        }

        public static bool IsEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            this.DeleteRecipient();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.TxtBoxRecipient.Text = "";
        }

        private void BtnAddRecipient_Click(object sender, EventArgs e)
        {
            if (TxtBoxRecipient.Text.Trim().Length > 0)
            {
                ListViewItem item = new ListViewItem();
                item.Text = TxtBoxRecipient.Text.Trim();

                if (LvRecipients.Items.Count > 0)
                {
                    bool bContains = false;

                    for (int j = 0; j < LvRecipients.Items.Count; j++)
                    {
                        if (LvRecipients.Items[j].Text.Equals(item.Text))
                        {
                            bContains = true;
                            break;
                        }
                    }

                    if (!bContains)
                    {
                        // Check if the Email address is valid
                        if (!IsEmail(item.Text))
                        {
                            if (MessageBox.Show("This address does not appear to be valid, are you sure you wish to enter it?",
                                                "Email Validation",
                                                MessageBoxButtons.OKCancel,
                                                MessageBoxIcon.Exclamation,
                                                MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                            {
                                return;
                            }
                        }

                        dbManager.AddRecipient(item.Text);
                        LvRecipients.Items.Add(item);
                        LvRecipients.Invalidate();
                        this.Menu = this.mainMenu1;
                    }
                }
                else
                {
                    // Check if the Email address is valid
                    if (!IsEmail(item.Text))
                    {
                        if (MessageBox.Show("This address does not appear to be valid, are you sure you wish to enter it?",
                                            "Email Validation",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Exclamation,
                                            MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    dbManager.AddRecipient(item.Text);
                    LvRecipients.Items.Add(item);
                    LvRecipients.Invalidate();
                    this.Menu = this.mainMenu1;
                }
            }
            else
            {
                MessageBox.Show("Please enter a recipient address");
                this.Menu = this.mainMenu1;
            }
        }

        private void MenuDeleteRecipient_Click(object sender, EventArgs e)
        {
            this.DeleteRecipient();
        }

        private void DeleteRecipient()
        {
            if (LvRecipients.Items.Count > 0)
            {
                if (bRecipientSelected)
                {
                    if (MessageBox.Show("Are you sure you want to delete this Recipient?",
                                        "Delete",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        string address = Convert.ToString(LvRecipients.FocusedItem.Text.Trim());

                        dbManager.DeleteRecipient(address);

                        LvRecipients.Items.RemoveAt(LvRecipients.FocusedItem.Index);
                        LvRecipients.Invalidate();

                    }

                    this.Menu = this.mainMenu1;
                }
                else
                {
                    MessageBox.Show("Please select a Recipient");
                    this.Menu = this.mainMenu1;
                    return;
                }
            }
        }

        private void LvRecipients_ItemActivate(object sender, EventArgs e)
        {
            cxMenuRecipients.Show(LvRecipients, new System.Drawing.Point(20, 20));
        }
        
        private void LvRecipients_SelectedIndexChanged(object sender, EventArgs e)
        {
            bRecipientSelected = true;
        }

        private void ChkBoxPrompt_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.ChkBoxPrompt.Checked)
            {
                dbManager.SetConfig(DatabaseManager.ConfigValue.T,
                                    DatabaseManager.ConfigColumn.Prompt);
            }
            else
            {
                dbManager.SetConfig(DatabaseManager.ConfigValue.F,
                                    DatabaseManager.ConfigColumn.Prompt);
            }
        }

        private void ChkBoxSync_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.ChkBoxSync.Checked)
            {
                dbManager.SetConfig(DatabaseManager.ConfigValue.T,
                                    DatabaseManager.ConfigColumn.Synchronize);
            }
            else
            {
                dbManager.SetConfig(DatabaseManager.ConfigValue.F,
                                    DatabaseManager.ConfigColumn.Synchronize);
            }
        }

        private void ChkBoxEnable_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.ChkBoxEnable.Checked)
            {
                this.EnableInterceptor();
            }
            else
            {
                this.DisableInterceptor();
            }
        }

        private void DisableInterceptor()
        {
            // Make sure we're registered....
            if (MessageInterceptor.IsApplicationLauncherEnabled(AppRegisterName))
            {
                // instance the Interceptor
                smsInterceptor = new MessageInterceptor(AppRegisterName);

                try
                {
                    // unregister the interceptor.
                    smsInterceptor.DisableApplicationLauncher();
                }
                catch { }
                finally
                {
                    smsInterceptor.Dispose();
                }
            }
        }

        private void EnableInterceptor()
        {
            // Make sure we're unregistered....
            if (!MessageInterceptor.IsApplicationLauncherEnabled(AppRegisterName))
            {
                // instance the Interceptor
                smsInterceptor = new MessageInterceptor();

                try
                {
                    // register the interceptor.
                    string smsEngine = Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("SmsToEmail.exe", "SmsEngine.exe");
                    smsInterceptor.MessageCondition.Property = MessageProperty.MessageClass;
                    smsInterceptor.InterceptionAction = InterceptionAction.Notify;
                    smsInterceptor.MessageCondition.CaseSensitive = false;
                    smsInterceptor.MessageCondition.ComparisonType = MessagePropertyComparisonType.Contains;
                    smsInterceptor.MessageCondition.ComparisonValue = "SMS";
                    smsInterceptor.EnableApplicationLauncher(AppRegisterName,
                                                             smsEngine,
                                                             "1");
                }
                catch
                {
                    MessageBox.Show("Unable to register MessageInterceptor, please contact Carbon Software Tech-Support");
                }
                finally
                {
                    smsInterceptor.Dispose();
                }
            }
        }

        private void BtnActivate_Click(object sender, EventArgs e)
        {
            if (this.TxtSerial1.Text.Trim().Length <= 0)
            {
                MessageBox.Show("Please enter a serial code");
            }
            else
            {
                if (this.serials.Contains(this.TxtSerial1.Text.Trim()))
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("Carbon Software Ltd", true);

                    if (key != null)
                    {
                        key.SetValue("serial", this.TxtSerial1.Text.Trim());
                        key.Close();

                        MessageBox.Show("SMS To Email successfully activated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                        this.tpAccounts.Show();
                        this.tpRecipients.Show();
                        this.tpOptions.Show();
                        this.tabControl.TabPages.RemoveAt(3);

                        this.tabControl.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid serial code entered", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
            }
        }
    }

    internal class DatabaseManager
    {
        private const string DATA_SET = "DATA";
        private const string FILE_NAME = "DATA.xml";

        private const string CONFIG_TABLE = "CONFIG";
        private const string ACCOUNTS_TABLE = "ACCOUNTS";
        private const string RECIPIENTS_TABLE = "RECIPIENTS";

        private string appPath = "";
        private string xmlFile = "";

        private DataSet ds = null;
        private DataTable dtConfig = null;
        private DataTable dtAccounts = null;
        private DataTable dtRecipients = null;

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
        /// Write the XML data file
        /// </summary>
        public void SaveXmlData()
        {
            this.ds.AcceptChanges();
            this.ds.WriteXml(this.xmlFile);
        }

        /// <summary>
        /// Add a Recipient
        /// </summary>
        /// <param name="address"></param>
        public void AddRecipient(string address)
        {
            try
            {
                if (this.ds != null)
                {
                    if (this.ds.Tables[RECIPIENTS_TABLE] != null)
                    {
                        DataRow row = this.ds.Tables[RECIPIENTS_TABLE].NewRow();
                        row[this.dcRecipientsAddress] = address;

                        this.ds.Tables[RECIPIENTS_TABLE].Rows.Add(row);

                        this.ds.AcceptChanges();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Sets the one and only account name
        /// </summary>
        public void SetAccountName(string address)
        {
            try
            {
                if (this.ds != null)
                {
                    if (this.ds.Tables[ACCOUNTS_TABLE] != null)
                    {
                        if (this.ds.Tables[ACCOUNTS_TABLE].Rows.Count > 0)
                        {
                            this.ds.Tables[ACCOUNTS_TABLE].Rows[0][this.dcAccountsAddress] = address;
                        }
                        else
                        {
                            DataRow row = this.ds.Tables[ACCOUNTS_TABLE].NewRow();
                            row[this.dcAccountsAddress] = address;

                            this.ds.Tables[ACCOUNTS_TABLE].Rows.Add(row);

                            this.ds.AcceptChanges();
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Delete a Recipient from the Recipients table
        /// </summary>
        /// <param name="address"></param>
        public void DeleteRecipient(string address)
        {
            if (this.ds != null)
            {
                if (this.ds.Tables[RECIPIENTS_TABLE] != null)
                {
                    DataRow[] rows = this.ds.Tables[RECIPIENTS_TABLE].Select("ADDRESS='" + address + "'");

                    if (rows.Length > 0)
                    {
                        this.ds.Tables[RECIPIENTS_TABLE].Rows.Remove(rows[0]);
                    }
                }
            }
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