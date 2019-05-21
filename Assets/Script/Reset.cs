using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.Text.RegularExpressions;
using System.Net.Http;
using System;
using UnityEngine.SceneManagement;
using Api;

public class Reset : MonoBehaviour
{
    GComponent mainUI;

    GTextInput inputPhone;
    GTextInput inputCode;
    GTextInput inputPass;
    GTextInput inputConfirm;

    ErrorWindow errorWindow;

    // Start is called before the first frame update
    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;

        errorWindow = new ErrorWindow();

        inputPhone = mainUI.GetChild("inputPhone").asTextInput;
        inputCode = mainUI.GetChild("inputCode").asTextInput;
        inputPass = mainUI.GetChild("inputPass").asTextInput;
        inputConfirm = mainUI.GetChild("inputConfirm").asTextInput;

        mainUI.GetChild("btnReset").onClick.Add(btnResetClick);
        mainUI.GetChild("btnGetCode").onClick.Add(btnGetCodeClick);
        mainUI.GetChild("btnLogin").onClick.Add(() =>
        {
            SceneManager.LoadScene("Login");
        });
    }

    void btnGetCodeClick()
    {
        var j = Api.User.GetResetSmsCode(inputPhone.text);
        ShowError(j["msg"].str);
    }

    void btnResetClick()
    {
        if (!Regex.IsMatch(inputPhone.text, @"^1[34578]\d{9}$"))
        {
            ShowError("手机号格式不正确");
            return;
        }
        else if (!Regex.IsMatch(inputCode.text, @"\d+"))
        {
            ShowError("请输入收到的数字验证码");
            return;
        }
        else if (inputPass.text.Length < 6 || inputPass.text.Length > 30)
        {
            ShowError("密码长度必须在6~30之间");
            return;
        }
        else if (inputPass.text != inputConfirm.text)
        {
            ShowError("两次输入的密码不一致");
            return;
        }

        doReset();

    }


    void doReset()
    {
        Client client = new Client("");

        List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
        paramList.Add(new KeyValuePair<string, string>("type", "1"));
        paramList.Add(new KeyValuePair<string, string>("name", inputPhone.text));
        paramList.Add(new KeyValuePair<string, string>("pass", inputPass.text));
        paramList.Add(new KeyValuePair<string, string>("code", inputCode.text));

        string result = client.PutContent("/users/reset", paramList);

        JSONObject j = new JSONObject(result);

        if (j["code"].n != 0)
        {
            ShowError(j["msg"].str);
            return;
        }

        ShowError("密码重置成功");
        SceneManager.LoadScene("Login");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowError(string msg)
    {
        errorWindow.Show();
        errorWindow.SetMsg(msg);
        errorWindow.position = new Vector3();
        errorWindow.width = mainUI.width;
        errorWindow.height = mainUI.height;
    }
}
