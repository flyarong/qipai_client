using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FairyGUI;
using System.Text.RegularExpressions;
using Api;
using Utils;

public class Login : MonoBehaviour
{
    GTextInput inputPhone;
    GTextInput inputPass;
    GComponent mainUI;

    private void Awake()
    {
        Data.Event.Start();
        UIObjectFactory.SetLoaderExtension(typeof(Utils.HttpLoader));
        Data.User.Token = PlayerPrefs.GetString("token");
    }

    // Start is called before the first frame update
    void Start()
    {
        // 如果用户已经登录，就直接跳转到游戏菜单界面
        if (Data.User.Token.Length > 0)
        {
            string text = Client.Inst.GetContent("/users");
            Debug.Log(text);
            JSONObject j = new JSONObject(text);

            if (j["code"].n != 0)
            {
                MsgBox.ShowErr(j["msg"].str);
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene("Login");
                return;
            }
            SceneManager.LoadScene("Menu");
        }

        mainUI = GetComponent<UIPanel>().ui;


        inputPhone = mainUI.GetChild("inputPhone").asTextInput;
        inputPass = mainUI.GetChild("inputPass").asTextInput;
        
        mainUI.GetChild("btnLogin").onClick.Add(LoginBtnClick);

        mainUI.GetChild("btnReg").onClick.Add(() =>
        {
            Debug.Log("切换注册界面按钮被点击");
            SceneManager.LoadScene("Reg");
        });
        mainUI.GetChild("btnReset").asButton.color = Color.green;
        mainUI.GetChild("btnReset").onClick.Add(() =>
        {
            SceneManager.LoadScene("Reset");
        });
    }
    

    void LoginBtnClick()
    {
        if (!Regex.IsMatch(inputPhone.text, @"^1[34578]\d{9}$"))
        {
            ShowError("手机号格式不正确");
            return;
        }
        else if (inputPass.text.Length < 6 || inputPass.text.Length > 30)
        {
            ShowError("密码长度必须在6~30之间");
            return;
        }

        var j = new Api.User().Login("1", inputPhone.text, inputPass.text);
        if(j["code"].n != 0)
        {
            ShowError(j["msg"].str);
            return;
        }
        SceneManager.LoadScene("Menu");
    }
    

    void ShowError(string msg)
    {
        MsgBox.ShowErr(msg);
    }
}


