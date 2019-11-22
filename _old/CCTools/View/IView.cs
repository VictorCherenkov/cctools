using System.Collections.Generic;

namespace CCTools.View
{
    internal interface IView
    {
        //place all properties necessary for get/set view data 
        void LockUI();
        void UnlockUI();
        void SetPostBuildStatusText(string value);
        void SetTitle(string value);
        int SelectedViewIndex { get; set; }
        IList<string> Views { get; set; }

        string[] GetLogLines();
        string GetLogText();
        void SetLogText(string[] lines);
        void SetLogText(string text);
        void ClearLogger();
        void FlushLogger();
    }
}