using UnityEngine;
using System.Collections;
using ProtoBuf;
using System.IO;

public class IProtoTools
{
    /// <summary>
    /// 序列化给socket发送
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] Serialize(IExtensible msg)
    {
        byte[] result;
        using(var stream =new MemoryStream())
        {
            Serializer.Serialize(stream, msg);
            result = stream.ToArray();
        }
        return result;
    }
    /// <summary>
    ///  接收socket消息反序列化
    /// </summary>
    /// <typeparam name="IExtensible"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static IExtensible Deserialize<IExtensible>(byte[] message)
    {
        IExtensible result;
        using(var stream =new MemoryStream(message))
        {
            result = Serializer.Deserialize<IExtensible>(stream);
        }
        return result;
    }
}
