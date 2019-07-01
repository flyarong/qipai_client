using UnityEngine;
using System.Collections;

namespace Utils
{
    public class Helper
    {
        public static string GetReallyImagePath(string path)
        {

            if (path.StartsWith("http://") || path.StartsWith("HTTP://"))
            {
                return path;
            }
            return Config.HttpBaseHost + "/static" + path;
        }
    }
}