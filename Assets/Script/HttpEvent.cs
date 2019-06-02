using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Data.Event.Start();
        Invoke("GetEvents", 0.1f);
    }


    private void OnDestroy()
    {
        Data.Event.Stop();
        EventCenter.Broadcast(NoticeType.RoomExit);
    }

    void GetEvents()
    {
        var e = Data.Event.Get();
        doEvent(e);
        Invoke("GetEvents", e == null ? 0.3f : 0);
    }

    void doEvent(JSONObject e)
    {
        if (e == null)
        {
            return;
        }
        switch (e["name"].str)
        {
            case "RoomList":
                EventCenter.Broadcast(NoticeType.RoomList);
                break;
            case "RoomDelete":
                EventCenter.Broadcast<string>(NoticeType.RoomDelete, e["args"].list[0].str);
                break;
            case "RoomExit":
                EventCenter.Broadcast<string, string>(NoticeType.RoomExit, e["args"].list[0].str, e["args"].list[1].str);
                break;
            case "RoomCreate":
                EventCenter.Broadcast<string>(NoticeType.RoomCreate, e["args"].list[0].str);
                break;
            case "PlayerSitDown":
                EventCenter.Broadcast<string, string>(NoticeType.PlayerSitDown, e["args"].list[0].str, e["args"].list[1].str);
                break;
            case "RoomStart":
                EventCenter.Broadcast<string>(NoticeType.RoomStart, e["args"].list[0].str);
                break;
            case "GameBegin":
                EventCenter.Broadcast<string>(NoticeType.GameBegin, e["args"].list[0].str);
                break;
            case "GameOver":
                EventCenter.Broadcast<string>(NoticeType.GameOver, e["args"].list[0].str);
                break;
            case "SetTimes":
                EventCenter.Broadcast<string, string, string>(NoticeType.SetTimes, e["args"].list[0].str, e["args"].list[1].str, e["args"].list[2].str);
                break;
            case "SetBanker":
                EventCenter.Broadcast<string, string>(NoticeType.SetBanker, e["args"].list[0].str, e["args"].list[1].str);
                break;
            case "SetScore":
                EventCenter.Broadcast<string, string, string>(NoticeType.SetScore, e["args"].list[0].str, e["args"].list[1].str, e["args"].list[2].str);
                break;
            case "SetScoreAll":
                EventCenter.Broadcast<string>(NoticeType.SetScoreAll, e["args"].list[0].str);
                break;
            case "CardTypes":
                EventCenter.Broadcast<string, string>(NoticeType.CardTypes, e["args"].list[0].str, e["args"].list[1].str);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
