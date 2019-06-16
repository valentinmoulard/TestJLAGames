using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody rigidBody;

    //variables used to manage the movements of the player on the x and z axis
    private float inputHorizontal;
    private float inputVertical;

    //variables used to manage the jump
    private float verticalVelocity;
    private float gravity = 14f;
    public float jumpForce;
    private bool isGrounded;
    private Vector3 direction;


    //speed of the player
    public float speed;

    [HideInInspector]
    public bool controlsEnabled;
    public float knockBackForce;
    public Transform cameraTransform;


    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        controlsEnabled = true;
        isGrounded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //can jump if the player is on the gorund, else the player just falls
        if (isGrounded)
        {
            verticalVelocity = -gravity * 10 * Time.deltaTime;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                Bounce();
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (controlsEnabled)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");

            animator.SetFloat("inputHorizontal", inputHorizontal);
            animator.SetFloat("inputVertical", inputVertical);


            direction = CalculateDirection();
            direction = RotateWithView();
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, cameraTransform.transform.parent.localEulerAngles.y, transform.localEulerAngles.z);

            direction.x = direction.x * speed;
            direction.y = verticalVelocity;
            direction.z = direction.z * speed;
            rigidBody.velocity = direction;
        }
    }

    /// <summary>
    /// this function makes the player jump
    /// it is called when the player hits the spacebar or jump on an enemy
    /// </summary>
    public void Bounce()
    {
        verticalVelocity = jumpForce;
        animator.Play("JUMP00");
    }

    /// <summary>
    /// if the player gets hit, applies a knockback
    /// </summary>
    /// <param name="knockBackDirection"></param>
    public void KnockBack(Vector3 knockBackDirection)
    {
        rigidBody.AddForce(knockBackDirection * knockBackForce);
    }

    /// <summary>
    /// function that calculates the direction of the player considering the inputs
    /// </summary>
    /// <returns></returns>
    Vector3 CalculateDirection()
    {
        Vector3 direction = Vector3.zero;
        direction.x = inputHorizontal;
        direction.z = inputVertical;
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        return direction;
    }


    /// <summary>
    /// rotates the player with the mouse rotation
    /// </summary>
    /// <returns></returns>
    Vector3 RotateWithView()
    {
        if (cameraTransform != null)
        {
            Vector3 dir = cameraTransform.TransformDirection(direction);
            dir.Set(dir.x, 0f, dir.z);
            return dir.normalized * direction.magnitude;
        }
        else
        {
            cameraTransform = Camera.main.transform;
            return direction;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// function that resets teh velocity of the player, set it to the ground
    /// function used if we need the player to stop moving
    /// </summary>
    public void StopPlayer()
    {
        rigidBody.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        direction = Vector3.zero;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    public void PlayVictoryAnimation()
    {
        animator.Play("WIN00");
    }
}
