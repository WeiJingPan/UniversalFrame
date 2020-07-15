using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour 
{
    public enum FSMStateId
    {
        Idle,
        Attack,
        Run,
        MaxValue,
    }
    FSMManager manager;
    void Start()
    {
        RunFSM tmpRunFSM = new RunFSM();
        AttackFSM tmpAttackFSM = new AttackFSM();

        manager = new FSMManager(2);
        manager.AddState(tmpRunFSM);
        manager.AddState(tmpAttackFSM);
    }
    public void PlayAttack()
    {

    }
}
public abstract class FSMState
{
    private byte stateId;

    public virtual void OnBeforEnter() { }

    public virtual void CopyState(FSMState stateId) { }
    public virtual void OnEnter() { }
    public virtual void Update() { }
    public virtual void OnLeave() { }
}
public class AttackFSM : FSMState
{
    public override void OnEnter()
    {
    }
}
public class RunFSM : FSMState
{
    public override void OnEnter()
    {
    }
}
public class FSMManager
{
    private FSMState[] fsmManager;
    private byte curAdd;
    public byte curStateId;
    public FSMManager(byte stateNumber)
    {
        curAdd = 0;
        curStateId = 0;
        fsmManager = new FSMState[stateNumber];
    }
    public void AddState(FSMState state)
    {
        if (curAdd < fsmManager.Length)
        {
            fsmManager[curAdd] = state;
            curAdd++;
        }
    }
    public void ChangeState(byte stateId)
    {
        fsmManager[curStateId].OnLeave();
        fsmManager[stateId].CopyState(fsmManager[curStateId]);
        fsmManager[stateId].OnBeforEnter();
        fsmManager[stateId].OnEnter();
        curStateId = stateId;
    }
    public void Udpate()
    {
        fsmManager[curStateId].Update();
    }
}
