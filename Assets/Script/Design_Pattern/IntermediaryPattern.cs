using UnityEngine;
using System.Collections;

public class IntermediaryPattern : MonoBehaviour 
{
    float tmpDistance;
    Player player;
    NPC npc;
    public void ReduceBlood(Entity attacker, Entity attacked)
    {
        tmpDistance = Vector3.Distance(attacker.transform.position, attacked.transform.position);
        if (tmpDistance < 2)
        {
            attacked.ReduceBlood(attacker.m_aggressivity);
        }
    }
}
