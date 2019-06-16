using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float cameraSpeed;
    public GameObject objectToFollow;
    public float clampAngle;
    public float inputSensitivity;

    //store the x and y mouse inputs
    private float mouseX;
    private float mouseY;

    private float rotationX = 0;
    private float rotationY = 0;

    void Start()
    {
        //take the current rotation of the camera at the start
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotationX = rotation.x;
        rotationY = rotation.y;

        //lock cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

	void Update () {
        UpdateRotation();
    }

    void LateUpdate()
    {
        CameraUpdater();
    }

    /// <summary>
    /// sets the rotation of the camera considering the mouse inputs
    /// </summary>
    void UpdateRotation()
    {
        //set up the rotation
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseY * inputSensitivity * Time.deltaTime;
        rotationX += mouseX * inputSensitivity * Time.deltaTime;

        //the rotation can't go lower than -clampAngle and can't go higher than clampAngle
        rotationY = Mathf.Clamp(rotationY, -clampAngle, clampAngle);

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0.0f);
    }

    void CameraUpdater()
    {
        //set the target object to follow
        Transform target = objectToFollow.transform;
        //move toward the target
        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
