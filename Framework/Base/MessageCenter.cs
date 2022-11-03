using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter
{
    //管理类集合
    public static List<MonoBase> Managers =new List<MonoBase>();

    //发送消息
    public static void SendMessage(Message msg)
    {
        //给每一个管理器发消息
        foreach (MonoBase mb in Managers)
        {
            mb.ReceiveMessage(msg);
        }
    }

    public static void SendMessage(byte type, int command, object content)
    {
        Message msg = new Message(type, command, content);
        SendMessage(msg);
    }
}
