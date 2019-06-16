using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //initial position of the enemy : if the enemy chase and the player become out of 'stopChaseDistance' distance, it comes back to it's initial position
    Vector3 initialPosition;
    AudioSource audioSource;

    //using nav mesh agent to move the enemy
    NavMeshAgent agent;

    //the enemy start to chase the player when the player is within this range 'chasingTriggerRadius'
    public float chasingTriggerRadius;
    //the enemy chases for this amout of time
    public float chasingTime;
    //the enemy return to it's initial position if the player is out of this range 'stopChaseDistance'
    public float stopChaseDistance;
    [HideInInspector]
    public bool isDying;

    //reference to the coroutine used to chase the player
    Coroutine chasingCoroutineReference;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isDying = false;
        initialPosition = transform.position;
        //sets the radius of the chase trigger to a child object that contains the collider
        transform.GetChild(0).gameObject.GetComponent<SphereCollider>().radius = chasingTriggerRadius;
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chasingCoroutineReference = StartCoroutine(ChasingCoroutine(other.gameObject));
        }
    }

    /// <summary>
    /// chasing coroutine : the enemy chases the player for a certain time
    /// after this time, check if the player is too far or not to continue to chase or return to it's initial position
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator ChasingCoroutine(GameObject target)
    {
        float timeStamp = Time.time;
        //chase for 10 seconds and check if the player is too far to keep chasing
        while (Time.time - timeStamp < chasingTime)
        {
            SetDestination(target.transform.position);
            yield return new WaitForEndOfFrame();
        }
        if (Vector3.Distance(target.transform.position, transform.position) > stopChaseDistance)
        {
            agent.SetDestination(initialPosition);
        }
        else
        {
            StopCoroutine(chasingCoroutineReference);
            chasingCoroutineReference = StartCoroutine(ChasingCoroutine(target));
        }
    }

    /// <summary>
    /// function to set the nav mesh agent destination
    /// </summary>
    /// <param name="target"></param>
    void SetDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }

    /// <summary>
    /// function take damage of the enemy used when the player jumps on it
    /// </summary>
    public void TakeDamage()
    {
        StartCoroutine(EnemyDeath());
    }

    /// <summary>
    /// plays the visual feedback (scale reduced) and audio feedback play sound
    /// and disables the enemy object
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyDeath()
    {
        float timeStamp = Time.time;
        isDying = true;
        audioSource.Play();
        //play sound
        while (Time.time - timeStamp < 1)
        {
            transform.localScale -= new Vector3(0, Time.deltaTime / 10, 0);
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }

}
