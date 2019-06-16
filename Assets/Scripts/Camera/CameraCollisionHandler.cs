using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionHandler : MonoBehaviour {
    /// <summary>
    /// the scripts prevent the camera from getting through walls etc
    /// if there is a wall wlose to the back of the player, the camera should get closer to the player
    /// </summary>


    //min and max distance between the camera and the player
    public float minDistance;
    public float maxDistance;
    //smooth coefficient when the camera moves
    public float smooth;

    Vector3 direction;
    public float distance;
    public LayerMask layer;

    void Awake()
    {
        direction = transform.localPosition.normalized;
        //distance of the object from the origin
        distance = transform.localPosition.magnitude;
    }
    

	void Update () {
        //calculates the position of the camera in world space (we take the parent here because the main camera is a child of a gameobject)
        Vector3 desiredCameraPosition = transform.parent.TransformPoint(direction * maxDistance);
        RaycastHit hit;
        //check if the camera collides with anything when we move it
        if (Physics.Linecast(transform.parent.position, desiredCameraPosition, out hit, layer))
        {
            //the coefficient 0.8 prevents the camera from going into an object
            distance = Mathf.Clamp((hit.distance * 0.8f), minDistance, maxDistance);
        }
        else
        {
            //if the camera doesn't collide with anything, it goes to it's normal place
            distance = maxDistance;
        }
        //once the position is calculated we move the camera the the right position
        transform.localPosition = Vector3.Lerp(transform.localPosition, direction * distance, Time.deltaTime * smooth);

	}
}
