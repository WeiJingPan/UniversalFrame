using UnityEngine;
using System.Collections;
using System;

public class AttackerPattern : MonoBehaviour
{

    public Player player;
    public NPC npc;

    public float attackDistance = 3;

    void Start()
    {

    }

    void Update()
    {

    }

    public void CalculateAttack(Entity attacker, Entity attacked)
    {
        Transform transf_attacker = attacker.transform;
        Transform transf_attacked = attacked.transform;
        float angle = Vector3.Dot(transf_attacker.forward, (transf_attacker.position - transf_attacked.position).normalized);
        float distance = Vector3.Distance(transf_attacker.position, transf_attacked.position);
        if (angle > 0 && distance < attackDistance)
        {
            attacked.ReduceBlood(attacker.m_aggressivity);
        }
    }
}


