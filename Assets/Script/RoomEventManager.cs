using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomEventManager : MonoBehaviour
{
    private void Start()
    {
        EventCenter.AddListener<string,string>(NoticeType.RoomExit, ExitRoom);
        EventCenter.AddListener<string>(NoticeType.RoomDelete, RoomDelete);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string,string>(NoticeType.RoomExit, ExitRoom);
        EventCenter.RemoveListener<string>(NoticeType.RoomDelete, RoomDelete);
    }

    void ExitRoom(string rid,string uid)
    {
        if (uid == Data.User.Id + "")
        {
            Data.Room.Id = "";
            Debug.Log("用户退出房间，用户编号为:" + uid);
            SceneManager.LoadScene("Menu");
        }
    }

    void RoomDelete(string roomId)
    {
        if(roomId == Data.Room.Id)
        {
            Utils.MsgBox.ShowErr("超过10分钟未开始游戏，房间已解散。或房主解散房间");
            EventCenter.Broadcast<string,string>(NoticeType.RoomExit, roomId,Data.User.Id+"");
        }
    }
}
