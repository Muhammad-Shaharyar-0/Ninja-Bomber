using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenceManager : MonoBehaviour
{

    public void LoadSinglePlayer()
    {

        SceneManager.LoadScene("Survival");
    }

    public void LoadMultiplayer()
    {

        SceneManager.LoadScene("MyMultiplayer");

    }
    public void QuiteApplication()
    {
        Application.Quit();

    }
}
