using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Api;

namespace Club
{
    public class TableList : MonoBehaviour
    {
        private GComponent mainUI;
        private GList tables;
        // Start is called before the first frame update
        void Start()
        {
            mainUI = GetComponent<UIPanel>().ui;

            tables = mainUI.GetChild("tableList").asCom.GetChild("tables").asList;
            tables.itemRenderer = tablesItemRender;
            tables.numItems = 10;
        }

        void tablesItemRender(int index, GObject item)
        {
            GComponent table = item.asCom;
            var info = table.GetChild("info").asRichTextField;
            Dictionary<string, string> vars = new Dictionary<string, string>();
            vars["id"] = (index + 1) + "";
            vars["now"] = (index + 1) + "";
            vars["max"] = 10 + "";
            vars["score"] = "10/20";
            info.templateVars = vars;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}
