using UnityEngine;
using System.Collections;

    public enum ManagerID
    {
        GameManager=0,
        UIManager = FrameTools.MsgSpan,
        AudioManager = FrameTools.MsgSpan * 2,
        NPCManager = FrameTools.MsgSpan * 3,
        CharactorMnanger = FrameTools.MsgSpan * 4,
        AssestMnanger = FrameTools.MsgSpan * 5,
        NetManager = FrameTools.MsgSpan * 6,
    }
public class FrameTools
{
    public const int MsgSpan = 3000;
	
}
