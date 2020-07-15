using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public float m_blood = 10;
    public float m_aggressivity = 2;
    public virtual void ReduceBlood(float decrement)
    {
        if (m_blood > decrement)
            m_blood -= decrement;
        else
            Debug.Log("该单位已死亡！");
    }
}
