using System;
using UnityEngine;
using System.Collections;

public class SocketBuffer
{
    //定义消息头
    private byte[] headByte;

    private byte headLength=6;
    private byte[] allRecvData;//接收到数据
    //当前接收到的数据长度
    private int curRecvLength;
    //总共接收到的数据长度
    private int allDataLength;

    public SocketBuffer(byte tmpHeadLength,CallBackRecvOver callBackRecvOver)
    {
        headLength = tmpHeadLength;
        headByte = new byte[headLength];
        this.callBackRecvOver = callBackRecvOver;
    }

    public void RecvByte(byte[] recvByte, int realLength)
    {
        if (realLength==0)return;
        //当前接收到的数据小于头的长度
        if (curRecvLength<headByte.Length)
        {
            RecvHead(recvByte, realLength);
        }
        else
        {
            int tmpLength = curRecvLength + realLength;
            if ((tmpLength==allDataLength))
            {
                //刚好相等的情况
                RecvOneAll(recvByte,realLength);
            }
            else if (tmpLength>allDataLength)
            {
                RecvLarger(recvByte, realLength);
            }
            else
            {
                RecvSmall(recvByte, realLength);
            }
        }
    }

    private void RecvLarger(byte[] recvByte, int realLength)
    {
        int tmpLength = allDataLength - curRecvLength;
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, tmpLength);
        curRecvLength += realLength;
        RecvOneMsgOver();
        int remainLength = realLength - tmpLength;
        byte[] remainByte=new byte[remainLength];
        Buffer.BlockCopy(recvByte, tmpLength, remainByte, 0, remainLength);
        RecvByte(remainByte,remainLength);
    }

    private void RecvSmall(byte[] recvByte, int realLength)
    {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);
        curRecvLength += realLength;
    }

    private void RecvOneAll(byte[] recvByte, int realLength)
    {
        Buffer.BlockCopy(recvByte,0,allRecvData,curRecvLength,realLength);
        curRecvLength += realLength;
        RecvOneMsgOver();
    }

    private void RecvHead(byte[] recvByte, int realLeagth)
    {
        //差多少个字节才能组成一个头
        int tmpReal = headByte.Length - curRecvLength;
        //现在接收到和已经接收到的总长度是多少
        int tmpLength = curRecvLength + realLeagth;
        //总长度小于头部
        if (tmpLength<headByte.Length)
        {
            Buffer.BlockCopy(recvByte,0,headByte,curRecvLength,realLeagth);
            curRecvLength += realLeagth;
        }
        else//大于等于头
        {
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, tmpReal);
            curRecvLength += tmpReal;
            //头部已经凑齐了
            //去除四个字节转换成int
            allDataLength = BitConverter.ToInt32(headByte, 0)+headLength;
            allRecvData=new byte[allDataLength];

            Buffer.BlockCopy(recvByte, 0, allRecvData, 0, headLength);
            int tmpRemin = realLeagth - tmpReal;
            //表示recByte是否还有数据
            if (tmpRemin>0)
            {
                 byte[] tmpByte=new byte[tmpRemin];
                //表示将剩下的字节送入tmpByte
                Buffer.BlockCopy(recvByte, tmpReal, tmpByte, 0, tmpRemin);
                RecvByte(tmpByte,tmpRemin);
            }
            else
            {
                //只有消息头的情况
                RecvOneMsgOver();
            }
        }
    }

    #region 接收消息完成

    public delegate void CallBackRecvOver(byte[] allData);

    private CallBackRecvOver callBackRecvOver;
    private void RecvOneMsgOver()
    {
        if (callBackRecvOver !=null)
        {
            callBackRecvOver(allRecvData);
        }
        curRecvLength = 0;
        allDataLength = 0;
        allRecvData = null;
    } 
    #endregion
}
