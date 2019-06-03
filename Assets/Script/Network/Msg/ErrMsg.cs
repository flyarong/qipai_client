using UnityEngine;

namespace Network.Msg
{
    public class ErrMsg : BaseMsg
    {
        public int code;
        public string msg;
        
        public ErrMsg(MsgID Id) : base(Id)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.Default.GetString(data);
            ResLogin jsonData = JsonUtility.FromJson<ResLogin>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}