using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuEvenManager : MonoBehaviour
{
    private void Awake()
    {
        EventCenter.AddListener<string>(NoticeType.RoomCreate, RoomCreate);
        EventCenter.AddListener<string, string>(NoticeType.PlayerSitDown, RoomJoin);
        EventCenter.AddListener<string>(NoticeType.JoinClub, JoinClub);
        EventCenter.AddListener<string>(NoticeType.RoomDelete, RoomDelete);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(NoticeType.RoomCreate, RoomCreate);
        EventCenter.RemoveListener<string, string>(NoticeType.PlayerSitDown, RoomJoin);
        EventCenter.RemoveListener<string>(NoticeType.JoinClub, JoinClub);
        EventCenter.RemoveListener<string>(NoticeType.RoomDelete, RoomDelete);
    }

    void RoomCreate(string roomId)
    {
        var j = new Api.Room().Join(roomId);

        if (j["code"].n != 0)
        {
            Utils.MsgBox.ShowErr(j["msg"].str);
            return;
        }

        j = new Api.Room().SitDown(roomId);
        if (j["code"].n != 0)
        {
            Utils.MsgBox.ShowErr(j["msg"].str);
            return;
        }
        Data.Room.Id = roomId;
    }

    void RoomJoin(string roomId, string uid)
    {
        Debug.Log("join room:" + uid);
        if (uid == Data.User.Id + "")
        {
            SceneManager.LoadScene("Game");
        }
    }

    void JoinClub(string clubId)
    {
        Data.Club.Id = clubId;
        SceneManager.LoadScene("Club");
    }

    void RoomDelete(string roomId)
    {
        EventCenter.Broadcast(NoticeType.RoomList);
    }
}
