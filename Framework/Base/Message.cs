using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message 
{
    //类型
    public byte Type;
    //命令
    public int Command;
    //参数
    public object Content;

    public Message()
    {

    }

    public Message(byte type,int command, object content)
    {
        Type = type;
        Command = command;
        Content = content;
    }
}

public class MessageType
{
    public static byte Type_Audio = 1;
    public static byte Type_UI = 2;

    public static int Audio_PlaySound = 100;
    public static int Audio_PlayMusic = 101;
    public static int Audio_StopMusic = 102;
    public static int Audio_ChangeVolumme = 103;

    public static int UI_ShowPanel = 200;//打开面板
    public static int UI_AddScore = 201;//加分
    public static int UI_ShowShop = 202;//打开商店
}
