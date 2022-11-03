using System.Collections.Generic;
using UnityEngine;

//约束传入的参数必须是MonoBase或MonoBase的子类
public class ManagerBase<T> : MonoBase where T:MonoBase
{
    public static T Instance;
    //管理的消息接收者
    public List<MonoBase> ReceiveList = new List<MonoBase>();
    //当前管理器类接收的消息类型
    protected byte messageType;
    protected virtual void Awake()
    {
        //所有继承自ManagerBase的管理器类都是单例
        Instance = this as T;
        //设置管理器类的消息类型
        messageType = SetMessageType();
        //将当前的管理类添加到消息中心
        MessageCenter.Managers.Add(this);
    }

    //必须实现,返回当前管理器类的消息类型
    protected virtual byte SetMessageType()
    {
        return MessageType.Type_UI;
    }
    
    //注册消息监听
    public void RegisterReceiver(MonoBase mb)
    {
        //如果消息接收者列表中不包含当前接收者
        if (!ReceiveList.Contains(mb))
        {
            ReceiveList.Add(mb);
        }
    }

    public void RemoveReceiver(MonoBase mb)
    {
        if (ReceiveList.Contains(mb))
        {
            ReceiveList.Remove(mb);
        }
    }

    //接收到消息，并向下分发消息
    public override void ReceiveMessage(Message message)
    {
        base.ReceiveMessage(message);
        //如果消息类型不匹配，不向下分发消息
        if(message.Type != messageType)
        {
            return;
        }
        //给每一个消息接收者发消息
        foreach (MonoBase mb in ReceiveList)
        {
            mb.ReceiveMessage(message);
        }

    }
}
