using UnityEngine;
using FairyGUI;

namespace Utils
{
    public class MsgBox
    {
        static ErrorWindow errorWindow = new ErrorWindow();
        public static void ShowErr(string msg, float time=3)
        {
            GRoot.inst.ShowPopup(errorWindow);
            errorWindow.SetMsg(msg);
            errorWindow.T = time;
        }
    }
}
