using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    [SerializeField] private float waitTime;
    public float minWaitTime = 3f;
    public float maxWaitTime = 15f;
    public float moveSpeed = 0.7f;
    public bool isTalking;
    private Animator anim;
    private Vector2 direction;

void Start()
{
    anim = GetComponent<Animator>();
}

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
            UpdateAnimationParameters();

        // Check if NPC is waiting
        if (!isWaiting)
        {
            
            // Move towards the current patrol point
            Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;
            direction = (targetPosition - (Vector2)transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Update animation parameters based on movement direction

            // Check if NPC has reached the patrol point
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Start waiting
                isWaiting = true;
                waitTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }
        else
        {
            StandStill();
            // Decrease wait time
            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                isWaiting = false;
                // Move to next patrol point
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    public void StandStill()
    {
            anim.SetFloat("Speed", 0); // Assuming "Speed" parameter is based on magnitude of movement
    }

    public void StartPatrol()
    {
        //  property is a built-in property that controls whether a component is enabled or disabled. When a component is enabled,
        //  it means that its Update() method (and other related methods like FixedUpdate(), LateUpdate(), etc.) will be called
        //  by Unity each frame. When a component is disabled, Unity skips calling these methods for that component.
        enabled = true;
    }

        void UpdateAnimationParameters()
    {
        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Vertical", direction.y);
        anim.SetFloat("Speed", direction.magnitude); // Assuming "Speed" parameter is based on magnitude of movement

        if (Mathf.Abs(direction.x) >= 0.9f || Mathf.Abs(direction.y) >= 0.9f)
        {
            anim.SetFloat("LastMoveX", direction.x);
            anim.SetFloat("LastMoveY", direction.y);
        }
    }

    public void StopPatrol()
    {
        StandStill();
        enabled = false;
    }
}
