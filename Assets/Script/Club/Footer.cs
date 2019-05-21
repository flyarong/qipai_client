using UnityEngine;
using System.Collections;
using FairyGUI;

namespace Club
{
    public class Footer : MonoBehaviour
    {
        GComponent mainUI;
        ManageWindow manageWindow;
        UsersWindow usersWindow;
        // Use this for initialization
        void Start()
        {
            mainUI = GetComponent<UIPanel>().ui;
            manageWindow = new ManageWindow();
            usersWindow = new UsersWindow();

            var footer = mainUI.GetChild("footer").asCom;
            var btnManage = footer.GetChild("btnManage").asButton;
            btnManage.onClick.Add(onBtnManageClick);
            footer.GetChild("btnUsers").asButton.onClick.Add(onBtnUsersClick);

        }

        void onBtnManageClick()
        {
            manageWindow.Show();
        }

        void onBtnUsersClick()
        {
            usersWindow.Show();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}