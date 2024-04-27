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
    private bool stunned;
    //for field of view
    public float radius = 7;
    [Range(0, 360)]
    public float angle = 100;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask ObstructionMask;
    Animator animator;

    public bool canSeePlayer;
    AudioManager audioManager;

    bool playaudio = true;


    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    private void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        // Changed this to use new input method
        if (InputControls.getSneak()) 
        {
            sneaking = true;
        } 
        else {
            sneaking = false;
        }

        // Changed from else if to a new set of if statements
        if (playerInSight == true && sneaking == false && canSeePlayer == true)
        {
            if (playaudio == true)
            {
                audioManager.PlaySFX(audioManager.guardAlert);
                Debug.Log("audio");
                playaudio = false;
            }
            Invoke("guardaudio", 10);
            enemy.SetDestination(Player.position);
            animator.SetFloat("Walk", 1);
            Debug.Log("Enemy is targeting player");
        }

        else if (playerInSight != true)
        {
            enemy.SetDestination(Enemypos.position);
            animator.SetFloat("Walk", 0);
            Debug.Log("Enemy is standing still");
        }

        if (stunned == true)
        {
            Debug.Log("The Enemy is stunned");
            enemy.SetDestination(Enemypos.position);
            Invoke("stunevent", 5);
        }
    }
    private void guardaudio()
    {
        playaudio = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "sleep")
        {
            Debug.Log("Entered collision with " + other.gameObject.name);
            stunned = true;
        }
    }

    private void stunevent()
    {
        stunned = false;
    }

   //Start FOV script Addition
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        animator = GetComponent<Animator>();
    }


    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
    //end FOV Script Addition
}

