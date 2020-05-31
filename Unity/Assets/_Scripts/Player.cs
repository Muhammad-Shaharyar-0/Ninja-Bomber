using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public new Rigidbody rigidbody;
    public PlayerBrain playerBrain;
    public StateManager stateManager;
    public FixedJoystick joystick;
    public Animator animator;
    public Light playerLight;
    public bool AbleToKick=false;
    Quaternion newRotation;
    Vector3 newVelocity;
    Vector3 direction;
    // Use this for initialization
    void Start () {

        playerBrain.playerGO = gameObject;
        playerLight.color = playerBrain.lightColor;

	}

    // Update is called once per frame
    void Update() {

        PlayerPrefs.SetFloat("maxActiveBombs", 3);
        animator.SetBool("Moving", false);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPlayer
        if (Input.GetKey(playerBrain.upKey))
        {
            newVelocity = new Vector3(0, 0, playerBrain.movementSpeed.value);
            newRotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Moving", true);
        }

        if (Input.GetKey(playerBrain.downKey))
        {
            newVelocity = new Vector3(0, 0, -playerBrain.movementSpeed.value);
            newRotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Moving", true);
        }

        if (Input.GetKey(playerBrain.rightKey))
        {
            newVelocity = new Vector3(playerBrain.movementSpeed.value, 0, 0);
            newRotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Moving", true);
        }

        if (Input.GetKey(playerBrain.leftKey))
        {
            newVelocity = new Vector3(-playerBrain.movementSpeed.value, 0, 0);
            newRotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Moving", true);
        }

        if (Input.GetKey(playerBrain.leftKey))
        {
            newVelocity = new Vector3(-playerBrain.movementSpeed.value, 0, 0);
            newRotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Moving", true);

        }
        if(Input.GetKey(playerBrain.bombKey))
        {
            TryDropingBomb();
        }

#endif

        direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        if (direction.sqrMagnitude > 0.1)
        {
            float  angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            newRotation = Quaternion.Euler(0, angle, 0 );
            animator.SetBool("Moving", true);
        }

        //Debug.Log(direction.x);
        // Debug.Log(direction.y);
        // Debug.Log(direction.z);


    }
    public void TryDropingBomb()
    {
        if (stateManager.currentGameState == GameState.GameActive)
            DropBomb();
        else if (stateManager.currentGameState == GameState.PreGame)
        {
            Debug.Log("sad-1");
            playerBrain.ready = true;
            playerLight.enabled = true;
        }
    }
    private void FixedUpdate()
    {

        if (stateManager.currentGameState == GameState.GameActive)
        {
            rigidbody.velocity= newVelocity;
            rigidbody.AddForce(direction * playerBrain.movementSpeed.value, ForceMode.VelocityChange);
            transform.rotation = newRotation;
        }

        newVelocity = Vector3.zero;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPlayer
        if (newVelocity != Vector3.zero&&  stateManager.currentGameState == GameState.GameActive)
        {
            rigidbody.velocity = newVelocity;
            rigidbody.AddForce(direction * playerBrain.movementSpeed.value, ForceMode.VelocityChange);
            transform.rotation = newRotation;
            
        }
        newVelocity = Vector3.zero;
#endif
    }


    void DropBomb()
    {
        Debug.Log("Dropping bomb "+ playerBrain.activeBombs.Count);
       
        if(Time.time > playerBrain.nextBombTime && playerBrain.activeBombs.Count < playerBrain.maxActiveBombs.value)
        {
            GameObject.Instantiate(playerBrain.bombPrefab, new Vector3(Utils.RoundToInt(transform.position.x), Utils.RoundToInt(transform.position.y+1f), Utils.RoundToInt(transform.position.z)), Quaternion.identity);
            playerBrain.nextBombTime = Time.time + playerBrain.bombPlacementDelay.value;
        }
    }
}
