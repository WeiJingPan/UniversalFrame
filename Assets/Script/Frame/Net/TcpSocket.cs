using UnityEngine;
using System.Collections;
using ProtoBuf;
using System;

public enum TCPEvent
{
    eTcpConnect=ManagerID.NetManager+1,
    eTcpSendMsg,
    eMaxValue,
}

public class TCPConnectMsg : MsgBase
{
    public string ip;
    public ushort port;

    public TCPConnectMsg(ushort tmpId, string ip, ushort port)
    {
        this.msgId = tmpId;
        this.ip = ip;
        this.port = port;
    }
}

public class TCPMsg : MsgBase
{
    public NetMsgBase netMsg;

    public TCPMsg(ushort tmpId, NetMsgBase tmpBase)
    {
        this.msgId = tmpId;
        this.netMsg = tmpBase;
    }
}

/// <summary>
/// C#专用
/// </summary>
/// <typeparam name="T"></typeparam>
public class TCPNetMsgs <T> : NetMsgBase where T : IExtensible
{
    /// <summary>
    /// 从socket接受过来的数据转变成protobuffer类
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    public U GetPBClass<U>() where U : IExtensible
    {
        byte[] tmpByte = new byte[this.buffer.Length - 6];
        Buffer.BlockCopy(buffer, 6, tmpByte, 0, buffer.Length - 6);
        return IProtoTools.Deserialize<U>(tmpByte);
    }
    public U GetPBClass<U>(byte[] tmpArr) where U : IExtensible
    {
        this.buffer = tmpArr;
        return GetPBClass<U>();
    }
    /// <summary>
    /// 只要外界传递msgid，PBClass
    /// </summary>
    /// <param name="tmp"></param>
    /// <param name="msgid"></param>
    public TCPNetMsgs(T tmp,ushort msgid)
    {
        this.msgId = msgId;
        byte[] tmpByte = IProtoTools.Serialize(tmp);
        buffer = new byte[tmpByte.Length + 6];
        //怎么样组成发送byte[]
        //长度
        byte[] dataLength = BitConverter.GetBytes(tmpByte.Length);

        Buffer.BlockCopy(dataLength, 0, buffer, 0, 4);
        byte[] eventId = BitConverter.GetBytes(msgId);
        Buffer.BlockCopy(eventId, 0, buffer, 4, 2);
        Buffer.BlockCopy(tmpByte, 0, buffer, 6, tmpByte.Length);
    }
    
    public void ChangeMsgData<V>(V tmp)where V : IExtensible
    {
        byte[] tmpByte = IProtoTools.Serialize(tmp);
        buffer = new byte[tmpByte.Length + 6];
        //怎么样组成发送byte[]
        //长度
        byte[] dataLength = BitConverter.GetBytes(tmpByte.Length);

        Buffer.BlockCopy(dataLength, 0, buffer, 0, 4);
        byte[] eventId = BitConverter.GetBytes(this.msgId);
        Buffer.BlockCopy(eventId, 0, buffer, 4, 2);
        Buffer.BlockCopy(tmpByte, 0, buffer, 6, tmpByte.Length);
    }
    
    public void ChangeMsgData<V>(V tmp,ushort msgId) where V : IExtensible
    {
        byte[] tmpByte = IProtoTools.Serialize(tmp);
        buffer = new byte[tmpByte.Length + 6];
        //怎么样组成发送byte[]
        //长度
        byte[] dataLength = BitConverter.GetBytes(tmpByte.Length);

        Buffer.BlockCopy(dataLength, 0, buffer, 0, 4);
        byte[] eventId = BitConverter.GetBytes(this.msgId);
        Buffer.BlockCopy(eventId, 0, buffer, 4, 2);
        Buffer.BlockCopy(tmpByte, 0, buffer, 6, tmpByte.Length);
    }
    
    public void ChangeMsgId(ushort msgid)
    {
        this.msgId = msgId;
        byte[] eventId = BitConverter.GetBytes(this.msgId);
        Buffer.BlockCopy(eventId, 0, buffer, 4, 2);
    }
    
    public override byte[] GetNetBytes()
    {
        return buffer;
    }
}

public class TcpSocket:NetBase
{
    private NetWorkToServer socket;
    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort) TCPEvent.eTcpConnect,
            (ushort) TCPEvent.eTcpSendMsg,
        };
        RegistSelf(this,msgIds);
    }

    void Update()
    {
        if (socket!=null)
        {
            socket.Update();
        }
    }

    public override void ProccessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)TCPEvent.eTcpConnect:
                TCPConnectMsg connectMsg = (TCPConnectMsg) tmpMsg;
                socket = new NetWorkToServer(connectMsg.ip, connectMsg.port);
                break;
            case (ushort)TCPEvent.eTcpSendMsg:
                TCPMsg sendMsg = (TCPMsg) tmpMsg;
                socket.PutSendMsgToPool(sendMsg.netMsg);
                break;
        }
    }
}
