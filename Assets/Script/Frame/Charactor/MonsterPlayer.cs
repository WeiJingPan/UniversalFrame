using UnityEngine;
using System.Collections;

public enum CharactorEvent
{
    Initial=ManagerID.CharactorMnanger+1,
    eIdle,
    eRun,
    eAttack,
    eAttackBig,
    eJump,
    eDie,
    eJoyStickBegin,
    eJoyStick,
    eJoyStickEnd,

    eLooseBlood,

    eMaxValue,
}
public enum CharactorEventTwo
{
    eInitial=CharactorEvent.eMaxValue+1,
    eChangeModel,
    eMaxValue,
}
public class MonsterPlayerData
{
    float blood = 100;
    public void ReduceBlood(float tmpBlood)
    {
        blood -= tmpBlood;
    }
    public float GetBlood()
    {
        return blood / 100.0f;
    }
}
[RequireComponent(typeof(CharacterController))]
public class MonsterPlayer:CharactorBase
{
    public CharacterController controlMove;
    Vector3 moveDirector = Vector3.zero;
    Vector3 moveSpeed = Vector3.zero;
    float tmpSpeed;

    MonsterPlayerData monsterData;

    public MsgFloat controlMsg;
    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)CharactorEvent.eJoyStickBegin,
            (ushort)CharactorEvent.eJoyStick,
            (ushort)CharactorEvent.eJoyStickEnd,
            (ushort)CharactorEvent.eLooseBlood,
        };
        RegistSelf(this, msgIds);
    }
    private void Start()
    {
        controlMove = transform.GetComponent<CharacterController>();
        controlMsg = new MsgFloat((ushort)CharactorEvent.eRun,0);
        monsterData = new MonsterPlayerData();
    }
    void MoveCtr(Vector2 joyStick)
    {
        //人物移动
        float speedX = Mathf.Abs(joyStick.x);
        float speedY = Mathf.Abs(joyStick.y);
        tmpSpeed = Mathf.Sqrt(speedX * speedX + speedY * speedY);
        moveSpeed.x = joyStick.x;
        moveSpeed.y = joyStick.y;
        moveSpeed.z = 0;
        controlMove.SimpleMove(moveSpeed * tmpSpeed);

        //旋转角度
        float angle = Mathf.Atan2(joyStick.x, joyStick.y);
        angle = angle * Mathf.Rad2Deg;
        moveDirector.y = angle;
        transform.rotation = Quaternion.Euler(moveDirector);
    }

    public override void ProccessEvent(MsgBase Msg)
    {
        switch (Msg.msgId)
        {
            case (ushort)CharactorEvent.eJoyStickBegin:
                controlMsg.ChangeEventId((ushort)CharactorEvent.eRun);
                controlMsg.value = tmpSpeed;
                SendMsg(controlMsg);
                break;
            case (ushort)CharactorEvent.eJoyStick:
                MsgVector2 tmpMsg = (MsgVector2)Msg;
                MoveCtr(tmpMsg.joyStick);

                controlMsg.ChangeEventId((ushort)CharactorEvent.eRun);
                controlMsg.value = tmpSpeed;
                SendMsg(controlMsg);

                break;
            case (ushort)CharactorEvent.eJoyStickEnd:
                controlMsg.ChangeEventId((ushort)CharactorEvent.eIdle);
                controlMsg.value = 0;
                SendMsg(controlMsg);
                break;
            case (ushort)CharactorEvent.eLooseBlood:
                MsgFloat bloodMsg = (MsgFloat)Msg;
                monsterData.ReduceBlood(bloodMsg.value);
                MsgFloat tmp = new MsgFloat((ushort)CharactorEvent.eLooseBlood, monsterData.GetBlood());
                SendMsg(tmp);
                break;
            default:
                break;
        }
    }
}
