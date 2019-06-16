using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    /// <summary>
    /// the game manager has the singleton pattern so that any external script can access it
    /// </summary>

    //the game manager is used to keep track of the number of life the player has.
    public static GameManager instance;

    public int lifeCounter;

    //used to store the start position of the player in the game and respawn at the right location
    [HideInInspector]
    public Vector3 levelStartPosition;
    //stores the reference of the player
    public GameObject player;

    void Awake()
    {
        //singleton pattern
        //the gamemanager will be accessible from anywhere
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        UImanager.instance.GameOverUI.SetActive(false);
        lifeCounter = 3;
        UImanager.instance.SetLifeCounter(GameManager.instance.lifeCounter.ToString());
        levelStartPosition = player.transform.position;
    }

    public void DecreaseLifeCounter()
    {
        lifeCounter--;
        if (lifeCounter <= 0)
        {
            Time.timeScale = 0;
            UImanager.instance.ShowGameOverUI();
        }
        //update UI
        UImanager.instance.SetLifeCounter(GameManager.instance.lifeCounter.ToString());
    }

    public void IncreaseLifeCounter()
    {
        lifeCounter++;
        //update UI
        UImanager.instance.SetLifeCounter(GameManager.instance.lifeCounter.ToString());
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        UImanager.instance.SetLifeCounter(GameManager.instance.lifeCounter.ToString());
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
