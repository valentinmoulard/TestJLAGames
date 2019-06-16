using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    AudioSource audioSource;

	void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	void Update () {
        //simple visual feedback on the collectibles
        transform.Rotate(new Vector3(15, 30, 0) * Time.deltaTime);
	}

    //function called when an item is collected by the player
    public void CollectedItem()
    {
        StartCoroutine(Collected());
    }

    /// <summary>
    /// Function that plays the visual and audio feedback when the item is collected and disables it after
    /// </summary>
    /// <returns></returns>
    IEnumerator Collected()
    {
        float timeStamp = Time.time;
        audioSource.Play();
        while (Time.time - timeStamp < 1f)
        {
            transform.Rotate(new Vector3(50, 50, 20) * Time.deltaTime * 20);
            transform.Translate(Vector3.up * Time.deltaTime * 20);
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
    }
}
