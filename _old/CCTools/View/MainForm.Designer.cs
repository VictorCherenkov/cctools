namespace CCTools.View
{
    partial class MainForm
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
            this.btnGetCs = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBrowseTCmd = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.cbViews = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnReloadViewLight = new System.Windows.Forms.Button();
            this.btnSwitchPostBuild = new System.Windows.Forms.Button();
            this.btnGetLatestCs = new System.Windows.Forms.Button();
            this.btnOmakeRelease = new System.Windows.Forms.Button();
            this.btnUpdateView = new System.Windows.Forms.Button();
            this.btnOmakeDebug = new System.Windows.Forms.Button();
            this.btnSetCs = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getConfigSpecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setConfigSpecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartEchoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vobSyncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbLog = new CCTools.View.ExtendedRichTextBox();
            this.makeViewEmptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetCs
            // 
            this.btnGetCs.Location = new System.Drawing.Point(3, 10);
            this.btnGetCs.Name = "btnGetCs";
            this.btnGetCs.Size = new System.Drawing.Size(101, 23);
            this.btnGetCs.TabIndex = 0;
            this.btnGetCs.Text = "Get config spec";
            this.btnGetCs.UseVisualStyleBackColor = true;
            this.btnGetCs.Click += new System.EventHandler(this.OnBtnGetCsClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnBrowseTCmd);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.cbViews);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(697, 39);
            this.panel1.TabIndex = 3;
            // 
            // btnBrowseTCmd
            // 
            this.btnBrowseTCmd.Location = new System.Drawing.Point(614, 10);
            this.btnBrowseTCmd.Name = "btnBrowseTCmd";
            this.btnBrowseTCmd.Size = new System.Drawing.Size(80, 23);
            this.btnBrowseTCmd.TabIndex = 4;
            this.btnBrowseTCmd.Text = "Browse Tcmd";
            this.btnBrowseTCmd.UseVisualStyleBackColor = true;
            this.btnBrowseTCmd.Click += new System.EventHandler(this.OnBtnBrowseTCmdClick);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(523, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(85, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.OnBtnBrowseClick);
            // 
            // cbViews
            // 
            this.cbViews.FormattingEnabled = true;
            this.cbViews.Location = new System.Drawing.Point(12, 12);
            this.cbViews.Name = "cbViews";
            this.cbViews.Size = new System.Drawing.Size(505, 21);
            this.cbViews.TabIndex = 2;
            this.cbViews.SelectedIndexChanged += new System.EventHandler(this.OnCbViewsSelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btnReloadViewLight);
            this.panel2.Controls.Add(this.btnSwitchPostBuild);
            this.panel2.Controls.Add(this.btnGetLatestCs);
            this.panel2.Controls.Add(this.btnOmakeRelease);
            this.panel2.Controls.Add(this.btnUpdateView);
            this.panel2.Controls.Add(this.btnOmakeDebug);
            this.panel2.Controls.Add(this.btnSetCs);
            this.panel2.Controls.Add(this.btnGetCs);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 463);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(697, 66);
            this.panel2.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(488, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Make Clean&&Release";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnButton2Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(488, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Make Clean&&Debug";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnButton1Click);
            // 
            // btnReloadViewLight
            // 
            this.btnReloadViewLight.Location = new System.Drawing.Point(242, 39);
            this.btnReloadViewLight.Name = "btnReloadViewLight";
            this.btnReloadViewLight.Size = new System.Drawing.Size(115, 23);
            this.btnReloadViewLight.TabIndex = 8;
            this.btnReloadViewLight.Text = "Reload View (Light)";
            this.btnReloadViewLight.UseVisualStyleBackColor = true;
            this.btnReloadViewLight.Click += new System.EventHandler(this.OnBtnReloadViewLightClick);
            // 
            // btnSwitchPostBuild
            // 
            this.btnSwitchPostBuild.Location = new System.Drawing.Point(110, 39);
            this.btnSwitchPostBuild.Name = "btnSwitchPostBuild";
            this.btnSwitchPostBuild.Size = new System.Drawing.Size(126, 23);
            this.btnSwitchPostBuild.TabIndex = 7;
            this.btnSwitchPostBuild.Text = "DisablePostBuild";
            this.btnSwitchPostBuild.UseVisualStyleBackColor = true;
            this.btnSwitchPostBuild.Click += new System.EventHandler(this.OnBtnSwitchPostBuildClick);
            // 
            // btnGetLatestCs
            // 
            this.btnGetLatestCs.Location = new System.Drawing.Point(110, 10);
            this.btnGetLatestCs.Name = "btnGetLatestCs";
            this.btnGetLatestCs.Size = new System.Drawing.Size(126, 23);
            this.btnGetLatestCs.TabIndex = 6;
            this.btnGetLatestCs.Text = "Get Latest Config Spec";
            this.btnGetLatestCs.UseVisualStyleBackColor = true;
            this.btnGetLatestCs.Click += new System.EventHandler(this.OnBtnGetLatestCsClick);
            // 
            // btnOmakeRelease
            // 
            this.btnOmakeRelease.Location = new System.Drawing.Point(391, 39);
            this.btnOmakeRelease.Name = "btnOmakeRelease";
            this.btnOmakeRelease.Size = new System.Drawing.Size(91, 23);
            this.btnOmakeRelease.TabIndex = 5;
            this.btnOmakeRelease.Text = "Make Release";
            this.btnOmakeRelease.UseVisualStyleBackColor = true;
            this.btnOmakeRelease.Click += new System.EventHandler(this.OnBtnOmakeReleaseClick);
            // 
            // btnUpdateView
            // 
            this.btnUpdateView.Location = new System.Drawing.Point(242, 10);
            this.btnUpdateView.Name = "btnUpdateView";
            this.btnUpdateView.Size = new System.Drawing.Size(115, 23);
            this.btnUpdateView.TabIndex = 3;
            this.btnUpdateView.Text = "Update";
            this.btnUpdateView.UseVisualStyleBackColor = true;
            this.btnUpdateView.Click += new System.EventHandler(this.OnBtnUpdateClick);
            // 
            // btnOmakeDebug
            // 
            this.btnOmakeDebug.Location = new System.Drawing.Point(391, 10);
            this.btnOmakeDebug.Name = "btnOmakeDebug";
            this.btnOmakeDebug.Size = new System.Drawing.Size(91, 23);
            this.btnOmakeDebug.TabIndex = 2;
            this.btnOmakeDebug.Text = "Make Debug";
            this.btnOmakeDebug.UseVisualStyleBackColor = true;
            this.btnOmakeDebug.Click += new System.EventHandler(this.OnBtnOmakeDebugClick);
            // 
            // btnSetCs
            // 
            this.btnSetCs.Location = new System.Drawing.Point(3, 39);
            this.btnSetCs.Name = "btnSetCs";
            this.btnSetCs.Size = new System.Drawing.Size(101, 23);
            this.btnSetCs.TabIndex = 1;
            this.btnSetCs.Text = "Set Config spec";
            this.btnSetCs.UseVisualStyleBackColor = true;
            this.btnSetCs.Click += new System.EventHandler(this.OnBtnSetCsClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sRToolStripMenuItem,
            this.otherToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(697, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sRToolStripMenuItem
            // 
            this.sRToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createViewToolStripMenuItem,
            this.getConfigSpecToolStripMenuItem,
            this.setConfigSpecToolStripMenuItem,
            this.updateToolStripMenuItem,
            this.makeDebugToolStripMenuItem,
            this.makeReleaseToolStripMenuItem,
            this.makeViewEmptyToolStripMenuItem});
            this.sRToolStripMenuItem.Name = "sRToolStripMenuItem";
            this.sRToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.sRToolStripMenuItem.Text = "View";
            // 
            // createViewToolStripMenuItem
            // 
            this.createViewToolStripMenuItem.Name = "createViewToolStripMenuItem";
            this.createViewToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.createViewToolStripMenuItem.Text = "Create View";
            this.createViewToolStripMenuItem.Click += new System.EventHandler(this.CreateViewToolStripMenuItemClick);
            // 
            // getConfigSpecToolStripMenuItem
            // 
            this.getConfigSpecToolStripMenuItem.Name = "getConfigSpecToolStripMenuItem";
            this.getConfigSpecToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.getConfigSpecToolStripMenuItem.Text = "Get config spec";
            // 
            // setConfigSpecToolStripMenuItem
            // 
            this.setConfigSpecToolStripMenuItem.Name = "setConfigSpecToolStripMenuItem";
            this.setConfigSpecToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.setConfigSpecToolStripMenuItem.Text = "Set config spec";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.updateToolStripMenuItem.Text = "Update";
            // 
            // makeDebugToolStripMenuItem
            // 
            this.makeDebugToolStripMenuItem.Name = "makeDebugToolStripMenuItem";
            this.makeDebugToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.makeDebugToolStripMenuItem.Text = "Make debug";
            // 
            // makeReleaseToolStripMenuItem
            // 
            this.makeReleaseToolStripMenuItem.Name = "makeReleaseToolStripMenuItem";
            this.makeReleaseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.makeReleaseToolStripMenuItem.Text = "Make release";
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restartEchoToolStripMenuItem,
            this.vobSyncToolStripMenuItem});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.otherToolStripMenuItem.Text = "Other";
            // 
            // restartEchoToolStripMenuItem
            // 
            this.restartEchoToolStripMenuItem.Name = "restartEchoToolStripMenuItem";
            this.restartEchoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.restartEchoToolStripMenuItem.Text = "Restart Echo";
            this.restartEchoToolStripMenuItem.Click += new System.EventHandler(this.OnRestartEchoToolStripMenuItemClick);
            // 
            // vobSyncToolStripMenuItem
            // 
            this.vobSyncToolStripMenuItem.Name = "vobSyncToolStripMenuItem";
            this.vobSyncToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.vobSyncToolStripMenuItem.Text = "Vob sync";
            this.vobSyncToolStripMenuItem.Click += new System.EventHandler(this.OnVobSyncToolStripMenuItemClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExitToolStripMenuItemClick);
            // 
            // rtbLog
            // 
            this.rtbLog.BufferDelayMsec = 5D;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 63);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(697, 400);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // makeViewEmptyToolStripMenuItem
            // 
            this.makeViewEmptyToolStripMenuItem.Name = "makeViewEmptyToolStripMenuItem";
            this.makeViewEmptyToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.makeViewEmptyToolStripMenuItem.Text = "Make View Empty";
            this.makeViewEmptyToolStripMenuItem.Click += new System.EventHandler(this.makeViewEmptyToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 529);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "CCTools - Idle";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetCs;
        private ExtendedRichTextBox rtbLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSetCs;
        private System.Windows.Forms.Button btnOmakeDebug;
        private System.Windows.Forms.ComboBox cbViews;
        private System.Windows.Forms.Button btnUpdateView;
        private System.Windows.Forms.Button btnOmakeRelease;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnBrowseTCmd;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getConfigSpecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setConfigSpecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeReleaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnGetLatestCs;
        private System.Windows.Forms.Button btnSwitchPostBuild;
        private System.Windows.Forms.ToolStripMenuItem restartEchoToolStripMenuItem;
        private System.Windows.Forms.Button btnReloadViewLight;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem vobSyncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeViewEmptyToolStripMenuItem;
    }
}