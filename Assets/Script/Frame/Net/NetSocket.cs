using System;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public enum ErrorSockect
{
    eSuccess = 0,
    eTimeOut,
    eSockectNull,
    eSockectUnConnect,

    eConnectSuccess,
    eConnectUnSuccessUnKown,
    eConnectError,

    eSendSuccess,
    eSendUnSuccessUnKown,

    eRecvUnSuccessUnkown,

    eDisConnectSuccess,
    eDisConnectUnkown,
}
public class NetSocket
{

    public delegate void CallBackNormal(bool success, ErrorSockect error, string exception);
    public delegate void CallBackRecv(bool success, ErrorSockect error, string exception, byte[] byteMessage, string strMessage);

    private CallBackNormal callBackConnect;
    private CallBackNormal callBackSend;
    private CallBackNormal callBackDisconnect;
    private CallBackRecv callBackRecv;

    private ErrorSockect errorSocket;
    private Socket clientSocket;
    private string addressIp;
    private ushort port;

    private SocketBuffer recvBuffer;

    public NetSocket()
    {
        recvBuffer = new SocketBuffer(6, RecvMsgOver);
        recvBuff = new byte[1024];
    }

    #region 链接
    public bool IsConnect()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AsyncConnect(string ip, ushort port, CallBackNormal connectBack, CallBackRecv tmpRecvBack)
    {
        errorSocket = ErrorSockect.eSuccess;
        this.callBackConnect = connectBack;
        this.callBackRecv = tmpRecvBack;
        if (clientSocket != null && clientSocket.Connected)
        {
            this.callBackConnect(false, ErrorSockect.eConnectError, "connect repeat");
        }
        else if (clientSocket == null || !clientSocket.Connected)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            IAsyncResult connect = clientSocket.BeginConnect(endPoint, ConnectCallBack, clientSocket);
            if (WriteDot(connect))
            {
                connectBack(false, errorSocket, "链接超时");
            }
        }
    } 
    #endregion

    #region 链接后回调事件
    private void ConnectCallBack(IAsyncResult asConnect)
    {
        try
        {
            clientSocket.EndConnect(asConnect);
            if (clientSocket.Connected == false)
            {
                errorSocket = ErrorSockect.eConnectUnSuccessUnKown;
                callBackConnect(false, errorSocket, "链接超时");
                return;
            }
            else
            {
                //开始接收消息
                errorSocket = ErrorSockect.eConnectSuccess;
                this.callBackConnect(false, errorSocket, "success");
            }
        }
        catch (Exception e)
        {
            this.callBackConnect(false, errorSocket, e.ToString());
        }
    }

    internal void AsyncConnect(object callBackDisconnect)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region 数据接收
    private byte[] recvBuff;
    public void Receive()
    {
        if ((clientSocket != null && clientSocket.Connected))
        {
            IAsyncResult asynRecv = clientSocket.BeginReceive(recvBuff, 0, recvBuff.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
            if (!WriteDot(asynRecv))
            {
                callBackRecv(false, ErrorSockect.eRecvUnSuccessUnkown, "recv false", null, "");
            }
        }
    }

    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            clientSocket.EndReceive(ar);
            if (!clientSocket.Connected)
            {
                callBackRecv(false, ErrorSockect.eRecvUnSuccessUnkown, "recv false", null, "");
                return;
            }
            int length = clientSocket.EndReceive(ar);
            if (length == 0)
            {
                return;
            }
            recvBuffer.RecvByte(recvBuff, length);
        }
        catch (Exception e)
        {
            callBackRecv(false, ErrorSockect.eRecvUnSuccessUnkown, e.ToString(), null, "");
        }
        Receive();
    }

    public void RecvMsgOver(byte[] allByte)
    {
        callBackRecv(true, ErrorSockect.eSuccess, "", null, "recv back success");
    }
    #endregion

    #region 数据发送
    public void AsynSend(byte[] sendBuffer, CallBackNormal tmpSendBack)
    {
        errorSocket = ErrorSockect.eSuccess;
        this.callBackSend = tmpSendBack;
        if (clientSocket == null)
        {
            this.callBackSend(false, ErrorSockect.eSockectNull, "");
        }
        else if (!clientSocket.Connected)
        {
            callBackSend(false, ErrorSockect.eSockectUnConnect, "");
        }
        else
        {
            IAsyncResult asySend = clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendCallBack, clientSocket);
            if (!WriteDot(asySend))
            {
                callBackSend(false, ErrorSockect.eSendUnSuccessUnKown, "send failed");
            }
        }
    }

    private void SendCallBack(IAsyncResult ar)
    {
        try
        {
            int byteSend = clientSocket.EndSend(ar);
            if (byteSend > 0)
            {
                callBackSend(true, ErrorSockect.eSendSuccess, "");
            }
            else
            {
                callBackSend(false, ErrorSockect.eSendUnSuccessUnKown, "");
            }
        }
        catch (Exception e)
        {
            callBackSend(false, ErrorSockect.eSendUnSuccessUnKown, "");
        }
    }
    #endregion

    #region 断开链接

    public void AsyncDisconnect(CallBackNormal tmpDisconnectBack)
    {
        try
        {
            errorSocket = ErrorSockect.eSuccess;
            this.callBackDisconnect = tmpDisconnectBack;
            if (clientSocket == null)
            {
                this.callBackDisconnect(false, ErrorSockect.eDisConnectUnkown, "client is null");
            }
            else if (!clientSocket.Connected)
            {
                this.callBackDisconnect(false, ErrorSockect.eDisConnectUnkown, "client is unConnect");
            }
            else
            {
                IAsyncResult asynDisconnet = clientSocket.BeginDisconnect(false, DisconnectCallBack, clientSocket);
                if (WriteDot(asynDisconnet))
                {
                    this.callBackDisconnect(false, ErrorSockect.eDisConnectUnkown, "disconnect failed");
                }
            }
        }
        catch (Exception e)
        {
            this.callBackDisconnect(false, ErrorSockect.eDisConnectUnkown, "disconnect failed");
        }
    }

    private void DisconnectCallBack(IAsyncResult ar)
    {
        try
        {
            clientSocket.EndDisconnect(ar);
            clientSocket.Close();
            clientSocket = null;
            this.callBackDisconnect(true, ErrorSockect.eDisConnectSuccess, "");
        }
        catch (Exception e)
        {
            this.callBackDisconnect(true, ErrorSockect.eDisConnectUnkown, "");
        }
    }

    #endregion

    #region 链接超时
    //true 表示可以写入，false 表示超时
    bool WriteDot(IAsyncResult ar)
    {
        int i = 0;
        while (ar.IsCompleted == false)
        {
            i++;
            if (i > 20)
            {
                errorSocket = ErrorSockect.eTimeOut;
                return false;
            }
            Thread.Sleep(100);
        }
        return true;
    }
    #endregion

}
