using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerBrain : ScriptableObject {

    public GameManager gameManager;
    public int playerNumber;
    public GameObject playerGO;
    public FloatVariable playerScore;
    public FloatVariable maxActiveBombs;
    public FloatVariable BlocksDestroyed;
    public FloatVariable HighScore;
    public FloatVariable MovementSpeed;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode bombKey;

    public bool ready = false;

    public Color lightColor = Color.green;

    public FloatVariable movementSpeed;

    public GameObject bombPrefab;
    public FloatVariable bombPlacementDelay;
    public float nextBombTime = 0;

    public List<Bomb> activeBombs = new List<Bomb>();

   

    private void OnEnable()
    {
        nextBombTime = 0;
        ready = false;
        BlocksDestroyed.value= PlayerPrefs.GetFloat("BlocksDestroyed");
        maxActiveBombs.value = PlayerPrefs.GetFloat("maxActiveBombs");
        bombPlacementDelay.value = PlayerPrefs.GetFloat("bombPlacementDelay");
        HighScore.value = PlayerPrefs.GetFloat("HighScore");
        MovementSpeed.value = PlayerPrefs.GetFloat("MovementSpeed");
        activeBombs.Clear();
    }
    public void SaveData()
    {
        if (playerScore.value > HighScore.value)
        {
            PlayerPrefs.SetFloat("HighScore", HighScore.value);
        }
        PlayerPrefs.SetFloat("BlocksDestroyed", BlocksDestroyed.value);
        PlayerPrefs.SetFloat("maxActiveBombs", maxActiveBombs.value);
        PlayerPrefs.SetFloat("bombPlacementDelay", bombPlacementDelay.value);
        PlayerPrefs.SetFloat("MovementSpeed", MovementSpeed.value);
    }
    public void AddActiveBomb(Bomb bomb)
    {
        if (!activeBombs.Contains(bomb))
        {
            activeBombs.Add(bomb);
        }
    }

    public void RemoveBomb(Bomb bomb)
    {
        if (activeBombs.Contains(bomb))
        {
            activeBombs.Remove(bomb);
        }
    }




}
