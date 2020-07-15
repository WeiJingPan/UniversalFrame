using UnityEngine;
using System.Collections;
using System;

public class JoyStickCtr: UIBase
{
    MsgVector2 directMsg;
    MsgBase buttonMsg;

    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)EUIEvent.Load,
        };
        RegistSelf(this, msgIds);
    }
    void Start()
    {
        EasyJoystick.On_JoystickMoveStart += OnJoyStickMoveBegin;
        EasyJoystick.On_JoystickMove += OnJoyStickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoyStickMoveEnd;

        EasyButton.On_ButtonDown += OnButtonPressed;
        EasyButton.On_ButtonUp += OnButtonUp;

        directMsg = new MsgVector2((ushort)CharactorEvent.eJoyStick, Vector2.zero);
        buttonMsg = new MsgBase((ushort)CharactorEvent.eAttack);
    }

    void OnButtonPressed(string buttonName)
    {
        buttonMsg.ChangeEventId((ushort)CharactorEvent.eAttack);
        SendMsg(buttonMsg);
    }
    void OnButtonUp(string buttonName)
    {
        buttonMsg.ChangeEventId((ushort)CharactorEvent.eIdle);
        SendMsg(buttonMsg);
    }

    void OnJoyStickMoveBegin(MovingJoystick move)
    {
        directMsg.ChangeEventId((ushort)CharactorEvent.eJoyStickBegin);
        SendMsg(directMsg);
    }
    void OnJoyStickMove(MovingJoystick move)
    {
        Debug.Log(move.joystickAxis);
        directMsg.joyStick = move.joystickAxis;
        SendMsg(directMsg);
    }
    void OnJoyStickMoveEnd(MovingJoystick move)
    {
        directMsg.ChangeEventId((ushort)CharactorEvent.eJoyStickEnd);
        SendMsg(directMsg);
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)EUIEvent.Load:
                break;
            default:
                break;
        }
    }
}
