using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region instances
    public static PlayerController instance;
    #endregion

    #region settings
    [Header("Movement Settings")]
    public float speed;
    public float rotationSpeed;
    #endregion

    #region privates
    private float horizontal;
    private float vertical;
    private Vector3 moveVector;
    #endregion

    #region components
    public Animator animator;
    private Rigidbody rb;
    public BodyRotation wizardRotationScr;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get input for movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        // Calculate movement vector
        moveVector = new Vector3(horizontal, 0, vertical).normalized;
        if (moveVector.magnitude >= 0.1f) // Don't move if there's no input
        {
            // Move player
            Vector3 moveDirection = moveVector * speed * Time.fixedDeltaTime;
            transform.position += moveDirection;

            // Set animator speed
            animator.SetFloat("Speed", moveVector.magnitude);

            // Handle rotation
            Quaternion targetRotation = Quaternion.LookRotation(moveVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // If no input, stop the animator
            animator.SetFloat("Speed", 0);
        }
    }
}
