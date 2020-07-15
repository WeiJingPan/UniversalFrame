using UnityEngine;
using System.Collections;

public class StatePattern : MonoBehaviour 
{
    public Animator m_animator;

    public void PlayAttack()
    {
        m_animator.SetBool("playAttack", true);
    }
    public void PlayRun()
    {
        m_animator.SetBool("playRun", true);
    }
}
