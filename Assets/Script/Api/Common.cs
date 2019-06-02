using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Api
{

    public class Common
    {
        public JSONObject GetEvent()
        {
            try
            {
                var content = Client.Inst.GetContent("/events");
                if (content == null)
                {
                    return null;
                }
                if (!content.Contains("null"))
                {
                    Debug.Log(content);
                }

                return new JSONObject(content);
            }
            catch (IOException e)
            {
                return null;
            }

        }
    }
}