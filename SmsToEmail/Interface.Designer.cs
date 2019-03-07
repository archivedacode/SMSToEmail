namespace SmsToEmail
{
    partial class Interface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.MenuExit = new System.Windows.Forms.MenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpAccounts = new System.Windows.Forms.TabPage();
            this.PnlAccounts = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tpRecipients = new System.Windows.Forms.TabPage();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnAddRecipient = new System.Windows.Forms.Button();
            this.TxtBoxRecipient = new System.Windows.Forms.TextBox();
            this.LvRecipients = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.tpOptions = new System.Windows.Forms.TabPage();
            this.ChkBoxSync = new System.Windows.Forms.CheckBox();
            this.ChkBoxPrompt = new System.Windows.Forms.CheckBox();
            this.ChkBoxEnable = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tpRegister = new System.Windows.Forms.TabPage();
            this.TxtSerial1 = new System.Windows.Forms.TextBox();
            this.BtnActivate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cxMenuRecipients = new System.Windows.Forms.ContextMenu();
            this.MenuDeleteRecipient = new System.Windows.Forms.MenuItem();
            this.tabControl.SuspendLayout();
            this.tpAccounts.SuspendLayout();
            this.tpRecipients.SuspendLayout();
            this.tpOptions.SuspendLayout();
            this.tpRegister.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.MenuExit);
            // 
            // MenuExit
            // 
            this.MenuExit.Text = "Exit";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpAccounts);
            this.tabControl.Controls.Add(this.tpRecipients);
            this.tabControl.Controls.Add(this.tpOptions);
            this.tabControl.Controls.Add(this.tpRegister);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(240, 268);
            this.tabControl.TabIndex = 0;
            // 
            // tpAccounts
            // 
            this.tpAccounts.Controls.Add(this.PnlAccounts);
            this.tpAccounts.Controls.Add(this.label1);
            this.tpAccounts.Location = new System.Drawing.Point(0, 0);
            this.tpAccounts.Name = "tpAccounts";
            this.tpAccounts.Size = new System.Drawing.Size(240, 245);
            this.tpAccounts.Text = "Accounts";
            // 
            // PnlAccounts
            // 
            this.PnlAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlAccounts.AutoScroll = true;
            this.PnlAccounts.BackColor = System.Drawing.Color.LightGray;
            this.PnlAccounts.Location = new System.Drawing.Point(7, 27);
            this.PnlAccounts.Name = "PnlAccounts";
            this.PnlAccounts.Size = new System.Drawing.Size(217, 205);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 18);
            this.label1.Text = "1. Select device Email Account :-";
            // 
            // tpRecipients
            // 
            this.tpRecipients.AutoScroll = true;
            this.tpRecipients.Controls.Add(this.BtnClear);
            this.tpRecipients.Controls.Add(this.BtnDelete);
            this.tpRecipients.Controls.Add(this.label2);
            this.tpRecipients.Controls.Add(this.BtnAddRecipient);
            this.tpRecipients.Controls.Add(this.TxtBoxRecipient);
            this.tpRecipients.Controls.Add(this.LvRecipients);
            this.tpRecipients.Location = new System.Drawing.Point(0, 0);
            this.tpRecipients.Name = "tpRecipients";
            this.tpRecipients.Size = new System.Drawing.Size(232, 242);
            this.tpRecipients.Text = "Recipients";
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(74, 68);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(72, 20);
            this.BtnClear.TabIndex = 5;
            this.BtnClear.Text = "Clear";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(152, 222);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(72, 20);
            this.BtnDelete.TabIndex = 3;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(7, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(226, 18);
            this.label2.Text = "2. Add Recipients to forward SMS to :-";
            // 
            // BtnAddRecipient
            // 
            this.BtnAddRecipient.Location = new System.Drawing.Point(152, 68);
            this.BtnAddRecipient.Name = "BtnAddRecipient";
            this.BtnAddRecipient.Size = new System.Drawing.Size(72, 20);
            this.BtnAddRecipient.TabIndex = 2;
            this.BtnAddRecipient.Text = "Add";
            this.BtnAddRecipient.Click += new System.EventHandler(this.BtnAddRecipient_Click);
            // 
            // TxtBoxRecipient
            // 
            this.TxtBoxRecipient.Location = new System.Drawing.Point(7, 27);
            this.TxtBoxRecipient.MaxLength = 320;
            this.TxtBoxRecipient.Multiline = true;
            this.TxtBoxRecipient.Name = "TxtBoxRecipient";
            this.TxtBoxRecipient.Size = new System.Drawing.Size(217, 35);
            this.TxtBoxRecipient.TabIndex = 1;
            // 
            // LvRecipients
            // 
            this.LvRecipients.Columns.Add(this.columnHeader1);
            this.LvRecipients.Location = new System.Drawing.Point(7, 94);
            this.LvRecipients.Name = "LvRecipients";
            this.LvRecipients.Size = new System.Drawing.Size(217, 122);
            this.LvRecipients.TabIndex = 0;
            this.LvRecipients.View = System.Windows.Forms.View.Details;
            this.LvRecipients.ItemActivate += new System.EventHandler(this.LvRecipients_ItemActivate);
            this.LvRecipients.SelectedIndexChanged += new System.EventHandler(this.LvRecipients_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Recipients";
            this.columnHeader1.Width = 226;
            // 
            // tpOptions
            // 
            this.tpOptions.Controls.Add(this.ChkBoxSync);
            this.tpOptions.Controls.Add(this.ChkBoxPrompt);
            this.tpOptions.Controls.Add(this.ChkBoxEnable);
            this.tpOptions.Controls.Add(this.label3);
            this.tpOptions.Location = new System.Drawing.Point(0, 0);
            this.tpOptions.Name = "tpOptions";
            this.tpOptions.Size = new System.Drawing.Size(232, 242);
            this.tpOptions.Text = "Options";
            // 
            // ChkBoxSync
            // 
            this.ChkBoxSync.Location = new System.Drawing.Point(7, 81);
            this.ChkBoxSync.Name = "ChkBoxSync";
            this.ChkBoxSync.Size = new System.Drawing.Size(208, 20);
            this.ChkBoxSync.TabIndex = 8;
            this.ChkBoxSync.Text = "Synchronize Email immediately";
            this.ChkBoxSync.CheckStateChanged += new System.EventHandler(this.ChkBoxSync_CheckStateChanged);
            // 
            // ChkBoxPrompt
            // 
            this.ChkBoxPrompt.Location = new System.Drawing.Point(7, 55);
            this.ChkBoxPrompt.Name = "ChkBoxPrompt";
            this.ChkBoxPrompt.Size = new System.Drawing.Size(208, 20);
            this.ChkBoxPrompt.TabIndex = 7;
            this.ChkBoxPrompt.Text = "Prompt when SMS received";
            this.ChkBoxPrompt.CheckStateChanged += new System.EventHandler(this.ChkBoxPrompt_CheckStateChanged);
            // 
            // ChkBoxEnable
            // 
            this.ChkBoxEnable.Location = new System.Drawing.Point(7, 29);
            this.ChkBoxEnable.Name = "ChkBoxEnable";
            this.ChkBoxEnable.Size = new System.Drawing.Size(208, 20);
            this.ChkBoxEnable.TabIndex = 6;
            this.ChkBoxEnable.Text = "Enable SMS To Email";
            this.ChkBoxEnable.CheckStateChanged += new System.EventHandler(this.ChkBoxEnable_CheckStateChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(7, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(208, 20);
            this.label3.Text = "3. Configure Options :-";
            // 
            // tpRegister
            // 
            this.tpRegister.Controls.Add(this.TxtSerial1);
            this.tpRegister.Controls.Add(this.BtnActivate);
            this.tpRegister.Controls.Add(this.label4);
            this.tpRegister.Location = new System.Drawing.Point(0, 0);
            this.tpRegister.Name = "tpRegister";
            this.tpRegister.Size = new System.Drawing.Size(240, 245);
            this.tpRegister.Text = "Activate";
            // 
            // TxtSerial1
            // 
            this.TxtSerial1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.TxtSerial1.Location = new System.Drawing.Point(7, 30);
            this.TxtSerial1.MaxLength = 32;
            this.TxtSerial1.Name = "TxtSerial1";
            this.TxtSerial1.Size = new System.Drawing.Size(208, 21);
            this.TxtSerial1.TabIndex = 3;
            // 
            // BtnActivate
            // 
            this.BtnActivate.Location = new System.Drawing.Point(143, 68);
            this.BtnActivate.Name = "BtnActivate";
            this.BtnActivate.Size = new System.Drawing.Size(72, 20);
            this.BtnActivate.TabIndex = 2;
            this.BtnActivate.Text = "Activate";
            this.BtnActivate.Click += new System.EventHandler(this.BtnActivate_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(7, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 20);
            this.label4.Text = "Enter serial code :-";
            // 
            // cxMenuRecipients
            // 
            this.cxMenuRecipients.MenuItems.Add(this.MenuDeleteRecipient);
            // 
            // MenuDeleteRecipient
            // 
            this.MenuDeleteRecipient.Text = "Delete";
            this.MenuDeleteRecipient.Click += new System.EventHandler(this.MenuDeleteRecipient_Click);
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Interface";
            this.Text = "SMS To Email";
            this.Load += new System.EventHandler(this.Interface_Load);
            this.tabControl.ResumeLayout(false);
            this.tpAccounts.ResumeLayout(false);
            this.tpRecipients.ResumeLayout(false);
            this.tpOptions.ResumeLayout(false);
            this.tpRegister.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem MenuExit;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpAccounts;
        private System.Windows.Forms.TabPage tpRecipients;
        private System.Windows.Forms.ListView LvRecipients;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnAddRecipient;
        private System.Windows.Forms.TextBox TxtBoxRecipient;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenu cxMenuRecipients;
        private System.Windows.Forms.MenuItem MenuDeleteRecipient;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel PnlAccounts;
        private System.Windows.Forms.TabPage tpOptions;
        private System.Windows.Forms.CheckBox ChkBoxSync;
        private System.Windows.Forms.CheckBox ChkBoxPrompt;
        private System.Windows.Forms.CheckBox ChkBoxEnable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.TabPage tpRegister;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnActivate;
        private System.Windows.Forms.TextBox TxtSerial1;
    }
}