using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyNavmesh : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform Player;
    public Transform Enemypos;

    public LayerMask whatIsPlayer;

    public float sightRange;
    public bool playerInSight;
    public bool sneaking;



    void Start()
{

}

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (Input.GetKeyDown("c"))
        {
            sneaking = true;
        }
        if (Input.GetKeyUp("c"))
        {
            sneaking = false;   
        }

        else if (playerInSight == true && sneaking == false)
        {
            enemy.SetDestination(Player.position);
            Debug.Log("Enemy is targeting player");
        }

        else if (playerInSight != true)
        {
            enemy.SetDestination(Enemypos.position);
            Debug.Log("Enemy is standing still");
        }


    }



}
