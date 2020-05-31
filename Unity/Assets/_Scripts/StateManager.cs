using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StateManager : ScriptableObject {

    public GameManager gameManager;

    public GameState currentGameState = GameState.PreGame;
    public GameMode currentGameMode = GameMode.Multiplayer;



}
