using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class NetWorkToServer
{
    private Queue<NetMsgBase> recvMsgPool = null;
    private Queue<NetMsgBase> sendMsgPool = null;

    private NetSocket clientSocket;
    private Thread sendThread;

    public NetWorkToServer(string ip, ushort port)
    {
        recvMsgPool = new Queue<NetMsgBase>();
        sendMsgPool = new Queue<NetMsgBase>();

        clientSocket = new NetSocket();
        clientSocket.AsyncConnect(ip, port, AsysnConnectCallBack, AsysnRecvCallBack);
    }

    private void AsysnConnectCallBack(bool success, ErrorSockect error, string exception)
    {
        if (success)
        {
            sendThread = new Thread(LoopSendMsg);
            sendThread.Start();
        }
    }

    #region Send

    void LoopSendMsg()
    {
        while (clientSocket != null && clientSocket.IsConnect())
        {
            lock (sendMsgPool)
            {
                while (sendMsgPool.Count > 0)
                {
                    NetMsgBase tmpBody = sendMsgPool.Dequeue();
                    clientSocket.AsynSend(tmpBody.GetNetBytes(), CallBackSend);
                }
            }
            Thread.Sleep(100);
        }
    }

    private void CallBackSend(bool success, ErrorSockect error, string exception)
    {

    }

    public void PutSendMsgToPool(NetMsgBase msg)
    {
        lock (sendMsgPool)
        {
            sendMsgPool.Enqueue(msg);
        }
    }

    #endregion

    #region Receive
    private void AsysnRecvCallBack(bool success, ErrorSockect error, string exception, byte[] byteMessage,
    string strMessage)
    {
        if (success)
        {

        }
        else
        {

        }
    }

    void PutRecvMsgToPool(byte[] recMsg)
    {
        NetMsgBase tmpBase = new NetMsgBase(recMsg);
        recvMsgPool.Enqueue(tmpBase);
    }

    public void Update()
    {
        if (recvMsgPool != null)
        {
            while (recvMsgPool.Count > 0)
            {
                NetMsgBase tmpBase = recvMsgPool.Dequeue();
                AnalyseData(tmpBase);
            }
        }
    }

    void AnalyseData(NetMsgBase msg)
    {
        MsgCenter.Instance.SendToMsg(msg);
    }
    #endregion

    #region Disconnect

    public void Disconnect()
    {
        if (clientSocket != null && clientSocket.IsConnect())
        {
            clientSocket.AsyncDisconnect(CallBackDisconnect);
        }
    }

    private void CallBackDisconnect(bool success, ErrorSockect error, string exception)
    {
        if (success)
        {
            sendThread.Abort();
        }
    }

    #endregion
}
