using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinish : MonoBehaviour {

    /// <summary>
    /// when the player enters the 'stage clear' area, shows the stage clear UI, disables the player's controls, play victory animation.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UImanager.instance.ShowStageClearUI();
            PlayerController playercontroller = other.gameObject.GetComponent<PlayerController>();
            playercontroller.PlayVictoryAnimation();
            playercontroller.controlsEnabled = false;
            playercontroller.StopPlayer();
        }
    }

}
