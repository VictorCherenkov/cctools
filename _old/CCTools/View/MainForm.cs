using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CCTools.View
{
    public partial class MainForm : Form, IView
    {
        private readonly MainFormPresenter m_presenter;
        private readonly IEnumerable<Control> m_lockableControls;

        public MainForm()
        {
            InitializeComponent();
            m_lockableControls = new Control[]
            {
                btnGetCs, 
                btnSetCs, 
                btnUpdateView,                                                          
                btnOmakeDebug, 
                btnOmakeRelease,
                btnGetLatestCs,
                btnSwitchPostBuild,
                btnReloadViewLight,
                button1,
                button2,
                cbViews,
                rtbLog
            };
            m_presenter = new MainFormPresenter(this, new RtbLoggerAdapter(rtbLog));            
        }

        void IView.SetTitle(string value)
        {
            Text = value;
        }
        void IView.LockUI()
        {
            ExecuteLockAction(LockAction.Lock);
        }
        void IView.UnlockUI()
        {
            ExecuteLockAction(LockAction.Unlock);
        }
        void IView.SetPostBuildStatusText(string value)
        {
            btnSwitchPostBuild.Text = value;
        }
        int IView.SelectedViewIndex
        {
            get { return cbViews.SelectedIndex; }
            set { cbViews.SelectedIndex = value; }
        }
        IList<string> IView.Views
        {
            get { return (IList<string>)cbViews.DataSource; }
            set { cbViews.DataSource = value; }
        }
        string[] IView.GetLogLines()
        {
            return rtbLog.Lines;
        }
        string IView.GetLogText()
        {
            return rtbLog.Text;
        }
        void IView.SetLogText(string[] lines)
        {
            rtbLog.SetText(lines);
        }
        void IView.SetLogText(string text)
        {
            rtbLog.SetText(text);
        }
        void IView.ClearLogger()
        {
            rtbLog.Clear();
        }
        void IView.FlushLogger()
        {
            rtbLog.Flush();
        }

        private void ExecuteLockAction(LockAction action)
        {
            ExecuteLockAction(action, Controls);
        }
        private void ExecuteLockAction(LockAction action, Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c.HasChildren)
                {
                    ExecuteLockAction(action, c.Controls);
                }
                if (m_lockableControls.Contains(c))
                {
                    c.Enabled = action == LockAction.Unlock ? true : false;
                }
            }
        }        

        private void OnFormLoad(object sender, EventArgs e)
        {
            m_presenter.OnLoad();
        }                   
        private void OnBtnGetCsClick(object sender, EventArgs e)
        {
            m_presenter.GetConfigSpec();
        }
        private void OnBtnSetCsClick(object sender, EventArgs e)
        {
            m_presenter.SetConfigSpec();            
        }
        private void OnBtnUpdateClick(object sender, EventArgs e)
        {
            m_presenter.UpdateCurrentView();
        }
        private void OnBtnOmakeDebugClick(object sender, EventArgs e)
        {
            m_presenter.MakeDebug();            
        }
        private void OnBtnOmakeReleaseClick(object sender, EventArgs e)
        {
            m_presenter.MakeRelease();
        }
        private void OnBtnBrowseClick(object sender, EventArgs e)
        {
            m_presenter.Browse();            
        }
        private void OnBtnBrowseTCmdClick(object sender, EventArgs e)
        {
            m_presenter.BrowseTotalCommander();            
        }
        private void OnExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }
        private void OnCbViewsSelectedIndexChanged(object sender, EventArgs e)
        {
            m_presenter.OnSelectedViewChanged();
        }
        private void OnBtnGetLatestCsClick(object sender, EventArgs e)
        {
            m_presenter.GetLatestConfigSpec();
        }
        private void OnBtnSwitchPostBuildClick(object sender, EventArgs e)
        {
            m_presenter.SwitchTestPostBuildStatus();            
        }
        private void OnRestartEchoToolStripMenuItemClick(object sender, EventArgs e)
        {
            m_presenter.RestartEcho();
        }
        private void OnBtnReloadViewLightClick(object sender, EventArgs e)
        {
            m_presenter.ReloadViewLight();
        }
        private void OnButton1Click(object sender, EventArgs e)
        {
            m_presenter.MakeCleanAndDebug();
        }
        private void OnButton2Click(object sender, EventArgs e)
        {
            m_presenter.MakeCleanAndRelease();
        }
        private void OnVobSyncToolStripMenuItemClick(object sender, EventArgs e)
        {
            m_presenter.OpenVobSync();
        }        

        private enum LockAction { Lock, Unlock}

        private void CreateViewToolStripMenuItemClick(object sender, EventArgs e)
        {
            m_presenter.CreateSrView();
        }

        private void makeViewEmptyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_presenter.WipeView();
        }
    }    
}