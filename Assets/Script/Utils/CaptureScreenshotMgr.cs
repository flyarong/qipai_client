using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace Utils
{

    public class CaptureScreenshotMgr : MonoBehaviour
    {

        /// <summary>
        /// 保存截屏图片，并且刷新相册（Android和iOS）
        /// </summary>
        /// <param name="name">若空就按照时间命名</param>
        public void CaptureScreenshot(string name = "")
        {
            string _name = "";
            if (string.IsNullOrEmpty(name))
            {
                _name = "Screenshot_" + GetCurTime() + ".png";
            }
            //编辑器下
            //string path = Application.persistentDataPath + "/" + _name;
            //Application.CaptureScreenshot(path, 0);
            //EDebug.Log("图片保存地址" + path);


            //Android版本
            StartCoroutine(CutImage(_name));
            Debug.Log("图片保存地址" + _name);

        }
        //截屏并保存
        IEnumerator CutImage(string name)
        {
            //图片大小  
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            yield return new WaitForEndOfFrame();
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
            tex.Apply();
            yield return tex;
            byte[] byt = tex.EncodeToPNG();
            string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android"));
            File.WriteAllBytes(path + "/Pictures/Screenshots/" + name, byt);
            string[] paths = new string[1];
            paths[0] = path;
            ScanFile(paths);
        }
        //刷新图片，显示到相册中
        void ScanFile(string[] path)
        {
            using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
                using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
                {
                    Conn.CallStatic("scanFile", playerActivity, path, null, null);
                }
            }
        }
        /// <summary>
        /// 获取当前年月日时分秒，如201803081916
        /// </summary>
        /// <returns></returns>
        string GetCurTime()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }
    }
}