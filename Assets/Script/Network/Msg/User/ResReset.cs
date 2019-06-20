using System;

namespace Network.Msg
{
    [Serializable]
    public class ResReset : CommonMsg
    {
        public ResReset() : base(MsgID.ResReset)
        {
        }
    }

}