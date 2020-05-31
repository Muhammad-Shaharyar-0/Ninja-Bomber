using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public ParticleSystem mainParticle;
    public ParticleSystem[] subParticles;
    public PlayerBrain ownerBrain;
    public GameManager gameManager;
    public StateManager stateManager;
    public FloatVariable BombRange;
    public FloatVariable bombTimerLength;
    public Rigidbody rigidbody;
    public float explosionTick;
    bool exploded = true;
    public BoolVariable CanKick;
    public AudioSource audioSource;
    public AudioClip beepingSound;
    public AudioClip explosionSound;
    int ColliderCount;
    private void OnEnable()
    {
        //TODO particles and systems
        gameManager = stateManager.gameManager;

        explosionTick = Time.time + bombTimerLength.value;

        var particle = mainParticle.main;
        particle.startLifetime = bombTimerLength.value;

        for (int i = 0; i < subParticles.Length; i++)
        {

            var emitter = subParticles[i].main;
            emitter.startDelay = bombTimerLength.value;
            if (BombRange.value == 1f)
            {
                emitter.startLifetime = 0.21f;
                emitter.startSpeed = new ParticleSystem.MinMaxCurve(1, 7);
            }
            if (BombRange.value == 2f)
            {
                emitter.startLifetime = 0.32f;
                emitter.startSpeed = new ParticleSystem.MinMaxCurve(1, 8);
            }
            if (BombRange.value == 3f)
            {
                emitter.startLifetime = 0.32f;
                emitter.startSpeed = new ParticleSystem.MinMaxCurve(1, 11);
            }
            if (BombRange.value == 4f)
            {
                emitter.startLifetime = 0.29f;
                emitter.startSpeed = new ParticleSystem.MinMaxCurve(1, 15);
            }
            if (BombRange.value == 5f)
            {
                emitter.startLifetime = 0.27f;
                emitter.startSpeed = new ParticleSystem.MinMaxCurve(1, 20);
            }
            if (BombRange.value == 6f)
            {
                emitter.startLifetime = 0.32f;
                emitter.startSpeed = new ParticleSystem.MinMaxCurve(1, 20);
            }
            if (BombRange.value == 7f)
            {
                emitter.startLifetime = 0.37f;
                emitter.startSpeed = new ParticleSystem.MinMaxCurve(1, 20);
            }

        }

        mainParticle.Play();

        exploded = false;

        if (ownerBrain)
        {
            ownerBrain.AddActiveBomb(this);
        }
        if (CanKick.value == false)
        {
            rigidbody.isKinematic = true;
            gameObject.GetComponent<Collider>().enabled = false;
        }
        else
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BreakableBlock"))
        {
            rigidbody.isKinematic = true;
        }
        if (other.CompareTag("Walls"))
        {
            rigidbody.isKinematic = true;

        }

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            rigidbody.isKinematic = true;

        }
        if (other.CompareTag("Bomb"))
        {
            rigidbody.isKinematic = true;

        }
        if (other.CompareTag("Player"))
        {
            if (CanKick.value == true)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                if (other.gameObject.transform.rotation.y == 1)
                {
                    rigidbody.constraints = RigidbodyConstraints.None;
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;

                }
                if (other.gameObject.transform.rotation.y == 0)
                {
                    rigidbody.constraints = RigidbodyConstraints.None;
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
                }
                if (other.gameObject.transform.rotation.y == 0.7071068f)
                {
                    rigidbody.constraints = RigidbodyConstraints.None;
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
                }
            }

        }

    }
    void FixedUpdate()
    {

        if (rigidbody.mass == 1.5f)
        {
            Debug.Log("error");
            explosionTick = 0;
            var particle = mainParticle.main;
            particle.startDelay = 0;
            for (int i = 0; i < subParticles.Length; i++)
            {

                var emitter = subParticles[i].main;
                emitter.startDelay = 0;
            }
        }
        if (!exploded && Time.time > explosionTick)
        {
            exploded = true;
            HandleExplosion();
        }

    }


    void HandleExplosion()
    {
        audioSource.PlayOneShot(explosionSound);
        Debug.Log("Exploding.");



        RaycastHit hit;

        if (Utils.RoundedVector3(gameManager.player1.playerGO.transform.position).x == transform.position.x && Utils.RoundedVector3(gameManager.player1.playerGO.transform.position).z == transform.position.z)
        {
            gameManager.OnPlayerKilled(gameManager.player1);
        }

        if (gameManager.player2.playerGO && Utils.RoundedVector3(gameManager.player2.playerGO.transform.position) == transform.position)
        {
            gameManager.OnPlayerKilled(gameManager.player2);
        }

        if (gameManager.gameMode == GameMode.Singleplayer)
        {
            for (int i = 0; i < gameManager.enemyManager.spawnedEnemies.Count; i++)
            {
                if (Utils.RoundedVector3(gameManager.enemyManager.spawnedEnemies[i].transform.position).x == transform.position.x && Utils.RoundedVector3(gameManager.enemyManager.spawnedEnemies[i].transform.position).x == transform.position.x)
                {
                    Destroy(gameManager.enemyManager.spawnedEnemies[i].gameObject);
                }
            }
        }



        for (int i = 0; i < 4; i++)
        {

            transform.rotation = Quaternion.Euler(0, 90 * i, 0);
            Debug.DrawRay(transform.position, transform.forward * 2f, Color.green, 60f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, BombRange.value))
            {
                Debug.Log("I hit " + hit.transform.name);

                if (hit.transform.tag == "Player")
                {
                    gameManager.OnPlayerKilled(hit.transform.GetComponent<Player>().playerBrain);
                }
                else if (hit.transform.tag == "BreakableBlock")
                {
                    
                    ownerBrain.playerScore.value++;
                    ownerBrain.BlocksDestroyed.value++;
                    Destroy(hit.transform.gameObject,0.1f);
                }
                else if (hit.transform.tag == "Enemy")
                {
                    Destroy(hit.transform.gameObject);
                    ownerBrain.playerScore.value++;
                    ownerBrain.playerScore.value++;
                }
                if (hit.transform.tag == "Bomb")
                {
                    hit.transform.gameObject.GetComponent<Rigidbody>().mass = 1.5f;
                }
            }

        }


        Destroy(gameObject, 0.5f);
    }


    private void OnDestroy()
    {
        if (ownerBrain)
        {
            ownerBrain.RemoveBomb(this);
        }
    }

}
