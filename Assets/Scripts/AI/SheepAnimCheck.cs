using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepAnimCheck : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    
    void Update()
    {
        if(agent.enabled && agent.destination != null)
        {
            animator.Play("Walk");
        }
        else
        {
            animator.Play("Idle");
        }
    }
}
