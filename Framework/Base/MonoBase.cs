using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChangQFramework{
    public class MonoBase : MonoBehaviour
    {
        //发送消息
        public void SendCustomMessage(Message message)
        {
            MessageCenter.SendMessage(message);
        }
        public void SendCustomMessage(byte type,int command,object content)
        {
            Message message = new Message(type, command, content);
            MessageCenter.SendMessage(message);
        }
        //接收消息
        public virtual void ReceiveMessage(Message message)
        {

        }
    }
}
