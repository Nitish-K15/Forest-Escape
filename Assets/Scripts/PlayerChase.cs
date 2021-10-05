using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerChase : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator anim;
    public Vector3 startPos;
    private AudioSource asrc;
    FirstPersonController fps;

    private void OnEnable()
    {
        Menus.OnClicked += StartingPos;
    }

    private void OnDisable()
    {
        Menus.OnClicked -= StartingPos;
    }
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        asrc = GetComponent<AudioSource>();
        fps = player.GetComponent<FirstPersonController>();
    }

    void StartingPos() //PLace back at starting position
    {
        agent.transform.position = startPos;
    }
    private void Update()
    {
        if (fps.isChaseable == true && fps.CanMove) //chase player if he is out and game is not paused
        {
            agent.SetDestination(player.transform.position);
            if (agent.remainingDistance < 1)
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                anim.SetBool("isMoving", false);
                anim.SetTrigger("Attack");
            }
            else
            {
                agent.isStopped = false;
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
            agent.velocity = Vector3.zero;
            anim.SetBool("isMoving", false);
            agent.isStopped = true;
        }
    }
}
