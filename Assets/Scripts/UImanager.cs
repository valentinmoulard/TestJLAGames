using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UImanager : MonoBehaviour {
    /// <summary>
    /// The UI manager handles all the UI
    /// It has the singleton patterns so that any external script can access it and update any element of UI
    /// </summary>
    public static UImanager instance;

    public GameObject lifeCounter;
    public GameObject coinCounter;

    public GameObject healthBar;
    public GameObject inGameUI;

    public GameObject MissionUI;
    public GameObject GameOverUI;
    public GameObject StageClearUI;

    void Awake()
    {
        //singleton pattern
        //the UI manager will be accessible from anywhere
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    void Start()
    {
        //show the mission text when the game starts
        StartCoroutine(MissionTextCoroutine());
    }

    /// <summary>
    /// the following functions are used to update the amount of coins, hearts and the health bar
    /// </summary>
    /// <param name="number"></param>
    public void SetCoinCounter(string number)
    {
        coinCounter.GetComponent<TextMeshProUGUI>().SetText("x " + number);
    }
    public void SetLifeCounter(string number)
    {
        lifeCounter.GetComponent<TextMeshProUGUI>().SetText("x " + number);
    }

    public void UpdateHealthBar(int health)
    {
        switch (health)
        {
            case 3:
                for (int i = 0; i < health; i++)
                {
                    healthBar.transform.GetChild(i).gameObject.SetActive(true);
                }
                break;
            case 2:
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 0:
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// the following functions are used to display the correct UI at the right state of the game (Game Over or Stage Clear)
    /// </summary>
    public void ShowGameOverUI()
    {
        inGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowStageClearUI()
    {
        inGameUI.SetActive(false);
        StageClearUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator MissionTextCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        MissionUI.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        MissionUI.SetActive(false);
    }

}
