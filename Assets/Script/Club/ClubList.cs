using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FairyGUI;
using UnityEngine.SceneManagement;

namespace Club
{
    public class ClubList : MonoBehaviour
    {
        // 菜单页面俱乐部列表
        private  GList list;
        private  string[] scores = {
            "1/2",
            "2/4",
            "3/6",
            "4/8",
            "5/10",
            "10/20"
        };
 
        private void Awake()
        {
            var mainUI = GetComponent<UIPanel>().ui;
            list = mainUI.GetChild("list").asCom.GetChild("clubs").asCom.GetChild("list").asList;
            list.onClickItem.Add(onClickItem);
            EventCenter.AddListener(NoticeType.ClubList, RenderList);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(NoticeType.ClubList, RenderList);
        }

        private  void addClubItem(JSONObject club)
        {
            GComponent item = list.GetFromPool("ui://1ad63yxfbul89b").asCom;

            item.GetChild("id").asTextField.text = club["id"].n + "";
            item.GetChild("score").asTextField.text = scores[(int)club["score"].n];
            item.GetChild("pay").asTextField.text = club["pay"].n == 0 ? "老板" : "AA";
            item.GetChild("count").asTextField.text = club["count"].n + "";
            item.GetChild("boss").asTextField.text = club["boss"].str;
            list.AddChild(item);
        }

        public  void RenderList()
        {
            var j = new Api.Club().List();
            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr("更新窗口失败，请联系管理");
                return;
            }
            list.RemoveChildrenToPool();

            if (j["data"]["clubs"] == null || j["data"]["clubs"].list == null)
            {
                return;
            }

            foreach (var v in j["data"]["clubs"].list)
            {
                addClubItem(v);
            }
        }

        private void onClickItem(EventContext context)
        {
            var item = (GComponent)context.data;
            var id = item.GetChild("id").asTextField.text;
            Debug.Log("点击:" + id);
            Data.Club.Id = id;
            SceneManager.LoadScene("Club");
        }

    }
}