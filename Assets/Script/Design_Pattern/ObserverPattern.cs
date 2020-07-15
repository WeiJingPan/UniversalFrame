using UnityEngine;
using System.Collections;

public class ObserverPattern : MonoBehaviour 
{
    public Animation anim_attack;
    public float m_tmpTime;
    public bool m_isTesting = false;
    public void PlayAttack()
    {
        anim_attack.Play();
        m_isTesting=true;
    }
    public bool isPlayingAttack()
    {
        return anim_attack.isPlaying;
    }
    void Start()
    {
        PlayAttack();
    }
	void Update () 
    {
        if (m_isTesting&&!isPlayingAttack())
        {
            m_tmpTime += Time.deltaTime;
            if (m_tmpTime > 0.5f)
            {
                Debug.Log("攻击动作已经播放完，播放特效！");
                m_isTesting = false;
            }
        }
	}
}
