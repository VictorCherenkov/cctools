using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using CCTools.Model;
using CCTools.Platform;

namespace CCTools.View
{
    internal class MainFormPresenter
    {
        private const string c_textWhenPostBuildEnabled = "Disable post build";
        private const string c_textWhenPostBuildDisabled = "Enable post build";
        private const string c_workingText = "CCTools - Working...";
        private const string c_idleText = "CCTools - Idle";
        private const string c_setCsCompleteDialogText = "Set config spec complete! Update view now?";
        private const string c_setCsCompleteDialogTitle = "Run update?";
        
        private readonly IView m_view;
        private readonly MainModel m_model;

        private int m_currentViewIndex = -1;

        public MainFormPresenter(IView view, ILogger logger)
        {
            m_view = view;
            m_model = new PlatformLoader().Load();
            m_model.SetLogger(logger);
        }
                
        public void GetConfigSpec()
        {
            string[] configSpec = m_model.GetCs(GetCurrentViewPath());
            m_view.SetLogText(configSpec.Select(s => s.StartsWith("mkbranch ") && !s.Contains("sr") ? "#" + s : s).ToArray());
        }
        public void SetConfigSpec()
        {
            m_model.SetCs(GetCurrentViewPath(), m_view.GetLogLines());
            if (IsUpdateOpenDialog())
            {
                UpdateCurrentView();
            }
        }
        public void UpdateCurrentView()
        {
            Run(m_model.UpdateViewRunner(GetCurrentViewPath()));
            m_model.ViewUpdateLog(GetCurrentViewPath());
            m_model.RemoveCcUpdateLogs(GetCurrentViewPath());
        }
        public void MakeCleanAndDebug()
        {
            Run(m_model.CreateMakeCleanAndDebugRunner(GetCurrentViewPath()));
        }
        public void MakeDebug()
        {
            Run(m_model.CreateMakeDebugRunner(GetCurrentViewPath()));
        }
        public void MakeRelease()
        {
            Run(m_model.CreateMakeReleaseRunner(GetCurrentViewPath()));
        }
        public void OpenVobSync()
        {
            new FormSyncVob(m_model).Show(m_model.GetSyncVobData(GetCurrentViewPath()));
        }
        public void OnLoad()
        {
            Render();
        }
        public void OnSelectedViewChanged()
        {
            m_currentViewIndex = m_view.SelectedViewIndex;
            Render();
        }
        public void MakeCleanAndRelease()
        {
            Run(m_model.CreateMakeCleanAndReleaseRunner(GetCurrentViewPath()));
        }
        public void Browse()
        {
            m_model.Browse(GetCurrentViewPath());
        }
        public void BrowseTotalCommander()
        {
            m_model.BrowseTotal(GetCurrentViewPath());
        }
        public void GetLatestConfigSpec()
        {
            if (m_model.IsSrView(GetCurrentViewPath()))
            {
                FormGetString getReleaseForm = GetReleaseForm();
                if (getReleaseForm.ShowDialog() == DialogResult.OK)
                {
                    m_view.SetLogText(m_model.GetNewConfigSpecForSrView(GetCurrentViewPath(), getReleaseForm.Result));
                }
                return;
            }

            if (m_model.IsRelView(GetCurrentViewPath()))
            {
                m_view.SetLogText(m_model.GetNewConfigSpecForRelView(GetCurrentViewPath()));
            }
        }
        public void SwitchTestPostBuildStatus()
        {
            m_model.SwitchTestPostBuildStatus(GetCurrentViewPath());
            Render();
        }
        public void RestartEcho()
        {
            Run(m_model.RestartEchoRunners());
        }
        public void ReloadViewLight()
        {
            DoActionWithLock(() =>
                    {
                        m_model.DeleteViewFilesLight(GetCurrentViewPath());
                        UpdateCurrentView();
                    });
        }

        private static bool IsUpdateOpenDialog()
        {
            return MessageBox.Show(c_setCsCompleteDialogText, c_setCsCompleteDialogTitle, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
        private static FormGetString GetReleaseForm()
        {
            return new FormGetString("Enter release number",
                                     "Enter release number (1.1, 1.2, etc.). If Latest - enter \"LATEST\" or leave this field empty.");
        }
        private string GetCurrentViewPath()
        {
            return m_currentViewIndex > -1 && m_view.Views.Count > 0 ? 
                m_model.LocalViews[m_currentViewIndex] : 
                string.Empty;
        }
        private void Run(ICmdRunner runner)
        {
            Run(new[] { runner });
        }
        private void Run(IEnumerable<ICmdRunner> runners)
        {
            DoActionWithLock(() =>
                    {
                        m_view.ClearLogger();
                        foreach (ICmdRunner runner in runners)
                        {
                            runner.Run();
                        }
                        m_view.FlushLogger();
                    });
        }
        private void DoActionWithLock(Action action)
        {
            m_view.SetTitle(c_workingText);
            m_view.LockUI();

            action();

            m_view.UnlockUI();
            m_view.SetTitle(c_idleText);
        }
        private void Render()
        {
            if (m_view.Views == null || m_view.Views.Count == 0)
            {
                m_view.Views = m_model.LocalViews;
                m_view.SelectedViewIndex = (m_model.LocalViews.Count > 0) ? 0 : -1;
            }
            m_view.SetPostBuildStatusText(m_model.IsTestPostBuildEnabled(GetCurrentViewPath()) ? 
                c_textWhenPostBuildEnabled :
                c_textWhenPostBuildDisabled);
        }

        public void CreateSrView()
        {
            FormGetString getReleaseForm = CreateSrViewForm();
            if (getReleaseForm.ShowDialog() == DialogResult.OK)
            {
                string branchName = getReleaseForm.Result;
                Run(new[] {m_model.CreateSrView(branchName), m_model.CreateBranchType(branchName)});
                m_model.RefreshViewsList();
                int index = m_model.LocalViews.Select((s, i) => new { S = s, I = i}).First(s => s.S.Contains(branchName)).I;
                m_view.Views = m_model.LocalViews;
                m_currentViewIndex = index;
                m_view.SelectedViewIndex = index;
                m_view.SetPostBuildStatusText(m_model.IsTestPostBuildEnabled(GetCurrentViewPath()) ? c_textWhenPostBuildEnabled : c_textWhenPostBuildDisabled);
            }
        }

        private static FormGetString CreateSrViewForm()
        {
            return new FormGetString("Enter sr branch name", "Enter sr branch name");
        }

        public void WipeView()
        {
            DoActionWithLock(() => m_model.DeleteViewFilesLight(GetCurrentViewPath()));
        }
    }
}