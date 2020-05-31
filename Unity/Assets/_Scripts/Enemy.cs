using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    public StateManager stateManager;
    public EnemyManager enemyManager;

    public Animator animator;
    public new Rigidbody rigidbody;
    public new Light light;

    public float speed = 2f;
    public float spawnTime = 3f;
    float timeToSpawn;

    bool spawnTimerStarted = false;
    bool ready = false;

    private void Awake()
    {
        enemyManager.AddEnemy(this);
    }

    private void Start()
    {
        if(stateManager.currentGameState == GameState.PreGame)
        {
            ready = true;
            light.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if(stateManager.currentGameState == GameState.GameActive)
        {
            if (ready)
            {
                HandleMovement();
            }
            else
            {
                if (!spawnTimerStarted)
                {
                    spawnTimerStarted = true;
                    timeToSpawn = Time.time + spawnTime;
                }
                else
                {
                    if (Time.time >= timeToSpawn)
                    {
                        ready = true;
                        light.enabled = true;
                    }
                }
            }
        }
    }


    void HandleMovement()
    {
        animator.SetBool("Moving", true);

        rigidbody.velocity = transform.forward * speed;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, 6f, ~0, QueryTriggerInteraction.Ignore))
        {

            if(hit.transform.tag == "Player")
            {

            }
            else if(hit.distance < 0.55f)
            {
                int turnAmount = Random.Range(1, 5);

                transform.rotation = Quaternion.Euler(0, 90 * turnAmount, 0);
            }
            else if(hit.distance - Utils.RoundToInt(hit.distance) <= 0.005f)
            {
                int chance = Random.Range(1, 100);
                
                if(chance <= 5)
                {
                    int turnAmount = Random.Range(1, 5);
                    transform.rotation = Quaternion.Euler(0, 90 * turnAmount, 0);
                }
            }

        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player" && ready)
        {
            stateManager.gameManager.OnPlayerKilled(collision.gameObject.GetComponent<Player>().playerBrain);
            animator.SetBool("Moving", false);
        }
    }

    private void OnDestroy()
    {
        enemyManager.RemoveEnemy(this);
        stateManager.gameManager.OnEnemyKilled();
    }
}
