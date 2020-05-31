using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public PlayerBrain player1;
    public PlayerBrain player2;

    public BlockManager blockManager;
    public StateManager stateManager;
    public EnemyManager enemyManager;

    public RectTransform gameStatusText;

    public Text countdownTimerText;

    int countdownTimer = 3;

    float nextCountdownTick = 0;

    public AudioSource audioSource;
    public AudioClip countdownTick;
    public AudioClip countdownEnd;
    public AudioClip music;

    float roundStartTime = 0;
    public FloatVariable roundTimer;

    public GameMode gameMode;

    public float startingRandomizedBlocks = 8;


    public float escalationStartTime = 10f;
    public float escalationFrequency = 5f;
    float nextEscalationTick;
    public FloatVariable escalationTier;
    public GameObject neutralBomb;
    public FloatVariable Enemyspawntimer;
    public FloatVariable Blockspawntimer;
    int NoofEnemiesToSpawn=1;

    // Use this for initialization
    void Awake () {

        player1.gameManager = this;
        player2.gameManager = this;

        stateManager.gameManager = this;

        stateManager.currentGameState = GameState.PreGame;

        roundTimer.value = 0;
        countdownTimer = 3;

        escalationTier.value = 1;
        nextEscalationTick = escalationStartTime;

        if(gameMode == GameMode.Singleplayer)
        {
            player1.playerScore.value = 0;
            player2.playerScore.value = 0;
        }
        Blockspawntimer.value = 0;
        Enemyspawntimer.value = 0;
        NoofEnemiesToSpawn = 1;
        nextEscalationTick = 60;
        Debug.Log("sad0");
    }

    private void Start()
    {
        SpawnRandomBlocks(startingRandomizedBlocks);
    }

    // Update is called once per frame
    void Update () {

        if(stateManager.currentGameState == GameState.PreGame && player1.ready ==true)
        {
            stateManager.currentGameState = GameState.PlayersReady;
            nextCountdownTick = Time.time + 1;
            gameStatusText.gameObject.SetActive(false);
            audioSource.PlayOneShot(countdownTick);
            countdownTimerText.text = countdownTimer + "";
            Debug.Log("sad");
        }
        else if(stateManager.currentGameState == GameState.PlayersReady)
        {
            if(Time.time > nextCountdownTick)
            {
                CountdownTick();
                nextCountdownTick = Time.time + 1;
            }
        }
        else if(stateManager.currentGameState == GameState.GameActive)
        {
            roundTimer.value = Utils.RoundToInt(Time.time - roundStartTime);
            if(roundTimer.value== nextEscalationTick)
            {
                NoofEnemiesToSpawn++;
                nextEscalationTick += 60;
            }
            if(enemyManager.spawnedEnemies.Count == 0)
            {
                bool Enemypawned = false;

                int attempts = 0;
                while (!Enemypawned)
                {
                    int x = Random.Range(-4, 5);
                    int z = Random.Range(-3, 6);
                    Vector3 position = new Vector3(x, 0, z);

                    int enemyNumber = Random.Range(0, enemyManager.enemyTypes.Count);

                    if (PositionIsOpen(position))
                    {
                        Enemypawned = true;
                        GameObject.Instantiate(enemyManager.enemyTypes[enemyNumber], position, Quaternion.identity);
                    }

                    if (attempts > 50)
                        break;

                    attempts++;
                }
                Enemyspawntimer.value = roundTimer.value;
            }
            else if (Enemyspawntimer.value + 10 <= roundTimer.value && enemyManager.spawnedEnemies.Count<5)
            {
                for (int i = 0; i < NoofEnemiesToSpawn; i++)
                {
                    bool Enemypawned = false;

                    int attempts = 0;
                    while (!Enemypawned)
                    {
                        int x = Random.Range(-4, 5);
                        int z = Random.Range(-3, 6);
                        Vector3 position = new Vector3(x, -0.35f, z);

                        int enemyNumber = Random.Range(0, enemyManager.enemyTypes.Count);

                        if (PositionIsOpen(position))
                        {
                            Enemypawned = true;
                            GameObject.Instantiate(enemyManager.enemyTypes[enemyNumber], position, Quaternion.identity);
                        }

                        if (attempts > 50)
                            break;

                        attempts++;
                    }
                   
                }
                Enemyspawntimer.value = roundTimer.value;

            }
            if (Blockspawntimer.value + 5 <= roundTimer.value)
            {
                for (int i = 0; i < NoofEnemiesToSpawn; i++)
                {
                    bool blockSpawned = false;

                    int attempts = 0;

                    while (!blockSpawned)
                    {
                        int x = Random.Range((int)blockManager.minBlock.x, (int)blockManager.maxBlock.x);
                        int z = Random.Range((int)blockManager.minBlock.y, (int)blockManager.maxBlock.y);

                        Vector3 position = new Vector3(x, 0, z);

                        if (PositionIsOpen(position))
                        {
                            blockSpawned = true;
                            GameObject.Instantiate(blockManager.breakableBlockPrefab, position, Quaternion.identity);
                        }

                        if (attempts > 50)
                            blockSpawned = true;

                        attempts++;

                    }
                    
                }
                Blockspawntimer.value = roundTimer.value;
            }

        }
        else if(stateManager.currentGameState == GameState.GameOver)
        {
            if(Input.GetKey(player1.bombKey) || Input.GetKey(player2.bombKey))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
       
    }


    void CountdownTick()
    {
        countdownTimer--;

        if(countdownTimer == 0)
        {
            audioSource.PlayOneShot(countdownEnd);
            stateManager.currentGameState = GameState.GameActive;
            countdownTimerText.text = "";
            roundStartTime = Time.time;

            if (music)
            {
                audioSource.clip = music;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            countdownTimerText.text = countdownTimer + "";
            audioSource.PlayOneShot(countdownTick);
        }
    }

    public void OnPlayerKilled(PlayerBrain playerKilled)
    {
   
            if (gameMode == GameMode.Multiplayer)
            {

                if (playerKilled == player1)
                {
                    Debug.Log("Player 2 Wins.");
                    countdownTimerText.fontSize = 30;
                    countdownTimerText.text = "Player 2 Wins! \n Hit Bomb Key to Restart";
                    player2.playerScore.value++;
                }
                else if (playerKilled == player2)
                {
                    Debug.Log("Player 1 Wins.");
                    countdownTimerText.fontSize = 30;
                    countdownTimerText.text = "Player 1 Wins! \n Hit Bomb Key to Restart";
                    player1.playerScore.value++;
                }
            }
            else
            {
                countdownTimerText.fontSize = 30;
                countdownTimerText.text = "Game Over \n Hit Bomb Key to Restart";
                player1.SaveData();
            }

        stateManager.currentGameState = GameState.GameOver;
        player1.ready = false;
        player2.ready = false;

    }

    bool PlayersReady()
    {

        if(player1.ready )
        {
            Debug.Log("sad2");
            return true;
        }

        return false;
    }

    private void SpawnRandomBlocks(float count)
    {

        if (!blockManager.breakableBlockPrefab)
            return;

        for(int i = 0; i < count; i++)
        {
            bool blockSpawned = false;

            int attempts = 0;

            while (!blockSpawned)
            {
                int x = Random.Range((int)blockManager.minBlock.x, (int)blockManager.maxBlock.x);
                int z = Random.Range((int)blockManager.minBlock.y, (int)blockManager.maxBlock.y);

                Vector3 position = new Vector3(x, 0, z);

                if (PositionIsOpen(position))
                {
                    blockSpawned = true;
                    GameObject.Instantiate(blockManager.breakableBlockPrefab, position, Quaternion.identity);
                }

                if (attempts > 50)
                    blockSpawned = true;

                attempts++;

            }

        }
        
    }


    bool PositionIsOpen(Vector3 position)
    {

        if (blockManager.activeBlocksDictionary.ContainsKey(position))
        {
            return false;
        }

        if (player1.playerGO && position == Utils.RoundedVector3(player1.playerGO.transform.position))
        {
            return false;
        }

        if(player2.playerGO && position == Utils.RoundedVector3(player2.playerGO.transform.position))
        {
            return false;
        }


        return true;
    }

    public void OnEnemyKilled()
    {
        if (stateManager.currentGameState == GameState.GameActive)
        {
        }
    }


    



}
