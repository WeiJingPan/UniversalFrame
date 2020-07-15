//using UnityEngine;
//using System.Collections;
//using System.Text;

//public class Debugger
//{
//    public static bool EnableLog = true;

//    public static UDPSocket udpSocket = null;

//    public static UDPSocket UdpSocket
//    {
//        get
//        {
//            if (udpSocket==null)
//            {
//                udpSocket = new UDPSocket();
//                udpSocket.BindSocket(18001, 1024, null);
//            }
//            return udpSocket;
//        }
//    }

//    public static void Log(object message, Object context)
//    {
//        if (EnableLog)
//        {
//            if (Application.platform==RuntimePlatform.WindowsEditor||Application.platform==RuntimePlatform.OSXEditor)
//            {
//                Debug.Log(message, context);
//            }
//            else
//            {
//                byte[] data = Encoding.Default.GetBytes(message.ToString());
//                udpSocket.SendData("", data, 18001);
//            }
//        }
//    }
//    public static void Log(object message, float context)
//    {
//        if (EnableLog)
//        {
//            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
//            {
//                Debug.Log(message, context);
//            }
//            else
//            {
//                byte[] data = Encoding.Default.GetBytes(message.ToString());
//                udpSocket.SendData("", data, 18001);
//            }
//        }
//    }
//    public static void LogError(object message, Object context)
//    {
//        if (EnableLog)
//        {
//            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
//            {
//                Debug.LogError(message, context);
//            }
//            else
//            {
//                byte[] data = Encoding.Default.GetBytes(message.ToString());
//                udpSocket.SendData("255.255.255.255", data, 18001);
//            }
//        }
//    }
//}
