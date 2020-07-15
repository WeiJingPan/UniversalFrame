using UnityEngine;
using System.Collections;

public class DelegatePattern : MonoBehaviour 
{
    DelegatePlayer player = new DelegatePlayer();
    public void Finished()
    {
        Debug.Log("攻击动作已经播放完，播放特效！");
    }
    void Start()
    {
        player.PlayAttack();
        player.Finished = Finished;
    }
}
public class DelegatePlayer
{
    public delegate void isFinish();
    public isFinish Finished;

    public Animation anim_attack;
    public float m_tmpTime;
    public void PlayAttack()
    {
        anim_attack.Play();
    }
    public bool isPlayingAttack()
    {
        return anim_attack.isPlaying;
    }
    void Update()
    {
        if (!isPlayingAttack())
        {
            m_tmpTime += Time.deltaTime;
            if (m_tmpTime > 0.5f)
            {
                Finished();
            }
        }
    }
}
