using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {

    Animator animator;
    PlayerController playerController;

    [HideInInspector]
    public int healthPoint;
    [HideInInspector]
    public int coinsHeld;
    //when hit, the player becomes invincible for a short moment
    public int invincibilityTime;
    private bool invincible;

    //allow to set how hard it is to beat an enemy
    //defines if the player must jump right over and be really strict
    //or let it to be more permissive
    //0.1 seems to be balanced
    [Range(0f,0.3f)]
    public float offset;

	// Initialize the player's stats
	void Start () {
        healthPoint = 3;
        UImanager.instance.UpdateHealthBar(healthPoint);
        coinsHeld = 0;
        UImanager.instance.SetCoinCounter(coinsHeld.ToString());
        invincible = false;
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
	

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectibleCoin"))
        {
            coinsHeld++;
            if (coinsHeld >= 10)
            {
                coinsHeld = 0;
                GameManager.instance.IncreaseLifeCounter();
            }
            UImanager.instance.SetCoinCounter(coinsHeld.ToString());
            other.gameObject.GetComponent<Collectible>().CollectedItem();
        }
        if (other.CompareTag("CollectibleHeart"))
        {
            GameManager.instance.IncreaseLifeCounter();
            other.gameObject.GetComponent<Collectible>().CollectedItem();
        }
        if (other.CompareTag("LevelLimit"))
        {
            StartCoroutine(Invincibility(invincibilityTime));
            GameManager.instance.DecreaseLifeCounter();
            Respawn();
        }
    }

    /// <summary>
    /// when the player hits the enemy, the height of the player is check to determine if the enemy is killed or the player takes damage
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && invincible == false)
        {
            PlayerController playerController = gameObject.GetComponent<PlayerController>();
            //checks the contact point and if it's above the top half of the enemy, the enemy is killed, else the health bar of the player is decreased by one
            if (collision.contacts[0].point.y > collision.gameObject.transform.position.y + offset)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage();
                playerController.Bounce();
            }
            else if(collision.gameObject.GetComponent<Enemy>().isDying == false)
            {
                healthPoint--;
                UImanager.instance.UpdateHealthBar(healthPoint);
                Vector3 hitPosition = new Vector3(collision.contacts[0].point.x, transform.position.y, collision.contacts[0].point.z);
                Vector3 knockBackDirection = (transform.position - hitPosition).normalized;
                playerController.KnockBack(knockBackDirection);
                StartCoroutine(Invincibility(invincibilityTime));
            }
        }
        //respawns the player when the health point reach 0
        if (healthPoint <= 0)
        {
            GameManager.instance.DecreaseLifeCounter();
            if (GameManager.instance.lifeCounter > 0)
            {
                Respawn();
            }
        }
    }

    /// <summary>
    /// function that respawns the player at the begining of the level with full health
    /// </summary>
    void Respawn()
    {
        transform.position = GameManager.instance.levelStartPosition;
        healthPoint = 3;
        UImanager.instance.UpdateHealthBar(healthPoint);
    }

    /// <summary>
    /// the invincibility coroutine that set the player invincible and plays the visual feedback
    /// </summary>
    /// <param name="invicibleTime"></param>
    /// <returns></returns>
    IEnumerator Invincibility(int invicibleTime)
    {
        float timeStamp = Time.time;
        //disable the controlls of the player
        playerController.controlsEnabled = false;
        //to give an 'effect' of damage taken
        Transform mesh = transform.Find("mesh_root");
        bool tmp = false;
        animator.Play("DAMAGED01");
        while(Time.time - timeStamp < invicibleTime)
        {
            invincible = true;
            //to give an 'effect' of damage taken
            mesh.gameObject.SetActive(tmp);
            tmp = !tmp;

            if (Time.time - timeStamp < 1.5f)
            {
                playerController.controlsEnabled = true;
            }

            yield return new WaitForEndOfFrame();
        }
        invincible = false;
        mesh.gameObject.SetActive(true);
    }

}
