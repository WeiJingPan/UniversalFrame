using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSocket : MonoBehaviour
{
    public delegate void UDPSocketDelegate(byte[] pBuf, int dwCount, string tmpIp, ushort tmpPort);

    private UDPSocketDelegate udpDelegate;

    private IPEndPoint udpIp;
    private Socket udpSocket;
    private byte[] recvData;
    private Thread recvThread;

    public bool BindSocket(ushort port, int bufferLength, UDPSocketDelegate tmpDelegate)
    {
        udpIp=new IPEndPoint(IPAddress.Any, port);
        UdpConnect();
        udpDelegate = tmpDelegate;
        recvData=new byte[bufferLength];
        recvThread=new Thread(RecvDataThread);
        recvThread.Start();
        return true;
    }

    public void UdpConnect()
    {
        udpSocket=new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
        //带服务器功能
        udpSocket.Bind(udpIp);
    }

    private bool isRunning = true;
    public void RecvDataThread()
    {
        while (isRunning)
        {
            if (udpSocket==null||udpSocket.Available<1)
            {
                Thread.Sleep(100);
                continue;
            }
            lock (this)
            {
                IPEndPoint sender=new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint) sender;
                int myCount = udpSocket.ReceiveFrom(recvData, ref remote);
                if (udpDelegate!=null)
                {
                    udpDelegate(recvData, myCount, remote.AddressFamily.ToString(), (ushort)sender.Port);
                }
            }
        }
    }

    public int SendData(string ip, byte[] data, ushort uport)
    {
        IPEndPoint sendToIp=new IPEndPoint(IPAddress.Parse(ip),uport);
        if (!udpSocket.Connected)
        {
            UdpConnect();
        }
        int mySend = udpSocket.SendTo(data, data.Length, SocketFlags.None, sendToIp);
        return mySend;
    }
}
