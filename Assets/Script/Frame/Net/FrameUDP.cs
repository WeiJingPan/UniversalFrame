using UnityEngine;
using System.Collections;

public enum UDPEvent
{
    eInitail=TCPEvent.eMaxValue+1,
    eSendTo,
    MaxValue
}

public class UDPMsg : MsgBase
{
    public ushort port;
    public int recvBufferLength;
    public UDPSocket.UDPSocketDelegate recvDelegate;

    public UDPMsg(ushort msgid, ushort port, int recvLength, UDPSocket.UDPSocketDelegate tmpDelegate)
    {
        this.port = port;
        this.msgId = msgid;
        this.recvBufferLength = recvLength;
        this.recvDelegate = tmpDelegate;
    }
}
public class UDPSendMsg : MsgBase
{
    public string ip;
    public byte[] data;
    public ushort port;

    public UDPSendMsg(string ip,byte[] data,ushort port)
    {
        this.port = port;
        this.data = data;
        this.ip = ip;
    }
}

public class FrameUDP : NetBase
{
    private UDPSocket socket;
    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort) UDPEvent.eInitail,
            (ushort) UDPEvent.eSendTo,
        };
        RegistSelf(this, msgIds);
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)UDPEvent.eInitail:
                UDPMsg msg = (UDPMsg) tmpMsg;
                socket = new UDPSocket();
                socket.BindSocket(msg.port, msg.recvBufferLength, msg.recvDelegate);
                break;
            case (ushort)UDPEvent.eSendTo:
                UDPSendMsg sendMsg = (UDPSendMsg)tmpMsg;
                socket.SendData(sendMsg.ip, sendMsg.data, sendMsg.port);
                break;
            default:
                break;
        }
    }
}
